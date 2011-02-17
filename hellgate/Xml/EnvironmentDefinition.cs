using System;

namespace Hellgate.Xml
{
    class EnvironmentDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "dwDefFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32,
                FlagOffsetChange = -sizeof(Int32)
            },
            new XmlCookElement
            {
                Name = "ENVIRONMENTDEF_FLAG_DIR1_OPPOSITE_DIR0",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "ENVIRONMENTDEF_FLAG_HAS_APP_SH_COEFS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "ENVIRONMENTDEF_FLAG_HAS_BG_SH_COEFS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "ENVIRONMENTDEF_FLAG_SPECULAR_FAVOR_FACING",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "ENVIRONMENTDEF_FLAG_FLASHLIGHT_EMISSIVE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "ENVIRONMENTDEF_FLAG_USE_BLOB_SHADOWS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = 0x20,
                IsTestCentre = true
            },
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "szSkyBoxFileName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nSkyboxDefID",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "szEnvMapFileName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "szBackgroundLightingEnvMapFileName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "szAppearanceLightingEnvMapFileName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nBackgroundLightingEnvMap",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nAppearanceLightingEnvMap",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nEnvMapTextureID",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nEnvMapMIPLevels",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "fWindMin",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fWindMax",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vWindDirection.fX",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vWindDirection.fY",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vWindDirection.fZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nClipDistance",
                DefaultValue = 100,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSilhouetteDistance",
                DefaultValue = 60,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nFogStartDistance",
                DefaultValue = 30,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tFogColor",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAmbientColor",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "fAmbientIntensity",
                DefaultValue = 0.25f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundColor",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "tHemiLightColors0",
                TrueName = "tHemiLightColors[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "tHemiLightColors1",
                TrueName = "tHemiLightColors[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "fHemiLightIntensity",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fShadowIntensity",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nLocation",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fBackgroundSHIntensity",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fAppearanceSHIntensity",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCharacterLight_Distance",
                DefaultValue = 2.0f,
                ElementType = ElementType.Float,
                IsResurrection = true
            },
            new XmlCookElement
            {
                Name = "fCharacterLight_FalloffStart",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCharacterLight_FalloffEnd",
                DefaultValue = 3.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tCharacterLight_Color",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfRed0",
                TrueName = "tBackgroundSHCoefs_O2.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfGreen0",
                TrueName = "tBackgroundSHCoefs_O2.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfBlue0",
                TrueName = "tBackgroundSHCoefs_O2.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfRed1",
                TrueName = "tBackgroundSHCoefs_O2.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfGreen1",
                TrueName = "tBackgroundSHCoefs_O2.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfBlue1",
                TrueName = "tBackgroundSHCoefs_O2.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfRed2",
                TrueName = "tBackgroundSHCoefs_O2.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfGreen2",
                TrueName = "tBackgroundSHCoefs_O2.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfBlue2",
                TrueName = "tBackgroundSHCoefs_O2.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfRed3",
                TrueName = "tBackgroundSHCoefs_O2.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfGreen3",
                TrueName = "tBackgroundSHCoefs_O2.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O2.pfBlue3",
                TrueName = "tBackgroundSHCoefs_O2.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfRed0",
                TrueName = "tAppearanceSHCoefs_O2.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfGreen0",
                TrueName = "tAppearanceSHCoefs_O2.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfBlue0",
                TrueName = "tAppearanceSHCoefs_O2.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfRed1",
                TrueName = "tAppearanceSHCoefs_O2.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfGreen1",
                TrueName = "tAppearanceSHCoefs_O2.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfBlue1",
                TrueName = "tAppearanceSHCoefs_O2.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfRed2",
                TrueName = "tAppearanceSHCoefs_O2.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfGreen2",
                TrueName = "tAppearanceSHCoefs_O2.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfBlue2",
                TrueName = "tAppearanceSHCoefs_O2.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfRed3",
                TrueName = "tAppearanceSHCoefs_O2.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfGreen3",
                TrueName = "tAppearanceSHCoefs_O2.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O2.pfBlue3",
                TrueName = "tAppearanceSHCoefs_O2.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed0",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen0",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue0",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed1",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen1",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue1",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed2",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen2",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue2",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed3",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen3",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue3",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed4",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen4",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue4",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed5",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen5",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue5",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed6",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen6",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue6",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed7",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen7",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue7",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfRed8",
                TrueName = "tBackgroundSHCoefs_O3.pfRed[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfGreen8",
                TrueName = "tBackgroundSHCoefs_O3.pfGreen[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefs_O3.pfBlue8",
                TrueName = "tBackgroundSHCoefs_O3.pfBlue[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed0",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen0",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue0",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed1",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen1",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue1",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed2",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen2",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue2",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed3",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen3",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue3",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed4",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen4",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue4",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed5",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen5",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue5",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed6",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen6",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue6",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed7",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen7",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue7",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfRed8",
                TrueName = "tAppearanceSHCoefs_O3.pfRed[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfGreen8",
                TrueName = "tAppearanceSHCoefs_O3.pfGreen[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefs_O3.pfBlue8",
                TrueName = "tAppearanceSHCoefs_O3.pfBlue[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfRed0",
                TrueName = "tBackgroundSHCoefsLin_O2.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfGreen0",
                TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfBlue0",
                TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfRed1",
                TrueName = "tBackgroundSHCoefsLin_O2.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfGreen1",
                TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfBlue1",
                TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfRed2",
                TrueName = "tBackgroundSHCoefsLin_O2.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfGreen2",
                TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfBlue2",
                TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfRed3",
                TrueName = "tBackgroundSHCoefsLin_O2.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfGreen3",
                TrueName = "tBackgroundSHCoefsLin_O2.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O2.pfBlue3",
                TrueName = "tBackgroundSHCoefsLin_O2.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfRed0",
                TrueName = "tAppearanceSHCoefsLin_O2.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfGreen0",
                TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfBlue0",
                TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfRed1",
                TrueName = "tAppearanceSHCoefsLin_O2.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfGreen1",
                TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfBlue1",
                TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfRed2",
                TrueName = "tAppearanceSHCoefsLin_O2.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfGreen2",
                TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfBlue2",
                TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfRed3",
                TrueName = "tAppearanceSHCoefsLin_O2.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfGreen3",
                TrueName = "tAppearanceSHCoefsLin_O2.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O2.pfBlue3",
                TrueName = "tAppearanceSHCoefsLin_O2.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed0",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen0",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue0",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed1",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen1",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue1",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed2",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen2",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue2",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed3",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen3",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue3",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed4",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen4",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue4",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed5",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen5",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue5",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed6",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen6",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue6",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed7",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen7",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue7",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfRed8",
                TrueName = "tBackgroundSHCoefsLin_O3.pfRed[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfGreen8",
                TrueName = "tBackgroundSHCoefsLin_O3.pfGreen[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tBackgroundSHCoefsLin_O3.pfBlue8",
                TrueName = "tBackgroundSHCoefsLin_O3.pfBlue[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed0",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen0",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue0",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[0]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed1",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen1",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue1",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[1]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed2",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen2",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue2",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[2]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed3",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen3",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue3",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[3]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed4",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen4",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue4",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[4]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed5",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen5",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue5",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[5]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed6",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen6",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue6",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[6]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed7",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen7",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue7",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[7]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfRed8",
                TrueName = "tAppearanceSHCoefsLin_O3.pfRed[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfGreen8",
                TrueName = "tAppearanceSHCoefsLin_O3.pfGreen[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAppearanceSHCoefsLin_O3.pfBlue8",
                TrueName = "tAppearanceSHCoefsLin_O3.pfBlue[8]",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tDirLights",
                DefaultValue = null,
                ElementType = ElementType.TableArrayFixed,
                ChildType = typeof (EnvLightDefinition),
                Count = 3
            }
        };

        public EnvironmentDefinition()
        {
            RootElement = "ENVIRONMENT_DEFINITION";
            base.Elements.AddRange(Elements);
            Flags = new Int32[] { -1 };
        }
    }
}