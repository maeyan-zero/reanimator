using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class QuestTemplate : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class QuestTemplateTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 updatePartyKillOnSetup; //boolean
            public Int32 removeOnJoinGame; //boolean
            public Int32 undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string initFunction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] undefined2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string freeFunction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] undefined3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string onEnterGameFunction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] undefined4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string completeFunction;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] undefined5;
        }

        public QuestTemplate(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<QuestTemplateTable>(data, ref offset, Count);
        }
    }
}
