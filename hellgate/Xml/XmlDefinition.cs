using System;
using System.Collections.Generic;

namespace Hellgate.Xml
{
    public class XmlDefinition
    {
        public String RootElement;
        public UInt32 RootHash;
        public String Value;
        public readonly List<XmlCookElement> Elements;
        public Int32[] BitFields;

        protected XmlDefinition()
        {
            Elements = new List<XmlCookElement>();
        }

        public XmlCookElement this[int index]
        {
            get { return Elements[index]; }
        }

        public int ElementCount
        {
            get { return Elements.Count; }
        }
    }
}