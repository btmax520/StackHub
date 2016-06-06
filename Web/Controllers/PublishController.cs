using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class PublishController : Controller
    {
        // GET: Subpackage
        public ActionResult Subpackage()
        {
            ViewBag.Title = "发布分包";
            ViewBag.ShowTop = true;
            ViewBag.Type = "subpackage";
            return View();
        }
    }
}