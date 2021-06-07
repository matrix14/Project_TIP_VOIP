using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    /// <summary>
    /// 0 - ToSend 1 - Sended 2 - Accepted 3 - Declined (To delete)
    /// </summary>
    public enum CallStatus
    {
        NOT_SENDED = 0,
        SENDED = 1,
        ACCEPTED = 2,
        DECLINED = 3,
    }
}
