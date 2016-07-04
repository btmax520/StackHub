using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ResumeBasic> ResumeBasic { get; set; }
        public DbSet<ResumeProject> ResumeProject { get; set; }
        public DbSet<ResumeWork> ResumeWork { get; set; }
        public DbSet<ResumeEducation> ResumeEducation { get; set; }
        public DbSet<ResumeSkill> ResumeSkill { get; set; }


        public DbSet<UIOption> UIOption { get; set; }
    }

    public class BaseModel
    {
        [Key]
        public long ID { get; set; }
    }

    public class UIOption : BaseModel
    {
        public string Category { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        [NotMapped]
        public string Selected { get; set; }
    }

    public class User : BaseModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public bool Remember { get; set; }

        public int Repeat { get; set; }
        public string Token { get; set; }
    }

    public class Package : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public string Period { get; set; }
        public string Skill { get; set; }
        public string Document { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }


    public class Company : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Industry { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public long OwnerID { get; set; }
        //public User Owner { get; set; }
        //public virtual List<Job> Jobs { get; set; }
    }

    public class ResumeBasic : BaseModel
    {
        public string Name { get; set; }
        public string Sex { get; set; }
        public string School { get; set; }
        public string Birthday { get; set; }
        public string Experience { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Salary1 { get; set; }
        public string Salary2 { get; set; }
        public string Work { get; set; }
        public string Kind { get; set; }
        public string Education { get; set; }
        public List<string> Area { get; set; }
        public string Description { get; set; }
        public long PublisherID { get; set; }

        public DateTime ModifyOn { get; set; }
    }

    public class ResumeProject : BaseModel
    {
        public string Name { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Descript { get; set; }

        public long PublisherID { get; set; }
        public DateTime ModifyOn { get; set; }
    }

    public class ResumeWork : BaseModel
    {
        public string Begin { get; set; }
        public string End { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Descript { get; set; }

        public long PublisherID { get; set; }
        public DateTime ModifyOn { get; set; }
    }

    public class ResumeEducation : BaseModel
    {
        public string School { get; set; }
        public string Major { get; set; }
        public string End { get; set; }
        public string Degree { get; set; }

        public long PublisherID { get; set; }
        public DateTime ModifyOn { get; set; }
    }

    public class ResumeSkill : BaseModel
    {
        public string Name { get; set; }
        public string Rating { get; set; }

        public long PublisherID { get; set; }
        public DateTime ModifyOn { get; set; }
    }


    public class Job : BaseModel
    {
        public string Position { get; set; }
        public string Salary { get; set; }
        public string Area { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; }
        public string Kind { get; set; }
        public string Skill { get; set; }
        public string Description { get; set; }

        public long CompanyID { get; set; }
        public long PublisherID { get; set; }

        public DateTime ModifyOn { get; set; }

        [NotMapped]
        public Dictionary<string, string> Dict { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string CompanyType { get; set; }
        [NotMapped]
        public string CompanySize { get; set; }
    }

    public class UIFilter
    {
        public string Category { get; set; }
        public string Value { get; set; }
    }

    public class Search
    {
        public string KeyWords { get; set; }
        public int CurrentPage { get; set; }
        public Dictionary<string,string[]> QueryFilters { get;set;}
    }
}