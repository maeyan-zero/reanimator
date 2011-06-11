namespace Hellgate.Xml
{
    class XmlInventoryViewInfo : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "vCamFocus.fX",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vCamFocus.fY",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vCamFocus.fZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCamRotation",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCamPitch",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCamDistance",
                DefaultValue = 3.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCamFOV",
                DefaultValue = 1.047198f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pszEnvName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszBoneName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nBone",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            }
        };

        public XmlInventoryViewInfo()
        {
            RootElement = "INVENTORY_VIEW_INFO";
            base.Elements.AddRange(Elements);
        }
    }
}