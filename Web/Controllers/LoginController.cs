using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using StackHub.Core.Service;
using StackExchange.Redis;
using System.Web;

namespace Web.Controllers
{

    public class LoginController : ApiController
    {
        private class LoginUser
        {
            public string name { get; set; }
            public string password { get; set; }
            public string auto { get; set; }
        }

        private ResultJson GetResponseContent(JObject login)
        {
            var result = new ResultJson();
            try
            {
                var loginUser = login.ToObject<LoginUser>();
                string id = string.Empty;
                if (loginUser.name.Contains("@"))
                {
                    id = RedisSercice.DB.StringGet("user:email:" + loginUser.name);
                }
                else
                {
                    id = RedisSercice.DB.StringGet("user:phone:" + loginUser.name);
                }
                if (string.IsNullOrEmpty(id))
                {
                    result.success = false;
                    result.messages.Add("找不到注册用户");
                    return result;
                }
                string value = RedisSercice.DB.StringGet("user:" + id);

                JObject user = JObject.Parse(value);
                var repeat = RedisSercice.DB.StringIncrement("login:repeat:" + id);
                if (repeat > 5)
                {
                    result.success = false;
                    result.messages.Add("错误密码次数已经超过5次");
                    return result;
                }
                if (user["password"].ToString() != loginUser.password)
                {
                    result.success = false;
                    result.messages.Add(string.Format("密码不正确，还可以尝试{0}次", 5 - repeat));
                    return result;
                }
                else
                {
                    RedisSercice.DB.StringGetSet("login:repeat:" + id, 0);
                }

                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
                RedisSercice.DB.StringSet("token:" + token, id);
                RedisSercice.DB.SetAdd("online", id);
                result.success = true;
                result.rows.Add(token);

                //var cookie = new System.Net.Http.Headers.CookieHeaderValue("session-id", "12345");
                //cookie.Expires = DateTimeOffset.Now.AddDays(1);
                //cookie.Domain = Request.RequestUri.Host;
                //cookie.Path = "/";

                //var cookie = new HttpCookie("session-id", "12312312");
                //cookie.Expires = DateTime.Now.AddDays(1);
                //cookie.Domain = Request.RequestUri.Host;
                //cookie.Path = "/";
                //cookie.Secure = true;
                //HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception e)
            {
                result.success = false;
                result.messages.Add(e.Message);
            }
            return result;
        }

        
        public HttpResponseMessage Post([FromBody]JObject login)
        {
            var result = this.GetResponseContent(login);
            var resp = new HttpResponseMessage();
            if(result.rows.Count > 0)
            { 
                var cookie = new System.Net.Http.Headers.CookieHeaderValue("token", result.rows[0].ToString());
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";
                resp.Headers.AddCookies(new System.Net.Http.Headers.CookieHeaderValue[] { cookie });
            }
            resp.Content = new StringContent(result.ToString());
            return resp;

        }
    }
}
