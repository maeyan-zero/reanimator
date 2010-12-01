using System;

namespace Hellgate.Xml
{
    class FmodReverbProperties : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "Instance",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "Environment",
                DefaultValue = 0,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "EnvSize",
                DefaultValue = 7.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "EnvDiffusion",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "Room",
                DefaultValue = -1000,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "RoomHF",
                DefaultValue = -100,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "RoomLF",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "DecayTime",
                DefaultValue = 1.49f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "DecayHFRatio",
                DefaultValue = 0.83f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "DecayLFRatio",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "Reflections",
                DefaultValue = -2602,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "ReflectionsDelay",
                DefaultValue = 0.007f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "ReflectionsPan",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayFixed,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "Reverb",
                DefaultValue = 200,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "ReverbDelay",
                DefaultValue = 0.011f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "ReverbPan",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayFixed,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "EchoTime",
                DefaultValue = 0.25f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "EchoDepth",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "ModulationTime",
                DefaultValue = 0.25f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "ModulationDepth",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "AirAbsorptionHF",
                DefaultValue = -5.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "HFReference",
                DefaultValue = 5000.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "LFReference",
                DefaultValue = 250.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "RoomRolloffFactor",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "Diffusion",
                DefaultValue = 100.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "Density",
                DefaultValue = 100.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_DECAYTIMESCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_REFLECTIONSSCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_REFLECTIONSDELAYSCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_REVERBSCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_REVERBDELAYSCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_DECAYHFLIMIT",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 5)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_ECHOTIMESCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 6)
            },
            new XmlCookElement
            {
                Name = "FMOD_REVERB_FLAGS_MODULATIONTIMESCALE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                FlagMask = (1 << 7)
            }
        };

        public FmodReverbProperties()
        {
            RootElement = "FMOD_REVERB_PROPERTIES";
            base.Elements.AddRange(Elements);
            Flags = new Int32[] { -1 };
        }
    }
}