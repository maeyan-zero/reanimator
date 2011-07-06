using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SKYBOX_DEFINITION")]
    public class SkyboxDefinition
    {
        [XmlCookedAttribute(
            Name = "bLoaded",
            DefaultValue = false,
            ElementType = ElementType.NonCookedInt32,
            CustomType = ElementType.Bool)]
        public bool Loaded;

        [XmlCookedAttribute(
            Name = "tBackgroundColor",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] BackgroundColors;

        [XmlCookedAttribute(
            Name = "fWorldScale",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float WorldScale;

        [XmlCookedAttribute(
            Name = "nRegionID",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int RegionId;

        [XmlCookedAttribute(
            Name = "pModels",
            DefaultValue = 0,
            ElementType = ElementType.TableArrayVariable,
            ChildType = typeof(SkyboxModel))]
        public SkyboxModel[] Models;

    }
}