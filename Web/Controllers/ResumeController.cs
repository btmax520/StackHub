using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using StackHub.Core.Service;
//using System.Web.Http;

namespace Web.Controllers
{
    public class ResumeController : BaseController
    {
        private JObject GetResume(string id)
        {
            var resume = RedisSercice.DB.StringGet("resume:" + id);
            resume = "{name:123}";
            return JObject.Parse(resume);
        }

        [HttpPost]
        public ResultJson Post([System.Web.Http.FromBody]JObject data)
        {
            ResultJson result = new ResultJson();
            if (data["type"] == null)
            {
                result.success = false;
                result.messages.Add("数据类型不能为空");
                return result;
            }
            string type = data["type"].ToString();
            switch (type)
            {
                case "info":
                    var success = RedisSercice.DB.StringSet("resume:info:" + data["phone"], data.ToString());
                    result.success = success;
                    if (success)
                    {
                        result.rows.Add(data);
                        result.messages.Add("保存成功");
                    }
                    else
                    {
                        result.messages.Add("保存失败");
                    }
                    break;
            }
            return result;
        }

        // GET: Resume
        public ActionResult Index()
        {
            SetTopActive("job");
            SetLeftActive("resume");
            return View();
        }
    }
}