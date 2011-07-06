namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SOUND_REVERB_DEFINITION")]
    public class SoundReverbDefinition
    {
        [XmlCookedAttribute(
            Name = "bOff",
            DefaultValue = false,
            ElementType = ElementType.NonCookedInt32,
            CustomType = ElementType.Bool)]
        public bool Off;

        [XmlCookedAttribute(
            Name = "pReverb",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(FmodReverbProperties))]
        public FmodReverbProperties[] Reverb;
    }
}