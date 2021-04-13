using DbLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class User: IEquatable<User>
    {
        public DbMethods dbConnection;
        public bool logged;
        public int userId;
        public string username;

        public User()
        {
            username = "";
            dbConnection = new DbMethods();
            logged = false;
        }

        public bool Equals(User user)
        {
            return this.username == user.username;
        }

        public override int GetHashCode()
        {
            return username.GetHashCode();
        }

    }
}
