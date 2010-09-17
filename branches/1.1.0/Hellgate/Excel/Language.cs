using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LanguageRow
    {
        ExcelFile.TableHeader header;
        [ExcelFile.ExcelOutput(IsStringOffset = true, SortId = 1)]
        public Int32 language;
        Int32 null1;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 languageAtlas;
        Int32 null2;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
        public Int32 stringsPath;
        Int32 null3;
        [ExcelFile.ExcelOutput(IsStringOffset = true)]
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