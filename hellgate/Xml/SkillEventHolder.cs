namespace Hellgate.Xml
{
    class SkillEventHolder : XmlDefinition
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
                ChildType = typeof(SkillEvent),
                ElementType = ElementType.TableArrayVariable
            }
        };

        public SkillEventHolder()
        {
            RootElement = "SKILL_EVENT_HOLDER";
            base.Elements.AddRange(Elements);
        }
    }
}