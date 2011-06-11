namespace Hellgate.Xml
{
    class XmlSkyboxDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "bLoaded",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "tBackgroundColor",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "fWorldScale",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nRegionID",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pModels",
                DefaultValue = 0,
                ElementType = ElementType.TableArrayVariable,
                ChildType = typeof(XmlSkyboxModel)
            }
        };

        public XmlSkyboxDefinition()
        {
            RootElement = "SKYBOX_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}