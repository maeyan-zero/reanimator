using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ENV_LIGHT_DEFINITION")]
    public class EnvLightDefinition
    {
        [XmlCookedAttribute(
            Name = "tColor",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4 Color;

        [XmlCookedAttribute(
            Name = "fIntensity",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Intensity;

        [XmlCookedAttribute(Name = "vVec", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "vVec.fX",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vVec.fY",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vVec.fZ",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public Vector3 Vec = new Vector3();
    }
}