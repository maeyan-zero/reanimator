namespace Hellgate.Xml
{
    class AIDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tTable",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof(AIBehaviorDefinitionTable)
            }
        };

        public AIDefinition()
        {
            RootElement = "AI_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}