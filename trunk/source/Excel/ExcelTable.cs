using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public abstract class ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct ExcelHeader
        {
            public Int32 flag;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public Int32[] unknown; // is there a CRC in here?
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct StringHeader
        {
            public Int32 flag;
            public Int32 stringBlockSize;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        protected struct CountHeader
        {
            public Int32 flag;
            public Int32 count;
        }

        protected byte[] excelData;
        protected int offset;
        protected ExcelHeader excelHeader;
        protected StringHeader stringHeader;
        protected CountHeader countHeader;

        public ExcelTable(byte[] data)
        {
            excelData = data;
            offset = 0;

            excelHeader = (ExcelHeader)FileTools.ByteArrayToStructure(data, typeof(ExcelHeader), offset);
            offset += Marshal.SizeOf(typeof(ExcelHeader));

            ParseStringHeader();

            countHeader = (CountHeader)FileTools.ByteArrayToStructure(data, typeof(CountHeader), offset);
            offset += Marshal.SizeOf(typeof(CountHeader));
        }

        public int Count
        {
            get
            {
                return countHeader.count;
            }
        }

        private void ParseStringHeader()
        {
            stringHeader = (StringHeader)FileTools.ByteArrayToStructure(excelData, typeof(StringHeader), offset);
            offset += Marshal.SizeOf(typeof(StringHeader));

            if (stringHeader.stringBlockSize != 0)
            {
                byte[] strings = new byte[stringHeader.stringBlockSize];
                Buffer.BlockCopy(excelData, offset, strings, 0, strings.Length);
                offset += stringHeader.stringBlockSize;
            }
        }
    }
}
