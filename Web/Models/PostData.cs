using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Web.Models
{
    public class PostData
    {
        public string Type { get; set; }

        public JToken Data { get; set; }
    }
}