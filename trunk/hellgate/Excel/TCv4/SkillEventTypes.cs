using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillEventTypesTCv4
    {
        ExcelFile.RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc4;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask02 bitmask02;
        public Int32 attachmentTable;//tbl
        [ExcelOutput(IsBool = true)]
        public Int32 paramContainsCount;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 doesNotRequireTableEntry;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 applySkillStats;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesAttachment;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesBones;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesBoneWeights;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 serverOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clientOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 needsDuration;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 aimingEvent;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isMelee;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isRanged;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isSpell;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 subskillInherit;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 convertDegreesToDot;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesTargetIndex;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 testsItsCondition;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesLasers;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesMissiles;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 canMultiBullets;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 paramsAreUsedInSkillString;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 startsCoolingAndPowerCost;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 consumesItem;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 checkPetPowerCost;//bool
        public Int32 undefined1_tcv4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string eventHandler;
        public Int32 eventStringFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        byte[] undefined1;
        public Int32 undefined2_tcv4;
    }
}