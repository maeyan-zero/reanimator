using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DisplayRow
    {
        ExcelFile.TableHeader header;
        [ExcelOutput(SortAscendingID = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string key;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string exampleDescription;
        [ExcelOutput(IsBool = true)]
        public Int32 rider;
        public Int32 rule1;
        public Int32 rule2;
        public Int32 rule3;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond1;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond2;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond3;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 displayCondition1;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 displayCondition2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 displayCondition3;
        [ExcelOutput(IsBool = true)]
        public Int32 newLine;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_DisplayFormat")]
        public Int32 formatString;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_DisplayFormat")]
        public Int32 formatShort;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_DisplayFormat")]
        public Int32 decripShort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        string iconFrame;
        public Int32 ctrlStat;//idx
        public Int32 displayCtrl;
        public Int32 displayFunc;
        public Int32 ctrl1;
        public Int32 ctrl2;
        public Int32 ctrl3;
        public Int32 ctrl4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 val1;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 val2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 val3;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 val4;
        public Int32 toolTipArea;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_DisplayFormat")]
        public Int32 toolTipText;//stridx
        [ExcelOutput(IsIntOffset = true)]
        public Int32 color;
    }
}