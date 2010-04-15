using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Inventory : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class InventoryTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string description;
            public Int32 inventoryType;//idx
            public Int32 location;//idx
            public Int32 colorSetPriority;
            public Int32 slotStat;//idx
            public Int32 maxSlotStat;//idx
            public Int32 unknown;
            public Int32 bitmask;/*dynamic-0, onesize-1, grid, wardrobe, autopickup-5, pet slot-6, don''t save-7, resurrectable-8, 
link deaths-9, equip location-11, use in random armor-12, offhand wardrobe-13, store-14, merchant warehouse-15, skills check on ultimate owner-16,
skills check on control unit-17, destroy pet on level change-19, known only when stash open-20, stash location-21, remove from inventory on owner death-22,
cannot accept no drop items-23, cannot accept no trade items-24, cannot dismantle items-25, weaponconfig location-26,
free on size change-27, enable cache location-28, disable cache location-29 */
            public Int32 types0;//these reference unitTypes
            public Int32 types1;
            public Int32 types2;
            public Int32 types3;
            public Int32 types4;
            public Int32 types5;
            public Int32 onlyItemsUsableByType0;//these reference unitTypes
            public Int32 onlyItemsUsableByType1;
            public Int32 onlyItemsUsableByType2;
            public Int32 onlyItemsUsableByType3;
            public Int32 onlyItemsUsableByType4;
            public Int32 onlyItemsUsableByType5;
            public Int32 width;
            public Int32 height;
            public Int32 filter;
            public Int32 checkClass;
            public Int32 preventloc1;//idx
            public Int32 preventtype1a;
            public Int32 preventtype1b;
            public Int32 preventtype1c;
            public Int32 preventtype1d;
            public Int32 preventtype1e;
            public Int32 preventtype1f;
            public Int32 allowskill1;
            public Int32 allowskill2;
            public Int32 allowskill3;
            public Int32 skillallowtype1a;
            public Int32 skillallowtype1b;
            public Int32 skillallowtype1c;
            public Int32 skillallowtype1d;
            public Int32 skillallowtype1e;
            public Int32 skillallowtype1f;
            public Int32 skillallowtype2a;
            public Int32 skillallowtype2b;
            public Int32 skillallowtype2c;
            public Int32 skillallowtype2d;
            public Int32 skillallowtype2e;
            public Int32 skillallowtype2f;
            public Int32 skillallowtype3a;
            public Int32 skillallowtype3b;
            public Int32 skillallowtype3c;
            public Int32 skillallowtype3d;
            public Int32 skillallowtype3e;
            public Int32 skillallowtype3f;
            public Int32 allowskilllevel1;//idx
            public Int32 allowskilllevel2;//idx
            public Int32 allowskilllevel3;//idx
            public Int32 weaponIndex;//idx
            public Int32 physicallyInContainer;//bool
            public Int32 tradeLocation;//bool
            public Int32 returnStuckItemsToStandardLoc;//bool
            public Int32 standardLocation;//bool
            public Int32 onPersonLocation;//bool
            public Int32 allowedHotkeySourceLocation;//bool
            public Int32 rewardLocation;//bool
            public Int32 serverOnlyLocation;//bool
            public Int32 cursorLocation;//bool
            public Int32 playerPutRestricted;//intptr
            public Int32 playerTakeRestricted;//intptr
            public Int32 invLocFallbackOnLoadError;//idx


        }

        public Inventory(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<InventoryTable>(data, ref offset, Count);
        }
    }
}
