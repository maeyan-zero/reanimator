namespace Hellgate.Xml
{
    internal class TextureDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nFormat",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tBinding",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nFileSize",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nMipMapLevels",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nArraySize",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nMipMapUsed",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "pszMaterialName",
                DefaultValue = "Default",
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nMeshLODPriority",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nBlendWidth",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nBlendHeight",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "pBlendRLE",
                DefaultValue = 0,
                ElementType = ElementType.TableMultiple,
                ChildType = typeof(BlendRLE)
            },
            new XmlCookElement
            {
                Name = "nFramesU",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nFramesV",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nWidth",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nHeight",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "dwConvertFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fBlurFactor",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nSharpenFilter",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSharpenPasses",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nMIPFilter",
                DefaultValue = 6,
                ElementType = ElementType.Int32
            }
        };

        public TextureDefinition()
        {
            RootElement = "TEXTURE_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}