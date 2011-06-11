namespace Hellgate.Xml
{
    class XmlAnimEvent : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "eType",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fTime",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fRandChance",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fParam",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.eType",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.nVolume",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.pszAttached",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.nAttachedDefId",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.pszBone",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.nBoneId",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vPosition",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayFixed,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vNormal",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayFixed,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.fScale",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float,
                IsResurrection = true
            },
            new XmlCookElement
            {
                Name = "tCondition",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof(XmlConditionDefinition)
            }
        };

        public XmlAnimEvent()
        {
            RootElement = "ANIM_EVENT";
            base.Elements.AddRange(Elements);
        }
    }
}