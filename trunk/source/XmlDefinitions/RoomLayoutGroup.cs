using System;

namespace Reanimator.XmlDefinitions
{
    class RoomLayoutGroup : XmlDefinition
    {
        private new static readonly XmlCookElement[] Elements =
        {
            new XmlCookElement
            {
                Name = "pszName",
                ElementType = ElementType.String,
                DefaultValue = null
            },
            new XmlCookElement
            {
                Name = "nTheme",
                ElementType = ElementType.ExcelIndex,
                DefaultValue = 0,
                ExcelTableCode = 20529 // LEVEL_THEMES
            },
            new XmlCookElement
            {
                Name = "pGroups",
                ElementType = ElementType.TableCount,
                DefaultValue = null,
                ChildType = typeof (RoomLayoutGroup)
            },
            new XmlCookElement
            {
                Name = "eType",
                ElementType = ElementType.Int32,
                DefaultValue = 0,
            },
            new XmlCookElement
            {
                Name = "nWeight",
                ElementType = ElementType.Int32,
                DefaultValue = 0,
            },
            new XmlCookElement
            {
                Name = "vPosition",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArray,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "vNormal",
                DefaultValue = 0.0f,
                ElementType = ElementType.FloatArray,
                Count = 3
            },
            new XmlCookElement
            {
                Name = "fRotation",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "nModelId",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = -1   // INVALID_ID
            },
            new XmlCookElement
            {
                Name = "nLayoutId",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0   // INVALID_ID
            },
            new XmlCookElement
            {
                Name = "bInitialized",
                ElementType = ElementType.Int32,
                DefaultValue = 0   // FALSE
            },
            new XmlCookElement
            {
                Name = "dwUnitType",
                ElementType = ElementType.Int32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "dwCode",
                ElementType = ElementType.Int32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "nVolume",
                ElementType = ElementType.Int32,
                DefaultValue = 0
            },
            new XmlCookElement
            {
                Name = "nQuest",
                ElementType = ElementType.ExcelIndex,
                DefaultValue = 0,
                ExcelTableCode = 18226 // QUEST
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_WEIGHT_PERCENTAGE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 0)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_RANDOM_ROTATIONS",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 1)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_AI_NODE_CROUCH",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 2)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_AI_NODE_DOORWAY",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 3)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_AI_NODE_LARGE_COVER",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 4)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_AI_NODE_STONE",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 8)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_NOT_THEME",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 5)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_NO_THEME",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 6)
            },
            new XmlCookElement
            {
                Name = "ROOM_LAYOUT_FLAG_EXPANDED",
                DefaultValue = false,
                ElementType = ElementType.Flag,
                FlagId = 1,
                BitMask = (1 << 7)
            },
            new XmlCookElement
            {
                Name = "fBuffer",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fFalloffNear",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "fFalloffFar",
                ElementType = ElementType.Float,
                DefaultValue = 0.0f
            },
            new XmlCookElement
            {
                Name = "bReadOnly",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "bFollowed",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "bPropIsValid",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "bPropIsChecked",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
            new XmlCookElement
            {
                Name = "bDisplayAppearance",
                ElementType = ElementType.NonCookedInt32,
                DefaultValue = 0 // FALSE
            },
        };

        public RoomLayoutGroup()
        {
            RootElement = "ROOM_LAYOUT_GROUP";
            base.Elements.AddRange(Elements);
            BitFields = new Int32[] {-1};
        }
    }
}