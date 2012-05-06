using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LevelsFilePathBeta
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
        public Language language; // XLS_InternalIndex_Language (XLS_LEVEL_FILE_PATH+A6), 0x0F
        public PakFile pakFlie; // XLS_InternalIndex_Pakfile (XLS_LEVEL_FILE_PATH+DA), 0x09
		public Int32 overrideFolderCodeMask1;
		public Int32 overrideFolderCodeMask2;
		public Int32 overrideFolderCodeMask3;
		public Int32 overrideFolderCodeMask4;
		public Int32 overrideFolderCodeMask5;

        public enum Language
        {
            Null = -1,
            English = 0,
            Korean = 1,
            ChineseSimplified = 2,
            ChineseTraditional = 3,
            Japanese = 4,
            French = 5,
            Spanish = 6,
            German = 7,
            Italian = 8,
            Polish = 9,
            Czech = 10,
            Hungarian = 11,
            Russian = 12,
            Thai = 13,
            Vietnamese = 14
        }

        public enum PakFile
        {
            Null = -1,
            Default = 0,
            GraphicsHigh = 1,
            Sound = 2,
            SoundHigh = 3,
            SoundLow = 4,
            SoundMusic = 5,
            Localized = 6,
            unknown7 = 7,
            unknown8 = 8,
            unknown9 = 9,
            unknown10 = 10,
            unknown11 = 11,
            Advert = 12,
            AdvertLocalized = 13
        }
    }
}