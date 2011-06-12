namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "STATE_DEFINITION")]
    public class StateDefinition
    {
        [XmlCookedAttribute(
            Name = "pEvents",
            ElementType = ElementType.TableArrayVariable,
            DefaultValue = 0,
            ChildType = typeof(StateEvent))]
        public StateEvent[] Events;
    }
}