using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SKYBOX_MODEL")]
    public class SkyboxModel
    {
        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "szModelFile",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String ModelFile;

        [XmlCookedAttribute(
            Name = "nModelID",
            DefaultValue = -1,
            ElementType = ElementType.Int32_0x0A00)]
        public Int32 ModelId;

        [XmlCookedAttribute(
            Name = "nPass",
            DefaultValue = -1,
            ElementType = ElementType.Int32)]
        public Int32 Pass;

        [XmlCookedAttribute(
            Name = "nFogStart",
            DefaultValue = 150,
            ElementType = ElementType.Int32)]
        public Int32 FogStart;

        [XmlCookedAttribute(
            Name = "nFogEnd",
            DefaultValue = 300,
            ElementType = ElementType.Int32)]
        public Int32 FogEnd;

        [XmlCookedAttribute(
            Name = "tFogColor",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] FogColors;

        [XmlCookedAttribute(
            Name = "fAltitude",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Altitude;

        [XmlCookedAttribute(
            Name = "fScatterRad",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float ScatterRad;

        [XmlCookedAttribute(
            Name = "fChance",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float Chance;
    }
}