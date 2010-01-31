using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reanimator.Excel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Reanimator
{
    class Treasure: ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct TreasureTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown01;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] treasureClass;
            public Int32 allowUnitTypes1 { get; set; }
            public Int32 allowUnitTypes2 { get; set; }
            public Int32 allowUnitTypes3 { get; set; }
            public Int32 allowUnitTypes4 { get; set; }
            public Int32 allowUnitTypes5 { get; set; }
            public Int32 allowUnitTypes6 { get; set; }
            public Int32 globalThemeRequired { get; set; }
            public Int32 revivalTheme { get; set; } // What is the real definition?
            public Int32 pickTypes { get; set; }
            public Int32 picks { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown02;
            public float noDrop { get; set; }
            public Int32 levelBoost { get; set; }
            public float moneyChanceMultiplier { get; set; }
            public float moneyLuckChanceMultiplier { get; set; }
            public float moneyAmountMultiplier { get; set; }
            public Int32 item1m { get; set; }
            public Int32 item1 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown03;
            public Int32 item1q { get; set; }
            public Int32 item2m { get; set; }
            public Int32 item2 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown04;
            public Int32 item2q { get; set; }
            public Int32 item3m { get; set; }
            public Int32 item3 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown05;
            public Int32 item3q { get; set; }
            public Int32 item4m { get; set; }
            public Int32 item4 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown06;
            public Int32 item4q { get; set; }
            public Int32 item5m { get; set; }
            public Int32 item5 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown07;
            public Int32 item5q { get; set; }
            public Int32 item6m { get; set; }
            public Int32 item6 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown08;
            public Int32 item6q { get; set; }
            public Int32 item7m { get; set; }
            public Int32 item7 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown09;
            public Int32 item7q { get; set; }
            public Int32 item8m { get; set; }
            public Int32 item8 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
            public byte[] unknown10;
            public Int32 item8q { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8804)]
            public byte[] unknown11;
            public Int32 spawmCondition { get; set; }
            public Int32 spawnFromMonsterUnitType { get; set; }
            public Int32 spawnFromLevelTheme { get; set; }
        }

        List<TreasureTable> treasure;

        public Treasure(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return treasure.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            MessageBox.Show("Sorry, had to comment out the \"ParseTables\" function at the end of \"Treasure.cs\" as the Type \"ItemsTable\" couldn't be found.");
            //treasure = ExcelTables.ReadTables<ItemsTable>(data, ref offset, Count);
        }
    }
}
