using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Stats : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StatsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stat;

            public Int16 code;
            public Int16 type;
            public Int16 assocStat1;
            public Int16 assocStat2;
            public Int16 regenIntervalInMS;
            public Int16 regenDivisor;
            public Int16 regenDelayOnDec;
            public Int16 regenDelayOnZero;
            public Int16 regenDelayMonster;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte undefined1;
            public Int16 offset;
            public Int16 shift;
            public Int16 minSet;
            public Int16 maxSet;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte undefined3;
            public Int16 minAssert;
            public Int16 maxAssert;
            public Int16 undefined4;
            public Int16 accrueToTypes1;
            public Int16 accrueToTypes2;
            public Int16 unitType;
            public Int16 bitMask1;/*1 bit cur
	2 bit modlist
	3 bit vector
	4 bit float
	6 bit accrue
	7 bit accrue once only
	8 bit combat
	9 bit directdmg
	10 bit send
	11 bit sendall
	12 bit save
	14 bit no max/cur when dead
	15 bit state monitors c
	16 bit state monitors s
	17 bit transfer
	18 bit transfer to misile
	19 bit calc rider
	20 bit calc
	21 bit update database
	22 bit don't transfer to nonweapon missile */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte undefined5;
            public Int16 player;//bool
            public Int16 monster;
            public Int16 missile;
            public Int16 item;
            public Int16 Object;
            public Int16 valbits;
            public Int16 valWindow;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte undefined6;
            public Int16 valShift;
            public Int16 valOffs;
            public Int16 valTable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte undefined7;
            public Int16 param1Bits;
            public Int16 param2Bits;
            public Int16 param3Bits;
            public Int16 param4Bits;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte undefined8;
            public Int16 param1Shift;
            public Int16 param2Shift;
            public Int16 param3Shift;
            public Int16 param4Shift;
            public Int16 param1Offs;
            public Int16 param2Offs;
            public Int16 param3Offs;
            public Int16 param4Offs;
            public Int16 param1Table;
            public Int16 param2Table;
            public Int16 param3Table;
            public Int16 param4Table;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            byte undefined9;
            public Int16 minTicksBetweenDbCommand;
            public Int16 databaseUnitField;
            public Int16 specFunc;
            public Int16 sfStat1;
            public Int16 sfStat2;
            public Int16 sfStat3;
            public Int16 sfStat4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte undefined10;
            public Int16 checkAgainstTypes1;
            public Int16 checkAgainstTypes2;
            public Int16 reqFailString;//stridx
            public Int16 reqFailStringHellgate;
            public Int16 reqFailStringMythos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte undefined11;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string versionFunction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte undefined12;

        }

        public Stats(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StatsTable>(data, ref offset, Count);
        }

        public string GetStringFromId(int id)
        {
            foreach (StatsTable statsTable in tables)
            {
                if (statsTable.code == id)
                {
                    return statsTable.stat;
                }
            }

            return "NOT FOUND";
        }
    }
}
