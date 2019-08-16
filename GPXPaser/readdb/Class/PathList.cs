using System;
using System.IO;
using System.Collections.Generic;


namespace readdb.Class
{
    public class PathList
    {
        private System.IO.DirectoryInfo parentDirectory;                        //폴더들의 폴더 directory info
        private System.IO.DirectoryInfo childDirectory;                         //파일들의 폴더 directory info
        private List<FileInfo> files = new List<FileInfo>();                    //파일들이 담길 리스트 생ㅅ

        public PathList(string parentFolderPath)                                //맨위 폴더 경로로 초기화 해주면
        {   
            parentDirectory = new System.IO.DirectoryInfo(parentFolderPath);    //그 폴더로 디렉토리 설정
        }

        ~PathList()
        {
            GC.Collect();                                                       //garbage collection
        }


        private void SetChildFolerPath(string childFolderPath)                  //파일들의 폴더 경로설정하는 함수
        {
            childDirectory = new System.IO.DirectoryInfo(childFolderPath);
        }

        
        private List<FileInfo> GetFiles()                                       //파일리스트를 반환해주는 함수
        {
            files.Clear();                                                      //클리어 한번 해주고
            files.AddRange(childDirectory.GetFiles());                          //파일들 리스트 형태로 add range해주고
            files.Sort(new CompareFileInfoEntries());                           //sorting까지 해주고
            return files;                                                       //반환
        }

        public List<DirectoryInfo> GetFolders()                                 //폴더 반환해주기
        {
            List<DirectoryInfo> folders = new List<DirectoryInfo>();            //폴더는 리스트 한번만 쓰므로 걍 함수 내에 local 변수
            folders.AddRange(parentDirectory.GetDirectories());                 //이하 중략
            folders.Sort(new CompareDirectoryInfoEntries());
            return folders;
        }


        public List<FileInfo> GetFiles(string childFolderPath)                  //파일 리스트를 설정 반환을 한번에 해주는 함수
        {
            SetChildFolerPath(childFolderPath);
            return GetFiles();
        }


        public class CompareDirectoryInfoEntries : IComparer<DirectoryInfo>     //이름 순으로 sorting을 우리해 만듬
        {
            public int Compare(DirectoryInfo f1, DirectoryInfo f2)
            {
                return (string.Compare(f1.Name, f2.Name));
            }
        }

        public class CompareFileInfoEntries : IComparer<FileInfo>               //이름 순으로 sorting을 우리해 만듬
        {
            public int Compare(FileInfo f1, FileInfo f2)
            {
                return (string.Compare(f1.Name, f2.Name));
            }
        }

    }



}
