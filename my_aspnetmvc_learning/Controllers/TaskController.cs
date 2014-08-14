using System.Globalization;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    public class TaskController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // GET: Task
        public ActionResult Index()
        {
            // Get a folder path whose directories should throw an UnauthorizedAccessException. 
            string path = Directory.GetParent(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.UserProfile)).FullName;
            // Use this line to throw UnauthorizedAccessException, which we handle.
            var task1 = Task<string[]>.Factory.StartNew(() => GetAllFiles(path));
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
                        Logger.Info("You do not have permission to access all folders in this path.");
                        Logger.Info("See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });
            }
            Logger.Info("task1 Status: {0}{1}", task1.IsCompleted ? "Completed," : "", task1.Status);
            return View();
        }

        public static string[] GetAllFiles(string str)
        {
            // Should throw an UnauthorizedAccessException exception. 
            return Directory.GetFiles(str, "*.txt", SearchOption.AllDirectories);
        }

        public ActionResult Test()
        {
            var watch = Stopwatch.StartNew();
            watch.Start();
            Parallel.Invoke(
                () => Thread.Sleep(5000),
                () => Thread.Sleep(4000));
            return Content(watch.ElapsedMilliseconds.ToString());
        }


        public ActionResult dict()
        {
            var dictDays = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
            //dictSortDays
            var dictSortDays = new SortedDictionary<string, string>();
            try
            {
                System.Threading.Tasks.Parallel.For(0, 35,
                    i =>
                        dictDays.TryAdd(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"),
                            DateTime.Now.AddDays(i + 1).ToString("yyyy-MM-dd")));
                for (int i = 0; i < 35; i++)
                {
                    dictSortDays.Add(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"),
                        DateTime.Now.AddDays(i + 1).ToString("yyyy-MM-dd"));
                }
            }
            catch (AggregateException ex)
            {
                foreach (var single in ex.InnerExceptions)
                {
                    Logger.Error("HanTingServiceJob AggregateException: " + ex.Message + ex.InnerException);
                }
            }
            return Content(dictDays.Count() + " : " + dictSortDays.Count());
        }

        public ActionResult Range()
        {
            int[] nums = Enumerable.Range(0, 1000000).ToArray();
            long total = 0;
            // Use type parameter to make subtotal a long, not an int
            Parallel.For<long>(0, nums.Length, () => 0, (j, loop, subtotal) =>
                {
                    subtotal += nums[j];
                    return subtotal;
                },
                x => Interlocked.Add(ref total, x)
            );
            return Content(total.ToString(CultureInfo.InvariantCulture));
        }
    }
}