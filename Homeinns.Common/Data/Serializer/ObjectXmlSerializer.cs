using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Diagnostics;
/**
 * MSDN;http://msdn.microsoft.com/zh-cn/library/System.Xml.Serialization.XmlSerializer.aspx 
 * 
 * 
 */
namespace Homeinns.Common.Data.Serialize
{
    /// <summary>
    /// XML序列化工具类
    /// </summary>
    public class ObjectXmlSerializer
    {
        /// <summary>
        /// 返回 XML字符串 节点value
        /// </summary>
        /// <param name="xmlDoc">XML格式 数据</param>
        /// <param name="xmlNode">节点</param>
        /// <returns>节点value</returns>
        public static string GetStrForXml(string xmlDoc, string xmlNode)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlDoc);
            XmlNode xn = xml.SelectSingleNode(xmlNode);
            return xn.InnerText;
        }

        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, NewLineChars = "\r\n", Encoding = encoding, IndentChars = "    " };
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string content, Encoding encoding)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(content)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }


        /// <summary>
        /// deserialize an object from a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m_FileNamePattern"></param>
        /// <returns>
        /// loggingEnabled==true: Null is returned if any error occurs.
        /// loggingEnabled==false: throw exception
        /// </returns>
        public static T LoadFromXml<T>(string fileName) where T : class
        {
            return LoadFromXml<T>(fileName, true);
        }

        public static T LoadFromXml<T>(string fileName, bool loggingEnabled) where T : class
        {
            FileStream fs = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception e)
            {
                if (loggingEnabled)
                {
                    LogLoadFileException(fileName, e);
                    return null;
                }
                else throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// serialize an object to a file.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m_FileNamePattern"></param>
        /// <returns>
        /// loggingEnabled==true: log exception
        /// loggingEnabled==false: throw exception
        /// </returns>
        public static void SaveToXml<T>(string fileName, T data) where T : class
        {
            SaveToXml<T>(fileName, data, true);
        }

        public static void SaveToXml<T>(string fileName, T data, bool loggingEnabled) where T : class
        {
            FileStream fs = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                serializer.Serialize(fs, data);
            }
            catch (Exception e)
            {
                if (loggingEnabled) LogSaveFileException(fileName, e);
                else throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        public static string XmlSerializer<T>(T serialObject) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);
            ser.Serialize(writer, serialObject);
            writer.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static T XmlDeserialize<T>(string str) where T : class
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            StreamReader sr = new StreamReader(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(str)), System.Text.Encoding.UTF8);
            return (T)mySerializer.Deserialize(sr);
        }

        public static T DataContractDeserializer<T>(string xmlData) where T : class
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlData));
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            reader.Close();
            stream.Close();
            return (T)ser.ReadObject(reader, true);
        }

        public static string DataContractSerializer<T>(T myObject) where T : class
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(stream, myObject);
            stream.Close();
            return System.Text.UnicodeEncoding.UTF8.GetString(stream.ToArray());
        }

        #region Logging
        private const string LogCategory = "Framework.ObjectXmlSerializer";
        private const int LogEventLoadFileException = 1;
        [Conditional("TRACE")]
        private static void LogLoadFileException(string fileName, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Fail to load xml file: ");
            sb.Append(fileName + System.Environment.NewLine);
            sb.Append(ex.ToString());
            //  Logger.LogEvent(LogCategory, LogEventLoadFileException, sb.ToString());
        }

        [Conditional("TRACE")]
        private static void LogSaveFileException(string fileName, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Fail to save xml file: ");
            sb.Append(fileName + System.Environment.NewLine);
            sb.Append(ex.ToString());
            // Logger.LogEvent(LogCategory, LogEventLoadFileException, sb.ToString());
        }
        #endregion
    }
}
