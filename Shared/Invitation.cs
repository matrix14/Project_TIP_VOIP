using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Invitation : Username
    {
        public int invitationId;

        public Invitation(string sender,int invitationId):base(sender)
        {
            this.invitationId = invitationId;
        }
    }
}
