using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Reanimator.XmlDefinitions;

namespace Reanimator
{
    partial class XmlCookedFile
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class XmlCookFileHeader
        {
            public UInt32 MagicWord;
            public Int32 Version;
            public UInt32 XmlRootDefinition;
            public Int32 XmlRootElementCount;
        }

        /// <summary>
        /// Initialize XML Definitions and generate String Hash values for xml element names.
        /// Must be called before usage of the class.
        /// </summary>
        /// <param name="tableFiles">The loaded table files for excel lookups.</param>
        public static void Initialize(TableFiles tableFiles)
        {
            Debug.Assert(tableFiles != null);

            _tableFiles = tableFiles;
            _xmlDefinitions = new XmlDefinition[]
            {
                // AI
                new AIDefinition(),
                new AIBehaviorDefinitionTable(),
                new AIBehaviorDefinition(),

                // Shared (used in States and Skills)
                new ConditionDefinition(),

                // Level Layout (contains object positions etc)
                new RoomLayoutGroupDefinition(),
                new RoomLayoutGroup(),

                // Level Pathing (huge-ass list of nodes/points)
                new RoomPathNodeDefinition(),
                new RoomPathNodeSet(),
                new RoomPathNode(),
                new RoomPathNodeConnection(),
                new RoomPathNodeConnectionRef(),

                // Material (makes things look like things)
                new Material(),

                // Skills (defines skill effect/appearance mostly, not so much the skill itself)
                new SkillEventsDefinition(),
                new SkillEventHolder(),
                new SkillEvent(),

                // Sound Effects
                new SoundEffectDefinition(),
                new SoundEffect(),

                // States
                new StateDefinition(),
                new StateEvent()
            };

            foreach (XmlDefinition xmlDefinition in _xmlDefinitions)
            {
                xmlDefinition.RootHash = Crypt.GetStringHash(xmlDefinition.RootElement);

                foreach (XmlCookElement xmlCookElement in xmlDefinition.Elements)
                {
                    String stringToHash = String.IsNullOrEmpty(xmlCookElement.TrueName) ? xmlCookElement.Name : xmlCookElement.TrueName;
                    xmlCookElement.NameHash = Crypt.GetStringHash(stringToHash);
                }
            }
        }

        /// <summary>
        /// Searches for a known XML Definition using their Root Element String Hash.
        /// </summary>
        /// <param name="stringHash">The String Hash of the Root Element to find.</param>
        /// <returns>Found XML Definition or null if not found.</returns>
        private static XmlDefinition GetXmlDefinition(UInt32 stringHash)
        {
            return _xmlDefinitions.FirstOrDefault(xmlDefinition => xmlDefinition.RootHash == stringHash);
        }

        /// <summary>
        /// Searches for an XML Cook Element in an XML Definition for an Element with a particular String Hash.
        /// </summary>
        /// <param name="xmlDefinition">The XML Definition to search through.</param>
        /// <param name="stringHash">The String Hash of the Element Name to find.</param>
        /// <returns>Found XML Cook Element or null if not found.</returns>
        private static XmlCookElement GetXmlCookElement(XmlDefinition xmlDefinition, UInt32 stringHash)
        {
            return xmlDefinition.Elements.FirstOrDefault(xmlCookElement => xmlCookElement.NameHash == stringHash);
        }
    }
}
