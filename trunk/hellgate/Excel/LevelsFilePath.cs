using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LevelsFilePath
    {
        RowHeader header;
        /*code is supposed to be the index, however, it appears that it's in
         reverse, ie the four digits are reverse what is displayed. entry #9 is first, but 
         it's SF01, yet if they're reversed, 10FS. #34 is 2nd CR01, or 10RC.
         On another note, for some reason only the first 3 digits of code are displayed*/
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        [ExcelOutput(SortColumnOrder = 1)]
        public Int32 code;
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