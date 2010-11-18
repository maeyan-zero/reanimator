using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiBehaviourRow
    {
        ExcelFile.TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string stringDesc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string param0Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string param1Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string param2Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string param3Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string param4Desc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string param5Desc;
        public float priority;
        public float param0Default;
        public float param1Default;
        public float param2Default;
        public float param3Default;
        public float param4Default;
        public float param5Default;
        public Int32 functionOld;
        [ExcelOutput(IsBool = true)]
        public Int32 usesSkill1;
        [ExcelOutput(IsBool = true)]
        public Int32 usesSkill2;
        [ExcelOutput(IsBool = true)]
        public Int32 usesState;
        [ExcelOutput(IsBool = true)]
        public Int32 usesStat;
        [ExcelOutput(IsBool = true)]
        public Int32 usesSound;
        [ExcelOutput(IsBool = true)]
        public Int32 usesMonster;
        [ExcelOutput(IsBool = true)]
        public Int32 usesString;
        [ExcelOutput(IsBool = true)]
        public Int32 canBranch;
        [ExcelOutput(IsBool = true)]
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
