using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Homeinns.Common.Base
{
    public class FileUtil
    {
        /// <summary>
        /// 检查文件夹路径是否存在(不存在则创建)
        /// </summary>
        /// <param name="fileName"></param>
        public static void CheckPath(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        //.Net 把 一个文件夹中的内容复制到另一个文件夹  http://www.cnblogs.com/Kiss920Zz/archive/2012/11/30/2795643.html
        //c#.net 上传文件到网络上的共享文件夹里面 http://bbs.csdn.net/topics/350021936
    }
}
