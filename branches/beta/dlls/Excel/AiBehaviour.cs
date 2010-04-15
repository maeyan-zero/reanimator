using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class AiBehaviour : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AiBehaviourTable
        {
            TableHeader header;

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
            public Int32 usesSkill1;//bool
            public Int32 usesSkill2;//bool
            public Int32 usesState;//bool
            public Int32 usesStat;//bool
            public Int32 usesSound;//bool
            public Int32 usesMonster;//bool
            public Int32 usesString;//bool
            public Int32 canBranch;//bool
            public Int32 forceExitBranch;//bool
            public Int32 timerIndex;
            public Int32 usesOnce;
            public Int32 usesRun;
            public Int32 usesFly;
            public Int32 usesDontStop;
            public Int32 usesWarp;
            public Int32 undefined2;
        }

        public AiBehaviour(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AiBehaviourTable>(data, ref offset, Count);
        }
    }
}
