namespace Hellgate.Xml
{
    class ColorSetDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pColorDefinitions",
                ElementType = ElementType.TableMultiple,
                DefaultValue = null,
                ChildType = typeof (ColorDefinition)
            }
        };


        public ColorSetDefinition()
        {
            RootElement = "COLOR_SET_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}
