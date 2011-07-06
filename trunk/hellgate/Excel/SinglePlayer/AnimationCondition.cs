using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class AnimationCondition
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string condition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined1;
        public Int32 priorityBoostSuccess;
        public Int32 priorityBoostFailure;
        [ExcelOutput(IsBool = true)]
        public Int32 removeOnFailure;
        [ExcelOutput(IsBool = true)]
        public Int32 checkOnPlay;
        [ExcelOutput(IsBool = true)]
        public Int32 checkOnContextChange;
        [ExcelOutput(IsBool = true)]
        public Int32 checkOnUpdateWeights;
        public Int32 undefined2;
        [ExcelOutput(IsBool = true)]
        public Int32 ignoreOnFailure;
        [ExcelOutput(IsBool = true)]
        public Int32 ignoreStanceOutsideCondition;
        public Int32 undefined3;
    }
}
