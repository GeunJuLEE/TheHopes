using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using readdb.Class;
using readdb.Models;

namespace readdb
{
    public class readdatabase
    {
        static DirectoryInfo relativePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent; // 상대주소
        static string path = relativePath + "/MobilityData"; // 상대주소 + 원하는 폴더명

        static PathList pathList = new PathList(path);  
        static Parser parser = new Parser();           
        static Upload upload = new Upload();           

        static List<DirectoryInfo> folders = new List<DirectoryInfo>(); // 폴더 리스트들이 담길 곳
        static List<FileInfo> files = new List<FileInfo>();             // 파일 리스트들이 담길 곳

        static List<MobilityData> mobilityDatas = new List<MobilityData>(); //raw data 객체들이 담길 곳


        public static void Main(string[] args)
        {
            Console.WriteLine("---------start---------");   // 시작
            //Console.WriteLine(relativePath);
            folders = pathList.GetFolders();                // 폴더들의 리스트
            
            foreach (var folder in folders)                 
            {
                upload.Run(folder);                         // 한 폴더를 이름파싱, 디비에 존재하는지에 따라 insert 등 기본적인 run
                
                files = pathList.GetFiles(folder.FullName);
                foreach (var file in files)                
                {
                    mobilityDatas = parser.ReadXml(file.FullName); 
                    foreach (var mobilitydata in mobilityDatas)    
                    {
                        // Console.WriteLine(mobilitydata); 이건 그냥 출력용 코드 
                        upload.InsertData(mobilitydata);            
                    }
                    Console.WriteLine(file.FullName + " is done!!"); 
                }                                            
            }
            Console.WriteLine("\n---------everything is done!!---------"); 
            
        }
    }
}
