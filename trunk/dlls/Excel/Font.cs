using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Font : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class FontTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string systemName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string localPath;

            public Int32 bold;//bool
            public Int32 italic;//bool
            public Int32 fontSize;
            public Int32 sizeInTexture;
            public Int32 undefined1;
            public Int32 letersAcross;
            public Int32 lettersDown;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 131)]
            Int32[] undefined2;
        }

        public Font(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<FontTable>(data, ref offset, Count);
        }
    }
}
