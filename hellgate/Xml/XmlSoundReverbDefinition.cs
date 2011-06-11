namespace Hellgate.Xml
{
    class XmlSoundReverbDefinition : XmlDefinition
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
                ElementType = ElementType.TableArrayVariable,
                ChildType = typeof(XmlFmodReverbProperties)
            }
        };

        public XmlSoundReverbDefinition()
        {
            RootElement = "SOUND_REVERB_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}