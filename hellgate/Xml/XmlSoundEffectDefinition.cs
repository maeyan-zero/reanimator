namespace Hellgate.Xml
{
    class XmlSoundEffectDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tEffect",
                ElementType = ElementType.Table,
                DefaultValue = null,
                ChildType = typeof (XmlSoundEffect)
            }
        };

        public XmlSoundEffectDefinition()
        {
            RootElement = "SOUND_EFFECT_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}