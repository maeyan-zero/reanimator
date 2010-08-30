namespace Reanimator.XmlDefinitions
{
    class AIDefinition : XmlDefinition
    {
        private static readonly XmlCookElement Table = new XmlCookElement();

        public AIDefinition()
        {
            RootElement = "AI_DEFINITION";

            Table.Name = "tTable";
            Table.DefaultValue = null;
            Table.ElementType = ElementType.Table;
            Table.ChildType = typeof(AIBehaviorDefinitionTable);
            Elements.Add(Table);
        }
    }
}