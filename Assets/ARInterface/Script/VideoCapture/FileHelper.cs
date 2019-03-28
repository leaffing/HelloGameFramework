using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Leaf.Common
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 将字节数组保存为二进制文件
        /// </summary>
        /// <param name="bytes">需要保存的字节数据</param>
        /// <param name="path">保存路径</param>
        /// <param name="fileName">文件名</param>
        public static void SaveBinaryFile(byte[] bytes, string path, string fileName)
        {
            //File.WriteAllBytes(path + fileName, bytes);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            using (FileStream file = File.Open(path + fileName, FileMode.OpenOrCreate))
            {
                BinaryWriter writer = new BinaryWriter(file);
                writer.Write(bytes);
                file.Close();
            }
        }

        /// <summary>
        /// 以二进制形式读取文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns>读取的字节数据</returns>
        public static byte[] LoadBinaryFile(string path, string fileName)
        {
            if(!File.Exists(path + fileName))
            {
                Debug.Log("File is not exist.");
                return null;
            }
            using (FileStream fs = new FileStream(path + fileName, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(0, SeekOrigin.Begin);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();
                fs.Dispose();
                return bytes;
            }
        }

        /// <summary>
        /// 获取路径下所有文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件名列表</returns>
        public static string[] GetDirectoryFiles(string path)
        {
            List<string> fileNames = new List<string>();
            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    fileNames.Add(files[i].Name);
                }
            }
            return fileNames.ToArray();
        }
    }
}