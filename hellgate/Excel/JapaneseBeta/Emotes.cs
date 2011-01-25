using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EmotesBeta
    {
        ExcelFile.RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 commandString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 decriptionString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 textString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 commandStringEnglish;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 descriptionStringEnglish;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 testStringEnglish;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITMODES")]//table 22
        public Int32 unitMode;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ACHIEVEMENTS")]//table B7
        public Int32 requiresAchievement;
    }
}
