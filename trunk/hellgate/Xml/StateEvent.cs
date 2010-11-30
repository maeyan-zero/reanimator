using System;

namespace Hellgate.Xml
{
    class StateEvent : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_FORCE_NEW",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_FIRST_PERSON",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_ADD_TO_CENTER",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_CONTROL_UNIT_ONLY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_FLOAT",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_OWNED_BY_CONTROL",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 5)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_SET_IMMEDIATELY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 6)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_CLEAR_IMMEDIATELY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 7)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_NOT_CONTROL_UNIT",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 8)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_ON_WEAPONS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 9)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_IGNORE_CAMERA",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 10)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_ON_CLEAR",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 11)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_CHECK_CONDITION_ON_CLEAR",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 12)
            },
            new XmlCookElement
            {
                Name = "STATE_EVENT_FLAG_SHARE_DURATION",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 13)
            },
            new XmlCookElement
            {
                Name = "eType",
                DefaultValue = null,
                ElementType = ElementType.ExcelIndex,
                ExcelTableCode = 22320 // STATE_EVENT_TYPES
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
                Name = "tAttachmentDef.nVolume",
                DefaultValue = 1,
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
                Name = "tAttachmentDef.fRotation",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.fYaw",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.fPitch",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "tAttachmentDef.fRoll",
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
                Name = "pszExcelString",
                DefaultValue = null,
                ElementType = ElementType.String
            },
            new XmlCookElement
            {
                Name = "nExcelIndex",
                DefaultValue = -1,
                ElementType = ElementType.NonCookedInt32
            },
            new XmlCookElement
            {
                Name = "nData",
                DefaultValue = 0,
                ElementType = ElementType.Int32
            },
            new XmlCookElement
            {
                Name = "fParam",
                DefaultValue = 0.0f,
                ElementType = ElementType.Float
            },
            new XmlCookElement
            {
                Name = "pszData",
                DefaultValue = null,
                ElementType = ElementType.String,
                TreatAsData = true
            },
            new XmlCookElement
            {
                Name = "tCondition",
                DefaultValue = null,
                ElementType = ElementType.TableSingle,
                ChildType = typeof(ConditionDefinition)
            }
        };

        public StateEvent()
        {
            RootElement = "STATE_EVENT";
            base.Elements.AddRange(Elements);
            BitFlags = new Int32[] {-1};
        }
    }
}
