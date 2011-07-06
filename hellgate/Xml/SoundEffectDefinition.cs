namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SOUND_EFFECT_DEFINITION")]
    public class SoundEffectDefinition
    {
        [XmlCookedAttribute(
            Name = "tEffect",
            ElementType = ElementType.Table,
            DefaultValue = null,
            ChildType = typeof(SoundEffect))]
        public SoundEffect Effect;
    }
}