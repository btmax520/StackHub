using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace StackHub.Core.Service
{
    public class RedisSercice
    {
        private static readonly long seqence = 1001000000000001L;

        private static ConnectionMultiplexer redis;
        

        static RedisSercice()
        {

            if (redis == null)
            {
                try
                {
                    string config = System.Configuration.ConfigurationManager.AppSettings.Get("redisServer");

                    redis = ConnectionMultiplexer.Connect(config);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("初始化Redis服务错误：{0}。", e.Message));
                }
            }
        }

        public static IDatabase DB
        {
            get
            {
                return redis.GetDatabase();
            }
        }

        public static List<long> GetSeqences(int count)
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
                result.Add(result[0] + i);
            }
            var next = result[result.Count - 1] + 1;
            RedisSercice.Set("system:seqence:global", next.ToString());
            return result;
        }

        public static long GetSequence()
        {
            string key = "global:seqence";
            if (DB.KeyExists(key))
            {
                long result = DB.StringIncrement(key);
                return result;
            }
            else
            {
                DB.StringSet(key, seqence);
                return seqence;
            }
        }

        public static Dictionary<string,string> GetHash(string key, Dictionary<string, string> values)
        {
            IDatabase db = redis.GetDatabase();
            HashEntry[] entry = db.HashGetAll(key);
            Dictionary<string, string> result = new Dictionary<string, string>(entry.Length);
            foreach (var item in entry)
            {
                result.Add(item.Name, item.Value);
            }
            return result;
        }

        public static void SetHash(string key,Dictionary<string,string> values)
        {
            IDatabase db = redis.GetDatabase();
            var entry = new HashEntry[values.Count];
            int i = 0;
            foreach (var dict in values)
            {
                entry[i] = new HashEntry(dict.Key, dict.Value);
                i++;
            }
            db.HashSet(key, entry);
        }

        public static bool Set(string key,string value)
        {
            IDatabase db = redis.GetDatabase();
            return db.StringSet(key, value);
        }

        public static string Get(string key)
        {
            IDatabase db = redis.GetDatabase();
            var value = db.StringGet(key);
            return value;
        }

        public static void Search(string key)
        {
            IDatabase db = redis.GetDatabase();
            
        }

        public static bool Insert(string schema, string value)
        {
            IDatabase db = redis.GetDatabase();
            string key = schema + ":" + GetSequence();
            return db.StringSet(key, value);
        }

    }
}