using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LevelsFilePath
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        public Int32 code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string path;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 localisedFolders7;
        public Int32 language; // XLS_InternalIndex_Language (XLS_LEVEL_FILE_PATH+A6), 0x0F
        public Int32 pakFlie; // XLS_InternalIndex_Pakfile (XLS_LEVEL_FILE_PATH+DA), 0x09
    }
}