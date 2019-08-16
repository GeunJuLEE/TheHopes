using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using readdb.Models;
using readdb.Class;


namespace readdb
{
    public class Parser
    {
        private readonly string type = "System.Xml.XmlDocument";

        public Parser()
        {
        }

        ~Parser()
        {
            GC.Collect();       //가비지 콜렉터
        }
        

        public List<MobilityData> ReadXml(string fileName)
        {
            Console.WriteLine(fileName + " is starting");       //파일 실	
            List<MobilityData> mobilityDatas = new List<MobilityData>();    //데이터가 담길 리스트
            XmlDocument xmlDocument = new XmlDocument();                    //xml reader 초기화
            try
            {
                xmlDocument.Load(fileName);                                 //파일을 불러옴
                if (xmlDocument.GetType().ToString().Equals(type))          //xml일 경우만 파싱시작
                {
                    foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)    //한 node 안으로 들어옴
                    {
                        foreach (XmlNode trk in node)
                        {
                            // thereare a couple child nodes here so only take data from node named loc 
                            if (trk.Name == "trkseg")                       //trkseg안에 latitude와 longitude가 있음
                            {
                                foreach (XmlNode trkseg in trk)
                                {
                                    string[] vs = trkseg.OuterXml.Split(Convert.ToChar("\""));      //"을 기준으로 나눔
                                    double Latitude = Convert.ToDouble(vs[1]);                      //그 2번째가 latitude
                                    double Longitude = Convert.ToDouble(vs[3]);                     //그 4번째가 ㅣongitude
                                    if (trkseg.Name == "trkpt")                                     
                                    {   
                                        XmlNodeList xmlNodeList = trkseg.ChildNodes;                    //trkpt로 들어감
                                        double Ele = Convert.ToDouble(xmlNodeList[0].InnerText);        //첫 내용이 ele
                                        DateTime Time = Convert.ToDateTime(xmlNodeList[1].InnerText);   //두번째 내용이 time

                                        /*Console.WriteLine("time = " + Time);
                                        Console.WriteLine("lat = " + Latitude);
                                        Console.WriteLine("lon = " + Longitude);
                                        Console.WriteLine("ele = " + Ele);
                                        Console.WriteLine();*/

                                        mobilityDatas.Add(new MobilityData(Time, Latitude, Longitude, Ele));    //리스트에 추가
                                    }
                                }
                            }
                        }
                    }
                }
                return mobilityDatas;       //리스트 반환
            }
            catch(Exception ex)             //try catch 구문
            {
                Console.WriteLine(ex);
                return mobilityDatas;
            }
        }
    }
}
