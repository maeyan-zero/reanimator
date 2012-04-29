using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitEvents
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknown2;
        public Record recordPlayer; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Record recordMonster; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Record recordMissile; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Record recordItem; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08
        public Record recordObject; // XLS_ReadIndex_UnitEventType (Excel_UNIT_EVENT_TYPE_DATA+65), 0x08

        public enum Record
        {
            Null = -1,
            New = 0,
            Count = 1,
            Damage = 2,
            SkillCount = 3,
            StatCount = 4,
            AddSummary = 5
        }
    }
}