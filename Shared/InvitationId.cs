using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class InvitationId
    {

        public int invitationId;

        public InvitationId(int invitationId)
        {
            this.invitationId = invitationId;
        }

        public InvitationId()
        {
            ;
        }

        public static implicit operator int(InvitationId i) => i.invitationId;

    }
}
