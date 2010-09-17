using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AnimationGroups
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelAttribute(IsBool = true)]
        public Int32 playRightLeft;
        [ExcelAttribute(IsBool = true)]
        public Int32 playLegs;
        [ExcelAttribute(IsBool = true)]
        public Int32 onlyPlaySubGroups;
        [ExcelAttribute(IsBool = true)]
        public Int32 showInHammer;
        [ExcelAttribute(IsBool = true)]
        public Int32 copyFootSteps;
        public Int32 defaultStance;//idx
        public Int32 defaultStanceInTown;//idx
        [ExcelAttribute(IsBool = true)]
        public Int32 canStartSkillWithLeftweapon;
        public Single secondsToHoldStanceInTown;
        public Int32 undefined;
        public Int32 fallback;//idx
        public Int32 rightWeapon;//idx
        public Int32 leftWeapon;//idx
        public Int32 rightAnims;//idx
        public Int32 leftAnims;//idx
        public Int32 legAnims;//idx
    }
}