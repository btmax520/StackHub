using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using StackHub.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Helper;
using Web.Models;

namespace Web.Controllers
{
    public class PackageController : BaseController
    {
        public ActionResult Index(string s)
        {
            var search = new Search();
            if (string.IsNullOrEmpty(s))
            {

            }
            else
            {
                search = JsonConvert.DeserializeObject<Search>(s);
            }

            var packages = new List<Package>();
            //using (var db = new DataBaseContext())
            //{
            //    var query = (from j in db.Job
            //                join c in db.Company on j.CompanyID equals c.ID
            //                select new
            //                 {
            //                     ID = j.ID,
            //                     Position = j.Position,
            //                     Area = j.Area,
            //                     ModifyOn = j.ModifyOn,
            //                     Salary = j.Salary,
            //                     Kind = j.Kind,
            //                     Experience = j.Experience,
            //                     Education = j.Education,
            //                     Skill = j.Skill,
            //                     CompanyName = c.Name,
            //                     CompanyType = c.Type,
            //                     CompanySize = c.Size
            //                 }).AsQueryable();
            //    if (!string.IsNullOrEmpty(search.KeyWords))
            //    {
            //        query = query.Where(j => j.Position.Contains(search.KeyWords) || j.CompanyName.Contains(search.KeyWords));
            //    }
            //    if (search.QueryFilters != null && search.QueryFilters.Count > 0)
            //    {
            //        foreach (var filter in search.QueryFilters)
            //        {
            //            switch (filter.Key)
            //            {
            //                case "薪水范围":
            //                    query = query.Where(j => filter.Value.Contains(j.Salary));
            //                    break;
            //                case "工作地点":
            //                    query = query.Where(j => filter.Value.Contains(j.Area));
            //                    break;
            //                case "企业性质":
            //                    query = query.Where(j => filter.Value.Contains(j.CompanyType));
            //                    break;
            //                case "工作经验":
            //                    query = query.Where(j => filter.Value.Contains(j.Experience));
            //                    break;
            //                case "学历":
            //                    query = query.Where(j => filter.Value.Contains(j.Education));
            //                    break;
            //                case "工作性质":
            //                    query = query.Where(j => filter.Value.Contains(j.Kind));
            //                    break;
            //                case "企业规模":
            //                    query = query.Where(j => filter.Value.Contains(j.CompanySize));
            //                    break;
            //            }

            //        }
            //    }
            //    var results = query.Take(10).ToList();
            //    foreach (var job in results)
            //    {
            //        jobs.Add(
            //            new Job()
            //            {
            //                ID = job.ID,
            //                Position = job.Position,
            //                Area = job.Area,
            //                ModifyOn = job.ModifyOn,
            //                Salary = job.Salary,
            //                Kind = job.Kind,
            //                Education = job.Education,
            //                Skill = job.Skill,
            //                CompanyName = job.CompanyName,
            //                CompanyType = job.CompanyType,
            //                CompanySize = job.CompanySize
            //            });
            //    }

            //}
            ViewBag.Packages = packages;
            //ViewBag.Dict = GetDict();
            //var dict = new Dictionary<string, string>();
            //dict.Add("Salary", "2000以下");
           
            
            return View();
        }

    }
}