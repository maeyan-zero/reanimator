namespace Hellgate.Xml
{
    public class XmlSkillEventsDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "szPreviewAppearance",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nPreviewAppearance",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pEventHolders",
                DefaultValue = 0,
                ChildType = typeof(XmlSkillEventHolder),
                ElementType = ElementType.TableArrayVariable
            }
        };

        public XmlSkillEventsDefinition()
        {
            RootElement = "SKILL_EVENTS_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}