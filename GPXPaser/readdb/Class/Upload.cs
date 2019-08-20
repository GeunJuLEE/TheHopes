using System;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using readdb.Models;



namespace readdb.Class
{
    public class Upload
    {

        private readonly string masterTable = "objMaster";    
        private readonly string success = "Success";          
        private readonly string fail = "Fail";                
        private readonly string strConn = "Server=223.194.70.34; Port=3307; Database=mysql; Uid=officium; Pwd=library@1989; CharSet=utf8";
        private MySqlConnection conn;      
        private string sql = "";     
        public MySqlCommand cmd; 

        private string objName = "";        // 실행될 object name
        private string objKorName = "";     // 실행될 object의 한국이름



        public Upload()                             // 초기화
        {
            conn = new MySqlConnection(strConn);    // conn을 해당 서버로 객체생성
            cmd = new MySqlCommand(sql, conn);      // sql문 해주는 cmd 생성
        }

        ~Upload()
        {
            conn.Close();                           // connection 닫음
            GC.Collect();                           // garbage collection
        }

        private void CreateTable()
        {
            try
            {
                conn.Open();
                cmd.CommandText = @"CREATE TABLE " + objName +
                                    " ( `Time` DATETIME NOT NULL ," +
                                    " `Latitude` DOUBLE NOT NULL ," +
                                    " `Longitude` DOUBLE NOT NULL ," +
                                    " `Altitude` DOUBLE NULL DEFAULT NULL," +
                                    " `Ele` DOUBLE NOT NULL ," +
                                    " PRIMARY KEY(`Time`)) ENGINE = InnoDB";    // create 문

                _ = cmd.ExecuteNonQuery();
                conn.Close();
                return;
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex);
                return;
            }
        }

        private bool Exists()
        {

            try
            {
                conn.Open();
                cmd.CommandText = @"SELECT COUNT(*) FROM " + masterTable + " WHERE objName = '" + objName + "'";
                MySqlDataReader reader = cmd.ExecuteReader();
                _ = reader.Read();                             
                if (reader[0].ToString().Equals("1"))                   // read한 값이 1이면 존재한다는 의미이다 
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex);
                return false;
            }
        }

        private void InsertObjName()
        {
            if (Exists())
            {
                conn.Close();
                return;
            }
            else // 해당 objName이 없으면
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = @"INSERT INTO " +
                                        masterTable + " " +
                                        "VALUES(@ObjName, @ObjKorName, 0, 0)";  
                    cmd.Parameters.AddWithValue("@ObjName", objName); 
                    cmd.Parameters.AddWithValue("@ObjKorName", objKorName);
                    Console.WriteLine(cmd.CommandText);
                    _ = cmd.ExecuteNonQuery(); 
                    conn.Close();
                    return;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    Console.WriteLine(ex);
                    return;
                }

            }
        }


        private void Set(DirectoryInfo directoryInfo)
        {
            try
            {
                // 폴더 이름은 . (점) 을 기준으로 앞, 뒤로 나눠서 vs에 저장한다 0 번쨰 인덱스에 앞의 내용이, 1번째 인덱스에 뒤의 내용을 저장 
                string[] vs = directoryInfo.Name.Split(Convert.ToChar("."));        
                objName = vs[0].ToLower();
                // vs[1]의 string을 ks_c_5601-1987의 형식으로 인코딩 해주어야 한다 
                byte[] bytes = Encoding.GetEncoding("ks_c_5601-1987").GetBytes(vs[1].ToCharArray());
                objKorName = Encoding.GetEncoding("ks_c_5601-1987").GetString(bytes);
                // System.Diagnostics.Debug.WriteLine(objKorName); 으로 제대로 인코딩 되었는지 확인 가능
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
            Set(directoryInfo);                            
            Console.WriteLine("objName is " + objName);
            Console.WriteLine("objKorName is " + objKorName);

            // 아래 두 줄 주석처리 되어 있다면 해제해야 됩니다
            InsertObjName();
            CreateTable();                        
            Console.WriteLine("ready for " + objName);
        }


        public void InsertData(MobilityData mobilityData) //raw data insert 함수
        {
            try
            {
                conn.Open();

                // 파라미터 초기화 시켜주는 부분 이거 없으면 "Parameter '@Time' has already been defined" 계속 뜸
                cmd.Parameters.Clear();

                cmd.CommandText = @"INSERT INTO " +
                                    objName + " " +
                                    "VALUES(@Time, @Latitude, @Longitude, NULL, @Ele)"; //insert문

                cmd.Parameters.AddWithValue("@Time", mobilityData.Time);               
                cmd.Parameters.AddWithValue("@Latitude", mobilityData.Latitude);       
                cmd.Parameters.AddWithValue("@Longitude", mobilityData.Longitude);   
                cmd.Parameters.AddWithValue("@Ele", mobilityData.Ele);                  
                _ = cmd.ExecuteNonQuery();                                      
                conn.Close();
                return;
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex);
                return;
            }

        }
    }
}
