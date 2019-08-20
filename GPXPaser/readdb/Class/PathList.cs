using System;
using System.IO;
using System.Collections.Generic;


namespace readdb.Class
{
    public class PathList
    {
        private System.IO.DirectoryInfo parentDirectory;                        // 폴더들의 폴더 directory info
        private System.IO.DirectoryInfo childDirectory;                         // 파일들의 폴더 directory info
        private List<FileInfo> files = new List<FileInfo>();                    // 파일들이 담길 리스트 생성 

        // 생성자
        public PathList(string parentFolderPath)                             
        {
            parentDirectory = new System.IO.DirectoryInfo(parentFolderPath);    // 해당 폴더를 디렉토리로 지정
        }

        // 소멸자
        ~PathList()
        {
            GC.Collect();                                                      
        }


        // 아래 함수는 폴더 리스트를 리턴하기 위해 필요한 함수임
        public List<DirectoryInfo> GetFolders()                                 // MobilityData 내 폴더들 반환
        {
            List<DirectoryInfo> folders = new List<DirectoryInfo>();            // 폴더는 리스트 한번만 쓰므로 걍 함수 내에 local 변수
            folders.AddRange(parentDirectory.GetDirectories());                 
            folders.Sort(new CompareDirectoryInfoEntries());
            return folders;
        }


        // 아래 3개의 함수는 파일 리스트를 리턴하기 위해 필요한 함수들임
        private List<FileInfo> GetFiles()                                       // 파일리스트를 반환해주는 함수
        {
            files.Clear();                                                      // local 변수 클리어 실행
            files.AddRange(childDirectory.GetFiles());                          // 파일들 리스트 형태로 AddRange
            files.Sort(new CompareFileInfoEntries());                           // Sort
            return files;                                                       
        }
        private void SetChildFolerPath(string childFolderPath)                  // 파일들의 폴더 경로설정하는 함수
        {
            childDirectory = new System.IO.DirectoryInfo(childFolderPath);
        }

        public List<FileInfo> GetFiles(string childFolderPath)                  // 파일 리스트를 설정 반환을 한번에 해주는 함수
        {
            SetChildFolerPath(childFolderPath);
            return GetFiles();
        }


        // 아래 두 함수는 폴더 및 파일 이름을 sorting 하기 위해서 필요한 함수들
        public class CompareDirectoryInfoEntries : IComparer<DirectoryInfo>  
        {
            public int Compare(DirectoryInfo f1, DirectoryInfo f2)
            {
                return (string.Compare(f1.Name, f2.Name));
            }
        }
        public class CompareFileInfoEntries : IComparer<FileInfo>         
        {
            public int Compare(FileInfo f1, FileInfo f2)
            {
                return (string.Compare(f1.Name, f2.Name));
            }
        }

    }
}
