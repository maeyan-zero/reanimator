using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class StateEventTypes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StateEventTypesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string paramText;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string setEventHandler;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            byte undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string clearEventHandler;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte undefined2;
            public Int16 usesTable0;
            public Int16 usesTable1;
            public Int16 comboFilterFunction0;
            public Int16 comboFilterFunction1;
            public Int16 clientOnly;
            public Int16 serverOnly;
            public Int16 bitMask1;/* bit uses force new, 0
	bit uses first person, 1
	bit uses add to center, 2
	bit uses control unit only, 3
	bit uses float, 4
	bit uses owned by control, 5
	bit uses set immediately, 6
	bit uses clear immediately, 7
	bit uses not control unit, 8
	bit uses on weapons, 9
	bit uses ignore camera, 10
	bit uses on clear, 11
	bit unknown, 12
	bit reapply on appearance load, 14
	bit uses share duration, 13
	bit uses check condition on weapons, 15  */
            public Int16 usesBone;
            public Int16 usesBoneWeight;
            public Int16 usesAttachments;
            public Int16 addsRopesOnAddTarget;
            public Int16 usesMaterialOverrideType;
            public Int16 refreshStatsPointer;
            public Int16 usesEnvironmentOverride;
            public Int16 usesScreenEffects;
            public Int16 canClearOnFree;
            public Int16 controlUnitOnly;
            public Int16 requiresLoadedGraphics;
            public Int16 applyToAppearanceOverride;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte unknown2;


        }

        public StateEventTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StateEventTypesTable>(data, ref offset, Count);
        }
    }
}
