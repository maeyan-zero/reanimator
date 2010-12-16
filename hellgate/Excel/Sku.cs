using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Sku
    {
        TableHeader header;
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
        public Int32 undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string movielistIntro;
        public Int32 undefined2;
        public Int32 titlescreenMovie;//idx;
        public Int32 titlescreenMovieWide;//idx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Int32[] languages;
        public Int32 undefined3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        Int32[] regions;
    }
}
