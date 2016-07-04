using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using Newtonsoft.Json.Linq;
using StackHub.Core.Service;
using Newtonsoft.Json;
using System.Web.Mvc.Filters;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        private static Dictionary<string, string> dict = null;

        private User currentUser = null;

        public User CurrentUser
        {
            get
            {
                if (currentUser != null)
                    return currentUser;
                if (User.Identity.IsAuthenticated)
                {
                    string email = User.Identity.Name;
                    using (var db = new DataBaseContext())
                    {
                        currentUser = db.User.SingleOrDefault(u => u.Email == email);
                    }
                    return currentUser;
                }
                else
                    return null;
            }
        }

        public PartialViewResult Select(string category,string setting,List<string> selected)
        {
            List<UIOption> options = new List<UIOption>();
            using (var db = new DataBaseContext())
            {
                options = db.UIOption.Where(q => q.Category == category).ToList();
            }
            if (options == null)
            {
                options = new List<UIOption>();
            }
            else
            {
                foreach (UIOption option in options)
                {
                    if (selected.Contains(option.Key))
                    {
                        option.Selected = "selected";
                    }
                }
            }
            ViewBag.Options = options;
            ViewBag.Setting = setting;
            return PartialView();
        }

        public PartialViewResult Pagination()
        {
            return PartialView();
        }

        public PartialViewResult Filter(List<string> categories)
        {
            var filters = new Dictionary<string, List<UIOption>>();
            var options = new List<UIOption>();
            using (var db = new DataBaseContext())
            {
                
                if (categories == null)
                {
                    options = db.UIOption.ToList();
                }
                else
                {
                    options = db.UIOption.Where(q => categories.Contains(q.Category)).ToList();
                }
            }
            var search = new Search();
            if (!string.IsNullOrEmpty(Request.Params["s"])) { 
                search = JsonConvert.DeserializeObject<Search>(Request.Params["s"]);
                
            }
            foreach (UIOption option in options)
            {
                if (search.QueryFilters != null && search.QueryFilters.ContainsKey(option.Category) && search.QueryFilters[option.Category].Contains(option.Key))
                {
                    option.Selected = "selected";
                }
                if (filters.ContainsKey(option.Category))
                {
                    filters[option.Category].Add(option);
                }
                else
                {
                    filters[option.Category] = new List<UIOption>();
                    filters[option.Category].Add(option);
                }
            }
            ViewBag.Filters = filters;
            return PartialView();
        }

        protected Dictionary<string,string> GetDict(params string[] keys)
        {
            if (dict == null)
            {
                using (var db = new DataBaseContext())
                {
                    List<UIOption> options = db.UIOption.ToList();
                    dict = new Dictionary<string, string>();
                    foreach (var option in options)
                    {
                        string key = option.Category + ":" + option.Key;
                        string value = option.Value;
                        dict.Add(key, value);
                    }
                }
            }
            return dict;
        }

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