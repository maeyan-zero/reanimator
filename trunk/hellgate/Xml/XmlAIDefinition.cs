namespace Hellgate.Xml
{
    class XmlAIDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tTable",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof(XmlAIBehaviorDefinitionTable)
            }
        };

        public XmlAIDefinition()
        {
            RootElement = "AI_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}