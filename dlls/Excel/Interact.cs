using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Interact : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class InteractTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public Int32 faceDuringInteraction;//bool
            public Int32 allowGhost;//bool
            public Int32 priority;//bool
            public Int32 setTalkingTo;//bool
            public Int32 playGreeting;//bool
            public Int32 interactMenu;//index


        }
    

        public Interact(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<InteractTable>(data, ref offset, Count);
        }
    }
}
