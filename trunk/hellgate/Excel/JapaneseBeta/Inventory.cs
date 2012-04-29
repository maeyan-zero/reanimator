using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InventoryBeta
    {
        ExcelFile.RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string description;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1, SecondarySortColumn = "inventoryType", IsTableIndex = true, TableStringId = "INVENTORY_TYPES")]
        public Int32 inventoryType;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 location;//idx
        public Int32 colorSetPriority;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 slotStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 maxSlotStat;//idx
        public Int32 unknown;
        [ExcelFile.OutputAttribute(IsBitmask = true, DefaultBitmask = 0)]
        public Bitmask01 bitmask;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types0;//these reference unitTypes
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 types5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType0;//these reference unitTypes
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyItemsUsableByType5;
        public Int32 width;
        public Int32 height;
        public Int32 filter;
        public Int32 checkClass;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 preventloc1;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preventtype1f;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 allowskill1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 allowskill2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 allowskill3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype1f;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype2f;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillallowtype3f;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType1a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType1b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType1c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType1d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType1e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType1f;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType2a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType2b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType2c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType2d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType2e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType2f;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType3a;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType3b;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType3c;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType3d;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType3e;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 skillPrevetType3f;
        public Int32 allowskilllevel1;
        public Int32 allowskilllevel2;
        public Int32 allowskilllevel3;
        public Int32 weaponIndex;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 physicallyInContainer;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 tradeLocation;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 returnStuckItemsToStandardLoc;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 standardLocation;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 onPersonLocation;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 allowedHotkeySourceLocation;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 rewardLocation;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 serverOnlyLocation;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 cursorLocation;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 playerPutRestricted;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 playerTakeRestricted;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "INVLOCIDX")]
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
			cannotAcceptNoAuctionItems =33554432,
            cannotDismantleItems = 67108864,
            weaponconfigLocation = 134217728,
            freeOnSizeChange = 268435456,
            enableCacheLocation = 536870912,
            disableCacheLocation = 1073741824
        }
    }
}
