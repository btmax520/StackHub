using Newtonsoft.Json;
using StackHub.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login ,string password,string remember)
        {
            User loginUser = new User();
            loginUser.Remember = remember.ToLower() == "on";
            if (login.Contains("@"))
            {
                loginUser.Email = login;
            }
            else
            {
                loginUser.Phone = login;
            }
            if (string.IsNullOrEmpty(login))
            {
                return View();
            }
            else
            {
                
                return Login(loginUser,password);
            }
        }

        private ActionResult Login(User user,string password)
        {
            long id = user.ID;
            List<string> messages = new List<string>();
            ViewBag.Messages = messages;
            using (var db = new DataBaseContext())
            {
                User checkUser = null;
                if(string.IsNullOrEmpty(user.Phone))
                    checkUser = db.User.SingleOrDefault(u => u.Email == user.Email);
                else
                    checkUser = db.User.SingleOrDefault(u => u.Phone == user.Phone);

                if (checkUser == null)
                {
                    ViewBag.Success = false;
                    messages.Add("用户不存在");
                    return View();
                }
                else
                {
                    if (checkUser.Repeat > 5)
                    {
                        ViewBag.Success = false;
                        messages.Add("错误密码次数已经超过5次");
                        return View();
                    }
                    if (checkUser.Password != password)
                    {
                        ViewBag.Success = false;
                        messages.Add(string.Format("密码不正确，还可以尝试{0}次", 5 - checkUser.Repeat));
                        checkUser.Repeat++;
                        db.SaveChanges();
                        return View();
                    }
                    else
                    {
                        string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
                        checkUser.Token = token;
                        checkUser.Repeat = 0;
                        var result = db.SaveChanges();
                        bool success = result > 0;
                        if (success)
                        {
                            FormsAuthentication.SetAuthCookie(checkUser.Email, false);
                            ViewBag.Success = true;
                            messages.Add("登录成功");
                            //var cookie = new HttpCookie("token", token);
                            //cookie.Expires = DateTime.Now.AddDays(7);
                            //cookie.Path = "/";
                            ////cookie.Secure = true;
                            //Response.Cookies.Add(cookie);
                            string returnUrl = Request.Params["ReturnUrl"];
                            if (string.IsNullOrEmpty(returnUrl))
                                return Redirect("~/");
                            else
                                return Redirect(returnUrl);
                        }
                    }
                }
            }
            return View();
            
        }

        public ActionResult Logout(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
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

            using (var db = new DataBaseContext())
            {
                var checkUser = db.User.SingleOrDefault(u => u.Phone == user.Phone || u.Email == user.Email);
                if (checkUser != null)
                {
                    messages.Add("相同电话号码或电子邮箱已经注册");
                }
                else
                {
                    db.User.Add(user);
                    var result = db.SaveChanges();
                    bool success = true;
                    if (success)
                    {
                        ViewBag.Success = true;
                        messages.Add("用户注册成功");
                    }
                    ViewBag.Messages = messages;
                    return Login(user, user.Password);
                }
            }
            if (messages.Count > 0)
            {
                ViewBag.Success = false;
                ViewBag.Messages = messages;
            }
            return View();

        }



        //[HttpPost]
        //public JsonResult Post(Newtonsoft.Json.Linq.JObject post)
        //{
        //    JsonResult result = new JsonResult();
        //    return result;
        //}
    }
}