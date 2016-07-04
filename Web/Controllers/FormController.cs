using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using StackHub.Core.Service;
using StackExchange.Redis;

namespace Web.Controllers
{
    public class ResultJson
    {
        public bool success { get; set; }
        public JArray rows { get; set; }
        public JArray messages { get; set; }

        public ResultJson()
        {
            rows = new JArray();
            messages = new JArray();
        }
    }

    public class FormController : ApiController
    {
        public string Get(string id)
        {
            string key = string.Format("form:{0}", id);
            var tmpl = RedisSercice.DB.StringGet(id);
            return tmpl;
        }

        public ResultJson Post(string id, [FromBody]JObject value)
        {
            string schema = id;
            var result = new ResultJson();
            if (string.IsNullOrEmpty(schema))
            {
                result.success = false;
                result.messages.Add("键不能为空");
                return result;
            }
            if (value["id"] == null)
            {
                value["id"] = RedisSercice.GetSeqences(1)[0];
            }
            try
            {
                string key = string.Format("{0}:{1}", schema, value["id"]);
                var success = false;
                success = RedisSercice.DB.StringSet(key, value.ToString());
                if (success == false)
                {
                    result.success = false;
                    result.messages.Add("保存失败");
                    return result;
                }
                else
                {
                    result.success = true;
                    result.rows.Add(value);
                    result.messages.Add("保存成功");
                }
            }
            catch (Exception e)
            {
                result.success = false;
                result.messages.Add(e.Message);
            }
            return result;
        }
    }
}
