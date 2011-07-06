using System;
using Hellgate.Excel.JapaneseBeta;
using Revival.Common;

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
            Name = "tAttachmentDef.eType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 AttachmentType;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.dwFlags",
            DefaultValue = (UInt32)0,
            ElementType = ElementType.Int32,
            CustomType = ElementType.Unsigned)]
        public UInt32 AttachmentFlags;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nVolume",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public Int32 Volume;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszAttached",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Attached;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nAttachedDefId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 AttachedDefId;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Bone;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nBoneId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 BoneId;

        [XmlCookedAttribute(Name = "vPosition", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition.fX",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition.fY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vPosition.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 Position = new Vector3();

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fRotation",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Rotation;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fYaw",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Yaw;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fPitch",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Pitch;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fRoll",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Roll;

        [XmlCookedAttribute(Name = "vNormal", ElementType = ElementType.Float, CustomType = ElementType.Vector3)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal.fX",
            DefaultValue = -1.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal.fY",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        [XmlCookedAttribute(
            Name = "tAttachmentDef.vNormal.fZ",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public Vector3 Normal = new Vector3();

        [XmlCookedAttribute(
            Name = "tAttachmentDef.fScale",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float,
            IsResurrection = true)]
        public float Scale;

        [XmlCookedAttribute(
            Name = "pszExcelString",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String ExcelString; // todo: is this to a particular table??

        [XmlCookedAttribute(
            Name = "nExcelIndex",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public Int32 ExcelIndex; // todo: to what table? States?

        [XmlCookedAttribute(
            Name = "nData",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public Int32 DataCount; // I'm guessing it's the count for the pszData below

        [XmlCookedAttribute(
            Name = "fParam",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param;

        [XmlCookedAttribute(
            Name = "pszData",
            DefaultValue = null,
            ElementType = ElementType.String,
            IsByteArray = true)]
        public byte[] Data;

        [XmlCookedAttribute(
            Name = "tCondition",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof(ConditionDefinition))]
        public ConditionDefinition Condition;
    }
}