using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    public class TaskController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Task
        public ActionResult Index()
        {
            // Get a folder path whose directories should throw an UnauthorizedAccessException. 
            string path = Directory.GetParent(
                                    Environment.GetFolderPath(
                                    Environment.SpecialFolder.UserProfile)).FullName;

            // Use this line to throw UnauthorizedAccessException, which we handle.
            Task<string[]> task1 = Task<string[]>.Factory.StartNew(() => GetAllFiles(path));
            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        logger.Info("You do not have permission to access all folders in this path.");
                        logger.Info("See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });
            }
            logger.Info("task1 Status: {0}{1}", task1.IsCompleted ? "Completed," : "", task1.Status);
            return View();
        }

        public static string[] GetAllFiles(string str)
        {
            // Should throw an UnauthorizedAccessException exception. 
            return Directory.GetFiles(str, "*.txt", SearchOption.AllDirectories);
        }


        public static string Test()
        {
            return string.Empty;
        }
    }
}