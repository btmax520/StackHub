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
    public class JobController : BaseController
    {
        public ResumeBasic Resume
        {
            get; set;
        }

        private List<ResumeProject> GetProjects()
        {
            List<ResumeProject> projects = new List<ResumeProject>();
            string key = "resume:projects:" + CurrentUser.ID;
            string value = RedisSercice.DB.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                projects = JsonConvert.DeserializeObject<List<ResumeProject>>(value);
            }
            return projects;
        }

        //public ActionResult Index(dynamic filter)
        //{
        //    filter.option[""]
        //    var jobs = new List<Job>();
        //    using (var db = new DataBaseContext())
        //    {
        //        var query = from j in db.Job
        //                    join c in db.Company on j.CompanyID equals c.ID
        //                    select new
        //                    {
        //                        ID = j.ID,
        //                        Position = j.Position,
        //                        Area = j.Area,
        //                        ModifyOn = j.ModifyOn,
        //                        Salary = j.Salary,
        //                        Experience = j.Experience,
        //                        Education = j.Education,
        //                        Skill = j.Skill,
        //                        CompanyName = c.Name,
        //                        CompanyType = c.Type,
        //                        CompanySize = c.Size
        //                    };

        //        var results = query.Take(10).ToList();
        //        foreach (var job in results)
        //        {
        //            jobs.Add(
        //                new Job()
        //                {
        //                    ID = job.ID,
        //                    Position = job.Position,
        //                    Area = job.Area,
        //                    ModifyOn = job.ModifyOn,
        //                    Salary = job.Salary,
        //                    Experience = job.Experience,
        //                    Education = job.Education,
        //                    Skill = job.Skill,
        //                    CompanyName = job.CompanyName,
        //                    CompanyType = job.CompanyType,
        //                    CompanySize = job.CompanySize
        //                });
        //        }

        //    }
        //    ViewBag.Jobs = jobs;
        //    ViewBag.Dict = GetDict();
        //    //var dict = new Dictionary<string, string>();
        //    //dict.Add("Salary", "2000以下");


        //    return View();
        //}

        // GET: Job
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
            
            //search.KeyWords = "镕诚集团";
            //search.QueryFilters = new Dictionary<string, List<string>>();
            //search.QueryFilters.Add("薪水范围", new List<string> {
            //    "2000以下","2001-3000"
            //});
            //search.QueryFilters.Add("学历", new List<string> {
            //    "不限","不限"
            //});
            var jobs = new List<Job>();
            using (var db = new DataBaseContext())
            {
                var query = (from j in db.Job
                            join c in db.Company on j.CompanyID equals c.ID
                            select new
                             {
                                 ID = j.ID,
                                 Position = j.Position,
                                 Area = j.Area,
                                 ModifyOn = j.ModifyOn,
                                 Salary = j.Salary,
                                 Kind = j.Kind,
                                 Experience = j.Experience,
                                 Education = j.Education,
                                 Skill = j.Skill,
                                 CompanyName = c.Name,
                                 CompanyType = c.Type,
                                 CompanySize = c.Size
                             }).AsQueryable();

                //var list = db.Job.SqlQuery("select * from jobs where id = 4").ToList();

                //var query1 = db.Job.AsQueryable();
                //query = query.Where(c => c.ID == 3);
                //query = query.Where(c => c.Kind == "2");
                //var list1 = query.ToList();
                //foreach (dynamic item in list1)
                //{
                //    var s = item.ID;
                //}

                if (!string.IsNullOrEmpty(search.KeyWords))
                {
                    query = query.Where(j => j.Position.Contains(search.KeyWords) || j.CompanyName.Contains(search.KeyWords));
                }
                if (search.QueryFilters != null && search.QueryFilters.Count > 0)
                {
                    foreach (var filter in search.QueryFilters)
                    {
                        switch (filter.Key)
                        {
                            case "薪水范围":
                                query = query.Where(j => filter.Value.Contains(j.Salary));
                                break;
                            case "工作地点":
                                query = query.Where(j => filter.Value.Contains(j.Area));
                                break;
                            case "企业性质":
                                query = query.Where(j => filter.Value.Contains(j.CompanyType));
                                break;
                            case "工作经验":
                                query = query.Where(j => filter.Value.Contains(j.Experience));
                                break;
                            case "学历":
                                query = query.Where(j => filter.Value.Contains(j.Education));
                                break;
                            case "工作性质":
                                query = query.Where(j => filter.Value.Contains(j.Kind));
                                break;
                            case "企业规模":
                                query = query.Where(j => filter.Value.Contains(j.CompanySize));
                                break;
                        }

                    }
                }
                var results = query.Take(10).ToList();
                foreach (var job in results)
                {
                    jobs.Add(
                        new Job()
                        {
                            ID = job.ID,
                            Position = job.Position,
                            Area = job.Area,
                            ModifyOn = job.ModifyOn,
                            Salary = job.Salary,
                            Kind = job.Kind,
                            Education = job.Education,
                            Skill = job.Skill,
                            CompanyName = job.CompanyName,
                            CompanyType = job.CompanyType,
                            CompanySize = job.CompanySize
                        });
                }

            }
            ViewBag.Jobs = jobs;
            ViewBag.Dict = GetDict();
            //var dict = new Dictionary<string, string>();
            //dict.Add("Salary", "2000以下");
           
            
            return View();
        }

        public ActionResult Detail(string id)
        {
            long jobID = 0;
            long.TryParse(id, out jobID);
            if (jobID == 0)
            {
                return Redirect("~/job");
            }
            using (var db = new DataBaseContext())
            {
                var job = db.Job.SingleOrDefault(c => c.ID == jobID);
                if (job == null)
                    return new HttpNotFoundResult();
                ViewBag.Job = job;
                var company = db.Company.SingleOrDefault(c => c.ID == job.CompanyID);
                ViewBag.Company = company;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Basic(ResumeBasic basic)
        {
            basic.ID = CurrentUser.ID;
            string key = "resume:basic:" + basic.ID;
            string value = JsonConvert.SerializeObject(basic);
            RedisSercice.DB.StringSet(key, value);

            JsonResult result = new JsonResult();
            result.Data = basic;
            
            return result;
        }

        [HttpPost]
        public ActionResult AddProject(ResumeProject project)
        {
            project.ID = RedisSercice.GetSequence();

            string key = "resume:projects:" + CurrentUser.ID;
            var projects = GetProjects();
            projects.Add(project);
            string value = JsonConvert.SerializeObject(projects);
            RedisSercice.DB.StringSet(key, value);
            JsonResult result = new JsonResult();
            result.Data = project;
            return result;
        }

        [HttpPost]
        public ActionResult RemoveProject(long id)
        {
            string key = "resume:projects:" + CurrentUser.ID;
            var projects = GetProjects();
            var saving = new List<ResumeProject>();
            foreach (var item in projects)
            {
                if (item.ID != id)
                {
                    saving.Add(item);
                }
            }
            string value = JsonConvert.SerializeObject(saving);
            RedisSercice.DB.StringSet(key, value);
            JsonResult result = new JsonResult();
            result.Data = id;
            return result;
        }
    }
}