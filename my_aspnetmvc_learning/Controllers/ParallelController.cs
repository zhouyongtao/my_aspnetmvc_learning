using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    public class ParallelController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // GET: Parallel
        public ActionResult Index()
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
            //指定使用的硬件线程数为1
            Parallel.For(0, 10000, options, (i, state) =>
                    {
                        if (i == 100)
                        {
                            state.Break();
                            return;
                        }
                    });
            return View();
        }
        public ActionResult dict()
        {
            var dictParallelDays = new ConcurrentDictionary<string, string>();
            try
            {
                System.Threading.Tasks.Parallel.For(0, 35, i => dictParallelDays.TryAdd(DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"), DateTime.Now.AddDays(i + 1).ToString("yyyy-MM-dd")));
            }
            catch (AggregateException ex)
            {
                foreach (var single in ex.InnerExceptions)
                {
                    //logger.Error("HanTingServiceJob AggregateException: " + ex.Message);
                }
            }
            var dictDays = dictParallelDays.AsParallel().OrderBy((data => data.Key));
            dictDays.ForEach(data =>
            {
                string key = data.Key;
                string value = data.Value;
            });
            //Parallel
            Parallel.ForEach(dictParallelDays, item =>
            {
                Logger.Info("key: " + item.Key + "  value : " + item.Value);
            });
            return Content(dictDays.Count().ToString());
        }
    }
}