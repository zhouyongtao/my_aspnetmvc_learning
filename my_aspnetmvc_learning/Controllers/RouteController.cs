using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace my_aspnetmvc_learning.Controllers
{
    /// <summary>
    /// http://diaosbook.com/Post/2013/10/22/attribute-routing-in-asp-net-mvc-5
    /// </summary>
    public class RouteController : Controller
    {
        //
        // GET: /Route/
        public ActionResult Index()
        {
            return View();
        }
        // eg: /books
        // eg: /books/1430210079
        [Route("books/{isbn?}")]
        public ActionResult View(string isbn)
        {
            return View();
        }

        // eg: /books/lang
        // eg: /books/lang/en
        // eg: /books/lang/he
        [Route("books/lang/{lang=en}")]
        public ActionResult ViewByLanguage(string lang)
        {
            return View();
        }

        [Route("attribute-routing-in-asp-net-mvc-5")]
        public ActionResult Hotel()
        {
            return Content("http://diaosbook.com/Post/2013/10/22/attribute-routing-in-asp-net-mvc-5");
        }
    }
}