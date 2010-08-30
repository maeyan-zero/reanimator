namespace Reanimator.XmlDefinitions
{
    class AIBehaviorDefinitionTable : XmlDefinition
    {
        private static readonly XmlCookElement pBehaviors = new XmlCookElement();

        public AIBehaviorDefinitionTable()
        {
            RootElement = "AI_BEHAVIOR_DEFINITION_TABLE";

            pBehaviors.Name = "pBehaviors";
            pBehaviors.DefaultValue = null;
            pBehaviors.ElementType = ElementType.Int32;
            pBehaviors.ChildType = typeof(AIBehaviorDefinition);
            Elements.Add(pBehaviors);
        }
    }
}