namespace Hellgate.Xml
{
    class SoundEffectDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tEffect",
                ElementType = ElementType.Table,
                DefaultValue = null,
                ChildType = typeof (SoundEffect)
            }
        };

        public SoundEffectDefinition()
        {
            RootElement = "SOUND_EFFECT_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}