using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestCastRow
    {
        ExcelFile.TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        public Int32 unitType0;//idx
        public Int32 monster0;//idx
        public Int32 unitType1;//idx
        public Int32 monster1;//idx
        public Int32 unitType2;//idx
        public Int32 monster2;//idx
        public Int32 unitType3;//idx
        public Int32 monster3;//idx
        public Int32 unitType4;//idx
        public Int32 monster4;//idx
        public Int32 unitType5;//idx
        public Int32 monster5;//idx
        public Int32 unitType6;//idx
        public Int32 monster6;//idx
        public Int32 unitType7;//idx
        public Int32 monster7;//idx
    }
}