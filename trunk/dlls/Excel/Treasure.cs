using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    class Treasure: ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TreasureTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string treasureClass;
            public Int32 allowUnitTypes1;
            public Int32 allowUnitTypes2;
            public Int32 allowUnitTypes3;
            public Int32 allowUnitTypes4;
            public Int32 allowUnitTypes5;
            public Int32 allowUnitTypes6;
            public Int32 globalThemeRequired;
            public Int32 revivalTheme; // What is the real definition?
            public Int32 pickTypes;
            public Int32 picks;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown02;
            public float noDrop;
            public Int32 levelBoost;
            public float moneyChanceMultiplier;
            public float moneyLuckChanceMultiplier;
            public float moneyAmountMultiplier;
            public Int32 item1m;
            public Int32 item1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown03;
            public Int32 item1q;
            public Int32 item2m;
            public Int32 item2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown04;
            public Int32 item2q;
            public Int32 item3m;
            public Int32 item3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown05;
            public Int32 item3q;
            public Int32 item4m;
            public Int32 item4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown06;
            public Int32 item4q;
            public Int32 item5m;
            public Int32 item5;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown07;
            public Int32 item5q;
            public Int32 item6m;
            public Int32 item6;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown08;
            public Int32 item6q;
            public Int32 item7m;
            public Int32 item7;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown09;
            public Int32 item7q;
            public Int32 item8m;
            public Int32 item8;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown10;
            public Int32 item8q;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8804)]
            public byte[] unknown11;
            public Int32 spawmCondition;
            public Int32 spawnFromMonsterUnitType;
            public Int32 spawnFromLevelTheme;
        }

        public Treasure(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<TreasureTable>(data, ref offset, Count);
        }
    }
}
