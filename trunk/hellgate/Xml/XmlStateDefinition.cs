namespace Hellgate.Xml
{
    public class XmlStateDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                    Name = "pEvents",
                    ElementType = ElementType.TableArrayVariable,
                    DefaultValue = 0,
                    ChildType = typeof (XmlStateEvent)
            }
        };

        public XmlStateDefinition()
        {
            RootElement = "STATE_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}