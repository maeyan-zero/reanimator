using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Interact
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 faceDuringInteraction;
        [ExcelOutput(IsBool = true)]
        public Int32 allowGhost;
        [ExcelOutput(IsBool = true)]
        public Int32 priority;
        [ExcelOutput(IsBool = true)]
        public Int32 setTalkingTo;
        [ExcelOutput(IsBool = true)]
        public Int32 playGreeting;
        public Int32 interactMenu;//index
    }
}
