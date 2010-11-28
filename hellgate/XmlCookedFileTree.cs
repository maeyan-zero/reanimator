using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate.Xml;

namespace Hellgate
{
    class XmlCookedFileTree
    {
        private readonly LinkedList<XmlCookElement> _elements = new LinkedList<XmlCookElement>();

        public XmlCookedFileTree()
        {
            
        }

        public void AddElement(XmlCookElement xmlCookElement)
        {
            _elements.AddLast(xmlCookElement);
        }
    }
}
