using System;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "PARTICLE_SYSTEM_DEFINITION")]
    public class ParticleSystemDefinition
    {
        [XmlCookedAttribute(
            Name = "dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags;

        [XmlCookedAttribute(
            Name = "dwFlags2",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags2;

        [XmlCookedAttribute(
            Name = "dwFlags3",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 Flags3;

        [XmlCookedAttribute(
            Name = "dwUpdateFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32_0x0A00,
            CustomType = ElementType.Unsigned)]
        public UInt32 UpdateFlags;

        [XmlCookedAttribute(
            Name = "dwRuntimeFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.NonCookedInt32,
            CustomType = ElementType.Unsigned)]
        public UInt32 RuntimeFlags;

        [XmlCookedAttribute(
            Name = "nLighting",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 Lighting;

        [XmlCookedAttribute(
            Name = "nLaunchParticleCount",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 LaunchParticleCount;

        [XmlCookedAttribute(
            Name = "fMinParticlesPercentDropRate",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float,
            IsTestCentre = true)]
        public float MinParticlesPercentDropRate;

        [XmlCookedAttribute(
            Name = "pszTextureName",
            DefaultValue = "glow1.tga",
            ElementType = ElementType.String)]
        public String TextureName;

        [XmlCookedAttribute(
            Name = "nGPUShader",
            DefaultValue = -1,
            ElementType = ElementType.Int32)]
        public Int32 GPUShader;

        [XmlCookedAttribute(
            Name = "nSoundGroup",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.SOUNDS)] // 20784 SOUNDS
        public SoundsRow SoundGroup;
        public Int32 SoundGroupRowIndex;

        [XmlCookedAttribute(
            Name = "nFootstep",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.FOOTSTEPS)] // 14385 FOOTSTEPS
        public FootSteps Footstep;
        public Int32 FootstepRowIndex;

        [XmlCookedAttribute(
            Name = "nVolume",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 Volume;

        [XmlCookedAttribute(
            Name = "fSoundPlayChance",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float SoundPlayChance;

        [XmlCookedAttribute(
            Name = "pszLightName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String LightName;

        [XmlCookedAttribute(
            Name = "nLightDefId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 LightDefId;

        [XmlCookedAttribute(
            Name = "pszModelDefName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String ModelDefName;

        [XmlCookedAttribute(
            Name = "nModelDefId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 ModelDefId;

        [XmlCookedAttribute(
            Name = "pszNextParticleSystem",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String NextParticleSystem;

        [XmlCookedAttribute(
            Name = "nNextParticleSystem",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 NextParticleSystemId;

        [XmlCookedAttribute(
            Name = "pszFollowParticleSystem",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String FollowParticleSystem;

        [XmlCookedAttribute(
            Name = "nFollowParticleSystem",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 FollowParticleSystemId;

        [XmlCookedAttribute(
            Name = "pszRopeEndParticleSystem",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String RopeEndParticleSystem;

        [XmlCookedAttribute(
            Name = "nRopeEndParticleSystem",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 RopeEndParticleSystemId;

        [XmlCookedAttribute(
            Name = "pszRopePath",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String RopePath;

        [XmlCookedAttribute(
            Name = "nRopePathId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 RopePathId;

        [XmlCookedAttribute(
            Name = "pszDyingParticleSystem",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String DyingParticleSystem;

        [XmlCookedAttribute(
            Name = "nDyingParticleSystem",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 DyingParticleSystemId;

        [XmlCookedAttribute(
            Name = "fDuration",
            DefaultValue = 10.0f,
            ElementType = ElementType.Float)]
        public float Duration;

        [XmlCookedAttribute(
            Name = "fStartDelay",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float StartDelay;

        [XmlCookedAttribute(
            Name = "tLaunchScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchScale;

        [XmlCookedAttribute(
            Name = "tParticleScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleScale;

        [XmlCookedAttribute(
            Name = "tParticleRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleRotation;

        [XmlCookedAttribute(
            Name = "tParticleAcceleration",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleAcceleration;

        [XmlCookedAttribute(
            Name = "tParticleBounce",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleBounce;

        [XmlCookedAttribute(
            Name = "tParticleTurnSpeed",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleTurnSpeed;

        [XmlCookedAttribute(
            Name = "tParticleWorldAccelerationZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleWorldAccelerationZ;

        [XmlCookedAttribute(
            Name = "tParticleCenterX",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleCenterX;

        [XmlCookedAttribute(
            Name = "tParticleCenterY",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleCenterY;

        [XmlCookedAttribute(
            Name = "tParticleCenterRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleCenterRotation;

        [XmlCookedAttribute(
            Name = "tParticleWindInfluence",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleWindInfluence;

        [XmlCookedAttribute(
            Name = "tParticleDurationPath",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleDurationPath;

        [XmlCookedAttribute(
            Name = "tParticlesPerSecondPath",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticlesPerSecondPath;

        [XmlCookedAttribute(
            Name = "tParticlesPerMeterPerSecond",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticlesPerMeterPerSecond;

        [XmlCookedAttribute(
            Name = "tParticlesPerMeter",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticlesPerMeter;

        [XmlCookedAttribute(
            Name = "tParticleBurst",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleBurst;

        [XmlCookedAttribute(
            Name = "tParticleDistortionStrength",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleDistortionStrength;

        [XmlCookedAttribute(
            Name = "tLaunchOffsetX",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchOffsetX;

        [XmlCookedAttribute(
            Name = "tLaunchOffsetY",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchOffsetY;

        [XmlCookedAttribute(
            Name = "tLaunchOffsetZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchOffsetZ;

        [XmlCookedAttribute(
            Name = "tLaunchSphereRadius",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchSphereRadius;

        [XmlCookedAttribute(
            Name = "tLaunchCylinderRadius",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchCylinderRadius;

        [XmlCookedAttribute(
            Name = "tLaunchCylinderHeight",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchCylinderHeight;

        [XmlCookedAttribute(
            Name = "tLaunchVelocityFromSystem",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchVelocityFromSystem;

        [XmlCookedAttribute(
            Name = "tLaunchSpeedFromSystemForward",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchSpeedFromSystemForward;

        [XmlCookedAttribute(
            Name = "tLaunchDirRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchDirRotation;

        [XmlCookedAttribute(
            Name = "tLaunchDirPitch",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchDirPitch;

        [XmlCookedAttribute(
            Name = "tLaunchSpeed",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchSpeed;

        [XmlCookedAttribute(
            Name = "tLaunchRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchRotation;

        [XmlCookedAttribute(
            Name = "tAnimationRate",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AnimationRate;

        [XmlCookedAttribute(
            Name = "tAnimationSlidingRateX",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AnimationSlidingRateX;

        [XmlCookedAttribute(
            Name = "tAnimationSlidingRateY",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AnimationSlidingRateY;

        [XmlCookedAttribute(
            Name = "tLaunchRopeScale",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchRopeScale;

        [XmlCookedAttribute(
            Name = "tLaunchRopeAlpha",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchRopeAlpha;

        [XmlCookedAttribute(
            Name = "tRopeAlpha",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeAlpha;

        [XmlCookedAttribute(
            Name = "tRopeWorldAccelerationZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeWorldAccelerationZ;

        [XmlCookedAttribute(
            Name = "tLaunchRopeSpringiness",
            DefaultValue = 10.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchRopeSpringiness;

        [XmlCookedAttribute(
            Name = "tLaunchRopeStiffness",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchRopeStiffness;

        [XmlCookedAttribute(
            Name = "tRopeWaveAmplitudeUp",
            DefaultValue = 0.1f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeWaveAmplitudeUp;

        [XmlCookedAttribute(
            Name = "tRopeWaveAmplitudeSide",
            DefaultValue = 0.1f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeWaveAmplitudeSide;

        [XmlCookedAttribute(
            Name = "tRopeWaveFrequency",
            DefaultValue = 0.1f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeWaveFrequency;

        [XmlCookedAttribute(
            Name = "tRopeWaveSpeed",
            DefaultValue = 0.1f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeWaveSpeed;

        [XmlCookedAttribute(
            Name = "tRopeDampening",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeDampening;

        [XmlCookedAttribute(
            Name = "tRopeZOffsetOverTime",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeZOffsetOverTime;

        [XmlCookedAttribute(
            Name = "tParticleAlpha",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleAlpha;

        [XmlCookedAttribute(
            Name = "tParticleGlow",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleGlow;

        [XmlCookedAttribute(
            Name = "tParticleSpeedBounds",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleSpeedBounds;

        [XmlCookedAttribute(
            Name = "tParticleColor",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] ParticleColor;

        [XmlCookedAttribute(
            Name = "tLaunchRopeColor",
            DefaultValue = 0.5f,
            ElementType = ElementType.FloatQuadArrayVariable,
            CustomType = ElementType.Vector4)]
        public Vector4[] LaunchRopeColor;

        [XmlCookedAttribute(
            Name = "tParticleStretchBox",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleStretchBox;

        [XmlCookedAttribute(
            Name = "tParticleStretchDiamond",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleStretchDiamond;

        [XmlCookedAttribute(
            Name = "tLaunchRopeGlow",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] LaunchRopeGlow;

        [XmlCookedAttribute(
            Name = "tRopeGlow",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeGlow;

        [XmlCookedAttribute(
            Name = "tRopeMetersPerTexture",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopeMetersPerTexture;

        [XmlCookedAttribute(
            Name = "tRopePathScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] RopePathScale;

        [XmlCookedAttribute(
            Name = "tAttractorOffsetNormal",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorOffsetNormal;

        [XmlCookedAttribute(
            Name = "tAttractorOffsetSideX",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorOffsetSideX;

        [XmlCookedAttribute(
            Name = "tAttractorOffsetSideY",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorOffsetSideY;

        [XmlCookedAttribute(
            Name = "tAttractorWorldOffsetZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorWorldOffsetZ;

        [XmlCookedAttribute(
            Name = "tParticleAttractorAcceleration",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] ParticleAttractorAcceleration;

        [XmlCookedAttribute(
            Name = "tAttractorDestructionRadius",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorDestructionRadius;

        [XmlCookedAttribute(
            Name = "tAttractorForceRadius",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorForceRadius;

        [XmlCookedAttribute(
            Name = "tAttractorForceOverRadius",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AttractorForceOverRadius;

        [XmlCookedAttribute(
            Name = "tAlphaRef",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AlphaRef;

        [XmlCookedAttribute(
            Name = "tAlphaMin",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] AlphaMin;

        [XmlCookedAttribute(
            Name = "nMeshesPerBatchMax",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 MeshesPerBatchMax;

        [XmlCookedAttribute(
            Name = "nDrawOrder",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 DrawOrder;

        [XmlCookedAttribute(
            Name = "nShaderType",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.EFFECTS_SHADERS)] // 18993 EFFECTS_SHADERS
        public EffectsShaders ShaderType;
        public Int32 ShaderTypeRowIndex;

        [XmlCookedAttribute(
            Name = "nTextureId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 TextureId;

        [XmlCookedAttribute(
            Name = "fCullDistance",
            DefaultValue = 10.0f,
            ElementType = ElementType.Float)]
        public float CullDistance;

        [XmlCookedAttribute(
            Name = "nCullPriority",
            DefaultValue = -1,
            ElementType = ElementType.Int32)]
        public Int32 CullPriority;

        [XmlCookedAttribute(
            Name = "fViewSpeed",
            DefaultValue = 10.0f,
            ElementType = ElementType.Float)]
        public float ViewSpeed;

        [XmlCookedAttribute(
            Name = "fRopeEndSpeed",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float RopeEndSpeed;

        [XmlCookedAttribute(
            Name = "fTrailKnotDuration",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float TrailKnotDuration;

        [XmlCookedAttribute(
            Name = "fCircleRadius",
            DefaultValue = 3.0f,
            ElementType = ElementType.Float)]
        public float CircleRadius;

        [XmlCookedAttribute(
            Name = "fViewCircleRadius",
            DefaultValue = 3.0f,
            ElementType = ElementType.Float)]
        public float ViewCircleRadius;

        [XmlCookedAttribute(
            Name = "fViewHosePressure",
            DefaultValue = 10.0f,
            ElementType = ElementType.Float)]
        public float ViewHosePressure;

        [XmlCookedAttribute(
            Name = "fViewRangePercent",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float ViewRangePercent;

        [XmlCookedAttribute(
            Name = "fViewNovaAngle",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float ViewNovaAngle;

        [XmlCookedAttribute(
            Name = "nViewParticleSpawnThrottle",
            DefaultValue = 50,
            ElementType = ElementType.Int32)]
        public Int32 ViewParticleSpawnThrottle;

        [XmlCookedAttribute(
            Name = "nKnotCount",
            DefaultValue = 5,
            ElementType = ElementType.Int32)]
        public Int32 KnotCount;

        [XmlCookedAttribute(
            Name = "nKnotCountMax",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 KnotCountMax;

        [XmlCookedAttribute(
            Name = "fSegmentSize",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float SegmentSize;

        [XmlCookedAttribute(
            Name = "tFluidSmokeThickness",
            DefaultValue = 0.380333f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] FluidSmokeThicknesses;

        [XmlCookedAttribute(
            Name = "tFluidSmokeAmbientLight",
            DefaultValue = 0.088f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] FluidSmokeAmbientLights;

        [XmlCookedAttribute(
            Name = "tGlowMinDensity",
            DefaultValue = 0.4f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] GlowMinDensities;

        [XmlCookedAttribute(
            Name = "tFluidSmokeDensityModifier",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] FluidSmokeDensityModifiers;

        [XmlCookedAttribute(
            Name = "tFluidSmokeVelocityModifier",
            DefaultValue = 1.0f,
            ElementType = ElementType.FloatTripletArrayVariable,
            CustomType = ElementType.Vector3)]
        public Vector3[] FluidSmokeVelocityModifiers;

        [XmlCookedAttribute(
            Name = "vGlowCompensationColor.x",
            DefaultValue = -0.17f,
            ElementType = ElementType.Float)]
        public float GlowCompensationColorX;

        [XmlCookedAttribute(
            Name = "vGlowCompensationColor.y",
            DefaultValue = -0.8f,
            ElementType = ElementType.Float)]
        public float GlowCompensationColorY;

        [XmlCookedAttribute(
            Name = "vGlowCompensationColor.z",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        public float GlowCompensationColorZ;

        [XmlCookedAttribute(
            Name = "nGridWidth",
            DefaultValue = 128,
            ElementType = ElementType.Int32)]
        public Int32 GridWidth;

        [XmlCookedAttribute(
            Name = "nGridDepth",
            DefaultValue = 100,
            ElementType = ElementType.Int32)]
        public Int32 GridDepth;

        [XmlCookedAttribute(
            Name = "nGridHeight",
            DefaultValue = 128,
            ElementType = ElementType.Int32)]
        public Int32 GridHeight;

        [XmlCookedAttribute(
            Name = "nGridBorder",
            DefaultValue = 10,
            ElementType = ElementType.Int32)]
        public Int32 GridBorder;

        [XmlCookedAttribute(
            Name = "fVelocityMultiplier",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float VelocityMultiplier;

        [XmlCookedAttribute(
            Name = "fVelocityClamp",
            DefaultValue = 4.0f,
            ElementType = ElementType.Float)]
        public float VelocityClamp;

        [XmlCookedAttribute(
            Name = "fSizeMultiplier",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float SizeMultiplier;

        [XmlCookedAttribute(
            Name = "fVorticityConfinementScale",
            DefaultValue = 0.22f,
            ElementType = ElementType.Float)]
        public float VorticityConfinementScale;

        [XmlCookedAttribute(
            Name = "nGridDensityTextureIndex",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 GridDensityTextureIndex;

        [XmlCookedAttribute(
            Name = "nGridVelocityTextureIndex",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 GridVelocityTextureIndex;

        [XmlCookedAttribute(
            Name = "nGridObstructorTextureIndex",
            DefaultValue = 5,
            ElementType = ElementType.Int32)]
        public Int32 GridObstructorTextureIndex;

        [XmlCookedAttribute(
            Name = "fRenderScale",
            DefaultValue = 7.05f,
            ElementType = ElementType.Float)]
        public float RenderScale;

        [XmlCookedAttribute(
            Name = "pszTextureDensityName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String TextureDensityName;

        [XmlCookedAttribute(
            Name = "pszTextureVelocityName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String TextureVelocityName;

        [XmlCookedAttribute(
            Name = "pszTextureObstructorName",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String TextureObstructorName;

        [XmlCookedAttribute(
            Name = "nDensityTextureId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 DensityTextureId;

        [XmlCookedAttribute(
            Name = "nVelocityTextureId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 VelocityTextureId;

        [XmlCookedAttribute(
            Name = "nObstructorTextureId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 ObstructorTextureId;

        [XmlCookedAttribute(
            Name = "vLightOffset",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3[] LightOffsets;

        [XmlCookedAttribute(
            Name = "fSoftParticleScale",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float SoftParticleScale;

        [XmlCookedAttribute(
            Name = "fSoftParticleContrast",
            DefaultValue = 2.0f,
            ElementType = ElementType.Float)]
        public float SoftParticleContrast;
    }
}