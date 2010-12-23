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
        public short code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string miniIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string String;
        short undefined1;
        public Int32 color;
        public Int32 shieldHit;
        public Int32 criticalState;
        public Int32 softHitState;
        public Int32 mediumHitState;
        public Int32 bigHitState;
        public Int32 fumbleHitState;
        public Int32 damageOverTimeState;
        public Int32 fieldMissile;
        public Int32 invulnerableState;
        public Int32 invulnerableSfxState;
        public Int32 thornsState;
        public Int32 vulnerabilityInPVPTugboat;
        Int32 vulnerabilityInPVPHellgate;
    }
}