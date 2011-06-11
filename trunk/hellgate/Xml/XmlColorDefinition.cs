using System;

namespace Hellgate.Xml
{
    class XmlColorDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nId",
                DefaultValue = null,
                ExcelTableCode = 13360, // COLORSETS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nUnittype",
                DefaultValue = null,
                ExcelTableCode = 21040, // UNITTYPES
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "pdwColors",
                ElementType = ElementType.Int32ArrayFixed,
                DefaultValue = (UInt32)0x00000000,
                Count = 6
            }
        };


        public XmlColorDefinition()
        {
            RootElement = "COLOR_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}
