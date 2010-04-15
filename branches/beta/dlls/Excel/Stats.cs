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
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stat;

            public Int32 code;
            public Int32 type;
            public Int32 assocStat1;
            public Int32 assocStat2;
            public Int32 regenIntervalInMS;
            public Int32 regenDivisor;
            public Int32 regenDelayOnDec;
            public Int32 regenDelayOnZero;
            public Int32 regenDelayMonster;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] undefined1;
            public Int32 offset;
            public Int32 shift;
            public Int32 minSet;
            public Int32 maxSet;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] undefined3;
            public Int32 minAssert;
            public Int32 maxAssert;
            public Int32 undefined4;
            public Int32 accrueToTypes1;
            public Int32 accrueToTypes2;
            public Int32 unitType;
            public Int32 bitMask1;/*1 bit cur
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
                        byte[] undefined5;
            public Int32 player;//bool
            public Int32 monster;
            public Int32 missile;
            public Int32 item;
            public Int32 Object;
            public Int32 valbits;
            public Int32 valWindow;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] undefined6;
            public Int32 valShift;
            public Int32 valOffs;
            public Int32 valTable;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] undefined7;
            public Int32 param1Bits;
            public Int32 param2Bits;
            public Int32 param3Bits;
            public Int32 param4Bits;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] undefined8;
            public Int32 param1Shift;
            public Int32 param2Shift;
            public Int32 param3Shift;
            public Int32 param4Shift;
            public Int32 param1Offs;
            public Int32 param2Offs;
            public Int32 param3Offs;
            public Int32 param4Offs;
            public Int32 param1Table;
            public Int32 param2Table;
            public Int32 param3Table;
            public Int32 param4Table;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            byte[] undefined9;
            public Int32 minTicksBetweenDbCommand;
            public Int32 databaseUnitField;
            public Int32 specFunc;
            public Int32 sfStat1;
            public Int32 sfStat2;
            public Int32 sfStat3;
            public Int32 sfStat4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] undefined10;
            public Int32 checkAgainstTypes1;
            public Int32 checkAgainstTypes2;
            public Int32 reqFailString;//stridx
            public Int32 reqFailStringHellgate;
            public Int32 reqFailStringMythos;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] undefined11;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string versionFunction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] undefined12;

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
