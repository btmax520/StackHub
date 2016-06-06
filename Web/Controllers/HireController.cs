using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HireController : BaseController
    {
        // GET: Hire
        public ActionResult Index()
        {
            SetTopActive("hire");
            //SetLeftActive("resume");
            return View();
        }
    }
}