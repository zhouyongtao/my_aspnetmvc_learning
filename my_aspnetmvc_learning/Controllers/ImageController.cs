using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace my_aspnetmvc_learning.Controllers
{
    public class ImageController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            //检查文件夹
            string pathDir = Server.MapPath("~/upload");
            if (System.IO.File.Exists(pathDir))
            {
                System.IO.Directory.CreateDirectory(pathDir);
            }
            //保存文件
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file.IsNotNull())
                {
                    logger.Info(string.Format("FileName : {0}", file.FileName));
                    file.SaveAs(filename: AppDomain.CurrentDomain.BaseDirectory + "upload/" + file.FileName);
                }
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}