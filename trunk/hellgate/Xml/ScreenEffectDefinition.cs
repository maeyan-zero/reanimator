using System;

namespace Hellgate.Xml
{
    class ScreenEffectDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "szTechniqueName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "SCREEN_EFFECT_DEF_FLAG_EXCLUSIVE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "SCREEN_EFFECT_DEF_FLAG_DX10_ONLY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "nTechniqueInEffect",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nLayer",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "fFloats0",
                TrueName = "fFloats[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fFloats1",
                TrueName = "fFloats[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fFloats2",
                TrueName = "fFloats[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fFloats3",
                TrueName = "fFloats[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tColor0",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatQuadArrayVariable,
            },
            new XmlCookElement
            {
                Name = "tColor1",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatQuadArrayVariable,
            },
            new XmlCookElement
            {
                Name = "szTextureFilenames0",
                TrueName = "szTextureFilenames[0]",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "szTextureFilenames1",
                TrueName = "szTextureFilenames[1]",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nTextureIDs0",
                TrueName = "nTextureIDs[0]",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nTextureIDs1",
                TrueName = "nTextureIDs[1]",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nPriority",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fTransitionIn",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fTransitionOut",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            }
        };

        public ScreenEffectDefinition()
        {
            RootElement = "SCREEN_EFFECT_DEFINITION";
            base.Elements.AddRange(Elements);
            BitFlags = new Int32[] { -1 };
            BitFlagsBaseMask = 0xFDFDFDFC;
        }
    }
}