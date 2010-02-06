using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Sku : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SkuTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

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
            public Int32[] languages;
            public Int32 undefined3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Int32[] regions;
        }

        public Sku(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SkuTable>(data, ref offset, Count);
        }
    }
}
