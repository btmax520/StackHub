using StackHub.Core.Service;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Web.Controllers
{
    public class ValuesController : ApiController
    {
        private static readonly long seqence = 1001000000000001L;

        private List<long> GetSeqences(int count)
        {
            string current = RedisSercice.Get("system:seqence:global");
            List<long> result = new List<long>(count);
            if (current == null)
            {
                RedisSercice.Set("system:seqence:global", seqence.ToString());
                result.Add(seqence);
            }
            else
            {
                result.Add(long.Parse(current));
            }
            for (int i = 1; i < count; i++)
            {
                result.Add(result[0]+i);
            }
            var next = result[result.Count - 1] + 1;
            RedisSercice.Set("system:seqence:global", next.ToString());
            return result;
        }

        public class JsonDTO
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        // GET api/values
        public JObject Get([FromUri]string q)
        {
            JObject result = new JObject();
            JObject query = JObject.Parse(q);
            if (query["type"] != null) {
                string type = query["type"].ToString();
                switch (type)
                {
                    case "schema":
                        JArray rows = new JArray();
                        var redisValues = RedisSercice.DB.SetMembers("system:schema");
                        foreach (var item in redisValues)
                        {
                            var id = item.ToString();
                            var value = RedisSercice.DB.StringGet("schema:" + id);
                            rows.Add(JObject.Parse(value));
                        }
                        result["rows"] = rows;
                        break;
                }
            }

            return result;
        }

        // POST api/values
        public JObject Post(JObject postData)
        {
            if (postData["type"] != null)
            {
                string type = postData["type"].ToString();
                
                switch (type)
                {
                    case "schema":
                        JArray rows = (JArray)postData["rows"];
                        
                        
                        foreach (var item in rows)
                        {
                            if (item["id"] == null || item["id"].ToString() == string.Empty)
                            {
                                item["id"] = GetSeqences(1)[0];
                            }
                            if (item["code"] != null && item["code"].ToString() != string.Empty)
                            {
                                RedisSercice.DB.StringSet("system:id:" + item["id"], item["code"].ToString());
                                RedisSercice.DB.SetAdd("system:schema", item["id"].ToString());
                                RedisSercice.DB.StringSet("schema:" + item["id"], item.ToString());


                                //var entry = new SortedSetEntry(item["code"].ToString(), double.Parse(item["id"].ToString()));
                                //var entrys = RedisSercice.DB.SortedSetRangeByScore("system:schemas", double.Parse(item["id"].ToString()));
                                //if (entrys.Length > 0)
                                //{
                                //    string old = entrys[0];
                                //    if (old != item["code"].ToString())
                                //    {
                                //        RedisSercice.DB.
                                //    }
                                //}
                                //else { 
                                //    RedisSercice.DB.SortedSetAdd("system:schemas", item["code"].ToString(), double.Parse(item["id"].ToString()));
                                
                                    
                                //}
                            }
                        }
                        //RedisSercice.DB.HashSet("system:schema", entrys.ToArray());

                        //foreach (var item in rows)
                        //{
                        //    if (item["id"] == null || item["id"].ToString() == string.Empty)
                        //    {
                        //        item["id"] = GetSeqences(1)[0];
                        //    }
                        //    string key = string.Format("schema:{0}", item["id"]);
                        //    RedisSercice.DB.StringSet(key, item.ToString());
                        //}

                        break;
                }
            }
            return postData;


            //foreach (JObject dateItem in postData["dataItems"])
            //{
            //    JToken value = dateItem["value"];
            //    if (value["id"] == null || value["id"].ToString() == string.Empty)
            //    {
            //        value["id"] = GetSeqences(1)[0];
            //    }
            //    string key = dateItem["key"].ToString() + ":" + value["id"];
            //    Dictionary<string, string> fieldAndValues = new Dictionary<string, string>();
            //    foreach (JProperty p in value)
            //    {
            //        fieldAndValues.Add(p.Name, p.Value.ToString());
            //    }
            //    RedisSercice.SetHash(key, fieldAndValues);
            //}
            //JToken data = JToken.Parse(dto.Value);
            //if (data.Type == JTokenType.Array)
            //{
            //    foreach(JToken item in data)
            //    if (item["id"] == null || item["id"].ToString() == string.Empty)
            //    {
            //        item["id"] = GetSeqences(1)[0];
            //    }
            //}
            //var success = RedisSercice.Set(dto.Key, data.ToString());

            //return data.ToString();
            return null;
        }

        // PUT api/values/5
        public void Put(string id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(string key)
        {
        }
    }
}
