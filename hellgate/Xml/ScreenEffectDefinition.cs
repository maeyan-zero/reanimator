using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SCREEN_EFFECT_DEFINITION")]
    public class ScreenEffectDefinition
    {
        [Flags]
        public enum ScreenEffectFlags : uint
        {
            SCREEN_EFFECT_DEF_FLAG_EXCLUSIVE = (1 << 0),
            SCREEN_EFFECT_DEF_FLAG_DX10_ONLY = (1 << 1)
        }

        [XmlCookedAttribute(
            Name = "szTechniqueName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String TechniqueName;

        [XmlCookedAttribute(
            Name = "Flags",
            DefaultValue = false,
            ElementType = ElementType.Flag,
            FlagId = 1)]
        public ScreenEffectFlags Flags;

        [XmlCookedAttribute(
            Name = "nTechniqueInEffect",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 TechniqueInEffect;

        [XmlCookedAttribute(
            Name = "nLayer",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 Layer;

        [XmlCookedAttribute(
            Name = "fFloats0",
            TrueName = "fFloats[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Floats0;

        [XmlCookedAttribute(
            Name = "fFloats1",
            TrueName = "fFloats[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Floats1;

        [XmlCookedAttribute(
            Name = "fFloats2",
            TrueName = "fFloats[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Floats2;

        [XmlCookedAttribute(
            Name = "fFloats3",
            TrueName = "fFloats[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Floats3;

        [XmlCookedAttribute(
            Name = "tColor0",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] Colors0;

        [XmlCookedAttribute(
            Name = "tColor1",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] Colors1;

        [XmlCookedAttribute(
            Name = "szTextureFilenames0",
            TrueName = "szTextureFilenames[0]",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String TextureFilenames0;

        [XmlCookedAttribute(
            Name = "szTextureFilenames1",
            TrueName = "szTextureFilenames[1]",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String TextureFilenames1;

        [XmlCookedAttribute(
            Name = "nTextureIDs0",
            TrueName = "nTextureIDs[0]",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 TextureIDs0;

        [XmlCookedAttribute(
            Name = "nTextureIDs1",
            TrueName = "nTextureIDs[1]",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 TextureIDs1;

        [XmlCookedAttribute(
            Name = "nPriority",
            DefaultValue = -1,
            ElementType = ElementType.Int32)]
        public Int32 Priority;

        [XmlCookedAttribute(
            Name = "fTransitionIn",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float TransitionIn;

        [XmlCookedAttribute(
            Name = "fTransitionOut",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float TransitionOut;
    }
}