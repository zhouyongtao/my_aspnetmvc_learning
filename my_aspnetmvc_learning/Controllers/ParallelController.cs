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
        // GET: Parallel
        public ActionResult Index()
        {
            var options = new ParallelOptions {MaxDegreeOfParallelism = 1};
            //指定使用的硬件线程数为1
            Parallel.For(0, 20000000, options,(i,state) =>
                    {
                        if (i == 1000)
                        {
                            state.Break();
                            return;
                        }
                    });
            return View();
        }
    }
}