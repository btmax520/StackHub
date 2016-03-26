using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StackHub.Core.Service
{
    public class RedisSercice
    {
        private ConnectionMultiplexer redis;

        public RedisSercice()
        {
            try
            {
                if(redis == null)
                {
                    string config = System.Configuration.ConfigurationManager.AppSettings.Get("redisServer");

                    redis = ConnectionMultiplexer.Connect(config);
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("初始化Redis服务错误：{0}。", e.Message));
            }
        }

        public bool Save(string key,string value)
        {
            IDatabase db = redis.GetDatabase();
            return db.StringSet(key, value);
        }

    }
}