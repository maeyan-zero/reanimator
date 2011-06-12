namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "AI_DEFINITION")]
    public class AIDefinition
    {
        [XmlCookedAttribute(
            Name = "tTable",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof(AIBehaviorDefinitionTable))]
        public AIBehaviorDefinitionTable[] Table;
    }
}