using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Language
    {
        TableHeader header;
        [ExcelOutput(IsStringOffset = true, SortAscendingID = 1)]
        public Int32 language;
        Int32 null1;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 languageAtlas;
        Int32 null2;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 stringsPath;
        Int32 null3;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 languageString;
        Int32 null4;
        public Int32 isDefault;
        public Int32 unknown1;
        public Int32 unknown2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string languageShort;
        public Int32 reserved;
    }
}
