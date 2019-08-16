using System;
using System.IO;
using MySql.Data.MySqlClient;
using readdb.Models;



namespace readdb.Class
{
    public class Upload
    {

        private readonly string masterTable = "objMaster";      //마스터 테이블 명
        private readonly string success = "Success";            //성공시 반환
        private readonly string fail = "Fail";                  //실패시 반환
        private readonly string strConn = "Server=223.194.70.34; Port=3307; Database=mysql; Uid=officium; Pwd=library@1989; CharSet=utf8";  //nas 로그인 방법
        private MySqlConnection conn;       //connection
        private string sql = "";            //sql 문
        public MySqlCommand cmd;            //mysql 실행문

        private string objName = "";        //실행될 object name
        private string objKorName = "";     //실행될 object의 한국이름



        public Upload()                             //초기화
        {
            conn = new MySqlConnection(strConn);    //conn을 해당 서버로 객체생성
            cmd = new MySqlCommand(sql, conn);      //sql문 해주는 cmd 생성 
            conn.Open();                            //connection 연결
        }

        ~Upload()
        {
            conn.Close();                           //connection 닫음
            GC.Collect();                           //garbage collection
        }

        private void CreateTable()
        {
            try
            {
                cmd.CommandText = @"CREATE TABLE `" + objName +
                                    "`( `Time` DATETIME NOT NULL ," +
                                    "`Latitude` DOUBLE NOT NULL ," +
                                    " `Longitude` DOUBLE NOT NULL ," +
                                    " `Altitude` DOUBLE NULL DEFAULT NULL," +
                                    " `Ele` DOUBLE NOT NULL ," +
                                    " PRIMARY KEY(`Time`)) ENGINE = InnoDB";        //create 문

                _ = cmd.ExecuteNonQuery();                                          //실행
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }




        

        private bool Exists()
        {

            try
            {
                cmd.CommandText = @"SELECT COUNT(*) FROM " + masterTable + " WHERE objName = '" + objName + "'";    //해당 objName이 있나 select count
                MySqlDataReader reader = cmd.ExecuteReader();           //reader 객체 생성
                _ = reader.Read();                                      //read
                if (reader[0].ToString().Equals("1"))                   //read한 값이 1이면 존    
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private void InsertObjName()
        {
            if (Exists())
            {
                return;
            }
            else                                                                //해당 objName이 없으면
            {
                try
                {
                    cmd.CommandText = @"INSERT INTO " +
                                        masterTable + " " +
                                        "VALUES(@ObjName, @ObjKorName)";        //insert문
                    cmd.Parameters.AddWithValue("@objName", objName);           //parameter 추가
                    cmd.Parameters.AddWithValue("@objKorName", objKorName);     //parameter 추가
                    _ = cmd.ExecuteNonQuery();                                  //실행
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return;
                }

            }
        }




        private void Set(DirectoryInfo directoryInfo)
        {
            try
            {
                string[] vs = directoryInfo.Name.Split(Convert.ToChar("."));        //.으로 파싱
                objName = vs[0].ToLower();                                          //앞에 글자들 소문자로 바꾸고 값 대입
                objKorName = vs[1];                                                 //뒤에 글자 대입
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }



        public void Run(DirectoryInfo directoryInfo)
        {
            try
            {
                Set(directoryInfo);                             //DirectoryInfo로 Set
                InsertObjName();                                //마스터테이블에 Insert하는 함
                CreateTable();                                  //테이블 만드는 함수
                Console.WriteLine("ready for " + objName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }






        public void InsertData(MobilityData mobilityData)                               //raw data insert 함수
        {
            try
            {
                cmd.CommandText = @"INSERT INTO " +
                                    objName + " " +
                                    "VALUES(@Time, @Latitude, @Longitude, NULL, @Ele)"; //insert문
                cmd.Parameters.AddWithValue("@Time", mobilityData.Time);                //parameter 추가
                cmd.Parameters.AddWithValue("@Latitude", mobilityData.Latitude);        //parameter 추가
                cmd.Parameters.AddWithValue("@Longitude", mobilityData.Longitude);      //parameter 추가
                cmd.Parameters.AddWithValue("@Ele", mobilityData.Ele);                  //parameter 추가
                _ = cmd.ExecuteNonQuery();                                              //실행
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

        }


    }
}
