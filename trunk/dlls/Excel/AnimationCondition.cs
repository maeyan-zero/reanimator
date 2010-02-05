using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class AnimationCondition : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AnimationConditionTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string condition;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] undefined1;
            public Int32 priorityBoostSuccess;
            public Int32 priorityBoostFailure;
            public Int32 removeOnFailure;//bool
            public Int32 checkOnPlay;//bool
            public Int32 checkOnContextChange;//bool
            public Int32 checkOnUpdateWeights;//bool
            public Int32 undefined2;
            public Int32 ignoreOnFailure;//bool
            public Int32 ignoreStanceOutsideCondition;//bool
            public Int32 undefined3;
        }

        public AnimationCondition(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AnimationConditionTable>(data, ref offset, Count);
        }
    }
}
