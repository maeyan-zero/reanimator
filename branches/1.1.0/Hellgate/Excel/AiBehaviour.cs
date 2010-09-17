using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiBehaviour
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String stringDesc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public String function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String param0Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String param1Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String param2Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String param3Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String param4Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String param5Desc;
        public Single priority;
        public Single param0Default;
        public Single param1Default;
        public Single param2Default;
        public Single param3Default;
        public Single param4Default;
        public Single param5Default;
        public Int32 functionOld;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesSkill1;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesSkill2;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesState;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesStat;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesSound;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesMonster;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesString;
        [ExcelAttribute(IsBool = true)]
        public Int32 canBranch;
        [ExcelAttribute(IsBool = true)]
        public Int32 forceExitBranch;
        public Int32 timerIndex;
        public Int32 usesOnce;
        public Int32 usesRun;
        public Int32 usesFly;
        public Int32 usesDontStop;
        public Int32 usesWarp;
        public Int32 undefined2;
    }
}