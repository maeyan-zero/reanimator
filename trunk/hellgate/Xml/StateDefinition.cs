namespace Hellgate.Xml
{
    class StateDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                    Name = "pEvents",
                    ElementType = ElementType.TableArrayVariable,
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
