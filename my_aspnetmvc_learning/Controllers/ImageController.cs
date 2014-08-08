using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    public class ImageController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Upload()
        {
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null)
                {
                    file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "upload/" + file.FileName);
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}