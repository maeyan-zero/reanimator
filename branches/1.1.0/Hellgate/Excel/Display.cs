using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Display
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1, SortDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String key;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String exampleDescription;
        [ExcelAttribute(IsBool = true)]
        public Int32 rider;
        public Int32 rule1;
        public Int32 rule2;
        public Int32 rule3;
        [ExcelAttribute(IsBool = true)]
        public Int32 inclUnitInCond1;
        [ExcelAttribute(IsBool = true)]
        public Int32 inclUnitInCond2;
        [ExcelAttribute(IsBool = true)]
        public Int32 inclUnitInCond3;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 displayCondition1;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 displayCondition2;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 displayCondition3;
        [ExcelAttribute(IsBool = true)]
        public Int32 newLine;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 formatString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 formatShort;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 decripShort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String iconFrame;
        public Int32 ctrlStat;//idx
        public Int32 displayCtrl;
        public Int32 displayFunc;
        public Int32 ctrl1;
        public Int32 ctrl2;
        public Int32 ctrl3;
        public Int32 ctrl4;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 val1;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 val2;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 val3;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 val4;
        public Int32 toolTipArea;
        [ExcelAttribute(IsStringIndex= true)]
        public Int32 toolTipText;//stridx
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 color;
    }
}