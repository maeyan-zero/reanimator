using System;
using System.Xml;

namespace Reanimator
{
    class XmlCookedState : XmlCookedBase
    {
        protected override bool CookDataSegment(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        protected override bool ParseDataSegment(XmlElement dataElement)
        {
            throw new NotImplementedException();
        }
    }
}