using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageTypes
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int16 code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string miniIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string String;
        short undefined1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 color;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 shieldHit;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 criticalState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 softHitState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 mediumHitState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 bigHitState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 fumbleHitState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 damageOverTimeState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 fieldMissile;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 invulnerableState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 invulnerableSfxState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 thornsState;
        public Int32 vulnerabilityInPVPTugboat;
        Int32 vulnerabilityInPVPHellgate;
    }
}