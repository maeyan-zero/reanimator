using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator
{
    class Excel
    {
        protected byte[] buffer;

        protected int headLength;
        protected int bodyLength;
        protected int footLength;

        protected Description[] descriptions;

        public struct Description
        {
            int offset;
            int length;
            Type type;
            string alias;
        }
    }
}
