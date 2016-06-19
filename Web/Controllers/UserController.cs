using Newtonsoft.Json;
using StackHub.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            ViewBag.ShowLeft = false;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login ,string password)
        {
            string id = string.Empty;
            if (login.Contains("@"))
            {
                id = RedisSercice.DB.StringGet("user:email:" + login);
            }
            else
            {
                id = RedisSercice.DB.StringGet("user:phone:" + login);
            }
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                string value = RedisSercice.DB.StringGet("user:" + id);
                var user = JsonConvert.DeserializeObject<UserModel>(value);
                return Login(user,password);
            }
        }

        private ActionResult Login(UserModel user,string password)
        {
            long id = user.ID;
            var repeat = RedisSercice.DB.StringIncrement("login:repeat:" + id);
            if (repeat > 5)
            {
                ViewBag.Success = false;
                ViewBag.Messages = new List<string>()
                {
                    "错误密码次数已经超过5次"
                };
                return View();
            }
            if (user.Password != password)
            {
                ViewBag.Success = false;
                ViewBag.Messages = new List<string>()
                {
                    string.Format("密码不正确，还可以尝试{0}次", 5 - repeat)
                };
                return View();
            }
            else
            {
                RedisSercice.DB.StringGetSet("login:repeat:" + id, 0);
            }
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
            var oldToken = RedisSercice.DB.StringGetSet("online:" + id, token);
            RedisSercice.DB.KeyDelete("token:" + oldToken);
            RedisSercice.DB.StringSet("token:" + token, id);
            
            ViewBag.Success = true;
            var cookie = new HttpCookie("token", token);
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie.Path = "/";
            //cookie.Secure = true;
            Response.Cookies.Add(cookie);
            return Redirect("~/");
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel user)
        {
            List<string> messages = new List<string>();
            if (string.IsNullOrEmpty(user.Phone))
            {
                messages.Add("手机号不能为空");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                messages.Add("电子邮件不能为空");
            }
            if (string.IsNullOrEmpty(user.Name))
            {
                messages.Add("用户名不能为空");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                messages.Add("密码不能为空");
            }
            if (RedisSercice.DB.KeyExists(string.Format("user:phone:{0}", user.Phone)))
            {
                messages.Add("相同电话号码已经注册");
            }
            if (RedisSercice.DB.KeyExists(string.Format("user:email:{0}", user.Email)))
            {
                messages.Add("相同电子邮箱已经注册");
            }
            if (messages.Count > 0)
            {
                ViewBag.Success = false;
                ViewBag.Messages = messages;
                return View();
            }

            user.ID = RedisSercice.GetSequence();
            string value = JsonConvert.SerializeObject(user);
            
            var tran = RedisSercice.DB.CreateTransaction();

            tran.StringSetAsync(string.Format("user:{0}", user.ID), value);
            tran.StringSetAsync(string.Format("user:email:{0}", user.Email),user.ID);
            tran.StringSetAsync(string.Format("user:phone:{0}", user.Phone), user.ID);

            bool success = tran.Execute();
            if (success)
            {
                ViewBag.Success = true;
                messages.Add("用户注册成功");
            }
            ViewBag.Messages = messages;
            return Login(user, user.Password);
        }



        //[HttpPost]
        //public JsonResult Post(Newtonsoft.Json.Linq.JObject post)
        //{
        //    JsonResult result = new JsonResult();
        //    return result;
        //}
    }
}