﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class JobController : BaseController
    {
        // GET: Job
        public ActionResult Index()
        {
            SetTopActive("job");
            return View();
        }
    }
}