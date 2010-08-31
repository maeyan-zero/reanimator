namespace Reanimator.XmlDefinitions
{
    class ConditionDefinition : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nType",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nState",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nUnitType",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nSkill",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nMonsterClass",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nObjectClass",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "nStat",
                DefaultValue = null,
                ExcelTableCode = 0, // todo
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
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            },
            new XmlCookElement
            {
                // this and CHECK_TARGET are swapped for some reason...
                // see end of default values array in .xml.cooked ordering (0, 2, 1, 3, 4, 5, 6)
                Name = "CONDITION_BIT_CHECK_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_CHECK_WEAPON",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_NOT_DEAD_OR_DYING",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_IS_YOUR_PLAYER",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_OWNER_IS_YOUR_PLAYER",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            },
            new XmlCookElement
            {
                Name = "CONDITION_BIT_CHECK_STATE_SOURCE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1
            }
        };

        public ConditionDefinition()
        {
            RootElement = "CONDITION_DEFINITION";
            base.Elements.AddRange(Elements);
        }
    }
}