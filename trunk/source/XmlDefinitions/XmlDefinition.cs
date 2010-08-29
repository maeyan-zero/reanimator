using System;
using System.Collections.Generic;

namespace Reanimator.XmlDefinitions
{
    public class XmlDefinition
    {
        public String RootElement;
        public String Value;
        public readonly List<XmlCookElement> Elements;

        protected XmlDefinition()
        {
            Elements = new List<XmlCookElement>();
        }

        public XmlCookElement this[int index]
        {
            get { return Elements[index]; }
        }
    }
}