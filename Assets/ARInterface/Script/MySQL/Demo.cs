using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace SaveGetImageMySql
{
    class Demo
    {
        private static byte[] FileToBytes(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            byte[] buffer = new byte[fi.Length];
            FileStream fs = fi.OpenRead();
            fs.Read(buffer, 0, Convert.ToInt32(fi.Length));
            fs.Close();
            return buffer;
        }
        private static void CreateFile(byte[] fileBuffer, string newFilePath)
        {
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }
            FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(fileBuffer, 0, fileBuffer.Length); //用文件流生成一个文件
            bw.Close();
            fs.Close();
        }
        private static MySqlConnection CreateConnection()
        {
            string host = "localhost";
            string id = "bss217";
            string pwd = "bss0217";
            string database = "abc";
            string connectionStr = string.Format("Server={0};port={1};Database={2};UserID={3};Password={4};", host, "3306", database, id, pwd);
            MySqlConnection Connection = new MySqlConnection(connectionStr); //建立MySQL连接
            return Connection;
        }
        private static void SendFileBytesToDatabase(string title, byte[] fileArr)
        {
            MySqlConnection sendDataConnection = CreateConnection();
            string sendFileSql = "insert into file(title,file) values(?title,?file);";
            MySqlCommand sendCmd = new MySqlCommand(sendFileSql, sendDataConnection);
            sendCmd.Parameters.Add("?title", MySqlDbType.VarChar).Value = title;
            sendCmd.Parameters.Add("?file", MySqlDbType.MediumBlob).Value = fileArr;
            sendDataConnection.Open();
            try
            {
                sendCmd.ExecuteNonQuery();
                Console.WriteLine("向数据库储存数据完成");
            }
            catch (Exception e)
            {
                Console.WriteLine("向数据库存储数据失败：" + e.Message);
            }
            finally
            {
                sendDataConnection.Close();
            }
        }
        private static void GetFileFromDatabase(string searchTitle, string newFilePath)
        {
            MySqlConnection getFileConnection = CreateConnection();
            string getFileSql = "select * from file where title='" + searchTitle + "';";
            MySqlCommand getCmd = new MySqlCommand(getFileSql, getFileConnection);
            getFileConnection.Open();
            MySqlDataReader reader = getCmd.ExecuteReader();
            byte[] newFileBuffer = null;
            try
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        long len = reader.GetBytes(1, 0, newFileBuffer, 0, 0);
                        newFileBuffer = new byte[len];
                        len = reader.GetBytes(1, 0, newFileBuffer, 0, (int)len);
                        CreateFile(newFileBuffer, newFilePath);
                        Console.WriteLine("已成功创建文件：" + newFilePath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("查找数据库信息失败：" + e.Message);
            }
            finally
            {
                getFileConnection.Close();
            }
        }
        public void SendImgTest()
        {
            string imgPath = @"D:\pics\cloud.png";
            byte[] imgBuffer = FileToBytes(imgPath);
            SendFileBytesToDatabase("cloud", imgBuffer);
        }
        public void GetImgTest()
        {
            string imgPath = @"D:\pics\cloudtest.pdf";
            GetFileFromDatabase("cloud", imgPath);
        }
    }
}