﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;
/*
 下载示例
 *       1. Common.DownFile.DownloadFile(context, @"/Files/记事本.mp3");
 * 
 *       2.
            if (!Common.DownFile.ResponseFile(context.Request, context.Response, "vs2005.rar", @"D:\vs2005.rar", 1024000))
                context.Response.Write("下载文件出错！");
                context.Response.End();
 * 
 *       3.
         Common.DownFile.DownloadFile(context, @"D:\vs2005.rar", 1024000); 
 *
/*
 NickName:Irving
 Email:zhouyontao@outlook.com
 Date:2012年9月10日11:38:53
 */
namespace Homeinns.Common.Base.Web
{
    /// <summary>
    /// 文件下载类
    /// </summary>
    public class DownFile
    {
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="fileName">文件路径</param>
        public static void DownloadFile(HttpContext context, string filePath)
        {
            filePath = context.Server.MapPath(filePath);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                context.Response.Clear();
                context.Response.Charset = "GB2312";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //下载文件默认文件名
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)));
                //添加头信息，指定文件大小，让浏览器能显示下载进度
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.AddHeader("Connection", "Keep-Alive");
                context.Response.ContentType = "application/octet-stream";
                //把文件发送该客户段
                context.Response.WriteFile(file.FullName);
                context.Response.Flush();
                context.Response.End();
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="request"> Page.Request对象</param>
        /// <param name="response"> Page.Response对象</param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="fullPath">带文件名下载路径</param>
        /// <param name="speed">每秒允许下载的字节数</param>
        /// <returns></returns>
        public static bool DownloadFile(HttpRequest request, HttpResponse response, string fileName, string fullPath, double speed)
        {
            try
            {
                FileStream myFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    response.Clear();
                    response.AddHeader("Accept-Ranges", "bytes");
                    response.Buffer = false;
                    double fileLength = myFile.Length;
                    long startBytes = 0;
                    const int pack = 10240; //10K bytes
                    int sleep = (int)Math.Floor(1000 * pack / speed) + 1;     //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    if (request.Headers["Range"] != null)
                    {
                        response.StatusCode = 206;
                        string[] range = request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    response.AddHeader("Connection", "Keep-Alive");
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;
                    for (int i = 0; i < maxCount; i++)
                    {
                        if (response.IsClientConnected)
                        {
                            response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        //// <summary>
        /// 下载文件，支持大文件、续传、速度限制。支持续传的响应头Accept-Ranges、ETag，请求头Range 。
        /// Accept-Ranges：响应头，向客户端指明，此进程支持可恢复下载.实现后台智能传输服务（BITS），值为：bytes；
        /// ETag：响应头，用于对客户端的初始（200）响应，以及来自客户端的恢复请求，
        /// 必须为每个文件提供一个唯一的ETag值（可由文件名和文件最后被修改的日期组成），这使客户端软件能够验证它们已经下载的字节块是否仍然是最新的。
        /// Range：续传的起始位置，即已经下载到客户端的字节数，值如：bytes=1474560- 。
        /// 另外：UrlEncode编码后会把文件名中的空格转换中+（+转换为%2b），但是浏览器是不能理解加号为空格的，所以在浏览器下载得到的文件，空格就变成了加号；
        /// 解决办法：UrlEncode 之后, 将 "+" 替换成 "%20"，因为浏览器将%20转换为空格
        /// </summary>
        /// <param name="httpContext">当前请求的HttpContext</param>
        /// <param name="filePath">下载文件的物理路径，含路径、文件名</param>
        /// <param name="speed">下载速度：每秒允许下载的字节数</param>
        /// <returns>true下载成功，false下载失败</returns>
        public static bool DownloadFile(HttpContext httpContext, string filePath, long speed)
        {
            httpContext.Response.Clear();
            bool ret = true;
            try
            {
                //验证：HttpMethod，请求的文件是否存在#region#region //验证：HttpMethod，请求的文件是否存在#region
                switch (httpContext.Request.HttpMethod.ToUpper())
                { //目前只支持GET和HEAD方法
                    case "GET":
                    case "HEAD":
                        break;
                    default:
                        httpContext.Response.StatusCode = 501;
                        return false;
                }
                if (!File.Exists(filePath))
                {
                    httpContext.Response.StatusCode = 404;
                    return false;
                }

                //定义局部变量#region 定义局部变量#region 定义局部变量#region 定义局部变量
                long startBytes = 0;
                long stopBytes = 0;
                int packSize = 1024 * 10; //分块读取，每块10K bytes
                string fileName = Path.GetFileName(filePath);
                FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                long fileLength = myFile.Length;

                int sleep = (int)Math.Ceiling(1000.0 * packSize / speed);//毫秒数：读取下一数据块的时间间隔
                string lastUpdateTiemStr = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string eTag = HttpUtility.UrlEncode(fileName, Encoding.UTF8) + lastUpdateTiemStr;//便于恢复下载时提取请求头;
                //验证：文件是否太大，是否是续传，且在上次被请求的日期之后是否被修改过#region //验证：文件是否太大，是否是续传，且在上次被请求的日期之后是否被修改过
                if (myFile.Length > long.MaxValue)
                {////////-文件太大了//////-
                    httpContext.Response.StatusCode = 413;//请求实体太大
                    return false;
                }

                if (httpContext.Request.Headers["If-Range"] != null)//对应响应头ETag：文件名+文件最后修改时间
                {
                    ////////////上次被请求的日期之后被修改过//////////////
                    if (httpContext.Request.Headers["If-Range"].Replace("\"", "") != eTag)
                    {//文件修改过
                        httpContext.Response.StatusCode = 412;//预处理失败
                        return false;
                    }
                }
                try
                {
                    //////-添加重要响应头、解析请求头、相关验证#region //////-添加重要响应头、解析请求头、相关验证
                    httpContext.Response.Clear();

                    if (httpContext.Request.Headers["Range"] != null)
                    {////////如果是续传请求，则获取续传的起始位置，即已经下载到客户端的字节数//////
                        httpContext.Response.StatusCode = 206;//重要：续传必须，表示局部范围响应。初始下载时默认为200
                        string[] range = httpContext.Request.Headers["Range"].Split(new char[] { '=', '-' });//"bytes=1474560-"
                        startBytes = Convert.ToInt64(range[1]);//已经下载的字节数，即本次下载的开始位置  
                        if (startBytes < 0 || startBytes >= fileLength)
                        {//无效的起始位置
                            return false;
                        }
                        if (range.Length == 3)
                        {
                            stopBytes = Convert.ToInt64(range[2]);//结束下载的字节数，即本次下载的结束位置  
                            if (startBytes < 0 || startBytes >= fileLength)
                            {
                                return false;
                            }
                        }
                    }

                    httpContext.Response.Buffer = false;
                    httpContext.Response.AddHeader("Accept-Ranges", "bytes");//重要：续传必须
                    httpContext.Response.AppendHeader("ETag", String.Format("\"{0}\"", eTag));//重要：续传必须
                    httpContext.Response.AppendHeader("Last-Modified", lastUpdateTiemStr);//把最后修改日期写入响应                
                    httpContext.Response.ContentType = "application/octet-stream";//MIME类型：匹配任意文件类型
                    httpContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("+", "%20"));
                    httpContext.Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    httpContext.Response.AddHeader("Connection", "Keep-Alive");
                    httpContext.Response.ContentEncoding = Encoding.UTF8;
                    if (startBytes > 0)
                    {////////如果是续传请求，告诉客户端本次的开始字节数，总长度，以便客户端将续传数据追加到startBytes位置后//////////
                        httpContext.Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    //////-向客户端发送数据块//////////////////-#region //////-向客户端发送数据块//////////////////-
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Ceiling((fileLength - startBytes + 0.0) / packSize);//分块下载，剩余部分可分成的块数
                    for (int i = 0; i < maxCount && httpContext.Response.IsClientConnected; i++)
                    {//客户端中断连接，则暂停
                        httpContext.Response.BinaryWrite(br.ReadBytes(packSize));
                        httpContext.Response.Flush();
                        if (sleep > 1) Thread.Sleep(sleep);
                    }
                }
                catch
                {
                    ret = false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
    }
}
