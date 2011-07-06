using System;
using Hellgate.Excel.JapaneseBeta;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "SKILL_EVENT")]
    public class SkillEvent
    {
        [Flags]
        public enum SkillEventFlags1 : uint
        {
            SKILL_EVENT_FLAG_LASER_TURNS = (1 << 0),
            SKILL_EVENT_FLAG_REQUIRES_TARGET = (1 << 1),
            SKILL_EVENT_FLAG_FORCE_NEW = (1 << 2),
            SKILL_EVENT_FLAG_LASER_SEEKS_SURFACES = (1 << 3),
            SKILL_EVENT_FLAG_FACE_TARGET = (1 << 4),
            SKILL_EVENT_FLAG_USE_UNIT_TARGET = (1 << 5),
            SKILL_EVENT_FLAG_USE_EVENT_OFFSET = (1 << 6),
            SKILL_EVENT_FLAG_LOOP = (1 << 7),
            SKILL_EVENT_FLAG_USE_EVENT_OFFSET_ABSOLUTE = (1 << 8),
            SKILL_EVENT_FLAG_PLACE_ON_TARGET = (1 << 9),
            SKILL_EVENT_FLAG_USE_ANIM_CONTACT_POINT = (1 << 10),
            SKILL_EVENT_FLAG_TRANSFER_STATS = (1 << 11),
            SKILL_EVENT_FLAG_DO_WHEN_TARGET_IN_RANGE = (1 << 12),
            SKILL_EVENT_FLAG_ADD_TO_CENTER = (1 << 13),
            SKILL_EVENT_FLAG_360_TARGETING = (1 << 14),
            SKILL_EVENT_FLAG_USE_SKILL_TARGET_LOCATION = (1 << 15),
            SKILL_EVENT_FLAG_USE_AI_TARGET = (1 << 16),
            SKILL_EVENT_FLAG_USE_OFFHAND_WEAPON = (1 << 17),
            SKILL_EVENT_FLAG_FLOAT = (1 << 18),
            SKILL_EVENT_FLAG_DONT_VALIDATE_TARGET = (1 << 19),
            SKILL_EVENT_FLAG_RANDOM_FIRING_DIRECTION = (1 << 20),
            SKILL_EVENT_FLAG_AUTOAIM_PROJECTILE = (1 << 21),
            SKILL_EVENT_FLAG_TARGET_WEAPON = (1 << 22),
            SKILL_EVENT_FLAG_USE_WEAPON_FOR_CONDITION = (1 << 23),
            SKILL_EVENT_FLAG_FORCE_CONDITION_ON_EVENT = (1 << 24),
            SKILL_EVENT_FLAG_USE_HOLY_RADIUS_FOR_RANGE = (1 << 25),
            SKILL_EVENT_FLAG_USE_CHANCE_PCODE = (1 << 26),
            SKILL_EVENT_FLAG_SERVER_ONLY = (1 << 27),
            SKILL_EVENT_FLAG_CLIENT_ONLY = (1 << 28),
            SKILL_EVENT_FLAG_LASER_ATTACKS_LOCATION = (1 << 29),
            SKILL_EVENT_FLAG_AT_NEXT_COOLDOWN = (1 << 30),
            SKILL_EVENT_FLAG_AIM_WITH_WEAPON = ((UInt32)1 << 31)
        }

        [Flags]
        public enum SkillEventFlags2 : uint
        {
            SKILL_EVENT_FLAG2_AIM_WITH_WEAPON_ZERO = (1 << 0),
            SKILL_EVENT_FLAG2_USE_PARAM0_PCODE = (1 << 1),
            SKILL_EVENT_FLAG2_USE_PARAM1_PCODE = (1 << 2),
            SKILL_EVENT_FLAG2_USE_PARAM2_PCODE = (1 << 3),
            SKILL_EVENT_FLAG2_USE_PARAM3_PCODE = (1 << 4),
            SKILL_EVENT_FLAG2_USE_ULTIMATE_OWNER = (1 << 5),
            SKILL_EVENT_FLAG2_CHARGE_POWER_AND_COOLDOWN = (1 << 6),
            SKILL_EVENT_FLAG2_MARK_SKILL_AS_SUCCESSFUL = (1 << 7),
            SKILL_EVENT_FLAG2_LASER_INCLUDE_IN_UI = (1 << 8),
            SKILL_EVENT_FLAG2_LASER_DONT_TARGET_UNITS = (1 << 11),

            [XmlCookedAttribute(IsTestCentre = true)]
            SKILL_EVENT_FLAG2_DONT_EXECUTE_STATS = (1 << 12)
        }

        [XmlCookedAttribute(
            Name = "nType",
            DefaultValue = null,
            TableCode = Xls.TableCodes.SKILLEVENTTYPES, // 0x00006E30 (28208) SKILLEVENTTYPES
            ElementType = ElementType.ExcelIndex)]
        public SkillEventTypesRow SkillEventType;
        public int SkillEventTypeRowIndex;

        [XmlCookedAttribute(
            Name = "Flags1",
            ElementType = ElementType.Flag,
            DefaultValue = false,
            FlagId = 1)]
        public SkillEventFlags1 Flags1;

        [XmlCookedAttribute(
            Name = "Flags2",
            ElementType = ElementType.Flag,
            DefaultValue = false,
            FlagId = 2)]
        public SkillEventFlags2 Flags2;

        [XmlCookedAttribute(
            Name = "fTime",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Time;

        [XmlCookedAttribute(
            Name = "fRandChance",
            DefaultValue = 1.0f,
            ElementType = ElementType.Float)]
        public float RandChance;

        [XmlCookedAttribute(
            Name = "tParam0.flValue",
            TrueName = "tParam[0].flValue",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param0;

        [XmlCookedAttribute(
            Name = "tParam1.flValue",
            TrueName = "tParam[1].flValue",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param1;

        [XmlCookedAttribute(
            Name = "tParam2.flValue",
            TrueName = "tParam[2].flValue",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param2;

        [XmlCookedAttribute(
            Name = "tParam3.flValue",
            TrueName = "tParam[3].flValue",
            DefaultValue = 0.0f,
            ElementType = ElementType.Float)]
        public float Param3;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.eType",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int AttachmentType;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.dwFlags",
            DefaultValue = 0,
            ElementType = ElementType.Int32)]
        public int AttachmentFlags;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszAttached",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Attached;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nVolume",
            DefaultValue = 1,
            ElementType = ElementType.Int32)]
        public int Volume;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nAttachedDefId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int AttachedDefId;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.pszBone",
            DefaultValue = null,
            ElementType = ElementType.String)]
        public String Bone;

        [XmlCookedAttribute(
            Name = "tAttachmentDef.nBoneId",
            DefaultValue = -1,
            ElementType = ElementType.NonCookedInt32)]
        public int BoneId;

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
            Name = "tCondition",
            DefaultValue = null,
            ElementType = ElementType.Table,
            ChildType = typeof(ConditionDefinition))]
        public ConditionDefinition Condition;
    }
}