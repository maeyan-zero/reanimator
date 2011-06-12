using System;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "CONDITION_DEFINITION")]
    public class ConditionDefinition
    {
        [Flags]
        public enum ConditionFlags : uint
        {
            CONDITION_BIT_CHECK_OWNER = (1 << 0),
            CONDITION_BIT_CHECK_WEAPON = (1 << 1),
            CONDITION_BIT_CHECK_TARGET = (1 << 2),
            CONDITION_BIT_NOT_DEAD_OR_DYING = (1 << 3),
            CONDITION_BIT_IS_YOUR_PLAYER = (1 << 4),
            CONDITION_BIT_OWNER_IS_YOUR_PLAYER = (1 << 5),
            CONDITION_BIT_CHECK_STATE_SOURCE = (1 << 6)
        }

        [XmlCookedAttribute(
            Name = "nType",
            DefaultValue = null,
            TableCode = Xls.TableCodes.CONDITION_FUNCTIONS, // 26417 CONDITION_FUNCTIONS
            ElementType = ElementType.ExcelIndex)]
        public ConditionFunctionsRow CondFunc;
        public int CondFuncRowIndex;

        [XmlCookedAttribute(
            Name = "nState",
            DefaultValue = null,
            TableCode = Xls.TableCodes.STATES, // 22832 STATES
            ElementType = ElementType.ExcelIndex)]
        public StatesRow State;
        public int StateRowIndex;

        [XmlCookedAttribute(
            Name = "nUnitType",
            DefaultValue = null,
            TableCode = Xls.TableCodes.UNITTYPES, // 21040 UNITTYPES
            ElementType = ElementType.ExcelIndex)]
        public UnitTypesRow UnitType;
        public int UnitTypeRowIndex;

        [XmlCookedAttribute(
            Name = "nSkill",
            DefaultValue = null,
            TableCode = Xls.TableCodes.SKILLS, // 27952 SKILLS
            ElementType = ElementType.ExcelIndex)]
        public SkillsRow Skill;
        public int SkillRowIndex;

        [XmlCookedAttribute(
            Name = "nMonsterClass",
            DefaultValue = null,
            TableCode = Xls.TableCodes.MONSTERS, // 12338 MONSTERS
            ElementType = ElementType.ExcelIndex)]
        public UnitDataRow Monster;
        public int MonsterRowIndex;

        [XmlCookedAttribute(
            Name = "nObjectClass",
            DefaultValue = null,
            TableCode = Xls.TableCodes.OBJECTS, // 13106 OBJECTS
            ElementType = ElementType.ExcelIndex)]
        public UnitDataRow Object;
        public int ObjectRowIndex;

        [XmlCookedAttribute(
            Name = "nItemClass",
            DefaultValue = null,
            TableCode = Xls.TableCodes.ITEMS, // 27953 ITEMS
            ElementType = ElementType.ExcelIndex,
            IsTestCentre = true)]
        public UnitDataRow Item;
        public int ItemRowIndex;

        [XmlCookedAttribute(
            Name = "nStat",
            DefaultValue = null,
            TableCode = Xls.TableCodes.STATS, // 23088 STATS
            ElementType = ElementType.ExcelIndex)]
        public StatsRow Stat;
        public int StatRowIndex;

        [XmlCookedAttribute(
            Name = "tParams[0].fValue",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param0;

        [XmlCookedAttribute(
            Name = "tParams[1].fValue",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param1;

        [XmlCookedAttribute(
            Name = "Flags",
            ElementType = ElementType.BitFlag,
            DefaultValue = false,
            BitFlagIndex = 0,
            BitFlagCount = 7)]
        public ConditionFlags Flags;
    }
}