using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Call
    {
        public int callId;
        public List<string> usernames;
        [NonSerialized]
        public CallStatus status;
        public Call(int callId,List<string> usernames)
        {
            this.callId = callId;
            this.usernames = usernames; 
        }

        public void addUser(string username)
        {
            this.usernames.Add(username);
        }

        public void removeUser(string username)
        {
            this.usernames.Remove(username);
        }

        public Call()
        {
            usernames = new List<string>();
        }
    }
}
