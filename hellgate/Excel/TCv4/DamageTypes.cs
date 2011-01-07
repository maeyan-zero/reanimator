using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageTypesTCv4
    {
        ExcelFile.RowHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;
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
        public Int32 vulnerabilityInPVPHellgate;
        public Int32 sfxInvulnerabilityDurationMultInPvpTugboat_tcv4;
        public Int32 sfxInvulnerabilityDurationMultInPvpHellgate_tcv4;
    }
}