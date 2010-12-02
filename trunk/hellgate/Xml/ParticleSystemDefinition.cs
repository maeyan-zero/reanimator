namespace Hellgate.Xml
{
    class ParticleSystemDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "dwFlags2",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "dwFlags3",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "dwUpdateFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32_0x0A00
            },
            new XmlCookElement
            {
                Name = "dwRuntimeFlags",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nLighting",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nLaunchParticleCount",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fMinParticlesPercentDropRate",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float,
                IsTCv4 = true
            },
            new XmlCookElement
            {
                Name = "pszTextureName",
                DefaultValue = "glow1.tga",
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nGPUShader",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nSoundGroup",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 20784 // SOUNDS
            },
            new XmlCookElement
            {
                Name = "nFootstep",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 14385 // FOOTSTEPS
            },
            new XmlCookElement
            {
                Name = "nVolume",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fSoundPlayChance",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pszLightName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nLightDefId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pszModelDefName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nModelDefId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pszNextParticleSystem",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nNextParticleSystem",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pszFollowParticleSystem",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nFollowParticleSystem",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pszRopeEndParticleSystem",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nRopeEndParticleSystem",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pszRopePath",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nRopePathId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "pszDyingParticleSystem",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nDyingParticleSystem",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "fDuration",
                DefaultValue = 10.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fStartDelay",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tLaunchScale",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleScale",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleRotation",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleAcceleration",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleBounce",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleTurnSpeed",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleWorldAccelerationZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleCenterX",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleCenterY",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleCenterRotation",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleWindInfluence",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleDurationPath",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticlesPerSecondPath",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticlesPerMeterPerSecond",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticlesPerMeter",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleBurst",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleDistortionStrength",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchOffsetX",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchOffsetY",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchOffsetZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchSphereRadius",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchCylinderRadius",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchCylinderHeight",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchVelocityFromSystem",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchSpeedFromSystemForward",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchDirRotation",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchDirPitch",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchSpeed",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRotation",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAnimationRate",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAnimationSlidingRateX",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAnimationSlidingRateY",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRopeScale",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRopeAlpha",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeAlpha",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeWorldAccelerationZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRopeSpringiness",
                DefaultValue = 10.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRopeStiffness",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeWaveAmplitudeUp",
                DefaultValue = 0.1f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeWaveAmplitudeSide",
                DefaultValue = 0.1f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeWaveFrequency",
                DefaultValue = 0.1f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeWaveSpeed",
                DefaultValue = 0.1f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeDampening",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeZOffsetOverTime",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleAlpha",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleGlow",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleSpeedBounds",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleColor",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRopeColor",
                DefaultValue = 0.5f,
                ElementType = ElementType.FloatQuadArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleStretchBox",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleStretchDiamond",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tLaunchRopeGlow",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeGlow",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopeMetersPerTexture",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tRopePathScale",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorOffsetNormal",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorOffsetSideX",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorOffsetSideY",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorWorldOffsetZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tParticleAttractorAcceleration",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorDestructionRadius",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorForceRadius",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAttractorForceOverRadius",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAlphaRef",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tAlphaMin",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "nMeshesPerBatchMax",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nDrawOrder",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nShaderType",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 18993 // EFFECTS_SHADERS
            },
            new XmlCookElement
            {
                Name = "nTextureId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "fCullDistance",
                DefaultValue = 10.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nCullPriority",
                DefaultValue = -1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fViewSpeed",
                DefaultValue = 10.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fRopeEndSpeed",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fTrailKnotDuration",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fCircleRadius",
                DefaultValue = 3.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fViewCircleRadius",
                DefaultValue = 3.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fViewHosePressure",
                DefaultValue = 10.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fViewRangePercent",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fViewNovaAngle",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nViewParticleSpawnThrottle",
                DefaultValue = 50,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nKnotCount",
                DefaultValue = 5,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nKnotCountMax",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fSegmentSize",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tFluidSmokeThickness",
                DefaultValue = 0.380333f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tFluidSmokeAmbientLight",
                DefaultValue = 0.088f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tGlowMinDensity",
                DefaultValue = 0.4f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tFluidSmokeDensityModifier",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "tFluidSmokeVelocityModifier",
                DefaultValue = 1.0f,
                ElementType = ElementType.FloatTripletArrayVariable
            },
            new XmlCookElement
            {
                Name = "vGlowCompensationColor.x",
                DefaultValue = -0.17f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vGlowCompensationColor.y",
                DefaultValue = -0.8f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "vGlowCompensationColor.z",
                DefaultValue = -1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nGridWidth",
                DefaultValue = 128,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nGridDepth",
                DefaultValue = 100,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nGridHeight",
                DefaultValue = 128,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nGridBorder",
                DefaultValue = 10,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fVelocityMultiplier",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fVelocityClamp",
                DefaultValue = 4.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fSizeMultiplier",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fVorticityConfinementScale",
                DefaultValue = 0.22f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "nGridDensityTextureIndex",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nGridVelocityTextureIndex",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "nGridObstructorTextureIndex",
                DefaultValue = 5,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fRenderScale",
                DefaultValue = 7.05f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pszTextureDensityName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszTextureVelocityName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "pszTextureObstructorName",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nDensityTextureId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nVelocityTextureId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nObstructorTextureId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "vLightOffset",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayFixed,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "fSoftParticleScale",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fSoftParticleContrast",
                DefaultValue = 2.0f,
                ElementType = ElementType.Float
            }
        };

        public ParticleSystemDefinition()
        {
            RootElement = "PARTICLE_SYSTEM_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}