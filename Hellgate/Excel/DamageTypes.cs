using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageTypes
    {
        TableHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String stat;
        public UInt16 code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String miniIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String String;
        public UInt16 undefined1;
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
        public Int32 vulnerabilityInPVPHellgate;
    }
}