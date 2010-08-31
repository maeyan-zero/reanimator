namespace Reanimator.XmlDefinitions
{
    class SkillEventsDefinition : XmlDefinition
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
                ChildType = typeof(SkillEventHolder),
                ElementType = ElementType.TableCount
            }
        };

        public SkillEventsDefinition()
        {
            RootElement = "SKILL_EVENTS_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}