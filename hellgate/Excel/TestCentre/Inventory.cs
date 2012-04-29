using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.TestCentre
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InventoryTCv4
    {
        ExcelFile.RowHeader header;

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
        public Inventory.Bitmask01 bitmask;
        public Int32 undefined_TCV4;
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

        public abstract class Inventory
        {
            [FlagsAttribute]
            public enum Bitmask01 : uint
            {
                dynamic = (1 << 0),
                onesize = (1 << 1),
                grid = (1 << 2),
                wardrobe = (1 << 3),
                _undefined1 = (1 << 4),
                autopickup = (1 << 5),
                petSlot = (1 << 6),
                dontSave = (1 << 7),
                resurrectable = (1 << 8),
                linkDeaths = (1 << 9),
                _undefined2 = (1 << 10),// 10
                equipLocation = (1 << 11),
                useInRandomArmor = (1 << 12),
                offhandWardrobe = (1 << 13),
                store = (1 << 14),
                merchantWarehouse = (1 << 15),
                skillsCheckOnUltimateOwner = (1 << 16),
                skillsCheckOnControlUnit = (1 << 17),
                ingredientLoc = (1 << 18),
                destroyPetOnLevelChange = (1 << 19),
                knownOnlyWhenStashOpen = (1 << 20),
                stashLocation = (1 << 21),
                removeFromInventoryOnOwnerDeath = (1 << 22),
                cannotAcceptNoDropItems = (1 << 23),
                cannotAcceptNoTradeItems = (1 << 24),
                cannotDismantleItems = (1 << 25),
                weaponconfigLocation = (1 << 26),
                freeOnSizeChange = (1 << 27),
                enableCacheLocation = (1 << 28),
                disableCacheLocation = (1 << 29),
                isEmailLocation = (1 << 30),
                isEmailInbox = ((uint)1 << 31)
            }
        }
    }
}