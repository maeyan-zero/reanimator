using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AnimationCondition
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String condition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined1;
        public Int32 priorityBoostSuccess;
        public Int32 priorityBoostFailure;
        [ExcelAttribute(IsBool = true)]
        public Int32 removeOnFailure;
        [ExcelAttribute(IsBool = true)]
        public Int32 checkOnPlay;
        [ExcelAttribute(IsBool = true)]
        public Int32 checkOnContextChange;
        [ExcelAttribute(IsBool = true)]
        public Int32 checkOnUpdateWeights;
        public Int32 undefined2;
        [ExcelAttribute(IsBool = true)]
        public Int32 ignoreOnFailure;
        [ExcelAttribute(IsBool = true)]
        public Int32 ignoreStanceOutsideCondition;
        public Int32 undefined3;
    }
}