using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Newtonsoft.Json.Linq;
using StackHub.Core.Service;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class BaseController : Controller
    {

        protected void SetTopActive(string id)
        {
            if (ViewBag.Catetories != null && ViewBag.Catetories.Count > 0)
            {
                foreach (var category in ViewBag.Catetories)
                {
                    if (category.id == id)
                    {
                        category.active = true;
                    }
                    else
                    {
                        category.active = false;
                    }
                }
            }
        }

        protected void SetLeftActive(string id)
        {
            if (ViewBag.Catetories != null && ViewBag.Catetories.Count > 0)
            {
                foreach (var category in ViewBag.Catetories)
                {
                    if (category.subject != null && category.subject.Count > 0)
                    { 
                        foreach (var sub in category.subject)
                        {
                            if (sub.id == id)
                            {
                                sub.active = true;
                            }
                            else
                            {
                                sub.active = false;
                            }
                        }
                    }
                }
            }
        }

        private void SetCategory()
        {
            using (StreamReader reader = System.IO.File.OpenText(@"~\App_Data\category.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }
        }

        private JArray GetCategory()
        {
            JObject result = null;
            string path = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/category.json");
            using (StreamReader reader = System.IO.File.OpenText(path))
            {
                result = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }
            
            return (JArray)result["categories"];
        }

        public BaseController()
        {
            ViewBag.Catetories = GetCategory();
        } 
    }
}