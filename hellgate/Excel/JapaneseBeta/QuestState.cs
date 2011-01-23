using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestStateBeta
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 quest;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 activateWithQuest;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 restorePoint;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 showLogOnActivate;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 showLogOnComplete;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 autoTrackOnComplete;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 dontShowQuickMessageOnUpdate;//bool
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType0;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString0;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType1;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString1;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType2;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString2;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType3;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString3;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType4;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString4;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType5;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString5;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType6;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString6;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 logUnitType7;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logString7;//stridx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 dialogForState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monsterClassForStateDialog;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 interruptMonsterClassForStateDialog;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 murmurString;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 dialogFullDescription;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_TEMPLATE")]
        public Int32 template;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 gossipNpc1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 gossipNpc2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 gossipNpc3;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 gossipString1;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 gossipString2;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 gossipString3;//stridx
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table 9F
        public Int32 relatedTemplateStateForGossip;
    }
}
