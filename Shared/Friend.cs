using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Friend: Username
    {
        public int active;

        public Friend(string username, int active):base(username)
        {
            this.active = active;
        }

        public Friend():base()
        {
            active = 0;
        }
    }
}
