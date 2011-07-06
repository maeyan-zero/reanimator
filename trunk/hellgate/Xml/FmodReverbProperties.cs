using System;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "FMOD_REVERB_PROPERTIES")]
    public class FmodReverbProperties
    {
        [Flags]
        public enum FmodReverbFlags : uint
        {
            FMOD_REVERB_FLAGS_DECAYTIMESCALE = (1 << 0),
            FMOD_REVERB_FLAGS_REFLECTIONSSCALE = (1 << 1),
            FMOD_REVERB_FLAGS_REFLECTIONSDELAYSCALE = (1 << 2),
            FMOD_REVERB_FLAGS_REVERBSCALE = (1 << 3),
            FMOD_REVERB_FLAGS_REVERBDELAYSCALE = (1 << 4),
            FMOD_REVERB_FLAGS_DECAYHFLIMIT = (1 << 5),
            FMOD_REVERB_FLAGS_ECHOTIMESCALE = (1 << 6),
            FMOD_REVERB_FLAGS_MODULATIONTIMESCALE = (1 << 7)
        }

        [XmlCookedAttribute(
            Name = "Instance",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 Instance;

        [XmlCookedAttribute(
            Name = "Environment",
            DefaultValue = 0,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 Environment;

        [XmlCookedAttribute(
            Name = "EnvSize",
            DefaultValue = 7.5f,
            ElementType = ElementType.Float)]
        public float EnvSize;

        [XmlCookedAttribute(
            Name = "EnvDiffusion",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float EnvDiffusion;

        [XmlCookedAttribute(
            Name = "Room",
            DefaultValue = -1000,
            ElementType = ElementType.Int32)]
        public Int32 Room;

        [XmlCookedAttribute(
            Name = "RoomHF",
            DefaultValue = -100,
            ElementType = ElementType.Int32)]
        public Int32 RoomHF;

        [XmlCookedAttribute(
            Name = "RoomLF",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 RoomLF;

        [XmlCookedAttribute(
            Name = "DecayTime",
            DefaultValue = 1.49f,
            ElementType = ElementType.Float)]
        public float DecayTime;

        [XmlCookedAttribute(
            Name = "DecayHFRatio",
            DefaultValue = 0.83f,
            ElementType = ElementType.Float)]
        public float DecayHFRatio;

        [XmlCookedAttribute(
            Name = "DecayLFRatio",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float DecayLFRatio;

        [XmlCookedAttribute(
            Name = "Reflections",
            DefaultValue = -2602,
            ElementType = ElementType.Int32)]
        public Int32 Reflections;

        [XmlCookedAttribute(
            Name = "ReflectionsDelay",
            DefaultValue = 0.007f,
            ElementType = ElementType.Float)]
        public float ReflectionsDelay;

        [XmlCookedAttribute(
            Name = "ReflectionsPan",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3[] ReflectionsPan;

        [XmlCookedAttribute(
            Name = "Reverb",
            DefaultValue = 200,
            ElementType = ElementType.Int32)]
        public Int32 Reverb;

        [XmlCookedAttribute(
            Name = "ReverbDelay",
            DefaultValue = 0.011f,
            ElementType = ElementType.Float)]
        public float ReverbDelay;

        [XmlCookedAttribute(
            Name = "ReverbPan",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3[] ReverbPan;

        [XmlCookedAttribute(
            Name = "EchoTime",
            DefaultValue = 0.25f,
            ElementType = ElementType.Float)]
        public float EchoTime;

        [XmlCookedAttribute(
            Name = "EchoDepth",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float EchoDepth;

        [XmlCookedAttribute(
            Name = "ModulationTime",
            DefaultValue = 0.25f,
            ElementType = ElementType.Float)]
        public float ModulationTime;

        [XmlCookedAttribute(
            Name = "ModulationDepth",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float ModulationDepth;

        [XmlCookedAttribute(
            Name = "AirAbsorptionHF",
            DefaultValue = -5.0f,
            ElementType = ElementType.Float)]
        public float AirAbsorptionHF;

        [XmlCookedAttribute(
            Name = "HFReference",
            DefaultValue = 5000.0f,
            ElementType = ElementType.Float)]
        public float HFReference;

        [XmlCookedAttribute(
            Name = "LFReference",
            DefaultValue = 250.0f,
            ElementType = ElementType.Float)]
        public float LFReference;

        [XmlCookedAttribute(
            Name = "RoomRolloffFactor",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float RoomRolloffFactor;

        [XmlCookedAttribute(
            Name = "Diffusion",
            DefaultValue = 100.0f,
            ElementType = ElementType.Float)]
        public float Diffusion;

        [XmlCookedAttribute(
            Name = "Density",
            DefaultValue = 100.0f,
            ElementType = ElementType.Float)]
        public float Density;

        [XmlCookedAttribute(
            Name = "Flags",
            DefaultValue = false,
            ElementType = ElementType.Flag,
            FlagId = 1)]
        public FmodReverbFlags Flags;
    }
}