using System;
using Revival.Common;

namespace Hellgate
{
    public class XmlCookedElement
    {
        public readonly XmlCookedAttribute XmlAttribute;
        public readonly ObjectDelegator.FieldDelegate FieldDelegate;

        public ElementType ElementType { get { return XmlAttribute.ElementType; } }
        public bool IsTestCentre { get { return XmlAttribute.IsTestCentre; } }
        public bool IsResurrection { get { return XmlAttribute.IsResurrection; } }
        public String Name { get { return XmlAttribute.Name; } }
        public uint NameHash { get { return XmlAttribute.NameHash; } }

        // set to true for elements that should *not* be read from an XML file (e.g. the "root" element of 3xFloat -> 1xVector3)
        public bool IsCustomOrigin { get; set; }

        public Object Default;

        public XmlCookedElement(XmlCookedAttribute xmlAttribute, ObjectDelegator.FieldDelegate fieldDelegate)
        {
            XmlAttribute = xmlAttribute;
            FieldDelegate = fieldDelegate;
        }

        public override string ToString()
        {
            return XmlAttribute.Name;
        }
    }
}
