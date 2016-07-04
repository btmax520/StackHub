using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackHub.Core.Service
{
    public class UserService : RedisSercice
    {
        public bool Insert(string value)
        {
            long id = RedisSercice.GetSequence();
            string key = "user:" + id;
            return DB.KeyExists(string.Format("user:email:{0}", key));

        }
    }
}
