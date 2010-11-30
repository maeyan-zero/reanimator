namespace Hellgate.Xml
{
    class LightDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "eType",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nDurationType",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fStartTime",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fLoopTime",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fEndTime",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fSpotAngleDeg",
                DefaultValue = 45.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tColor",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "tFalloff",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tIntensity",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "szSpotUmbraTexture",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nSpotUmbraTextureID",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            }
        };

        public LightDefinition()
        {
            RootElement = "LIGHT_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}