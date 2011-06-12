using System;
using Hellgate.Excel;
using Hellgate.Excel.JapaneseBeta;
using Revival.Common;

namespace Hellgate.Xml
{
    [XmlCookedAttribute(Name = "ROOM_LAYOUT_GROUP")]
    public class RoomLayoutGroup
    {
        [Flags]
        public enum LayoutFlags : uint
        {
            ROOM_LAYOUT_FLAG_WEIGHT_PERCENTAGE = (1 << 0),
            ROOM_LAYOUT_FLAG_RANDOM_ROTATIONS = (1 << 1),
            ROOM_LAYOUT_FLAG_AI_NODE_CROUCH = (1 << 2),
            ROOM_LAYOUT_FLAG_AI_NODE_DOORWAY = (1 << 3),
            ROOM_LAYOUT_FLAG_AI_NODE_LARGE_COVER = (1 << 4),
            ROOM_LAYOUT_FLAG_AI_NODE_STONE = (1 << 8),
            ROOM_LAYOUT_FLAG_NOT_THEME = (1 << 5),
            ROOM_LAYOUT_FLAG_NO_THEME = (1 << 6),
            ROOM_LAYOUT_FLAG_EXPANDED = (1 << 7),

            [XmlCookedAttribute(IsTestCentre = true)]
            ROOM_LAYOUT_FLAG_TESTCENTER = (1 << 9)
        }

        [XmlCookedAttribute(
            Name = "pszName",
            ElementType = ElementType.String,
            DefaultValue = null)]
        public String Name;

        [XmlCookedAttribute(
            Name = "nTheme",
            ElementType = ElementType.ExcelIndex,
            DefaultValue = 0,
            TableCode = Xls.TableCodes.LEVEL_THEMES)] // 20529 LEVEL_THEMES
        public LevelThemesRow Theme;
        public int ThemeRowIndex;

        [XmlCookedAttribute(
            Name = "pGroups",
            ElementType = ElementType.TableArrayVariable,
            DefaultValue = null,
            ChildType = typeof(RoomLayoutGroup))]
        public RoomLayoutGroup[] Groups;

        [XmlCookedAttribute(
            Name = "eType",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public int Type;

        [XmlCookedAttribute(
            Name = "nWeight",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public int Weight;

        [XmlCookedAttribute(
            Name = "vPosition",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3 Position;

        [XmlCookedAttribute(
            Name = "vNormal",
            DefaultValue = 0.0f,
            ElementType = ElementType.FloatArrayFixed,
            Count = 3,
            CustomType = ElementType.Vector3)]
        public Vector3 Normal;

        [XmlCookedAttribute(
            Name = "fRotation",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float Rotation;

        [XmlCookedAttribute(
            Name = "nModelId",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = -1)] // INVALID_ID
        public int ModelId;

        [XmlCookedAttribute(
            Name = "nLayoutId",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = 0)] // INVALID_ID
        public int LayoutId;

        [XmlCookedAttribute(
            Name = "bInitialized",
            ElementType = ElementType.Int32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool Initialized;

        [XmlCookedAttribute(
            Name = "dwUnitType",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public int UnitType; // todo: what is this from? UnitType table? Or like UnitObjects UnitType values? (which appear to also be row index values from UnitTypes table anyways)

        [XmlCookedAttribute(
            Name = "dwCode",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public int Code;

        [XmlCookedAttribute(
            Name = "nVolume",
            ElementType = ElementType.Int32,
            DefaultValue = 0)]
        public int Volume;

        [XmlCookedAttribute(
            Name = "nQuest",
            ElementType = ElementType.ExcelIndex,
            DefaultValue = 0,
            TableCode = Xls.TableCodes.QUEST)] // 18226 QUEST
        public QuestRow Quest;
        public int QuestRowIndex;

        [XmlCookedAttribute(
            Name = "Flags",
            DefaultValue = false,
            ElementType = ElementType.Flag,
            FlagId = 1)]
        public LayoutFlags Flags;

        [XmlCookedAttribute(
            Name = "nMonsterLevelAdjust",
            ElementType = ElementType.Int32,
            DefaultValue = 0,
            IsResurrection = true)]
        public int MonsterLevelAdjust;

        [XmlCookedAttribute(
            Name = "fBuffer",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float Buffer;

        [XmlCookedAttribute(
            Name = "fFalloffNear",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float FalloffNear;

        [XmlCookedAttribute(
            Name = "fFalloffFar",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f)]
        public float FalloffFar;

        //[XmlCookedAttribute(
        //    Name = "vScale",
        //    ElementType = ElementType.FloatArrayFixed,
        //    DefaultValue = 1.0f,
        //    Count = 3,
        //    IsTestCentre = true,
        //    CustomType = ElementType.Vector3)]
        //public Vector3 Scale;

        [XmlCookedAttribute(
            Name = "fSpawnClassRadius",
            ElementType = ElementType.Float,
            DefaultValue = 0.0f,
            IsTestCentre = true)]
        public float SpawnClassRadius;

        [XmlCookedAttribute(
            Name = "iSpawnClassExecuteXTimes",
            ElementType = ElementType.Int32,
            DefaultValue = 1,
            IsTestCentre = true)]
        public int SpawnClassExecuteXTimes;

        [XmlCookedAttribute(
            Name = "bReadOnly",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool ReadOnly;

        [XmlCookedAttribute(
            Name = "bFollowed",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool Followed;

        [XmlCookedAttribute(
            Name = "bPropIsValid",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool PropIsValid;

        [XmlCookedAttribute(
            Name = "bPropIsChecked",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool PropIsChecked;

        [XmlCookedAttribute(
            Name = "bDisplayAppearance",
            ElementType = ElementType.NonCookedInt32,
            DefaultValue = false,
            CustomType = ElementType.Bool)]
        public bool DisplayAppearance;
		
        [XmlCookedAttribute(
            Name = "fScale",
            ElementType = ElementType.Float,
            DefaultValue = 1.0f,
            IsTestCentre = true,
			IsResurrection = true)]
        public float Scale;
    }
}