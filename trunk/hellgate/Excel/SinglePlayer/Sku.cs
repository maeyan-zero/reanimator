using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
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
        public Language language1;
        public Language language2;
        public Language language3;
        public Language language4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Language[] languages;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        Int32[] languagesUnused;
        Int32 undefined3;
        public Regions regions1;
        public Regions regions2;
        public Regions regions3;
        public Regions regions4;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        Int32[] regions;

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
        public enum Regions
        {
            Null = -1,
            NorthAmerica = 0,
            Europe = 1,
            Japan = 2,
            Korea = 3,
            China = 4,
            Taiwan = 5,
            SouthEastAsia = 6,
            SouthAmerica = 7,
            Australia = 8
        }
    }
}
