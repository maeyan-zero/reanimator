using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Inventory
    {
        RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string description;
        [ExcelOutput(SortColumnOrder = 1, SecondarySortColumn = "inventoryType", IsTableIndex = true, TableStringId = "INVENTORY_TYPES")]
        public Int32 inventoryType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 location;//idx
        public Int32 colorSetPriority;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 slotStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 maxSlotStat;//idx
        public Int32 unknown;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Bitmask01 bitmask;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types0;//these reference unitTypes
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType0;//these reference unitTypes
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType5;
        public Int32 width;
        public Int32 height;
        public Int32 filter;
        public Int32 checkClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 preventloc1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1a;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1b;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1c;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1d;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1e;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1f;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 allowskill1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 allowskill2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 allowskill3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1a;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1b;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1c;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1d;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1e;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1f;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2a;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2b;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2c;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2d;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2e;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2f;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3a;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3b;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3c;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3d;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3e;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3f;
        public Int32 allowskilllevel1;
        public Int32 allowskilllevel2;
        public Int32 allowskilllevel3;
        public Int32 weaponIndex;
        [ExcelOutput(IsBool = true)]
        public Int32 physicallyInContainer;
        [ExcelOutput(IsBool = true)]
        public Int32 tradeLocation;
        [ExcelOutput(IsBool = true)]
        public Int32 returnStuckItemsToStandardLoc;
        [ExcelOutput(IsBool = true)]
        public Int32 standardLocation;
        [ExcelOutput(IsBool = true)]
        public Int32 onPersonLocation;
        [ExcelOutput(IsBool = true)]
        public Int32 allowedHotkeySourceLocation;
        [ExcelOutput(IsBool = true)]
        public Int32 rewardLocation;
        [ExcelOutput(IsBool = true)]
        public Int32 serverOnlyLocation;
        [ExcelOutput(IsBool = true)]
        public Int32 cursorLocation;
        [ExcelOutput(IsScript = true)]
        public Int32 playerPutRestricted;
        [ExcelOutput(IsScript = true)]
        public Int32 playerTakeRestricted;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 invLocFallbackOnLoadError;//idx

        [FlagsAttribute]
        public enum Bitmask01 : uint
        {
            dynamic = 1,
            onesize = 2,
            grid = 4,
            wardrobe = 8,
            _undefined1 = 16, //5
            autopickup = 32,
            petSlot = 64,
            dontSav = 128,
            resurrectable = 256,
            linkDeaths = 512,
            _undefined2 = 1024,// 10
            equipLocation = 2048,
            useInRandomArmor = 4096,
            offhandWardrobe = 8192,
            store = 16384,
            merchantWarehouse = 32768,
            skillsCheckOnUltimateOwner = 65536,
            skillsCheckOnControlUnit = 131072,
            _undefined3 = 262144,// 19
            destroyPetOnLevelChange = 524288,
            knownOnlyWhenStashOpen = 1048576,
            stashLocation = 2097152,
            removeFromInventoryOnOwnerDeath = 4194304,
            cannotAcceptNoDropItems = 8388608,
            cannotAcceptNoTradeItems = 16777216,
            cannotDismantleItems = 33554432,
            weaponconfigLocation = 67108864,
            freeOnSizeChange = 134217728,
            enableCacheLocation = 268435456,
            disableCacheLocation = 536870912
        }
    }
}
