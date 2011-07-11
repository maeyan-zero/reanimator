using System;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "AI_BEHAVIOR_DEFINITION")]
    public class AIBehaviorDefinition
    {
        [Flags]
        public enum BehaviorFlags : uint
        {
            AI_BEHAVIOR_FLAG_ONCE = (1 << 0),
            AI_BEHAVIOR_FLAG_RUN = (1 << 1),
            AI_BEHAVIOR_FLAG_FLY = (1 << 2),
            AI_BEHAVIOR_FLAG_DONT_STOP = (1 << 3),
            AI_BEHAVIOR_FLAG_WARP = (1 << 4)
        }

        [XmlCookedAttribute(
            Name = "fPriority",
            DefaultValue = 0.5f,
            ElementType = ElementType.Float)]
        public float Priority;

        [XmlCookedAttribute(
            Name = "fChance",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float Chance;

        [XmlCookedAttribute(
            Name = "pfParams",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 6)]
        public float[] Params;

        [XmlCookedAttribute(
            Name = "nBehaviorId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.AI_BEHAVIOR, // 17713 AI_BEHAVIOR
            ElementType = ElementType.ExcelIndex)]
        public AiBehaviorRow Behavior;
        public int BehaviorRowIndex;

        [XmlCookedAttribute(
            Name = "nSkillId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.SKILLS, // 27952 SKILLS
            ElementType = ElementType.ExcelIndex)]
        public AiBehaviorRow Skill1;
        public int Skill1RowIndex;

        [XmlCookedAttribute(
            Name = "nSkillId2",
            DefaultValue = null,
            TableCode = Xls.TableCodes.SKILLS, // 27952 SKILLS
            ElementType = ElementType.ExcelIndex)]
        public AiBehaviorRow Skill2;
        public int Skill2RowIndex;

        [XmlCookedAttribute(
            Name = "nStateId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.STATES, // 22832 STATES
            ElementType = ElementType.ExcelIndex)]
        public StatesRow State;
        public int StateRowIndex;

        [XmlCookedAttribute(
            Name = "nStatId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.STATS, // 23088 STATS
            ElementType = ElementType.ExcelIndex)]
        public StatsRow Stat;
        public int StatRowIndex;

        [XmlCookedAttribute(
            Name = "nSoundId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.SOUNDS, // 20784 SOUNDS
            ElementType = ElementType.ExcelIndex)]
        public Sounds Sound;
        public int SoundRowIndex;

        [XmlCookedAttribute(
            Name = "nMonsterId",
            DefaultValue = null,
            TableCode = Xls.TableCodes.SPAWN_CLASS, // 14642 SPAWN_CLASS
            ElementType = ElementType.ExcelIndex)]
        public UnitDataRow Monster;
        public int MonsterRowIndex;

        [XmlCookedAttribute(
            Name = "Flags",
            DefaultValue = false,
            ElementType = ElementType.Flag,
            FlagId = 1)]
        public BehaviorFlags Flags;

        [XmlCookedAttribute(
            Name = "pszString",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String String;

        [XmlCookedAttribute(
            Name = "tTable",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof(AIBehaviorDefinitionTable))]
        public AIBehaviorDefinitionTable Table;
    }
}