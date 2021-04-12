using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Login: Username
    {

        public string passwordHash;

        public Login(string username,string passwordHash): base(username)
        {
            this.passwordHash = passwordHash;
        }
    }
}
