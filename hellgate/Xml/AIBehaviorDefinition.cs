using System;

namespace Hellgate.Xml
{
    internal class AIBehaviorDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "fPriority",
                DefaultValue = 0.5f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fChance",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pfParams",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArrayFixed,
                Count = 6
            },
            new XmlCookElement
            {
                Name = "nBehaviorId",
                DefaultValue = null,
                ExcelTableCode = 17713, // AI_BEHAVIOR
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nSkillId",
                DefaultValue = null,
                ExcelTableCode = 27952, // SKILLS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nSkillId2",
                DefaultValue = null,
                ExcelTableCode = 27952, // SKILLS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nStateId",
                DefaultValue = null,
                ExcelTableCode = 22832, // STATES
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nStatId",
                DefaultValue = null,
                ExcelTableCode = 23088, // STATS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nSoundId",
                DefaultValue = null,
                ExcelTableCode = 20784, // SOUNDS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nMonsterId",
                DefaultValue = null,
                ExcelTableCode = 14642, // SPAWN_CLASS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "AI_BEHAVIOR_FLAG_ONCE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "AI_BEHAVIOR_FLAG_RUN",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "AI_BEHAVIOR_FLAG_FLY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "AI_BEHAVIOR_FLAG_DONT_STOP",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "AI_BEHAVIOR_FLAG_WARP",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "pszString",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "tTable",
                DefaultValue = null,
                ElementType = ElementType.Table,
                ChildType = typeof (AIBehaviorDefinitionTable)
            }
        };

        public AIBehaviorDefinition()
        {
            RootElement = "AI_BEHAVIOR_DEFINITION";
            base.Elements.AddRange(Elements);
            Flags = new Int32[] {-1};
        }
    }
}