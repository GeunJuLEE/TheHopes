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
        static DirectoryInfo relativePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;    //상대주소
        static string path = relativePath + "/MobilityData";        //상대주소 + 원하는 폴더명

        static PathList pathList = new PathList(path);  //폴더들이 있는 경로로 초기화 및 객체 생성
        static Parser parser = new Parser();            //파싱하는 객체 생성
        static Upload upload = new Upload();            //서버관련 객체 생성

        static List<DirectoryInfo> folders = new List<DirectoryInfo>(); //폴더 리스트들이 담길 곳
        static List<FileInfo> files = new List<FileInfo>();             //파일 리스트들이 담길 곳

        static List<MobilityData> mobilityDatas = new List<MobilityData>(); //raw data 객체들이 담길 곳


        public static void Main(string[] args)
        {
            Console.WriteLine("---------start---------");   //시작
            //Console.WriteLine(relativePath);
            folders = pathList.GetFolders();                //폴더들의 리스트
            foreach (var folder in folders)                 //한 폴더마다로 for 문 시작
            {
                upload.Run(folder);                         //한 폴더를 이름파싱,  디비에 존재하는지에 따라 insert 등 기본적인 run
                files = pathList.GetFiles(folder.FullName); //해당 폴더에 들은 파일들의 리스트
                foreach (var file in files)                 //한 파일마다로 for 문 시작
                {
                    mobilityDatas = parser.ReadXml(file.FullName);  //한 파일안에 raw data 리스트 받음
                    foreach (var mobilitydata in mobilityDatas)     //raw data 리스트에서 하나씩 for 문 시작
                    {
                        upload.InsertData(mobilitydata);            //한 raw data씩 insert
                    }
                    Console.WriteLine(file.FullName + " is done!!");    //한 파일 끝
                }                                                       //한 폴더 끝
            }
            Console.WriteLine("\n---------everything is done!!---------");  //다 끝
            
        }
    }
}
