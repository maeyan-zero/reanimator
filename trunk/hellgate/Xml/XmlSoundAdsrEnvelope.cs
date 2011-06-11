namespace Hellgate.Xml
{
    class XmlSoundAdsrEnvelope : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tEnvelopePath",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "nLoopPointMin",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nLoopPointMax",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fLengthInSeconds",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            }
        };

        public XmlSoundAdsrEnvelope()
        {
            RootElement = "SOUND_ADSR_ENVELOPE";
            base.Elements.AddRange(Elements);
        }
    }
}