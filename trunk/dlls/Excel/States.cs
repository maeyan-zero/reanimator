using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Reanimator.Excel
{
    public class States : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StatesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 name;
            Int32 buffer;
            public Int32 code;
            Int32 buffer1;              // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 file;
            Int32 buffer2;              // always 0
            public Int32 isA0;
            public Int32 isA1;
            public Int32 isA2;
            public Int32 isA3;
            public Int32 isA4;
            public Int32 isA5;
            public Int32 isA6;
            public Int32 isA7;
            public Int32 isA8;
            public Int32 isA9;
            public Int32 statePreventedBy;
            public Int32 duration;
            public Int32 onDeath;
            public Int32 skillScriptParam;
            Int32 unknown18;            // always 0
            public Int32 element;
            public Int32 pulseRateInMs;
            public Int32 pulseRateInMsClient;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 pulseSkill;
            Int32 unknown23;            // always 0
            Int32 unknown24;            // always 0
            Int32 unknown25;            // always 0
            public Int32 iconOrder;            // always 0
            Int32 unknown27;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 uiIcon;
            Int32 unknown29;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 uiIconTexture;
            Int32 unknown31;            // always 0
            public Int32 unknown32;     //not defined, even though it's used.
            Int32 unknown33;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown34;     //undefined as well.
            Int32 unknown35;            // always 0
            public Int32 iconBackColor;
            Int32 unknown37;            // always 0
            Int32 unknown38;            // always 0
            public Int32 iconTooltipStringHellgate;//stridx
            public Int32 iconTooltipStringMythos;
            public Int32 iconTooltipStringAll;
            public Int32 assocState1;            // always -1
            public Int32 assocState2;            // always -1
            public Int32 assocState3;            // always -1
            public Int32 bitMask1;/*bit execute attack script melee, 0
	bit execute attack script ranged, 1
	bit execute skill script on remove, 2
	bit execute script on source, 3
	bit pulse on client too, 4*/
            public Int32 gameFlag;
            public Int32 bitMask2;/*bit stacks, 1
	bit stacks per source, 2
	bit send to all, 3
	bit send to self, 4
	bit send stats, 5
	bit client needs duration, 6
	bit client only, 7
	bit execute parent events,8
	bit trigger notarget on set,9
	bit save position on set, 10
	bit save with unit, 11
	bit flag for load, 12
	but sharing mod state, 13
	bit used in hellgate, 14
	bit used in tugboat, 15
	bit is bad, 16
	bit pulse on source, 17
	bit on change repaint item ui, 18
	bit save in unitfile header, 19
	bit update chat server on change, 20
	bit trigger digest save, 22*/
            Int32 unknown48;            // always 0
            Int32 unknown49;            // always 0
        }

        public States(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StatesTable>(data, ref offset, Count);
        }
    }
}
