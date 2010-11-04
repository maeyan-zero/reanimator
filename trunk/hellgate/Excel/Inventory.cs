using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Inventory
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
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Bitmask01 bitmask;
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
        [ExcelOutput(IsIntOffset = true)]
        public Int32 playerPutRestricted;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 playerTakeRestricted;
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
