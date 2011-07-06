using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "LIGHT_DEFINITION")]
    public class LightDefinition
    {
        [XmlCookedAttribute(
            Name = "eType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 Type;

        [XmlCookedAttribute(
            Name = "nDurationType",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 DurationType;

        [XmlCookedAttribute(
            Name = "fStartTime",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float StartTime;

        [XmlCookedAttribute(
            Name = "fLoopTime",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float LoopTime;

        [XmlCookedAttribute(
            Name = "fEndTime",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float EndTime;

        [XmlCookedAttribute(
            Name = "fSpotAngleDeg",
            DefaultValue = 45.0f,
            ElementType = ElementType.Float)]
        public float SpotAngleDeg;

        [XmlCookedAttribute(
            Name = "tColor",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] Colors;

        [XmlCookedAttribute(
            Name = "tFalloff",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] Falloffs;

        [XmlCookedAttribute(
            Name = "tIntensity",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] Intensities;

        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.NonCookedInt32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "szSpotUmbraTexture",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String SpotUmbraTexture;

        [XmlCookedAttribute(
            Name = "nSpotUmbraTextureID",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 SpotUmbraTextureId;
    }
}