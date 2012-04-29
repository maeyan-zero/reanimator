using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ActionBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelFile.Output(IsTableIndex = true, TableStringId = "SKILLS")] // 0x29
        public Int32 startSkill;
        [ExcelFile.Output(IsTableIndex = true, TableStringId = "STATES")] // 0x4B
        public Int32 stateSuccess;
        [ExcelFile.Output(IsTableIndex = true, TableStringId = "STATES")] // 0x4B
        public Int32 stateFail;
        [ExcelFile.Output(IsTableIndex = true, TableStringId = "STATES")] // 0x4B
        public Int32 stateDestroy;
        [ExcelFile.Output(IsStringIndex = true)]
        public Int32 stringSuccess;
        [ExcelFile.Output(IsStringIndex = true)]
        public Int32 stringFail;
        [ExcelFile.Output(IsStringIndex = true)]
        public Int32 stringDestroy;
    }
}
