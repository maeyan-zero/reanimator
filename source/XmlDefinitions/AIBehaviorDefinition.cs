namespace Reanimator.XmlDefinitions
{
    class AIBehaviorDefinition : XmlDefinition
    {
        private static readonly XmlCookElement fPriority = new XmlCookElement();
        private static readonly XmlCookElement fChance = new XmlCookElement();
        private static readonly XmlCookElement pfParams = new XmlCookElement();

        public AIBehaviorDefinition()
        {
            RootElement = "AI_BEHAVIOR_DEFINITION";

            fPriority.Name = "fPriority";
            fPriority.DefaultValue = 0.5f;
            fPriority.ElementType = ElementType.Float;
            Elements.Add(fPriority);
        }
    }
}