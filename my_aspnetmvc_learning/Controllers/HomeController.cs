using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Detail(string id)
        {
            Guid sId = Guid.Empty;
            if (!Guid.TryParse(id, out sId))
            {
                return new HttpNotFoundResult();
            }
            else
            {
                return View();
            }
        }
    }
}