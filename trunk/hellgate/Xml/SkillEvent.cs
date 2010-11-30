using System;

namespace Hellgate.Xml
{
    class SkillEvent : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "nType",
                DefaultValue = null,
                ExcelTableCode = 0x00006E30, // (28208)	SKILLEVENTTYPES
                ElementType = ElementType.ExcelIndex
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_LASER_TURNS",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_REQUIRES_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_FORCE_NEW",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_LASER_SEEKS_SURFACES",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_FACE_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_UNIT_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 5)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_EVENT_OFFSET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 6)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_LOOP",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 7)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_EVENT_OFFSET_ABSOLUTE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 8)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_PLACE_ON_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 9)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_ANIM_CONTACT_POINT",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 10)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_TRANSFER_STATS",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 11)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_DO_WHEN_TARGET_IN_RANGE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 12)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_ADD_TO_CENTER",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 13)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_360_TARGETING",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 14)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_SKILL_TARGET_LOCATION",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 15)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_AI_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 16)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_OFFHAND_WEAPON",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 17)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_FLOAT",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 18)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_DONT_VALIDATE_TARGET",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 19)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_RANDOM_FIRING_DIRECTION",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 20)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_AUTOAIM_PROJECTILE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 21)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_TARGET_WEAPON",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 22)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_WEAPON_FOR_CONDITION",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 23)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_FORCE_CONDITION_ON_EVENT",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 24)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_HOLY_RADIUS_FOR_RANGE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 25)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_USE_CHANCE_PCODE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 26)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_SERVER_ONLY",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 27)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_CLIENT_ONLY",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 28)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_LASER_ATTACKS_LOCATION",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 29)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_AT_NEXT_COOLDOWN",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = (1 << 30)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG_AIM_WITH_WEAPON",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 1,
                BitMask = ((UInt32)1 << 31)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_AIM_WITH_WEAPON_ZERO",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_USE_PARAM0_PCODE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_USE_PARAM1_PCODE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_USE_PARAM2_PCODE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_USE_PARAM3_PCODE",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_USE_ULTIMATE_OWNER",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 5)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_CHARGE_POWER_AND_COOLDOWN",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 6)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_MARK_SKILL_AS_SUCCESSFUL",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 7)
            },
            new XmlCookElement
            {
                Name = "SKILL_EVENT_FLAG2_LASER_INCLUDE_IN_UI",
                ElementType = ElementType.Flag,
                DefaultValue = false,
                FlagId = 2,
                BitMask = (1 << 8)
            },
            new XmlCookElement
            {
                Name = "fTime",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "fRandChance",
                DefaultValue = 1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tParam0.flValue",
                TrueName = "tParam[0].flValue",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tParam1.flValue",
                TrueName = "tParam[1].flValue",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tParam2.flValue",
                TrueName = "tParam[2].flValue",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tParam3.flValue",
                TrueName = "tParam[3].flValue",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.eType",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.dwFlags",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.pszAttached",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.nVolume",
                DefaultValue = 1,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.nAttachedDefId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.pszBone",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.nBoneId",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vPosition.fX",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vPosition.fY",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vPosition.fZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vNormal.fX",
                DefaultValue = -1.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vNormal.fY",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.vNormal.fZ",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tCondition",
                DefaultValue = null,
                ElementType = ElementType.TableSingle,
                ChildType = typeof(ConditionDefinition)
            }
        };

        public SkillEvent()
        {
            RootElement = "SKILL_EVENT";
            base.Elements.AddRange(Elements);
            BitFlags = new Int32[] { -1, -1 };
        }
    }
}