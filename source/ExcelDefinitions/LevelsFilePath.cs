using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsFilePathRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string code;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string path;
        public Int32 localisedFolders0;
        public Int32 localisedFolders1;
        public Int32 localisedFolders2;
        public Int32 localisedFolders3;
        public Int32 localisedFolders4;
        public Int32 localisedFolders5;
        public Int32 localisedFolders6;
        public Int32 localisedFolders7;
        public Int32 language;
        public Int32 pakFlie;
    }
}