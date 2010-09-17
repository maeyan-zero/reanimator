using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hellgate
{
    public partial class StringFile
    {
        public String FilePath { get { return "excel\\strings\\english\\"; } }
        public String FileExtension { get { return "xls.uni.cooked"; } }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private class StringsHeader
        {
            public Int32 Header;
            public Int32 Version;
            public Int32 Count;
        }

        private abstract class FileTokens
        {
            public const Int32 Header = 0x68667374;
        }
        private const Int32 Version = 6;
        private const int MaxAttributes = 4;

        public class StringBlock
        {
            public Int32 ReferenceId { get; set; }
            public Int32 Unknown { get; set; }
            public String StringId { get; set; }
            public Int32 Reserved;
            public String String { get; set; }
            public Int32 AttributeCount;
            public String Attribute1 { get; set; }
            public String Attribute2 { get; set; }
            public String Attribute3 { get; set; }
            public String Attribute4 { get; set; }

            public StringBlock()
            {
                StringId = String.Empty;
                String = String.Empty;
                Attribute1 = String.Empty;
                Attribute2 = String.Empty;
                Attribute3 = String.Empty;
                Attribute4 = String.Empty;
            }
        }
    }
}
