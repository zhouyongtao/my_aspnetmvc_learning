using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description  DataSet辅助类
 * @date 2012年9月9日11:02:12
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @msdn;http://support.microsoft.com/kb/829740/zh-cn
 */
namespace Homeinns.Common.Data.Serialize
{
    public class DataSetSerializer
    {/// <summary>
        /// 序列化DataSet
        /// </summary>
        /// <param name="dataSet">需要序列化的DataSet</param>
        /// <returns>序列化后的byte数组</returns>
        public static byte[] DataSetToByte(DataSet dataSet)
        {
            //通过dataset创建DataSetSurrogate对象
            DataSetSurrogate surrogate = new DataSetSurrogate(dataSet);
            MemoryStream str = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            //将DataSetSurrogate实例转化成内存流
            bf.Serialize(str, surrogate);
            return str.ToArray();
        }

        /// <summary>
        /// 反序列化DataSet
        /// </summary>
        /// <param name="dsByte">Byte数组</param>
        /// <returns>DataSet</returns>
        public static DataSet ByteToDataSet(byte[] dsByte)
        {
            //将byte数组转化成内存流
            MemoryStream str = new MemoryStream(dsByte);
            BinaryFormatter bf = new BinaryFormatter();
            //将内存流转化成object对象
            object o = bf.Deserialize(str);
            //强行转化成DataSetSurrogate实例
            DataSetSurrogate surrogate = (DataSetSurrogate)o;
            //通过DataSetSurrogate转化成DataSet
            return surrogate.ConvertToDataSet();
        }

        #region 序列化

        /// <summary>
        /// 二进制的方式序列化DataTable
        /// </summary>
        /// <param name="o">需要转换的对象</param>
        /// <returns>二进制数组</returns>
        private static byte[] GetBinaryFormatDataTable(DataTable dt)
        {
            MemoryStream memory = new MemoryStream();//使用内存流来存这些byte[]
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memory, dt);
            byte[] buff = memory.GetBuffer(); //这里就可你想要的byte[],可以使用它来传输
            memory.Close();
            memory.Dispose();
            return buff;
        }
        /// <summary>
        /// 反序列化byteDataTable
        /// </summary>
        /// <param name="bt">二进制数据</param>
        /// <returns>DataTable</returns>
        private static DataTable RetrieveDataTable(byte[] bt)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (DataTable)formatter.Deserialize(new MemoryStream(bt)); //byte[]转回的datatable
        }

        /// <summary>
        /// DataSet序列化
        /// </summary>
        /// <param name="ds">需要序列化的DataSet</param>
        /// <returns></returns>
        private static byte[] GetBinaryFormatDataSet(DataSet ds)
        {
            MemoryStream memStream = new MemoryStream();   //创建内存流
            IFormatter formatter = new BinaryFormatter();//产生二进制序列化格式
            ds.RemotingFormat = SerializationFormat.Binary;//指定DataSet串行化格式是二进制
            formatter.Serialize(memStream, ds);//串行化到内存中
            byte[] binaryResult = memStream.ToArray();//将DataSet转化成byte[]
            memStream.Close();//清空和释放内存流
            memStream.Dispose();
            return binaryResult;
        }
        /// <summary>
        /// DataSet反序列化
        /// </summary>
        /// <param name="binaryData">需要反序列化的byte[]</param>
        /// <returns></returns>
        private static DataSet RetrieveDataSet(byte[] binaryData)
        {
            MemoryStream memStream = new MemoryStream(binaryData);//创建内存流
            IFormatter formatter = new BinaryFormatter();//产生二进制序列化格式
            object obj = formatter.Deserialize(memStream);//反串行化到内存中
            //类型检验
            if (obj is DataSet)
            {
                DataSet dataSetResult = (DataSet)obj;
                return dataSetResult;
            }
            else
            {
                return null;
            }
        }
        #endregion 序列化
    }
}
