using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class QuestState : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class QuestStateTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 quest;//idx
            public Int32 activateWithQuest;//bool
            public Int32 restorePoint;//bool
            public Int32 showLogOnActivate;//bool
            public Int32 showLogOnComplete;//bool
            public Int32 autoTrackOnComplete;//bool
            public Int32 dontShowQuickMessageOnUpdate;//bool
            public Int32 logUnitType0;//idx
            public Int32 logString0;//stridx;
            public Int32 logUnitType1;//idx
            public Int32 logString1;//stridx;
            public Int32 logUnitType2;//idx
            public Int32 logString2;//stridx;
            public Int32 logUnitType3;//idx
            public Int32 logString3;//stridx;
            public Int32 logUnitType4;//idx
            public Int32 logString4;//stridx;
            public Int32 logUnitType5;//idx
            public Int32 logString5;//stridx;
            public Int32 logUnitType6;//idx
            public Int32 logString6;//stridx;
            public Int32 logUnitType7;//idx
            public Int32 logString7;//stridx;
            public Int32 dialogForState;//idx
            public Int32 monsterClassForStateDialog;//idx
            public Int32 interruptMonsterClassForStateDialog;//idx
            public Int32 murmurString;//idx
            public Int32 dialogFullDescription;//idx
            public Int32 template;//idx
            public Int32 gossipNpc1;//idx
            public Int32 gossipNpc2;//idx
            public Int32 gossipNpc3;//idx
            public Int32 gossipString1;//stridx
            public Int32 gossipString2;//stridx
            public Int32 gossipString3;//stridx
            
        }

        public QuestState(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<QuestStateTable>(data, ref offset, Count);
        }
    }
}
