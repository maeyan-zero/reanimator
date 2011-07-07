using System;
using System.Collections.Generic;
using Hellgate.Xml;

namespace Hellgate
{
    public class XmlCookedDefinition
    {
        public static readonly Type[] DefinitionTypes =
        {
            // Shared
            typeof (ConditionDefinition),
            
            // AI Behavior
            typeof (AIDefinition),
            typeof (AIBehaviorDefinitionTable),
            typeof (AIBehaviorDefinition),

            // Appearance
            typeof (AppearanceDefinition),
            typeof (AnimationDefinition),
            typeof (AnimEvent),
            typeof (InventoryViewInfo),

            // Colorsets
            typeof (ColorSetDefinition),
            typeof (ColorDefinition),

            // Demo Levels
            typeof (DemoLevelDefinition),

            // Environments
            typeof (EnvironmentDefinition),
            typeof (EnvLightDefinition),

            // Lights
            typeof (LightDefinition),

            // Materials
            typeof (Material),

            // Particle System
            typeof (ParticleSystemDefinition),

            // Room Layouts
            typeof (RoomPathNodeDefinition),
            typeof (RoomPathNodeSet),
            typeof (RoomPathNode),
            typeof (RoomLayoutGroupDefinition),
            typeof (RoomLayoutGroup),

            // Room Paths
            typeof (RoomPathNodeConnection),
            typeof (RoomPathNodeConnectionRef),

            // Screen Effects
            typeof (ScreenEffectDefinition),

            // Skill Events
            typeof (SkillEventsDefinition),
            typeof (SkillEventHolder),
            typeof (SkillEvent),

            // Skyboxes
            typeof (SkyboxDefinition),
            typeof (SkyboxModel),

            // Sound Effects
            typeof (SoundEffectDefinition),
            typeof (SoundEffect),

            // Sounds Reverbs
            typeof (SoundReverbDefinition),
            typeof (FmodReverbProperties),

            // State Events
            typeof (StateDefinition),
            typeof (StateEvent),

            // Textures
            typeof (TextureDefinition),
            typeof (BlendRLE),
            typeof (BlendRun)
        };

        public readonly Dictionary<uint, XmlCookedElement> Elements;

        public readonly XmlCookedAttribute RootElement;
        public readonly Type XmlObjectType;
        public readonly int SinglePlayerElementCount;
        public readonly int TestCentreElementCount;
        public readonly int ResurrectionElementCount;

        public int Count { get { return Elements.Count; } }
        public UInt32 RootHash { get { return RootElement.NameHash; } }

        public XmlCookedDefinition(XmlCookedAttribute rootElement, Dictionary<uint, XmlCookedElement> xmlElements, Type xmlObjectType)
        {
            RootElement = rootElement;
            Elements = xmlElements;
            XmlObjectType = xmlObjectType;

            foreach (XmlCookedElement xmlElement in xmlElements.Values)
            {
                if (xmlElement.IsCustomOrigin) continue;

                if (xmlElement.IsResurrection) ResurrectionElementCount++;
                if (xmlElement.IsTestCentre) TestCentreElementCount++;
                if (!xmlElement.IsResurrection && !xmlElement.IsTestCentre) SinglePlayerElementCount++;
            }
        }

        public XmlCookedElement GetElement(uint nameHash)
        {
            XmlCookedElement xmlElement;
            return !Elements.TryGetValue(nameHash, out xmlElement) ? null : xmlElement;
        }

        public XmlCookedElement GetElement(String name)
        {
            return GetElement(Crypt.GetStringHash(name));
        }

        public override string ToString()
        {
            return RootElement.Name;
        }
    }
}
