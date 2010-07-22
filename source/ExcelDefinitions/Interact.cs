using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InteractRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 faceDuringInteraction;//bool
        public Int32 allowGhost;//bool
        public Int32 priority;//bool
        public Int32 setTalkingTo;//bool
        public Int32 playGreeting;//bool
        public Int32 interactMenu;//index
    }
}