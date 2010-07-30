using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DisplayRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string key;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string exampleDescription;
        public Int32 rider;//bool
        public Int32 rule1;
        public Int32 rule2;
        public Int32 rule3;
        public Int32 inclUnitInCond1;//bool
        public Int32 inclUnitInCond2;//bool
        public Int32 inclUnitInCond3;//bool
        public Int32 displayCondition1;//intptr
        public Int32 displayCondition2;//intptr
        public Int32 displayCondition3;//intptr
        public Int32 newLine;//bool
        public Int32 formatString;//stridx
        public Int32 formatShort;//stridx
        public Int32 decripShort;//stridx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string iconFrame;
        public Int32 ctrlStat;//idx
        public Int32 displayCtrl;
        public Int32 displayFunc;
        public Int32 ctrl1;
        public Int32 ctrl2;
        public Int32 ctrl3;
        public Int32 ctrl4;
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 1)]
        public Int32 val1;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 2)]
        public Int32 val2;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 3)]
        public Int32 val3;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 4)]
        public Int32 val4;//intptr
        public Int32 toolTipArea;
        public Int32 toolTipText;//stridx
        public Int32 color;//intptr
    }
}