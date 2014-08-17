using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    public class AsyncController : Controller
    {
        // GET: Async
        [AsyncTimeout(1000)]
        public async Task<ActionResult> Index()
        {
            var data = await GetPageTaskAsync("http://163.com");
            string otherData = "Irving";
            return data;
        }

        /// <summary>
        /// 处理异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPageTaskAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    await Task.Delay(3000);
                    var fetchTextTask = client.GetStringAsync(url);
                    return Json(new
                    {
                        fetchText = await fetchTextTask,
                        error = "NO"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (WebException exception)
            {
                throw exception;
                // TODO: Logging, update statistics etc.
            }
        }
    }
}