using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WampRouter.Model
{
    public class ArgumentsService : IArgumentsService
    {
        public void Ping()
        {
        }

        public int Add2(int a, int b)
        {
            return a + b;
        }

        public string Stars(string nick = "somebody", int stars = 0)
        {
            return string.Format("{0} starred {1}x", nick, stars);
        }
    }
}
