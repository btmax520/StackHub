using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StackHub.Core.Service;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisSercice rs = new RedisSercice();
            rs.Save("name", "gaoqi");
            Console.ReadLine();
        }
    }
}
