using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Sku
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 Default;
        public Int32 server;
        public Int32 developer;
        public Int32 censorLocked;
        public Int32 censorParticles;
        public Int32 censorBoneShrinking;
        public Int32 censorNoHumans;//Censor Monster Class Replacements No Humans
        public Int32 censorNoGore;//Censor Monster Class Replacements No Gore
        public Int32 censorPvPUnderAge;//Censor PvP For Under Age
        public Int32 censorPvP;
        public Int32 censorMovies;
        public Int32 lowQualityMoviesOnly;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string adPublicKey;
        Int32 undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string movielistIntro;
        Int32 undefined2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 titlescreenMovie;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 titlescreenMovieWide;//idx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public Int32[] languages;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        Int32[] languagesUnused;
        Int32 undefined3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "REGION")]
        public Int32 regions1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "REGION")]
        public Int32 regions2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "REGION")]
        public Int32 regions3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "REGION")]
        public Int32 regions4;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        Int32[] regions;
    }
}
