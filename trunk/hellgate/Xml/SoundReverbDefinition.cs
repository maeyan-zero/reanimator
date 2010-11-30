namespace Hellgate.Xml
{
    class SoundReverbDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "bOff",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pReverb",
                DefaultValue = 0,
                ElementType = ElementType.TableCount,
                ChildType = typeof(FmodReverbProperties)
            }
        };

        public SoundReverbDefinition()
        {
            RootElement = "SOUND_REVERB_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}