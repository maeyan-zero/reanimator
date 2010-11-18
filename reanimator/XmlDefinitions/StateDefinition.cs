namespace Reanimator.XmlDefinitions
{
    class StateDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                    Name = "pEvents",
                    ElementType = ElementType.TableCount,
                    DefaultValue = 0,
                    ChildType = typeof (StateEvent)
            }
        };

        public StateDefinition()
        {
            RootElement = "STATE_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}
