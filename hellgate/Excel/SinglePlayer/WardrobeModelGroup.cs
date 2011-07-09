using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeModelGroup
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public AppearanceGroupCategory appearanceGroupCategory; // XLS_InternalIndex_AppearanceGroupCategory (XLS_WARDROBE_MODEL_GROUP+58), 0x0D

        public enum AppearanceGroupCategory
        {
            None = -2,
            Ignore = -1,
            Zombie = 0,
            Minion = 1,
            ThreeP_Gender = 2,
            ThreeP_Faction = 3,
            OneP_Gender = 4,
            OneP_Faction = 5,
            Body = 6,
            Head = 7,
            Hair = 8,
            FaceExtras = 9
        }
    }
}