namespace Hellgate.Xml
{
    class XmlDemoLevelDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nLevelDefinition",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 29233 // LEVEL
            },
            new XmlCookElement
            {
                Name = "nDRLGDefinition",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 21553 // LEVEL_DRLGS
            },
            new XmlCookElement
            {
                Name = "dwDRLGSeed",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nCameraType",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fMetersPerSecond",
                DefaultValue = 5.0f,
                ElementType = ElementType.Float
            }
        };

        public XmlDemoLevelDefinition()
        {
            RootElement = "DEMO_LEVEL_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}