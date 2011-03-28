using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicGrooveLevels
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//seems to be by name, but with an empty entry first.
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELTYPES")]
        public Int32 grooveLevelType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSIC_REF")]
        public Int32 musicRef;//idx
        public Int32 trackNumber;
        public Int32 volume;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined1;
        public Int32 minPlayTimeInMeasures;
        public Int32 maxPlayTimeInMeasures;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 grooveLevelAfterMaxPlayTime;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUND_MIXSTATES")]
        public Int32 mixState;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 isAction;//bool
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERSETS")]
        public Int32 playStingerSets1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERSETS")]
        public Int32 playStingerSets2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERSETS")]
        public Int32 playStingerSets3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERSETS")]
        public Int32 playStingerSets4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERSETS")]
        public Int32 transitionStingerSetFromAction;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICSTINGERSETS")]
        public Int32 transitionStingerSetFromAmbient;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        Int32[] undefined;
    }
}