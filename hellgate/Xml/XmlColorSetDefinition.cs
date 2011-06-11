namespace Hellgate.Xml
{
    class XmlColorSetDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pColorDefinitions",
                ElementType = ElementType.TableArrayVariable,
                DefaultValue = null,
                ChildType = typeof (XmlColorDefinition)
            }
        };


        public XmlColorSetDefinition()
        {
            RootElement = "COLOR_SET_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}
