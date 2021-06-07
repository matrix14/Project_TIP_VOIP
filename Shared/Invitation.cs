using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Invitation : Username
    {
        public int invitationId;
        public string inviteeUsername;
        /// <summary>
        /// 0 - Created 1 - Sended 2 - Accepted 3 - Declined
        /// </summary>
        public int status;

        public DateTime date;

        public Invitation(string sender, int invitationId, string inviteeUsername,int status) : base(sender)
        {
            this.invitationId = invitationId;
            this.inviteeUsername = inviteeUsername;
            this.status = status;
        }

        public Invitation() : base()
        {
            this.status = 0;
        }
        public override string ToString()
        {
            return this.username;
        }

    }
}
