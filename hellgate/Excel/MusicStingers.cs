using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicStingers
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//first entry in index is empty.
        public Int32 type;
        public Int32 fadeOutBeats;
        public Int32 fadeInBeats;
        public Int32 fadeInDelayBeats;
        public Int32 fadeOutDelayBeats;
        public Int32 introBeats;
        public Int32 outroBeats;
        public Int32 soundGroup;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined;
    }
}
