using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PlayerLevelRow
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1, SecondarySortColumn = "level", IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType;//idx
        public Int32 level;
        public Int32 experience;
        public Int32 hpMax;
        public Int32 undefined1;
        public Int32 attackRating;
        public Int32 sfxDefense;
        public Int32 aiChangerAttack;
        public Int32 statPoints;
        public Int32 skillPoints;//not active, maybe for mythos?
        public Int32 craftingPoints;//mythos only?
        public Int32 skillPowerCost;
        public Int32 headstoneReturnCost;
        public Int32 deathExperiencePenaltyPercent;

        public Int32 unknown1;

        public Int32 restartHealthPercent;
        public Int32 restartPowerPercent;
        public Int32 restartShieldPercent;
        [ExcelOutput(IsScript = true)]
        public Int32 prop1;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 prop2;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 prop3;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 prop4;//intptr
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 spellSlot1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 spellSlot2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 spellSlot3;//idx
    }
}