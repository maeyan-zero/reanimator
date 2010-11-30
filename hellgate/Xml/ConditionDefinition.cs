using System;

namespace Hellgate.Xml
{
    class ConditionDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nType",
                DefaultValue = null,
                ExcelTableCode = 26417, // CONDITION_FUNCTIONS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nState",
                DefaultValue = null,
                ExcelTableCode = 22832, // STATES
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nUnitType",
                DefaultValue = null,
                ExcelTableCode = 21040, // UNITTYPES
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nSkill",
                DefaultValue = null,
                ExcelTableCode = 27952, // SKILLS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nMonsterClass",
                DefaultValue = null,
                ExcelTableCode = 12338, // MONSTERS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nObjectClass",
                DefaultValue = null,
                ExcelTableCode = 13106, // OBJECTS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nStat",
                DefaultValue = null,
                ExcelTableCode = 23088, // STATS
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "tParams0.fValue",
                TrueName = "tParams[0].fValue",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tParams1.fValue",
                TrueName = "tParams[1].fValue",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_CHECK_OWNER",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 0,
                BitCount = 7
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_CHECK_TARGET",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 2,
                BitCount = 7
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_CHECK_WEAPON",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 1,
                BitCount = 7
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_NOT_DEAD_OR_DYING",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 3,
                BitCount = 7
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_IS_YOUR_PLAYER",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 4,
                BitCount = 7
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_OWNER_IS_YOUR_PLAYER",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 5,
                BitCount = 7
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_CHECK_STATE_SOURCE",
                ElementType = ElementType.BitFlag,
                DefaultValue = false,
                BitIndex = 6,
                BitCount = 7
            }
        };

        public ConditionDefinition()
        {
            RootElement = "CONDITION_DEFINITION";
            base.Elements.AddRange(Elements);
            Flags = new UInt32[1]; // 7 / 32 = 1
        }
    }
}