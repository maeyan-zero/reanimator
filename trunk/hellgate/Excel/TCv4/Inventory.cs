using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InventoryTCv4
    {
        ExcelFile.RowHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string description;
        public Int32 inventoryType;//idx
        public Int32 location;//idx
        public Int32 colorSetPriority;
        public Int32 slotStat;//idx
        public Int32 maxSlotStat;//idx
        public Int32 unknown;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Inventory.Bitmask01 bitmask;
        public Int32 undefined_TCV4;
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
        [ExcelOutput(IsScript = true)]
        public Int32 playerPutRestricted;
        [ExcelOutput(IsScript = true)]
        public Int32 playerTakeRestricted;
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