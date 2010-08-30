namespace Reanimator.XmlDefinitions
{
    class SkillEventHolder : XmlDefinition
    {
        private static readonly XmlCookElement nUnitMode = new XmlCookElement();
        private static readonly XmlCookElement fDuration = new XmlCookElement();
        private static readonly XmlCookElement pEvents = new XmlCookElement();

        public SkillEventHolder()
        {
            RootElement = "SKILL_EVENT_HOLDER";

            nUnitMode.Name = "nUnitMode";
            nUnitMode.DefaultValue = 0;
            nUnitMode.ExcelTableCode = 0x00006630; // (26160)	UNITMODES
            nUnitMode.ElementType = ElementType.ExcelIndex;
            Elements.Add(nUnitMode);

            fDuration.Name = "fDuration";
            fDuration.DefaultValue = 0.0f;
            fDuration.ElementType = ElementType.Float;
            Elements.Add(fDuration);

            pEvents.Name = "pEvents";
            pEvents.DefaultValue = 0;
            pEvents.ChildType = typeof (SkillEvent);
            pEvents.ElementType = ElementType.TableCount;
            Elements.Add(pEvents);
        }
    }
}