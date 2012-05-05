using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Display
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string key;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string exampleDescription;
        [ExcelOutput(IsBool = true)]
        public Int32 rider;
        public Rule rule1;
        public Rule rule2;
        public Rule rule3;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond1;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond2;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond3;
        [ExcelOutput(IsScript = true)]
        public Int32 displayCondition1;
        [ExcelOutput(IsScript = true)]
        public Int32 displayCondition2;
        [ExcelOutput(IsScript = true)]
        public Int32 displayCondition3;
        [ExcelOutput(IsBool = true)]
        public Int32 newLine;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 formatString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 formatShort;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 decripShort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String iconFrame;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 ctrlStat;//idx
        public DisplayCtrl displayCtrl;
        public DisplayFunc displayFunc;
        public Ctrl ctrl1;
        public Ctrl ctrl2;
        public Ctrl ctrl3;
        public Ctrl ctrl4;
        [ExcelOutput(IsScript = true)]
        public Int32 val1;
        [ExcelOutput(IsScript = true)]
        public Int32 val2;
        [ExcelOutput(IsScript = true)]
        public Int32 val3;
        [ExcelOutput(IsScript = true)]
        public Int32 val4;
        public ToolTipArea toolTipArea;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 toolTipText;//stridx
        [ExcelOutput(IsScript = true)]
        public Int32 color;

        public enum ToolTipArea
        {
            Null = -1,
            Other = 0,
            Header = 1,
            Mod = 2,
            Damage = 3,
            Armor = 4,
            Level = 5,
            Price = 6,
            Requirements = 7,
            ItemUnique = 8,
            Ingredient = 9,
            ItemTypes = 10,
            ModIcons = 11,
            Dps = 12,
            Feeds = 13,
            Extras = 14,
            Usable = 15,
            Sfx = 16,
            WeaponStats = 17,
            AffixProps = 18,
            OtherPropsHdr = 19,
            OtherProps = 20,
            ModList = 21,
            Defense = 22,
            MeleeSpeed = 23,
            AboveHead = 24,
            SetItemPropsHdr = 25,
            SetItemProps = 26,
            None = 27,
            SpecialItem = 28
        }

        public enum DisplayCtrl
        {
            Null = -1,
            Range = 0,
            RangeBase = 1
        }

        public enum DisplayFunc
        {
            Null = -1,
            Format = 0,
            Inventory = 1
        }

        public enum Rule
        {
            Null = -1,
            Always = 0,
            Ditto = 1,
            ElseIf = 2,
            OneOf = 3
        }
        public enum Ctrl
        {
            Null = -1,
            ScriptNoprint = 0,
            Script = 1,
            ScriptPlusminus = 2,
            ScriptPlusminusNozero = 3,
            UnitName = 4,
            UnitNameOnly = 5,
            UnitClass = 6,
            UnitGuild = 7,
            ParamString = 8,
            StatValue = 9,
            Plusminus = 10,
            PlusminusNozero = 11,
            UnitType = 12,
            UnitTypeLastonly = 13,
            UnitTypeQuality = 14,
            UnitAdditional = 15,
            Affix = 16,
            FlavorText = 17,
            DivBy100 = 18,
            DivBy10 = 19,
            DivBy20 = 20,
            ParamStringPlural = 21,
            ItemQuality = 22,
            ClassRequirements = 23,
            FactionRequirements = 24,
            SkillGroup = 25,
            ItemUnique = 26,
            ItemIngredient = 27,
            AffixPropsList = 28,
            InvItemProperties = 29,
            ItemDmgDesc = 30,
            ScriptTime = 31,
            Time = 32,
            SetitemAffixPropsList = 33,
            ItemList = 34,
            SexRequirements = 35,
            ItemFlavorText = 36,
        }
    }
}