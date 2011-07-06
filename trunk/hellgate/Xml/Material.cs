using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "MATERIAL")]
    public class Material
    {
        [XmlCookedAttribute(
            Name = "fSpecularGlow",
            ElementType = ElementType.Float,
            DefaultValue = 0.1f)]
        public float SpecularGlow;

        [XmlCookedAttribute(
            Name = "fLightmapGlow",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float LightmapGlow;

        [XmlCookedAttribute(
            Name = "pszShaderName",
            ElementType = ElementType.String,
            DefaultValue = "Appearance")]
        public String ShaderName;

        [XmlCookedAttribute(
            Name = "dwFlags",
            ElementType = ElementType.Int32,
            DefaultValue = (UInt32)0,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "fGlossiness0",
            TrueName = "fGlossiness[0]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float Glossiness0;

        [XmlCookedAttribute(
            Name = "fGlossiness1",
            TrueName = "fGlossiness[1]",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float Glossiness1;

        [XmlCookedAttribute(
            Name = "fSpecularLevel0",
            TrueName = "fSpecularLevel[0]",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float SpecularLevel0;

        [XmlCookedAttribute(
            Name = "nShaderLineId",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 ShaderLineId;

        [XmlCookedAttribute(
            Name = "nSpecularLUTId",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 SpecularLUTId;

        [XmlCookedAttribute(
            Name = "fSpecularMaskOverride",
            ElementType = ElementType.Float,
            DefaultValue = 0.5f)]
        public float SpecularMaskOverride;

        [XmlCookedAttribute(
            Name = "fEnvMapPower",
            ElementType = ElementType.Float,
            DefaultValue = 0.1f)]
        public float EnvMapPower;

        [XmlCookedAttribute(
            Name = "fEnvMapGlossThreshold",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float EnvMapGlossThreshold;

        [XmlCookedAttribute(
            Name = "fEnvMapBlurriness",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float EnvMapBlurriness;

        [XmlCookedAttribute(
            Name = "szEnvMapFileName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String EnvMapFileName;

        [XmlCookedAttribute(
            Name = "nCubeMapTextureID",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 CubeMapTextureId;

        [XmlCookedAttribute(
            Name = "nCubeMapMIPLevels",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = 0)]
        public Int32 CubeMapMIPLevels;

        [XmlCookedAttribute(
            Name = "nSphereMapTextureID",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 SphereMapTextureId;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_DIFFUSE",
            TrueName = "nOverrideTextureID[ TEXTURE_DIFFUSE ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureDiffuse;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_NORMAL",
            TrueName = "nOverrideTextureID[ TEXTURE_NORMAL ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureNormal;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_SELFILLUM",
            TrueName = "nOverrideTextureID[ TEXTURE_SELFILLUM ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureSelfillum;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_DIFFUSE2",
            TrueName = "nOverrideTextureID[ TEXTURE_DIFFUSE2 ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureDiffuse2;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_SPECULAR",
            TrueName = "nOverrideTextureID[ TEXTURE_SPECULAR ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureSpecular;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_ENVMAP",
            TrueName = "nOverrideTextureID[ TEXTURE_ENVMAP ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureEnvmap;

        [XmlCookedAttribute(
            Name = "nOverrideTextureID_TEXTURE_LIGHTMAP",
            TrueName = "nOverrideTextureID[ TEXTURE_LIGHTMAP ]",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 OverrideTextureIdTextureLightmap;

        [XmlCookedAttribute(
            Name = "szDiffuseOverrideFileName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String DiffuseOverrideFileName;

        [XmlCookedAttribute(
            Name = "szNormalOverrideFileName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String NormalOverrideFileName;

        [XmlCookedAttribute(
            Name = "szSelfIllumOverrideFileName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String SelfIllumOverrideFileName;

        [XmlCookedAttribute(
            Name = "szDiffuse2OverrideFileName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String Diffuse2OverrideFileName;

        [XmlCookedAttribute(
            Name = "szSpecularOverrideFileName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String SpecularOverrideFileName;

        [XmlCookedAttribute(
            Name = "fSelfIlluminationMax",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float SelfIlluminationMax;

        [XmlCookedAttribute(
            Name = "fSelfIlluminationMin",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float SelfIlluminationMin;

        [XmlCookedAttribute(
            Name = "fSelfIlluminationNoiseFreq",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float SelfIlluminationNoiseFreq;

        [XmlCookedAttribute(
            Name = "fSelfIlluminationNoiseAmp",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float SelfIlluminationNoiseAmp;

        [XmlCookedAttribute(
            Name = "fSelfIlluminationWaveHertz",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float SelfIlluminationWaveHertz;

        [XmlCookedAttribute(
            Name = "fSelfIlluminationWaveAmp",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float SelfIlluminationWaveAmp;

        [XmlCookedAttribute(
            Name = "fScrollRateU0",
            TrueName = "fScrollRateU[0]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollRateU0;

        [XmlCookedAttribute(
            Name = "fScrollRateV0",
            TrueName = "fScrollRateV[0]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollRateV0;

        [XmlCookedAttribute(
            Name = "fScrollTileU0",
            TrueName = "fScrollTileU[0]",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float ScrollTileU0;

        [XmlCookedAttribute(
            Name = "fScrollTileV0",
            TrueName = "fScrollTileV[0]",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float ScrollTileV0;

        [XmlCookedAttribute(
            Name = "fScrollPhaseU0",
            TrueName = "fScrollPhaseU[0]",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 0.0f)]
        public float ScrollPhaseU0;

        [XmlCookedAttribute(
            Name = "fScrollPhaseV0",
            TrueName = "fScrollPhaseV[0]",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 0.0f)]
        public float ScrollPhaseV0;

        [XmlCookedAttribute(
            Name = "fScrollRateU1",
            TrueName = "fScrollRateU[1]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollRateU1;

        [XmlCookedAttribute(
            Name = "fScrollRateV1",
            TrueName = "fScrollRateV[1]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollRateV1;

        [XmlCookedAttribute(
            Name = "fScrollTileU1",
            TrueName = "fScrollTileU[1]",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float ScrollTileU1;

        [XmlCookedAttribute(
            Name = "fScrollTileV1",
            TrueName = "fScrollTileV[1]",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f)]
        public float ScrollTileV1;

        [XmlCookedAttribute(
            Name = "fScrollPhaseU1",
            TrueName = "fScrollPhaseU[1]",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 0.0f)]
        public float ScrollPhaseU1;

        [XmlCookedAttribute(
            Name = "fScrollPhaseV1",
            TrueName = "fScrollPhaseV[1]",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 0.0f)]
        public float ScrollPhaseV1;

        [XmlCookedAttribute(
            Name = "fScrollAmt_SAMPLER_DIFFUSE",
            TrueName = "fScrollAmt[SAMPLER_DIFFUSE]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollAmtSamplerDiffuse;

        [XmlCookedAttribute(
            Name = "fScrollAmt_SAMPLER_NORMAL",
            TrueName = "fScrollAmt[SAMPLER_NORMAL]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollAmtSamplerNormal;

        [XmlCookedAttribute(
            Name = "fScrollAmt_SAMPLER_SPECULAR",
            TrueName = "fScrollAmt[SAMPLER_SPECULAR]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollAmtSamplerSpecular;

        [XmlCookedAttribute(
            Name = "fScrollAmt_SAMPLER_SELFILLUM",
            TrueName = "fScrollAmt[SAMPLER_SELFILLUM]",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float ScrollAmtSamplerSelfillum;

        [XmlCookedAttribute(
            Name = "fScrollAmtLCM",
            ElementType = ElementType.NonCookedFloat,
            DefaultValue = 1.0f)]
        public float ScrollAmtLCM;

        [XmlCookedAttribute(
            Name = "szParticleSystemDef",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String ParticleSystemDef;

        [XmlCookedAttribute(
            Name = "nParticleSystemDefId",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)]
        public Int32 ParticleSystemDefId;

        [XmlCookedAttribute(
            Name = "fScatterIntensity",
            ElementType = ElementType.Float,
            DefaultValue = 0.3f)]
        public float ScatterIntensity;

        [XmlCookedAttribute(
            Name = "fScatterSharpness",
            ElementType = ElementType.Float,
            DefaultValue = 0.5f)]
        public float ScatterSharpness;

        [XmlCookedAttribute(
            Name = "tScatterColor",
            ElementType = ElementType.FloatQuadArrayVariable,
            DefaultValue = 0.7f,
            Count = 4,
            CustomType = ElementType.Vector4)]
        public Vector4[] ScatterColors;

        [XmlCookedAttribute(
            Name = "bNoCastShadow",
            ElementType = ElementType.Int32,
            DefaultValue = false,
            IsResurrection = true,
            CustomType = ElementType.Bool)]
        public bool NoCastShadow;

        [XmlCookedAttribute(
            Name = "fSpecularPower",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f,
            IsResurrection = true)]
        public float SpecularPower;

        [XmlCookedAttribute(
            Name = "fNormalPower",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f,
            IsResurrection = true)]
        public float NormalPower;
    }
}