namespace Hellgate.Xml
{
    class SkyboxModel : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "szModelFile",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nModelID",
                DefaultValue = -1,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "nPass",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nFogStart",
                DefaultValue = 150,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nFogEnd",
                DefaultValue = 300,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tFogColor",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "fAltitude",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fScatterRad",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fChance",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            }
        };

        public SkyboxModel()
        {
            RootElement = "SKYBOX_MODEL";
            base.Elements.AddRange(Elements);
        }
    }
}