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
            usernames = new List<string>(usernames); 
        }

        public Call()
        {
            usernames = new List<string>();
        }
    }
}
