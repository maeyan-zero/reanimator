namespace Hellgate.Xml
{
    class Material : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "fSpecularGlow",
                ElementType = ElementType.Float,
                DefaultValue = 0.1f
            },
            new XmlCookElement
            {
                Name = "fLightmapGlow",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "pszShaderName",
                ElementType = ElementType.String,
                DefaultValue = "Appearance"
            },
            new XmlCookElement
            {
                Name = "dwFlags",
                ElementType = ElementType.Int32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "fGlossiness0",
                TrueName = "fGlossiness[0]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fGlossiness1",
                TrueName = "fGlossiness[1]",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fSpecularLevel0",
                TrueName = "fSpecularLevel[0]",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "nShaderLineId",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nSpecularLUTId",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "fSpecularMaskOverride",
                ElementType = ElementType.Float,
                DefaultValue = 0.5f
            },
            new XmlCookElement
            {
                Name = "fEnvMapPower",
                ElementType = ElementType.Float,
                DefaultValue = 0.1f
            },
            new XmlCookElement
            {
                Name = "fEnvMapGlossThreshold",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fEnvMapBlurriness",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "szEnvMapFileName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "nCubeMapTextureID",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nCubeMapMIPLevels",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "nSphereMapTextureID",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_DIFFUSE",
                TrueName = "nOverrideTextureID[ TEXTURE_DIFFUSE ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_NORMAL",
                TrueName = "nOverrideTextureID[ TEXTURE_NORMAL ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_SELFILLUM",
                TrueName = "nOverrideTextureID[ TEXTURE_SELFILLUM ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_DIFFUSE2",
                TrueName = "nOverrideTextureID[ TEXTURE_DIFFUSE2 ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_SPECULAR",
                TrueName = "nOverrideTextureID[ TEXTURE_SPECULAR ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_ENVMAP",
                TrueName = "nOverrideTextureID[ TEXTURE_ENVMAP ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "nOverrideTextureID_TEXTURE_LIGHTMAP",
                TrueName = "nOverrideTextureID[ TEXTURE_LIGHTMAP ]",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "szDiffuseOverrideFileName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "szNormalOverrideFileName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "szSelfIllumOverrideFileName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "szDiffuse2OverrideFileName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "szSpecularOverrideFileName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "fSelfIlluminationMax",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fSelfIlluminationMin",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fSelfIlluminationNoiseFreq",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fSelfIlluminationNoiseAmp",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fSelfIlluminationWaveHertz",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fSelfIlluminationWaveAmp",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollRateU0",
                TrueName = "fScrollRateU[0]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollRateV0",
                TrueName = "fScrollRateV[0]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollTileU0",
                TrueName = "fScrollTileU[0]",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fScrollTileV0",
                TrueName = "fScrollTileV[0]",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fScrollPhaseU0",
                TrueName = "fScrollPhaseU[0]",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollPhaseV0",
                TrueName = "fScrollPhaseV[0]",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollRateU1",
                TrueName = "fScrollRateU[1]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollRateV1",
                TrueName = "fScrollRateV[1]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollTileU1",
                TrueName = "fScrollTileU[1]",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fScrollTileV1",
                TrueName = "fScrollTileV[1]",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "fScrollPhaseU1",
                TrueName = "fScrollPhaseU[1]",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollPhaseV1",
                TrueName = "fScrollPhaseV[1]",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollAmt_SAMPLER_DIFFUSE",
                TrueName = "fScrollAmt[SAMPLER_DIFFUSE]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollAmt_SAMPLER_NORMAL",
                TrueName = "fScrollAmt[SAMPLER_NORMAL]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollAmt_SAMPLER_SPECULAR",
                TrueName = "fScrollAmt[SAMPLER_SPECULAR]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollAmt_SAMPLER_SELFILLUM",
                TrueName = "fScrollAmt[SAMPLER_SELFILLUM]",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fScrollAmtLCM",
                ElementType = ElementType.NonCookedFloat,
                DefaultValue = 1.0f
            },
            new XmlCookElement
            {
                Name = "szParticleSystemDef",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "nParticleSystemDefId",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1
            },
            new XmlCookElement
            {
                Name = "fScatterIntensity",
                ElementType = ElementType.Float,
                DefaultValue = 0.3f
            },
            new XmlCookElement
            {
                Name = "fScatterSharpness",
                ElementType = ElementType.Float,
                DefaultValue = 0.5f
            },
            new XmlCookElement
            {
                Name = "tScatterColor",
                ElementType = ElementType.FloatQuadArrayVariable,
                DefaultValue = 0.7f,
                Count = 4
            },
            new XmlCookElement
            {
                Name = "bNoCastShadow",
                ElementType = ElementType.Int32,
                DefaultValue = 0,
                IsResurrection = true
            },
            new XmlCookElement
            {
                Name = "fSpecularPower",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f,
                IsResurrection = true
            },
            new XmlCookElement
            {
                Name = "fNormalPower",
                ElementType = ElementType.Float,
                DefaultValue = 1.0f,
                IsResurrection = true
            }
        };

        public Material()
        {
            RootElement = "MATERIAL";
            base.Elements.AddRange(Elements);
        }
    }
}
