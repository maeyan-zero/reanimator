namespace Hellgate.Xml
{
    class XmlAIBehaviorDefinitionTable : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pBehaviors",
                DefaultValue = 0,
                ElementType = ElementType.TableArrayVariable,
                ChildType = typeof(XmlAIBehaviorDefinition)
            }
        };

        public XmlAIBehaviorDefinitionTable()
        {
            RootElement = "AI_BEHAVIOR_DEFINITION_TABLE";
            base.Elements.AddRange(Elements);
        }
    }
}