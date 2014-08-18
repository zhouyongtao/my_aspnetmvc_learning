using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Homeinns.Common.Data.Serializer
{   /// <summary>
    /// 序列化与反序列化工具类
    /// </summary>
    public class BinarySerializer
    {   /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] Serialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, t);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将对象写入文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void SerializefilePath<T>(T obj, string filePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                byte[] bytes = new byte[ms.Length];
                bytes = ms.GetBuffer();
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bys">对象的字节数组</param>
        /// <returns>反序列化对象</returns>
        public static T Deserialize<T>(byte[] bys)
        {
            using (MemoryStream ms = new MemoryStream(bys))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }


        /// <summary>
        /// 将文件反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bys"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T DeserializeFile<T>(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

    }
}
