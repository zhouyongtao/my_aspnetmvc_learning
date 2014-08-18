using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Homeinns.Common.Base
{
    public class ShareUtil
    {
        public static bool connectState(string path)
        {
            return connectState(path, "", "");
        }

        public static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                // string dosLine = String.Format(@"net use {0} /PERSISTENT:YES", path);
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }


        /// <summary>
        /// 读取文本内容
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string ReadFiles(string path)
        {
            String text = string.Empty;
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(path))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    text = sr.ReadToEnd();
                }
                return text;
            }
            catch (Exception ex)
            {
                return "The file could not be read: " + ex.Message;
            }
        }

        /// <summary>
        /// 读取文本内容
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string ReadFiles_GB2312(string path)
        {
            String text = string.Empty;
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("GB2312")))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    text = sr.ReadToEnd();
                }
                return text;
            }
            catch (Exception ex)
            {
                return "The file could not be read: " + ex.Message;
            }
        }

        //write file
        public static void WriteFiles(string path)
        {
            try
            {
                // Create an instance of StreamWriter to write text to a file.
                // The using statement also closes the StreamWriter.
                using (StreamWriter sw = new StreamWriter(path))
                {
                    // Add some text to the file.
                    sw.Write("This is the ");
                    sw.WriteLine("header for the file.");
                    sw.WriteLine("-------------------");
                    // Arbitrary objects can also be written to the file.
                    sw.Write("The date is: ");
                    sw.WriteLine(DateTime.Now);
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 拷贝文件夹下所有文件
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="destdir"></param>
        /// <param name="recursive"></param>
        public static void FileCopy(string srcdir, string destdir, bool recursive)
        {
            DirectoryInfo dir = new DirectoryInfo(srcdir);
            //获取源地址所有文件   
            FileSystemInfo[] fsis = dir.GetFileSystemInfos();
            for (int i = 0; i < fsis.Length; i++)
            {
                FileSystemInfo fsi = fsis[i];
                string tmppath = Path.Combine(destdir, fsi.Name);
                if (fsi is FileInfo)
                {
                    if (fsi.Name.ToLower() != "web.config")
                        (fsi as FileInfo).CopyTo(tmppath, true);
                }
                else if (fsi is DirectoryInfo)
                {
                    if (!Directory.Exists(tmppath))
                        Directory.CreateDirectory(tmppath);
                    if (!recursive)
                        continue;
                    FileCopy(fsi.FullName, tmppath, recursive);
                }
            }
        }
    }
}
