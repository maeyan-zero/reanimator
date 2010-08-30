namespace Reanimator.XmlDefinitions
{
    class SkillEventsDefinition : XmlDefinition
    {
        private static readonly XmlCookElement szPreviewAppearance = new XmlCookElement();
        private static readonly XmlCookElement nPreviewAppearance = new XmlCookElement();
        private static readonly XmlCookElement pEventHolders = new XmlCookElement();

        public SkillEventsDefinition()
        {
            RootElement = "SKILL_EVENTS_DEFINITION";

            szPreviewAppearance.Name = "szPreviewAppearance";
            szPreviewAppearance.DefaultValue = null;
            szPreviewAppearance.ElementType = ElementType.String;
            Elements.Add(szPreviewAppearance);

            nPreviewAppearance.Name = "nPreviewAppearance";
            nPreviewAppearance.DefaultValue = -1;
            nPreviewAppearance.ElementType = ElementType.NonCookedInt32;
            Elements.Add(nPreviewAppearance);

            pEventHolders.Name = "pEventHolders";
            pEventHolders.DefaultValue = 0;
            pEventHolders.ChildType = typeof (SkillEventHolder);
            pEventHolders.ElementType = ElementType.TableCount;
            Elements.Add(pEventHolders);
        }
    }
}