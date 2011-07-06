using System;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SOUND_EFFECT")]
    public class SoundEffect
    {
        [XmlCookedAttribute(
            Name = "eType",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public Int32 Type;

        [XmlCookedAttribute(
            Name = "pszName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String Name;

        [XmlCookedAttribute(
            Name = "pfParamsBase",
            ElementType = ElementType.FloatArrayFixed,
            Count = 8,
            DefaultValue = 0.0f)]
        public float[] ParamsBase;

        [XmlCookedAttribute(
            Name = "pfParamsVariation",
            ElementType = ElementType.FloatArrayFixed,
            Count = 8,
            DefaultValue = 0.0f)]
        public float[] ParamsVariation;

        [XmlCookedAttribute(
            Name = "nWeight",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public Int32 Weight;

        [XmlCookedAttribute(
            Name = "nDelay",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public Int32 Delay;

        [XmlCookedAttribute(
            Name = "bReadOnly",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool ReadOnly;

        [XmlCookedAttribute(
            Name = "bEnabled",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool Enabled;

        [XmlCookedAttribute(
            Name = "pEffects",
            ElementType = ElementType.TableArrayVariable,
            DefaultValue = null,
            ChildType = typeof(SoundEffect))]
        public SoundEffect[] Effects;
    }
}