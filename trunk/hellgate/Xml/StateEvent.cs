using System;
using Hellgate.Excel.JapaneseBeta;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "STATE_EVENT")]
    public class StateEvent
    {
        [Flags]
        public enum StateEventFlags : uint
        {
            STATE_EVENT_FLAG_FORCE_NEW = (1 << 0),
            STATE_EVENT_FLAG_FIRST_PERSON = (1 << 1),
            STATE_EVENT_FLAG_ADD_TO_CENTER = (1 << 2),
            STATE_EVENT_FLAG_CONTROL_UNIT_ONLY = (1 << 3),
            STATE_EVENT_FLAG_FLOAT = (1 << 4),
            STATE_EVENT_FLAG_OWNED_BY_CONTROL = (1 << 5),
            STATE_EVENT_FLAG_SET_IMMEDIATELY = (1 << 6),
            STATE_EVENT_FLAG_CLEAR_IMMEDIATELY = (1 << 7),
            STATE_EVENT_FLAG_NOT_CONTROL_UNIT = (1 << 8),
            STATE_EVENT_FLAG_ON_WEAPONS = (1 << 9),
            [XmlCookedAttribute(IsResurrection = true)]
            STATE_EVENT_FLAG_ON_WEAPONS_ATTACHMENT = (1 << 16),
            STATE_EVENT_FLAG_IGNORE_CAMERA = (1 << 10),
            STATE_EVENT_FLAG_ON_CLEAR = (1 << 11),
            STATE_EVENT_FLAG_CHECK_CONDITION_ON_CLEAR = (1 << 12),
            STATE_EVENT_FLAG_SHARE_DURATION = (1 << 13),
            [XmlCookedAttribute(IsResurrection = true)]
            STATE_EVENT_FLAG_SAME_TEAM_ONLY = (1 << 17),
            [XmlCookedAttribute(IsResurrection = true)]
            STATE_EVENT_FLAG_OTHER_TEAM_ONLY = (1 << 18)
        }

        [XmlCookedAttribute(
            Name = "Flags",
            ElementType = ElementType.Flag,
            DefaultValue = false,
            FlagId = 1)]
        public StateEventFlags Flags;

        [XmlCookedAttribute(
            Name = "eType",
            DefaultValue = null,
            ElementType = ElementType.ExcelIndex,
            TableCode = Xls.TableCodes.STATE_EVENT_TYPES)] // 22320 STATE_EVENT_TYPES
        public SkillEventTypesRow EventType;
        public int EventTypeRowIndex;

        [XmlCookedAttribute(
            Name = "tAttachmentDef",
            ChildType = typeof(AttachmentDef),
            CustomType = ElementType.Object)]
        public AttachmentDef Attachment;

        [XmlCookedAttribute(
            Name = "pszExcelString",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String ExcelString; // todo: is this to a particular table??

        [XmlCookedAttribute(
            Name = "nExcelIndex",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32)]
        public int ExcelIndex; // todo: to what table? States?

        [XmlCookedAttribute(
            Name = "nData",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int DataCount; // I'm guessing it's the count for the pszData below

        [XmlCookedAttribute(
            Name = "fParam",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param;

        [XmlCookedAttribute(
            Name = "pszData",
            DefaultValue = null,
            ElementType = ElementType.String,
            TreatAsData = true)]
        public byte[] Data;

        [XmlCookedAttribute(
            Name = "tCondition",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof(ConditionDefinition))]
        public ConditionDefinition Condition;

        public StateEvent()
        {
            Attachment = new AttachmentDef();
        }
    }
}