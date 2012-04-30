using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ENVIRONMENT_DEFINITION")]
    public class EnvironmentDefinition
    {
        [Flags]
        public enum EnvironmentDefFlags : uint
        {
            ENVIRONMENTDEF_FLAG_DIR1_OPPOSITE_DIR0 = (1 << 0),
            ENVIRONMENTDEF_FLAG_HAS_APP_SH_COEFS = (1 << 1),
            ENVIRONMENTDEF_FLAG_HAS_BG_SH_COEFS = (1 << 2),
            ENVIRONMENTDEF_FLAG_SPECULAR_FAVOR_FACING = (1 << 3),
            ENVIRONMENTDEF_FLAG_FLASHLIGHT_EMISSIVE = (1 << 4),

            [XmlCookedAttribute(IsTestCentre = true)]
            ENVIRONMENTDEF_FLAG_USE_BLOB_SHADOWS = (1 << 5) // this appears to only be written in the cooked header iff it is present in the XML file...
        }

        [XmlCookedAttribute(
            Name = "dwDefFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            FlagOffsetChange = -sizeof(Int32),      // this field is needed (FlagOffsetChange) as the following element
            CustomType = ElementType.Unsigned)]     //  (or this element depending on your view) isn't read in
        public UInt32 DefFlags;                     //  i.e. DefFlags OR EnvFlags are read in (not sure which), thus after reading
                                                    //       dwDefFlags, just move the offset (back) the size of the element (-4),
        [XmlCookedAttribute(                        //       and read EnvFlags as if both elements were there
            Name = "Flags",
            DefaultValue = false,
            ElementType = ElementType.Flag,
            FlagId = 1)]
        public EnvironmentDefFlags EnvFlags;

        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.NonCookedInt32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "szSkyBoxFileName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String SkyBoxFileName;

        [XmlCookedAttribute(
            Name = "nSkyboxDefID",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int SkyboxDefId;

        [XmlCookedAttribute(
            Name = "szEnvMapFileName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String EnvMapFileName;

        [XmlCookedAttribute(
            Name = "szBackgroundLightingEnvMapFileName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String BackgroundLightingEnvMapFileName;

        [XmlCookedAttribute(
            Name = "szAppearanceLightingEnvMapFileName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String AppearanceLightingEnvMapFileName;

        [XmlCookedAttribute(
            Name = "nBackgroundLightingEnvMap",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int BackgroundLightingEnvMap;

        [XmlCookedAttribute(
            Name = "nAppearanceLightingEnvMap",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int AppearanceLightingEnvMap;

        [XmlCookedAttribute(
            Name = "nEnvMapTextureID",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int EnvMapTextureId;

        [XmlCookedAttribute(
            Name = "nEnvMapMIPLevels",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public int EnvMapMIPLevels;

        [XmlCookedAttribute(
            Name = "fWindMin",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float WindMin;

        [XmlCookedAttribute(
            Name = "fWindMax",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float WindMax;

        [XmlCookedAttribute(Name = "vWindDirection", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "vWindDirection.fX",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vWindDirection.fY",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "vWindDirection.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 WindDirection = new Vector3();

        [XmlCookedAttribute(
            Name = "nClipDistance",
            DefaultValue = 100,
            ElementType = ElementType.Int32)]
        public int ClipDistance;

        [XmlCookedAttribute(
            Name = "nSilhouetteDistance",
            DefaultValue = 60,
            ElementType = ElementType.Int32)]
        public int SilhouetteDistance;

        [XmlCookedAttribute(
            Name = "nFogStartDistance",
            DefaultValue = 30,
            ElementType = ElementType.Int32)]
        public int FogStartDistance;

        [XmlCookedAttribute(
            Name = "tFogColor",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] FogColor;

        [XmlCookedAttribute(
            Name = "tAmbientColor",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] AmbientColor;

        [XmlCookedAttribute(
            Name = "fAmbientIntensity",
            DefaultValue = 0.25f,
            ElementType = ElementType.Float)]
        public float AmbientIntensity;

        [XmlCookedAttribute(
            Name = "tBackgroundColor",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] BackgroundColor;

        [XmlCookedAttribute(
            Name = "tHemiLightColors0",
            TrueName = "tHemiLightColors[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] HemiLightColors0;

        [XmlCookedAttribute(
            Name = "tHemiLightColors1",
            TrueName = "tHemiLightColors[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] HemiLightColors1;

        [XmlCookedAttribute(
            Name = "fHemiLightIntensity",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float HemiLightIntensity;

        [XmlCookedAttribute(
            Name = "fShadowIntensity",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float ShadowIntensity;

        [XmlCookedAttribute(
            Name = "nLocation",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public int Location;

        [XmlCookedAttribute(
            Name = "fBackgroundSHIntensity",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float BackgroundSHIntensity;

        [XmlCookedAttribute(
            Name = "fAppearanceSHIntensity",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float AppearanceSHIntensity;

        [XmlCookedAttribute(
            Name = "fCharacterLight_Distance",
            DefaultValue = 2.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float CharacterLightDistance;

        [XmlCookedAttribute(
            Name = "fCharacterLight_FalloffStart",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float CharacterLightFalloffStart;

        [XmlCookedAttribute(
            Name = "fCharacterLight_FalloffEnd",
            DefaultValue = 3.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float CharacterLightFalloffEnd;

        [XmlCookedAttribute(
            Name = "tCharacterLight_Color",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4,
            IsResurrection = true)]
        public Vector4 CharacterLightColor;

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O20", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfRed0",
            TrueName = "tBackgroundSHCoefs_O2.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfGreen0",
            TrueName = "tBackgroundSHCoefs_O2.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfBlue0",
            TrueName = "tBackgroundSHCoefs_O2.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO20 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O21", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfRed1",
            TrueName = "tBackgroundSHCoefs_O2.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfGreen1",
            TrueName = "tBackgroundSHCoefs_O2.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfBlue1",
            TrueName = "tBackgroundSHCoefs_O2.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO21 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O22", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfRed2",
            TrueName = "tBackgroundSHCoefs_O2.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfGreen2",
            TrueName = "tBackgroundSHCoefs_O2.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfBlue2",
            TrueName = "tBackgroundSHCoefs_O2.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO22 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O23", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfRed3",
            TrueName = "tBackgroundSHCoefs_O2.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfGreen3",
            TrueName = "tBackgroundSHCoefs_O2.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O2.pfBlue3",
            TrueName = "tBackgroundSHCoefs_O2.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO23 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O20", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfRed0",
            TrueName = "tAppearanceSHCoefs_O2.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfGreen0",
            TrueName = "tAppearanceSHCoefs_O2.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfBlue0",
            TrueName = "tAppearanceSHCoefs_O2.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO20 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O21", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfRed1",
            TrueName = "tAppearanceSHCoefs_O2.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfGreen1",
            TrueName = "tAppearanceSHCoefs_O2.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfBlue1",
            TrueName = "tAppearanceSHCoefs_O2.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO21 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O22", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfRed2",
            TrueName = "tAppearanceSHCoefs_O2.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfGreen2",
            TrueName = "tAppearanceSHCoefs_O2.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfBlue2",
            TrueName = "tAppearanceSHCoefs_O2.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO22 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O23", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfRed3",
            TrueName = "tAppearanceSHCoefs_O2.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfGreen3",
            TrueName = "tAppearanceSHCoefs_O2.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O2.pfBlue3",
            TrueName = "tAppearanceSHCoefs_O2.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO23 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O30", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed0",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen0",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue0",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO30 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O31", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed1",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen1",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue1",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO31 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O32", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed2",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen2",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue2",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO32 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O33", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed3",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen3",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue3",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO33 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O34", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed4",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen4",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue4",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO34 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O35", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed5",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen5",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue5",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO35 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O36", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed6",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen6",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue6",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO36 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O37", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed7",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen7",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue7",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO37 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefs_O38", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfRed8",
            TrueName = "tBackgroundSHCoefs_O3.pfRed[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfGreen8",
            TrueName = "tBackgroundSHCoefs_O3.pfGreen[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefs_O3.pfBlue8",
            TrueName = "tBackgroundSHCoefs_O3.pfBlue[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsO38 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O30", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed0",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen0",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue0",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO30 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O31", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed1",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen1",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue1",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO31 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O32", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed2",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen2",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue2",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO32 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O33", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed3",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen3",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue3",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO33 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O34", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed4",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen4",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue4",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO34 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O35", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed5",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen5",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue5",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO35 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O36", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed6",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen6",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue6",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO36 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O37", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed7",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen7",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue7",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO37 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefs_O38", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfRed8",
            TrueName = "tAppearanceSHCoefs_O3.pfRed[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfGreen8",
            TrueName = "tAppearanceSHCoefs_O3.pfGreen[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefs_O3.pfBlue8",
            TrueName = "tAppearanceSHCoefs_O3.pfBlue[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsO38 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O20", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfRed0",
            TrueName = "tBackgroundSHCoefsLin_O2.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfGreen0",
            TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfBlue0",
            TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO20 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O21", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfRed1",
            TrueName = "tBackgroundSHCoefsLin_O2.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfGreen1",
            TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfBlue1",
            TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO21 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O22", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfRed2",
            TrueName = "tBackgroundSHCoefsLin_O2.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfGreen2",
            TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfBlue2",
            TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO22 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O23", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfRed3",
            TrueName = "tBackgroundSHCoefsLin_O2.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfGreen3",
            TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O2.pfBlue3",
            TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO23 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O20", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfRed0",
            TrueName = "tAppearanceSHCoefsLin_O2.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfGreen0",
            TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfBlue0",
            TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO20 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O21", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfRed1",
            TrueName = "tAppearanceSHCoefsLin_O2.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfGreen1",
            TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfBlue1",
            TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO21 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O22", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfRed2",
            TrueName = "tAppearanceSHCoefsLin_O2.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfGreen2",
            TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfBlue2",
            TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO22 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O23", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfRed3",
            TrueName = "tAppearanceSHCoefsLin_O2.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfGreen3",
            TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O2.pfBlue3",
            TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO23 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O30", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed0",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen0",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue0",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO30 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O31", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed1",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen1",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue1",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO31 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O32", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed2",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen2",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue2",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO32 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O33", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed3",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen3",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue3",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO33 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O34", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed4",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen4",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue4",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO34 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O35", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed5",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen5",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue5",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO35 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O36", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed6",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen6",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue6",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO36 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O37", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed7",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen7",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue7",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO37 = new Vector3();

        [XmlCookedAttribute(Name = "tBackgroundSHCoefsLin_O38", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfRed8",
            TrueName = "tBackgroundSHCoefsLin_O3.pfRed[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfGreen8",
            TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tBackgroundSHCoefsLin_O3.pfBlue8",
            TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 BackgroundSHCoefsLinO38 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O30", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed0",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen0",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue0",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[0]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO30 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O31", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed1",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen1",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue1",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[1]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO31 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O32", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed2",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen2",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue2",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[2]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO32 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O33", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed3",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen3",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue3",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[3]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO33 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O34", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed4",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen4",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue4",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[4]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO34 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O35", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed5",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen5",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue5",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[5]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO35 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O36", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed6",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen6",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue6",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[6]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO36 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O37", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed7",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen7",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue7",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[7]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO37 = new Vector3();

        [XmlCookedAttribute(Name = "tAppearanceSHCoefsLin_O38", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfRed8",
            TrueName = "tAppearanceSHCoefsLin_O3.pfRed[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfGreen8",
            TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAppearanceSHCoefsLin_O3.pfBlue8",
            TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[8]",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 AppearanceSHCoefsLinO38 = new Vector3();

        [XmlCookedAttribute(
            Name = "tDirLights",
            DefaultValue = null,
            ElementType = ElementType.TableArrayFixed,
            ChildType = typeof(EnvLightDefinition),
            Count = 3)]
        public EnvLightDefinition[] DirLights;
    }
}