using DbLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class User
    {
        public DbMethods dbConnection;
        public bool logged;
        public int userId;
        public string userName;

        public User()
        {
            userName = "";
            dbConnection = new DbMethods();
            logged = false;
        }
    }
}
