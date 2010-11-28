namespace Hellgate.Xml
{
    class ConfigDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSoundOutputType",
                DefaultValue = 2,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSoundSpeakerConfig",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSoundMemoryReserveType",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSoundMemoryReserveMegabytes",
                DefaultValue = 64,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fMouseSensitivity",
                DefaultValue = 1.5f,
                ElementType = ElementType.Float
            }
        };


        public ConfigDefinition()
        {
            RootElement = "CONFIG_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}
