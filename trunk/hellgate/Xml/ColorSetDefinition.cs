namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "COLOR_SET_DEFINITION")]
    public class ColorSetDefinition
    {
        [XmlCookedAttribute(
            Name = "pColorDefinitions",
            ElementType = ElementType.TableArrayVariable,
            DefaultValue = null,
            ChildType = typeof (ColorDefinition))]
        public ColorDefinition[] ColorDefinition;

        public ColorSetDefinition()
        {
            ColorDefinition = null;
        }
    }
}