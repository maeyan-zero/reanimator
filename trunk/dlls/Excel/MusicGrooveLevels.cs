using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MusicGrooveLevels : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MusicGrooveLevelsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 grooveLevelType;//idx
            public Int32 musicRef;//idx
            public Int32 trackNumber;
            public Int32 volume;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            Int32[] undefined1;
            public Int32 minPlayTimeInMeasures;
            public Int32 maxPlayTimeInMeasures;
            public Int32 grooveLevelAfterMaxPlayTime;//idx
            public Int32 mixState;//idx
            public Int32 isAction;//bool
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] playStingerSets;
            public Int32 noCollide;//idx
            public Int32 forParticles;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
            Int32[] undefined;
        }

        public MusicGrooveLevels(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MusicGrooveLevelsTable>(data, ref offset, Count);
        }
    }
}
