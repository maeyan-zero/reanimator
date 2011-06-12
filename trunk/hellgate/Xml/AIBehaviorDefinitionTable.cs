namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "AI_BEHAVIOR_DEFINITION_TABLE")]
    public class AIBehaviorDefinitionTable
    {
        [XmlCookedAttribute(
            Name = "pBehaviors",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(AIBehaviorDefinition))]
        public AIBehaviorDefinition[] Behaviors;
    }
}