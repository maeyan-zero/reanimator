using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageTypesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;
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