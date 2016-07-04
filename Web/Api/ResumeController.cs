using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class ResumeController : ApiController
    {
        [HttpPost]
        public JObject Basic([FromBody]JObject basic)
        {
            JObject result = new JObject();
            return result;
        }
    }
}
