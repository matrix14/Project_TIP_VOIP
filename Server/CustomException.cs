using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class CustomException: Exception
    {
        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
