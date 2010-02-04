using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Reanimator.Excel
{
    public class DamageTypes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class DamageTypesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
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
            public Int32 vulnerabilityInPVPHellgate;
        }

        public DamageTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<DamageTypesTable>(data, ref offset, Count);
        }
    }
}