using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Id
    {

        public int id;

        public Id(int id)
        {
            this.id = id;
        }

        public Id()
        {
            ;
        }

        public static implicit operator int(Id i) => i.id;

    }
}
