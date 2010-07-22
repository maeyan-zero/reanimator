using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AnimationConditionRow
    {
        ExcelFile.TableHeader header;

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
}