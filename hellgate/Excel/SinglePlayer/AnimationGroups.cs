using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class AnimationGroups
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 playRightLeft;
        [ExcelOutput(IsBool = true)]
        public Int32 playLegs;
        [ExcelOutput(IsBool = true)]
        public Int32 onlyPlaySubGroups;
        [ExcelOutput(IsBool = true)]
        public Int32 showInHammer;
        [ExcelOutput(IsBool = true)]
        public Int32 copyFootSteps;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_STANCE")]
        public Int32 defaultStance;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_STANCE")]
        public Int32 defaultStanceInTown;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 canStartSkillWithLeftweapon;
        public float secondsToHoldStanceInTown;
        public Int32 undefined;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 fallback;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 rightWeapon;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 leftWeapon;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 rightAnims;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 leftAnims;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 legAnims;//idx
    }
}
