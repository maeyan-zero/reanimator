using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitEvents
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknown2;
        public Int32 recordPlayer; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Int32 recordMonster; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Int32 recordMissile; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Int32 recordItem; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Int32 recordObject; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
    }
}