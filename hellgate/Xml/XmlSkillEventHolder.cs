namespace Hellgate.Xml
{
    class XmlSkillEventHolder : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nUnitMode",
                DefaultValue = null,
                ExcelTableCode = 0x00006630, // (26160)	UNITMODES
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "fDuration",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pEvents",
                DefaultValue = 0,
                ChildType = typeof(XmlSkillEvent),
                ElementType = ElementType.TableArrayVariable
            }
        };

        public XmlSkillEventHolder()
        {
            RootElement = "SKILL_EVENT_HOLDER";
            base.Elements.AddRange(Elements);
        }
    }
}