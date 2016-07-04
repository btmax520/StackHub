using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackHub.Core.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class PublishController : BaseController
    {
        [Authorize]
        public ActionResult Company(Company company)
        {
            if (company == null || string.IsNullOrEmpty(company.Name))
            {
                if (CurrentUser != null)
                {
                    using (var db = new DataBaseContext())
                    {
                        company = db.Company.SingleOrDefault(c => c.OwnerID == CurrentUser.ID);
                        
                    }
                    if (company == null)
                    {
                        company = new Company();
                        company.Phone = CurrentUser.Phone;
                        company.Email = CurrentUser.Email;
                    }
                    ViewBag.Company = company;
                }
            }
            else
            {
                using (var db = new DataBaseContext())
                {
                    company.OwnerID = CurrentUser.ID;
                    db.Company.Add(company);
                    db.SaveChanges();
                    ViewBag.Company = company;
                    string returnUrl = Request.Params["ReturnUrl"];
                    if (string.IsNullOrEmpty(returnUrl))
                        return Redirect("~/");
                    else
                        return Redirect(returnUrl);
                }
            }
            return View();
        }


        // GET: Subpackage
        public ActionResult Subpackage()
        {
            ViewBag.Title = "发布分包";
            ViewBag.ShowTop = true;
            ViewBag.Type = "subpackage";
            return View();
            
        }

        public ActionResult Package(string id)
        {
            ViewBag.Package = new Package();
            return View();
        }

        [HttpPost]
        public ActionResult Package(Package package)
        {
            return View();
        }

        [Authorize]
        public ActionResult Job(string id)
        {
            Company company = null;
            using (var db = new DataBaseContext())
            {
                company = db.Company.SingleOrDefault(c => c.OwnerID == CurrentUser.ID);
                ViewBag.CurrentCompany = company;
                if (company == null)
                {
                    return Redirect("~/publish/company?ReturnUrl=~/publish/job");
                }

                var recentJobs = db.Job.OrderByDescending(j => j.ModifyOn).Where(j => j.CompanyID == company.ID).Take(20).ToList();
                ViewBag.RecentJobs = recentJobs ?? new List<Job>();

                //var recentResumes = db.Job.OrderByDescending(j => j.ModifyOn).Where(j => j.CompanyID == company.ID).Take(20).ToList();
                //ViewBag.RecentJobs = recentJobs ?? new List<Job>();


                if (string.IsNullOrEmpty(id))
                {
                    var currentJob = new Job();
                    currentJob.ModifyOn = DateTime.Now;
                    currentJob.CompanyID = company.ID;
                    currentJob.PublisherID = CurrentUser.ID;
                    ViewBag.CurrentJob = currentJob;
                    return View();
                }
                var job = db.Job.Find(long.Parse(id));
                if (job == null)
                {
                    return new HttpNotFoundResult();
                }
                if (job.CompanyID != company.ID)
                {

                }
                else
                {
                    ViewBag.CurrentJob = job;
                }
                
            }
            return View();
        }

        [HttpPost]
        public ActionResult Job(string id,Job job)
        {
            if (job.CompanyID == 0)
            {
                return Redirect("~/publish/company?ReturnUrl=~/publish/job");
            }
            job.Area = Request["area"];
            using (var db = new DataBaseContext())
            {
                var company = db.Company.SingleOrDefault(c => c.OwnerID == CurrentUser.ID);
                job.CompanyID = company.ID;
                job.PublisherID = CurrentUser.ID;
                job.ModifyOn = DateTime.Now;
                if (job.ID == 0)
                {
                    db.Job.Add(job);
                }
                else
                {
                    db.Entry(job).State = EntityState.Modified;
                    company.Industry = Request["companyIndustry"];
                    company.Size = Request["companySize"];
                    company.Type = Request["companyType"];
                }
                db.SaveChanges();
                //ViewBag.CurrentJob = job;
                //ViewBag.CurrentCompany = company;
                //var recentJobs = new List<Job>();
                //ViewBag.RecentJobs = recentJobs;
                //db.Job.OrderByDescending(j => j.ModifyOn).Where(j => j.CompanyID == company.ID).Take(20);
            }
            return Redirect("~/publish/job/" + job.ID);
        }

        [Authorize]
        public ActionResult Resume()
        {
            using (var db = new DataBaseContext())
            {
                var resume = db.ResumeBasic.SingleOrDefault(r => r.PublisherID == CurrentUser.ID);
                ViewBag.Basic = resume ?? new ResumeBasic {
                    Name = CurrentUser.Name,
                    Phone = CurrentUser.Phone,
                    Email = CurrentUser.Email
                };

                ViewBag.Project = new ResumeProject();
                var projects = db.ResumeProject.OrderByDescending(p => p.End)
                    .Where(p => p.PublisherID == CurrentUser.ID).ToList();
                ViewBag.Projects = projects ?? new List<ResumeProject>();

                ViewBag.Work = new ResumeWork();
                var works = db.ResumeWork.OrderByDescending(w => w.End)
                    .Where(w => w.PublisherID == CurrentUser.ID).ToList();
                ViewBag.Works = works ?? new List<ResumeWork>();

                ViewBag.Education = new ResumeEducation();
                var educations = db.ResumeEducation.OrderByDescending(w => w.End)
                    .Where(w => w.PublisherID == CurrentUser.ID).ToList();
                ViewBag.Educations = educations ?? new List<ResumeEducation>();
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Resume(ResumeBasic basic)
        {
            basic.PublisherID = CurrentUser.ID;
            basic.ModifyOn = DateTime.Now;
            using (var db = new DataBaseContext())
            {
                if (basic.ID == 0)
                {
                    db.ResumeBasic.Add(basic);
                }
                else
                {
                    db.ResumeBasic.Attach(basic);
                    db.Entry(basic).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return this.Resume();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResumeProject(ResumeProject project)
        {
            project.PublisherID = CurrentUser.ID;
            project.ModifyOn = DateTime.Now;
            
            using (var db = new DataBaseContext())
            {
                if (project.ID == 0)
                {
                    db.ResumeProject.Add(project);
                }
                else
                {
                    db.ResumeProject.Attach(project);
                    db.Entry(project).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return Redirect("~/publish/resume");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResumeWork(ResumeWork work)
        {
            work.PublisherID = CurrentUser.ID;
            work.ModifyOn = DateTime.Now;

            using (var db = new DataBaseContext())
            {
                if (work.ID == 0)
                {
                    db.ResumeWork.Add(work);
                }
                else
                {
                    db.ResumeWork.Attach(work);
                    db.Entry(work).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return Redirect("~/publish/resume");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResumeEducation(ResumeEducation education)
        {
            education.PublisherID = CurrentUser.ID;
            education.ModifyOn = DateTime.Now;

            using (var db = new DataBaseContext())
            {
                if (education.ID == 0)
                {
                    db.ResumeEducation.Add(education);
                }
                else
                {
                    db.ResumeEducation.Attach(education);
                    db.Entry(education).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            return Redirect("~/publish/resume");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResumeDescription(string description)
        {
            using (var db = new DataBaseContext())
            {
                var resume = db.ResumeBasic.SingleOrDefault(r => r.PublisherID == CurrentUser.ID);
                resume.Description = description;
                db.SaveChanges();
            }
            return Redirect("~/publish/resume");
        }

    }
}