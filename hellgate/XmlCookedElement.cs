using System;
using Revival.Common;

namespace Hellgate
{
    public class XmlCookedElement
    {
        public readonly XmlCookedAttribute XmlAttribute;
        public readonly ObjectDelegator.FieldDelegate FieldDelegate;
        public readonly XmlCookedTree XmlTree;
        public readonly XmlCookedElement Parent;

        public ElementType ElementType { get { return XmlAttribute.ElementType; } }
        public bool IsTestCentre { get { return XmlAttribute.IsTestCentre; } }
        public bool IsResurrection { get { return XmlAttribute.IsResurrection; } }
        public String Name { get { return XmlAttribute.Name; } }
        public uint NameHash { get { return XmlAttribute.NameHash; } }

        // set to true for elements that should *not* be read from an XML file (e.g. the "root" element of 3xFloat -> 1xVector3)
        public bool IsCustomOrigin { get; set; }

        public Object GetValue(Object obj)
        {
            return FieldDelegate.GetValue(obj);
        }

        public Object Default;

        public XmlCookedElement(XmlCookedTree xmlTree)
        {
            XmlTree = xmlTree;
        }

        public XmlCookedElement(XmlCookedAttribute xmlAttribute, ObjectDelegator.FieldDelegate fieldDelegate, XmlCookedElement parent = null)
        {
            XmlAttribute = xmlAttribute;
            FieldDelegate = fieldDelegate;
            Parent = parent;
        }

        public override string ToString()
        {
            return XmlAttribute.Name;
        }
    }
}
