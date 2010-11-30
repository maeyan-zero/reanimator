namespace Hellgate.Xml
{
    class EnvLightDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "tColor",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "fIntensity",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vVec.fX",
                DefaultValue = -1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vVec.fY",
                DefaultValue = -1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vVec.fZ",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
        };

        public EnvLightDefinition()
        {
            RootElement = "ENV_LIGHT_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}