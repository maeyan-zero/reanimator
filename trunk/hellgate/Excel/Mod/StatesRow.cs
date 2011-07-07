using System;
using System.Runtime.InteropServices;
using Hellgate.Xml;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class StatesRow
    {
        RowHeader header;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 name;
        public String Name; // custom row - was Int32 buffer
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public StateDefinition StateEvents; // custom row - was Int32 buffer1;              // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 file;
        public String File; // custom row - was Int32 buffer2;              // always 0
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 isA9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 statePreventedBy;
        public Int32 duration;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 onDeath;
        public Int32 skillScriptParam;
        Int32 unknown18;            // always 0
        public Int32 element;
        [ExcelOutput(IsScript = true)]
        public Int32 pulseRateInMs;
        [ExcelOutput(IsScript = true)]
        public Int32 pulseRateInMsClient;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 pulseSkill;
        Int32 unknown23;            // always 0
        Int32 unknown24;            // always 0
        Int32 unknown25;            // always 0
        public Int32 iconOrder;            // always 0
        Int32 unknown27;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 uiIcon;
        Int32 unknown29;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 uiIconTexture;
        Int32 unknown31;            // always 0
        public Int32 uiconColor;     //not defined, even though it's used.
        Int32 unknown33;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 UiIconBack;     //undefined as well.
        Int32 unknown35;            // always 0
		Int32 unknown35a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 unIconFront;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 iconBackColor;
        Int32 unknown37;            // always 0
        Int32 unknown38;            // always 0
        [ExcelOutput(IsStringIndex = true)]
        public Int32 iconTooltipStringHellgate;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 iconTooltipStringMythos;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 iconTooltipStringAll;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 assocState1;            // always -1
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 assocState2;            // always -1
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 assocState3;            // always -1
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;
        public Int32 gameFlag;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask02 bitmask02;
        Int32 unknown48;            // always 0
        Int32 unknown49;            // always 0
    }
}