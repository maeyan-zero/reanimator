using System;
using System.Runtime.InteropServices;
using Revival.Common;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class StatsRow
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String stat;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 type;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 assocStat1;                                                                        // 48
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 assocStat2;                                                                        // 4C
        public Int32 regenIntervalInMS;                                                                 // 50
        public Int32 regenDivisor;                                                                      // 54
        public Int32 regenDelayOnDec;                                                                   // 58
        public Int32 regenDelayOnZero;                                                                  // 5C
        public Int32 regenDelayMonster;                                                                 // 60
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 undefined1;                                                                        // 64
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 undefined2;                                                                        // 68
        public Int32 offset;                                                                            // 6C
        public Int32 shift;                                                                             // 70
        public Int32 minSet;                                                                            // 74
        public Int32 maxSet;                                                                            // 78
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 undefined3;                                                                        // 7C
        public Int32 minAssert;                                                                         // 80
        public Int32 maxAssert;                                                                         // 84
        public Int32 undefined4;                                                                        // 88
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 accrueToTypes1;                                                                    // 8C
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 accrueToTypes2;                                                                    // 90
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType;                                                                          // 94
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;                                                                     // 98
        Int32 undefined5;                                                                               // 9C
        [ExcelOutput(IsBool = true)]
        public Int32 player;                                                                            // A0
        [ExcelOutput(IsBool = true)]
        public Int32 monster;                                                                           // A4
        [ExcelOutput(IsBool = true)]
        public Int32 missile;                                                                           // A8
        [ExcelOutput(IsBool = true)]
        public Int32 item;                                                                              // AC
        [ExcelOutput(IsBool = true)]
        public Int32 Object;                                                                            // B0
        public Int32 valbits;                                                                           // B4
        public Int32 valWindow;                                                                         // B8
        Int32[] undefined6;                                                                             // BC
        public Int32 valShift;                                                                          // C0
        public Int32 valOffs;                                                                           // C4
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 valTable;                                                                          // C8
        public ExcelFile ValueExcelTable; // custom row - this is total params bit count client-side    // CC
        public int ParamCount; // this is "param" count client-side (number of non-zero paramXBits)     // D0
        public Int32 param1Bits;                                                                        // D4
        public Int32 param2Bits;
        public Int32 param3Bits;
        public Int32 param4Bits;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined8;
        public Int32 param1Shift;
        public Int32 param2Shift;
        public Int32 param3Shift;
        public Int32 param4Shift;
        public Int32 param1Offs;
        public Int32 param2Offs;
        public Int32 param3Offs;
        public Int32 param4Offs;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 param1Table;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 param2Table;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 param3Table;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 param4Table;
        public ExcelFile Param1ExcelTable; // custom row
        public ExcelFile Param2ExcelTable; // custom row
        public ExcelFile Param3ExcelTable; // custom row
        public ExcelFile Param4ExcelTable; // custom row
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined9;
        public Int32 minTicksBetweenDbCommand;
        public Int32 databaseUnitField;
        public Int32 specFunc;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 sfStat1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 sfStat2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 sfStat3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 sfStat4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] undefined10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 checkAgainstTypes1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 checkAgainstTypes2;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 reqFailString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 reqFailStringHellgate;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 reqFailStringMythos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] undefined11;
        public Int32 undefined11a;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string versionFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefined12;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]//table 
        public Int32 param5Table;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public Int32[] undefined13;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            _undefined1 = (1 << 0),
            cur = (1 << 1),
            modList = (1 << 2),
            vector = (1 << 3),
            Float = (1 << 4),
            _undefined2 = (1 << 5),
            accrue = (1 << 6),
            accrueOnceOnly = (1 << 7),
            combat = (1 << 8),
            directDmg = (1 << 9),
            send = (1 << 10),
            sendAll = (1 << 11),
            save = (1 << 12),
            _undefined3 = (1 << 13),
            noMaxCurWhenDead = (1 << 14),
            stateMonitorsC = (1 << 15),
            stateMonitorsS = (1 << 16),
            transfer = (1 << 17),
            transferToMissile = (1 << 18),
            calcRider = (1 << 19),
            calc = (1 << 20),
            updateDatabase = (1 << 21),
            dontTranferToNonWeaponMissile = (1 << 22)
        }
    }
}
