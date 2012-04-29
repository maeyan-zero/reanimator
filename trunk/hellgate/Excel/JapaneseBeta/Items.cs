using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{

    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemsBeta
    {
        RowHeader header;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 name;
        Int32 buffer1;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 folder;
        Int32 buffer2;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearance;
        Int32 buffer3;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearanceFirst;
        Int32 buffer4;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 folderInventory;
        Int32 buffer5;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearanceInventory;
        Int32 buffer5a;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 icon;
        Int32 buffer5b;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 holyRadius;
        Int32 buffer6;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyHitParticle;
        Int32 buffer7;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lightHitParticle;
        Int32 buffer8;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumHitParticle;
        Int32 buffer9;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardHitParticle;
        Int32 buffer10;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killedParticle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown00;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fizzleParticle;
        Int32 buffer12;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 reflectParticle;
        Int32 buffer13;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 restoreVitalsParticle;
        Int32 buffer14;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 diffuse;
        Int32 buffer15;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 normal;
        Int32 buffer16;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 specular;
        Int32 buffer17;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lightmap;
        Int32 buffer18;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource1;
        Int32 buffer19;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest1;
        Int32 buffer20;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource2;
        Int32 buffer21;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest2;
        Int32 buffer22;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource3;
        Int32 buffer23;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest3;
        Int32 buffer24;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource4;
        Int32 buffer25;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest4;
        Int32 buffer26;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource5;
        Int32 buffer27;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest5;//pchar                          // 19   0x00000000000000E0  XLS_ReadCharPtr2
        Int32 buffer28;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 particleFolder;//pchar                         // 21   0x00000000000000E8  XLS_ReadCharPtr1
        Int32 buffer29;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 pickupFunction;//pchar                         // 259  0x00000000000000F0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown01;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 triggerString1;//pchar                         // 411  0x0000000000000100  XLS_ReadCharPtr1
        Int32 buffer31;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 triggerString2;//pchar                         // 412  0x0000000000000108  XLS_ReadCharPtr1
        Int32 buffer32;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 charSelectFont;//pchar                         // 47   0x0000000000000110  XLS_ReadCharPtr1
        Int32 buffer33;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tooltipDamageIcon;//pchar                      // 136  0x0000000000000118  XLS_ReadCharPtr1
        Int32 buffer34;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 damagingMeleeParticle;//pchar                  // 30   0x0000000000000120  XLS_ReadCharPtr1
        Int32 buffer35;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 muzzleFlash;//pchar                            // 417  0x0000000000000128  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown02;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfDefault;//pchar                              // 418  0x0000000000000138  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown03;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfPhysical;//pchar                             // 424  0x0000000000000148  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown04;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfFire;//pchar                                 // 422  0x0000000000000158  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown05;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfElectric;//pchar                             // 423  0x0000000000000168  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown06;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfSpectral;//pchar                             // 421  0x0000000000000178  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown07;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfToxic;//pchar                                // 419  0x0000000000000188  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknown08;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailSlashRopeNoTarget;//pchar                 // 433  0x0000000000000200  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown10;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailDefault;//pchar                           // 434  0x0000000000000210  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown11;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailPhysical;//pchar                          // 440  0x0000000000000220  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown12;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailFire;//pchar                              // 438  0x0000000000000230  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown13;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailElectric;//pchar                          // 439  0x0000000000000240  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown14;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailSpectral;//pchar                          // 437  0x0000000000000250  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown15;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailToxic;//pchar                             // 435  0x0000000000000260  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknown16;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projectileSlashRopeWithTarget;//pchar          // 425  0x00000000000002D8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown18;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projDefault;//pchar                            // 426  0x00000000000002E8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown19;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projPhysical;//pchar                           // 432  0x00000000000002F8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown20;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projFire;//pchar                               // 430  0x0000000000000308  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown21;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projElectric;//pchar                           // 431  0x0000000000000318  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown22;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projSpectral;//pchar                           // 429  0x0000000000000328  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown23;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projToxic;//pchar                              // 427  0x0000000000000338  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknown24;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactWall;//pchar                             // 441  0x00000000000003B0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown26;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallDefault;//pchar                         // 442  0x00000000000003C0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown27;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallPhysical;//pchar                        // 447  0x00000000000003D0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown28;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallFire;//pchar                            // 445  0x00000000000003E0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown29;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallElectric;//pchar                        // 446  0x00000000000003F0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown30;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallSpectral;//pchar                        // 444  0x0000000000000400  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown31;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallToxic;//pchar                           // 443  0x0000000000000410  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknown32;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnit;//pchar                             // 448  0x0000000000000488  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown34;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitDefault;//pchar                      // 449  0x0000000000000498  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown35;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitPhysical;//pchar                     // 454  0x00000000000004A8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown36;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitFire;//pchar                         // 452  0x00000000000004B8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown37;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitElectric;//pchar                     // 453  0x00000000000004C8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown38;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitSpectral;//pchar                     // 451  0x00000000000004D8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown39;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitToxic;//pchar                        // 450  0x00000000000004E8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 156)]
        byte[] unknown40;
        public Int32 baseRow;                                 // ???  0x0000000000000560  ??? Appears to be Int32, but not seen in code?
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask01 bitmask01;                           // 56   0x0000000000000564  XLS_ReadBitmask
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask02 bitmask02;                           // 56   0x0000000000000568
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask03 bitmask03;                           // 56   0x000000000000056C
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask04 bitmask04;                           // 56   0x0000000000000570
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask05 bitmask05;                           // 56   0x0000000000000574
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask06 bitmask06;                           // 56   0x0000000000000578
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask07 bitmask07;                           // 56   0x000000000000057C
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask08 bitmask08;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 String;                                        // 375  0x0000000000000580  XLS_ReadStringIndex
        [ExcelOutput(IsStringIndex = true)]
        public Int32 typeDescription;                               // 378  0x0000000000000584  XLS_ReadStringIndex
        [ExcelOutput(IsStringIndex = true)]
        public Int32 flavorText;                                    // 379  0x0000000000000588  XLS_ReadStringIndex
        [ExcelOutput(IsStringIndex = true)]
        public Int32 additionalDescription;                         // 380  0x000000000000058C  XLS_ReadStringIndex
        [ExcelOutput(IsStringIndex = true)]
        public Int32 additionalRaceDescription;                     // 381  0x0000000000000590  XLS_ReadStringIndex
        [ExcelOutput(IsStringIndex = true)]
        public Int32 analyze;                                       // 382  0x0000000000000594  XLS_ReadStringIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPELISTS")]
        public Int32 recipeList;//index                                    // 62   0x0000000000000598  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipeSingleUse;//index                        // 63   0x000000000000059C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 paperdollBackgroundLevel;//index               // 43   0x00000000000005A0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Int32[] paperdollWeapon;                             // 44   0x00000000000005A4  XLS_ReadIndexArray,2
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 paperdollSkill;//index                         // 45   0x00000000000005AC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "COLORSETS")]
        public Int32 paperdollColorset;//index                      // 46   0x00000000000005B0  XLS_ReadIndex
        public Int32 respawnChance;                                 // 35   0x00000000000005B4  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 respawnSpawnClass;//index                      // 34   0x00000000000005B8  XLS_ReadIndex,90
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;                                          // 32   0x00000000000005BC  XLS_ReadCode
        Int32 unknown41;
        public Int32 densityValueOverride;                          // 54   0x00000000000005C4  XLS_ReadInt32
        [ExcelOutput(IsScript = true)]
        public Int32 minionPackSize;                                // 55   0x00000000000005C8  XLS_ReadScript
        public float spinSpeed;                                     // 303  0x00000000000005CC  XLS_ReadFloat
        public float maxTurnRate;                                   // 304  0x00000000000005D0  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType;//index                               // 33   0x00000000000005D4  XLS_ReadIndex,17
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 subUnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeForLeveling;//index                    // 36   0x00000000000005D8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 preferedByUnitType;//index                     // 37   0x00000000000005DC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 family;//index                                 // 38   0x00000000000005E0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 censorClassNoHumans;//index                    // 39   0x00000000000005E4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 censorClassNoGore;//index                      // 40   0x00000000000005E8  XLS_ReadIndex
        public Sex sex; // XLS_InternalIndex_Sex (XLS_UNIT_DATA+623), 0x04    // 41   0x00000000000005EC  XLS_ReadInternalIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYER_RACE")]
        public Int32 race;//index                                   // 42   0x00000000000005F0  XLS_ReadIndex
        public Int32 rarity;                                        // 107  0x00000000000005F4  XLS_ReadInt32
        public Int32 spawnChance;                                   // 109  0x00000000000005F8  XLS_ReadInt32
        public Int32 minMonsterExperienceLevel;                     // 88   0x00000000000005FC  XLS_ReadInt32
        public Int32 level;                                         // 89   0x0000000000000600  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTER_QUALITY")]
        public Int32 monsterQuality;//index                         // 48   0x0000000000000604  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monsterClassAtUniqueQuality;//index            // 49   0x0000000000000608  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monsterClassAtChampionQuality;//index          // 50   0x000000000000060C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 minionClass;//index                            // 52   0x0000000000000610  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTER_NAME_TYPES")]
        public Int32 monsterNameType;//index                        // 53   0x0000000000000614  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 itemQuality;//index                            // 51   0x0000000000000618  XLS_ReadIndex
        public Int32 angerRange;                                    // 416  0x000000000000061C  XLS_ReadInt32
        public Int32 baseLevel;                                     // 90   0x0000000000000620  XLS_ReadInt32
        public Int32 capLevel;                                      // 91   0x0000000000000624  XLS_ReadInt32
        public Int32 minMerchantLevel;                              // 92   0x0000000000000628  XLS_ReadInt32
        public Int32 maxMerchantLevel;                              // 93   0x000000000000062C  XLS_ReadInt32
        public Int32 minSpawnLevel;                                 // 94   0x0000000000000630  XLS_ReadInt32
        public Int32 maxSpawnLevel;                                 // 95   0x0000000000000634  XLS_ReadInt32       .text:00000001402E7A4C mov     dword ptr [rsp+0A8h+var_88], 186A0h (186A0h = 100,000....)
        public Int32 maxLevel;                                      // 96   0x0000000000000638  XLS_ReadInt32
        public Int32 maxRank;
        [ExcelOutput(IsScript = true)]
        public Int32 fixedLevel;//intptr                            // 97   0x000000000000063C  XLS_ReadScript
        public Int32 hpMin;                                         // 98   0x0000000000000640  XLS_ReadInt32
        public Int32 hpMax;                                         // 99   0x0000000000000644  XLS_ReadInt32
        public Int32 powerMax;                                      // 100  0x0000000000000648  XLS_ReadInt32
        public Int32 experience;                                    // 101  0x000000000000064C  XLS_ReadInt32
        public Int32 attackRating;                                  // 143  0x0000000000000650  XLS_ReadInt32
        [ExcelOutput(IsScript = true)]
        public Int32 luckBonus;//intptr                             // 102  0x0000000000000654  XLS_ReadScript
        public Int32 luckChanceToSpawn;                             // 103  0x0000000000000658  XLS_ReadInt32
        public RoomPopulatePass roomPopulatePass; // XLS_InternalIndex_RoomPopulatePass (XLS_UNIT_DATA+AD1), 0x05  // 58   0x000000000000065C  XLS_ReadInternalIndex
        public WeaponBoneIndex weaponBoneIndex; // XLS_InternalIndex_WeaponBoneIndex (XLS_UNIT_DATA+91E), 0x04    // 57   0x0000000000000660  XLS_ReadInternalIndex
        public Int32 requiresAffixOrSuffix;                         // 108  0x0000000000000664  XLS_ReadInt32
        public float autoPickupDistance;                            // 86   0x0000000000000668  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 pickupPullState;//index                        // 87   0x000000000000066C  XLS_ReadIndex
        public float extraDyingTimeInSeconds;                       // 70   0x0000000000000670  XLS_ReadFloat
		public Int32 undefined_01;
        [ExcelOutput(IsTableIndex = true, TableStringId = "NPC")]
        public Int32 npcInfo;//index                                // 71   0x0000000000000674  XLS_ReadIndex
        public Int32 balanceTestCount;                              // 72   0x0000000000000678  XLS_ReadInt32
        public Int32 balanceTestGroup;                              // 73   0x000000000000067C  XLS_ReadInt32
        public Int32 merchantStartingPane;                          // 59   0x0000000000000680  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION")]
        public Int32 merchantFactionType;//index                    // 60   0x0000000000000684  XLS_ReadIndex
        public Int32 merchantFactionValueNeeded;                    // 61   0x0000000000000688  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questRequirement;//index                       // 64   0x000000000000068C  XLS_ReadIndex
        public float noSpawnRadius;                                 // 65   0x0000000000000690  XLS_ReadFloat
        public float monitorApproachRadius;                         // 68   0x0000000000000694  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 tasksGeneratedStat;//index                     // 69   0x0000000000000698  XLS_ReadIndex,1B       text:00000001402E7392 mov     dword ptr [rsp+0A8h+var_88], 1Bh      0x1B == 27 (27 = STATS table)
        public float serverMissileOffset;                           // 119  0x000000000000069C  XLS_ReadFloat
        public float homingTurnAngleRadians;                        // 74   0x00000000000006A0  XLS_ReadFloat
        public float homingModBasedDis;                             // 75   0x00000000000006A4  XLS_ReadFloat
        public float homeAfterUnitRadius;                           // 76   0x00000000000006A8  XLS_ReadFloat
        public float collidableAfterXSeconds;                       // 77   0x00000000000006AC  XLS_ReadFloat
        public float homeAfterXSeconds;                             // 78   0x00000000000006B0  XLS_ReadFloat
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask09 bitmask09;                                 // 85   0x00000000000006B4  XLS_ReadBitmask
        public float impactCameraShakeDuration;                     // 80   0x00000000000006B8  XLS_ReadFloat
        public float impactCameraShakeMagnitude;                    // 81   0x00000000000006BC  XLS_ReadFloat
        public float impactCameraShakeDegrade;                      // 82   0x00000000000006C0  XLS_ReadFloat
        public Int32 maximumImpactFrequency;                        // 83   0x00000000000006C4  XLS_ReadInt32
        public Int32 maximumHitFrequency;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyCollideWithUnitType;//index                // 79   0x00000000000006C8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 questDescription;//index                       // 260  0x00000000000006CC  XLS_ReadIndex
        [ExcelOutput(IsScript = true)]
        public Int32 pickUpCondition;                               // 258  0x00000000000006D0  XLS_ReadScript
        public Int32 useCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 scriptOnUse;                                   // 248  0x00000000000006D8  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 stackSize;                                     // 227  0x00000000000006DC  XLS_ReadScript
        public Int32 maxPickUp;                                     // 228  0x00000000000006E0  XLS_ReadInt32
        public Int32 baseCost;                                      // 221  0x00000000000006E4  XLS_ReadInt32
        public Int32 realWorldCost;                                 // 222  0x00000000000006E8  XLS_ReadInt32
        [ExcelOutput(IsScript = true)]
        public Int32 buyPriceMult;                                  // 223  0x00000000000006EC  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 buyPriceAdd;                                   // 224  0x00000000000006F0  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 sellPriceMult;                                 // 225  0x00000000000006F4  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 sellPriceAdd;                                  // 226  0x00000000000006F8  XLS_ReadScript
        public Int32 pvpPointCost;
        public Int32 eventPointCost;
        public Int32 bonusPointCost;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 inventoryWardrobe;                             // 110  0x00000000000006FC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 characterScreenWardrobe;                       // 111  0x0000000000000700  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 characterScreenState;                          // 112  0x0000000000000704  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BODY")]
        public Int32 wardrobeBody;                                  // 113  0x0000000000000708  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 wardrobeFallback;                              // 114  0x000000000000070C  XLS_ReadIndex
        public Int32 null0;                                         // 115  0x0000000000000710  XLS_ReadInt32
        public Int32 wardrobeMip;                                   // 118  0x0000000000000714  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 wardrobeAppearanceGroup;                       // 116  0x0000000000000718  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 wardrobeAppearanceGroup1st;                    // 117  0x000000000000071C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_STANCE")]
        public Int32 startingStance;                                // 120  0x0000000000000720  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes06;
        public Int32 summonedInvLoc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32[] containerUnitType;                           // 261  0x0000000000000738  XLS_ReadIndex_UnitTypeArray,4
        public Int32 firingErrorIncrease;                           // 127  0x0000000000000748  XLS_ReadInt32
        public Int32 firingErrorDecrease;                           // 128  0x000000000000074C  XLS_ReadInt32
        public Int32 firingErrorMax;                                // 129  0x0000000000000750  XLS_ReadInt32
        public Int32 accuracyBase;                                  // 130  0x0000000000000754  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes07;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 refillHotKey;//index                           // 262  0x0000000000000768  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "ANIMATION_GROUP")]
        public Int32 animGroup;//index                              // 121  0x000000000000076C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MELEEWEAPONS")]
        public Int32 meleeWeapon;//index                            // 122  0x0000000000000770  XLS_ReadIndex
        public Int32 cdTicks;                                       // 131  0x0000000000000774  XLS_ReadInt32
        public float approxDps;                                     // 132  0x0000000000000778  XLS_ReadFloat
        [ExcelOutput(IsStringIndex = true)]
        public Int32 tooltipDamageString;//stridx                   // 137  0x000000000000077C  XLS_ReadStringIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] requiredAffixGroups;                         // 126  0x0000000000000780  XLS_ReadIndexArray,8
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 spawnMonsterClass;//index                      // 133  0x00000000000007A0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 safeState;//index                              // 134  0x00000000000007A4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillGhost;//index                             // 135  0x00000000000007A8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillRef;//index                               // 233  0x00000000000007AC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "DAMAGETYPES")]
        public Int32 dmgType;//index                                // 138  0x00000000000007B0  XLS_ReadIndex
        public Int32 weaponDamageScale;                             // 142  0x00000000000007B4  XLS_ReadInt32
        public Int32 dontUseWeaponDamage;                           // 141  0x00000000000007B8  XLS_ReadInt32
        [ExcelOutput(IsScript = true)]
        public Int32 minBaseDmg;                                    // 139  0x00000000000007BC  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 maxBaseDmg;                                    // 140  0x00000000000007C0  XLS_ReadScript
        public Int32 sfxAttackPctFocus;                             // 144  0x00000000000007C4  XLS_ReadInt32
        public Int32 sfxUknown1;
        public Int32 sfxUknown2;
        public Int32 sfxUknown3;
        public Int32 sfxUknown4;
        public Int32 sfxUknown5;
        public Int32 sfxPhysicalAbilityPct;                         // 145  0x00000000000007DC  XLS_ReadInt32
        public Int32 sfxPhysicalDefensePct;                         // 146  0x00000000000007E0  XLS_ReadInt32
        public Int32 sfxPhysicalKnockbackInCm;                      // 147  0x00000000000007E4  XLS_ReadInt32
        public float sfxPhysicalStunDurationInSeconds;              // 148  0x00000000000007E8  XLS_ReadFloat
        public Int32 sfxUknown6;
        public Int32 sfxFireAbilityPct;                             // 153  0x00000000000007F0  XLS_ReadInt32
        public Int32 sfxFireDefensePct;                             // 154  0x00000000000007F4  XLS_ReadInt32
        public Int32 sfxFireDamagePercent1;
        public Int32 sfxFireDamagePercent2;
        public Int32 sfxFireDamagePercent3;
        public Int32 sfxElectricAbilityPct;                         // 149  0x0000000000000804  XLS_ReadInt32
        public Int32 sfxElectricDefensePct;                         // 150  0x0000000000000808  XLS_ReadInt32
        public Int32 sfxElectricDamagePctOfInitialPerShock;         // 151  0x000000000000080C  XLS_ReadInt32
        public float sfxElectricDurationInSeconds;                  // 152  0x0000000000000810  XLS_ReadFloat
        public Int32 sfxUknown7;
        public Int32 sfxSpectralAbilityPct;                         // 156  0x0000000000000818  XLS_ReadInt32
        public Int32 sfxSpectralDefensePct;                         // 157  0x000000000000081C  XLS_ReadInt32
        public Int32 sfxSpectralMinusOutDamagePctPlusInDamagePct;   // 158  0x0000000000000820  XLS_ReadInt32
        public float sfxSpectralDurationInSeconds;                  // 159  0x0000000000000824  XLS_ReadFloat
        public Int32 sfxUknown8;
        public Int32 sfxToxicAbilityPct;                            // 160  0x000000000000082C  XLS_ReadInt32
        public Int32 sfxToxicDefensePct;                            // 161  0x0000000000000830  XLS_ReadInt32
        public Int32 sfxToxicDamageAsPctOfDamageDeliveredFromAttack;// 162  0x0000000000000834  XLS_ReadInt32
        public float sfxToxicDurationInSeconds;                     // 163  0x0000000000000838  XLS_ReadFloat
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes12;
        public Int32 dmgIncrement;                                  // 168  0x00000000000008B8  XLS_ReadInt32
        public Int32 radialDmgIncrement;                            // 169  0x00000000000008BC  XLS_ReadInt32
        public Int32 fieldDmgIncrement;                             // 170  0x00000000000008C0  XLS_ReadInt32
        public Int32 dotDmgIncrement;                               // 171  0x00000000000008C4  XLS_ReadInt32
        public Int32 aiChangerDmgIncrement;                         // 172  0x00000000000008C8  XLS_ReadInt32
        public Int32 toHit;                                         // 185  0x00000000000008CC  XLS_ReadInt32
        public Int32 criticalPct;                                   // 173  0x00000000000008D0  XLS_ReadInt32
        public Int32 criticalMult;                                  // 174  0x00000000000008D4  XLS_ReadInt32
        public Int32 staminaDrainChancePct;                         // 175  0x00000000000008D8  XLS_ReadInt32
        public Int32 staminaDrain;                                  // 176  0x00000000000008DC  XLS_ReadInt32
        public Int32 interruptAttackPct;                            // 177  0x00000000000008E0  XLS_ReadInt32
        public Int32 interruptDefensePct;                           // 178  0x00000000000008E4  XLS_ReadInt32
        public Int32 interruptChanceOnAnyHit;                       // 179  0x00000000000008E8  XLS_ReadInt32
        public Int32 stealthDefensePct;                             // 181  0x00000000000008EC  XLS_ReadInt32
        public Int32 stealthAttackPct;                              // 180  0x00000000000008F0  XLS_ReadInt32
        public Int32 aiChangeDefense;                               // 184  0x00000000000008F4  XLS_ReadInt32
        public Int32 armor;                                         // 186  0x00000000000008F8  XLS_ReadInt32
        public Int32 armorPhys;                                     // 188  0x00000000000008FC  XLS_ReadInt32
        public Int32 armorFire;                                     // 189  0x0000000000000900  XLS_ReadInt32
        public Int32 armorElec;                                     // 190  0x0000000000000904  XLS_ReadInt32
        public Int32 armorSpec;                                     // 191  0x0000000000000908  XLS_ReadInt32
        public Int32 armorToxic;                                    // 192  0x000000000000090C  XLS_ReadInt32
        public Int32 maxArmor;                                      // 187  0x0000000000000928  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknownBytes16;                                      // 0x000000000000092C to 0x0000000000000958
        public Int32 shields;                                       // 194  0x0000000000000958  XLS_ReadInt32
        public Int32 shieldPhys;                                    // 195  0x000000000000095C  XLS_ReadInt32
        public Int32 shieldFire;                                    // 196  0x0000000000000960  XLS_ReadInt32
        public Int32 shieldElec;                                    // 197  0x0000000000000964  XLS_ReadInt32
        public Int32 shieldSpec;                                    // 198  0x0000000000000968  XLS_ReadInt32
        public Int32 shieldToxic;                                   // 199  0x000000000000096C  XLS_ReadInt32
        public Int32 strengthPercent;                               // 201  0x0000000000000988  XLS_ReadInt32
        public Int32 dexterityPercent;                              // 202  0x000000000000098C  XLS_ReadInt32
        public Int32 startingAccuracy;                              // 203  0x0000000000000990  XLS_ReadInt32
        public Int32 startingStrength;                              // 204  0x0000000000000994  XLS_ReadInt32
        public Int32 startingStamina;                               // 205  0x0000000000000998  XLS_ReadInt32
        public Int32 startingWillpower;                             // 206  0x000000000000099C  XLS_ReadInt32
        [ExcelOutput(IsScript = true)]		
        public Int32 codeDefenseProperties;
        [ExcelOutput(IsScript = true)]
        public Int32 props1;                                        // 207  0x00000000000009A0  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 props2;                                        // 208  0x00000000000009A4  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 props3;                                        // 209  0x00000000000009A8  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 props4;                                        // 210  0x00000000000009AC  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 props5;                                        // 211  0x00000000000009B0  XLS_ReadScript
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 props1AppliesToUnitype;//index                 // 212  0x00000000000009B4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 props2AppliesToUnitype;//index                 // 213  0x00000000000009B8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 props3AppliesToUnitype;//index                 // 214  0x00000000000009BC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 props4AppliesToUnitype;//index                 // 215  0x00000000000009C0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 props5AppliesToUnitype;//index                 // 216  0x00000000000009C4  XLS_ReadIndex
        [ExcelOutput(IsScript = true)]
        public Int32 perLevelProps1;                                // 218  0x00000000000009C8  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 perLevelProps2;                                // 219  0x00000000000009CC  XLS_ReadScript
        [ExcelOutput(IsScript = true)]
        public Int32 propsElite;                                    // 217  0x00000000000009D0  XLS_ReadScript
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Int32[] affix;//index                                // 220  0x00000000000009D4  XLS_ReadIndexArray,6
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_SETITEM_GROUPS")]
        public Int32 setGroup;
        public Int32 setAffix1NumRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        public Int32 setAffix1;//index
        public Int32 setAffix2NumRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        public Int32 setAffix2;//index
        public Int32 setAffix3NumRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        public Int32 setAffix3;//index
        public Int32 setAffix4NumRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        public Int32 setAffix4;//index
        public Int32 setAffix5NumRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        public Int32 setAffix5;//index
        public Int32 setAffix6NumRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES")]
        public Int32 setAffix6;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure;//index                               // 104  0x00000000000009EC  XLS_ReadIndex,65
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 championTreasure;//index                       // 105  0x00000000000009F0  XLS_ReadIndex,65
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 firstTimeTreasure;//index                      // 106  0x00000000000009F4  XLS_ReadIndex,65
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVENTORY_TYPES")]
        public Int32 inventory;//index                              // 229  0x00000000000009F8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 recipeIngredientInvLoc;//index                 // 230  0x00000000000009FC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 recipeResultInvLoc;//index                     // 231  0x0000000000000A00  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 startingTreasure;//index                       // 232  0x0000000000000A04  XLS_ReadIndex,65
        public Int32 invWidth;                                      // 256  0x0000000000000A08  XLS_ReadInt32
        public Int32 invHeight;                                     // 257  0x0000000000000A0C  XLS_ReadInt32
        public Int64 qualities;                                     // 123  0x0000000000000A10  XLS_ReadBits - I think this reads 8 bytes (64 bits); input = 0x40 = 64
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 requiredQuality;//index                        // 124  0x0000000000000A18  XLS_ReadIndex
        public QualityName qualityName; // XLS_InternalIndex_QualityName (XLS_UNIT_DATA+91E), 0x04  // 125  0x0000000000000A1C  XLS_ReadInternalIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 fieldMissile;//index                           // 84   0x0000000000000A20  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillHitUnit;//index                           // 391  0x0000000000000A24  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillHitBackground;//index                     // 392  0x0000000000000A28  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32[] skillMissed;                                 // 393  0x0000000000000A2C  XLS_ReadIndexArray,0F
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes20;                                      // 0x0000000000000A68 to 0x0000000000000A6C
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillOnFuse;//index                            // 394  0x0000000000000A6C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillOnDamageRepeat;//index                    // 395  0x0000000000000A70  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32[] startingskills;                              // 234  0x0000000000000A74  XLS_ReadIndexArray,0C
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 unitDieBeginSkill;//index                      // 235  0x0000000000000AA4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 unitDieBeginSkillClient;//index                // 236  0x0000000000000AA8  XLS_ReadIndex
        [ExcelOutput(IsScript = true)]
        public Int32 scriptOnUnitDieEnd;//intptr                    // 237  0x0000000000000AAC  XLS_ReadScript
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 unitDieEndSkill;//index                        // 238  0x0000000000000AB0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 deadSkill;//index                              // 239  0x0000000000000AB4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 kickSkill;//index                              // 240  0x0000000000000AB8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 rightSkill;//index                             // 241  0x0000000000000ABC  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32[] initSkill;//index                              // 242  0x0000000000000AC0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillLevelActive;//index                       // 243  0x0000000000000AC4  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32[] initState;                                   // 244  0x0000000000000AC8  XLS_ReadIndexArray,4
        public Int32 initStateTicks;                                // 245  0x0000000000000AD8  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 dyingState;//index                             // 246  0x0000000000000ADC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillWhenUsed;//index                          // 247  0x0000000000000AE0  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32[] verifyStateWhenUsed;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab1;//index                              // 249  0x0000000000000AE4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab2;//index                              // 250  0x0000000000000AE8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab3;//index                              // 251  0x0000000000000AEC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab4;//index                              // 252  0x0000000000000AF0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab5;//index                              // 253  0x0000000000000AF4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab6;//index                              // 254  0x0000000000000AF8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 SkillTab7;//index                              // 255  0x0000000000000AFC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 levelUpState;//index                           // 263  0x0000000000000B00  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 rankUpState;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 sounds;//index                                 // 264  0x0000000000000B04  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 flippySound;//index                            // 265  0x0000000000000B08  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 invPickUpSound;//index                         // 266  0x0000000000000B0C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 invPutDownSound;//index                        // 267  0x0000000000000B10  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 invEquipSound;//index                          // 268  0x0000000000000B14  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 pickUpSound;//index                            // 269  0x0000000000000B18  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 useSound;//index                               // 270  0x0000000000000B1C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantUseSoundA;//index                          	// 271  0x0000000000000B20  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantFireSound;//index                          // 272  0x0000000000000B24  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 blockSound;//index                             // 273  0x0000000000000B28  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 getHit0;//index                                // 274  0x0000000000000B2C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 getHit1;//index                                // 275  0x0000000000000B30  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 getHit2;//index                                // 276  0x0000000000000B34  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 getHit3;//index                                // 277  0x0000000000000B38  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 getHit4;//index                                // 278  0x0000000000000B3C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 getHit5;//index                                // 279  0x0000000000000B40  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 interactSound;//index                          // 280  0x0000000000000B44  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 damagingSound;//index                          // 281  0x0000000000000B48  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "FOOTSTEPS")]
        public Int32 forwardFootStepLeft;//index                    // 282  0x0000000000000B4C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "FOOTSTEPS")]
        public Int32 forwardFootStepRight;//index                   // 283  0x0000000000000B50  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "FOOTSTEPS")]
        public Int32 backwardFootStepLeft;//index                   // 284  0x0000000000000B54  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "FOOTSTEPS")]
        public Int32 backwardFootStepRight;//index                  // 285  0x0000000000000B58  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "FOOTSTEPS")]
        public Int32 firstPersonJumpFootStep;//index                // 286  0x0000000000000B5C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "FOOTSTEPS")]
        public Int32 firstPersonLandFootStep;//index                // 287  0x0000000000000B60  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 outOfManaSound;//index                         // 288  0x0000000000000B64  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 inventoryFullSound;//index                     // 289  0x0000000000000B68  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantSound;//index                              // 292  0x0000000000000B6C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantUseSoundB;//index                           // 290  0x0000000000000B70  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantUseYetSound;//index                        // 291  0x0000000000000B74  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantCastSound;//index                          // 293  0x0000000000000B78  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantCastYetSound;//index                       // 294  0x0000000000000B7C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 cantCastHereSound;//index                      // 295  0x0000000000000B80  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 lockedSound;//index                            // 296  0x0000000000000B84  XLS_ReadIndex
        public float pathingCollisionRadius;                        // 299  0x0000000000000B88  XLS_ReadFloat
        public float collisionRadius;                               // 297  0x0000000000000B8C  XLS_ReadFloat
        public float collisionRadiusHorizontal;                     // 298  0x0000000000000B90  XLS_ReadFloat
        public float collisionHeight;                               // 300  0x0000000000000B94  XLS_ReadFloat
        public float blockingRadiusOverride;                        // 301  0x0000000000000B98  XLS_ReadFloat
        public float warpPlaneForwardMultiplier;                    // 302  0x0000000000000B9C  XLS_ReadFloat
        public float warpOutDistance;                               // 305  0x0000000000000BA0  XLS_ReadFloat
        public Int32 snapToAngleInDegrees;                          // 306  0x0000000000000BA4  XLS_ReadInt32
        public float offsetUp;                                      // 307  0x0000000000000BA8  XLS_ReadFloat
        public float scale;                                         // 309  0x0000000000000BAC  XLS_ReadFloat
        public float weaponScale;                                   // 308  0x0000000000000BB0  XLS_ReadFloat
        public float scaleDelta;                                    // 310  0x0000000000000BB4  XLS_ReadFloat
        public float championScale;                                 // 315  0x0000000000000BB8  XLS_ReadFloat
        public float championScaleDelta;                            // 316  0x0000000000000BBC  XLS_ReadFloat
        public float scaleMultiplier;                               // 314  0x0000000000000BC0  XLS_ReadFloat
        public float ragdollForceMultiplier;                        // 318  0x0000000000000BC4  XLS_ReadFloat
        public float meleeImpactOffset;                             // 317  0x0000000000000BC8  XLS_ReadFloat
        public float meleeRangeMax;                                 // 319  0x0000000000000BCC  XLS_ReadFloat
        public float meleeRangeDesired;                             // 320  0x0000000000000BD0  XLS_ReadFloat
        public float maxAutoMapRadius;                              // 321  0x0000000000000BD4  XLS_ReadFloat
        public float fuse;                                          // 322  0x0000000000000BD8  XLS_ReadFloat
        public Int32 clientMinimumLifetime;                         // 323  0x0000000000000BDC  XLS_ReadInt32
        public float rangeBase;                                     // 324  0x0000000000000BE0  XLS_ReadFloat
        public float rangeDesiredMult;                              // 325  0x0000000000000BE4  XLS_ReadFloat
        public float force;                                         // 331  0x0000000000000BE8  XLS_ReadFloat
        public float horizontalAccuracy;                            // 329  0x0000000000000BEC  XLS_ReadFloat
        public float verticleAccuracy;                              // 328  0x0000000000000BF0  XLS_ReadFloat
        public float hitBackup;                                     // 330  0x0000000000000BF4  XLS_ReadFloat
        public float jumpVelocity;                                  // 332  0x0000000000000BF8  XLS_ReadFloat
        public float velocityForImpact;                             // 333  0x0000000000000BFC  XLS_ReadFloat
        public float acceleration;                                  // 334  0x0000000000000C00  XLS_ReadFloat
        public float bounce;                                        // 335  0x0000000000000C04  XLS_ReadFloat
        public float dampening;                                     // 336  0x0000000000000C08  XLS_ReadFloat
        public float friction;                                      // 337  0x0000000000000C0C  XLS_ReadFloat
        public float missileArcHeight;                              // 339  0x0000000000000C10  XLS_ReadFloat
        public float missileArcDelta;                               // 340  0x0000000000000C14  XLS_ReadFloat
        public float spawnRandomizeLengthPercent;                   // 338  0x0000000000000C18  XLS_ReadFloat
        public Int32 stopAfterXNumOfTicks;                          // 341  0x0000000000000C1C  XLS_ReadInt32
        public float walkSpeed;                                     // 343  0x0000000000000C20  XLS_ReadFloat
        public float walkMin;                                       // 344  0x0000000000000C24  XLS_ReadFloat
        public float walkMax;                                       // 355  0x0000000000000C28  XLS_ReadFloat
        public float strafeSpeed;                                   // 356  0x0000000000000C2C  XLS_ReadFloat
        public float strafeMin;                                     // 357  0x0000000000000C30  XLS_ReadFloat
        public float strafeMax;                                     // 358  0x0000000000000C34  XLS_ReadFloat
        public float jumpSpeed;                                     // 359  0x0000000000000C38  XLS_ReadFloat
        public float jumpMin;                                       // 360  0x0000000000000C3C  XLS_ReadFloat
        public float jumpMax;                                       // 361  0x0000000000000C40  XLS_ReadFloat
        public float runSpeed;                                      // 362  0x0000000000000C44  XLS_ReadFloat
        public float runMin;                                        // 363  0x0000000000000C48  XLS_ReadFloat
        public float runMax;                                        // 364  0x0000000000000C4C  XLS_ReadFloat
        public float backupSpeed;                                   // 365  0x0000000000000C50  XLS_ReadFloat
        public float backupMin;                                     // 366  0x0000000000000C54  XLS_ReadFloat
        public float backupMax;                                     // 367  0x0000000000000C58  XLS_ReadFloat
        public float knockBackSpeed;                                // 368  0x0000000000000C5C  XLS_ReadFloat
        public float knockBackMin;                                  // 369  0x0000000000000C60  XLS_ReadFloat
        public float knockBackMax;                                  // 370  0x0000000000000C64  XLS_ReadFloat
        public float meleeSpeed;                                    // 371  0x0000000000000C68  XLS_ReadFloat
        public float meleeMin;                                      // 372  0x0000000000000C6C  XLS_ReadFloat
        public float meleeMax;                                      // 373  0x0000000000000C70  XLS_ReadFloat
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknownBytes12a;
        public float walkAndRunDelta;                               // 342  0x0000000000000C74  XLS_ReadFloat
        public Int32 rangeMin;                                      // 326  0x0000000000000C78  XLS_ReadInt32
        public Int32 rangeMax;                                      // 327  0x0000000000000C7C  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 decoyMonster;//index                           // 415  0x0000000000000C80  XLS_ReadIndex
        public Int32 havokShape;                                    // 374  0x0000000000000C84  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 missileHitUnit;//index                         // 388  0x0000000000000C88  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 missileHitBackground;//index                   // 389  0x0000000000000C8C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 missileOnFreeOrFuse;//index                    // 390  0x0000000000000C90  XLS_ReadIndex
        public Int32 missileTag;                                    // 400  0x0000000000000C94  XLS_ReadInt32
        public Int32 damageRepeatRate;                              // 396  0x0000000000000C98  XLS_ReadInt32
        public Int32 damageRepeatChance;                            // 397  0x0000000000000C9C  XLS_ReadInt32
        public Int32 repeatDamageImmediately;                       // 398  0x0000000000000CA0  XLS_ReadInt32
        public Int32 serverSrcDamage;                               // 399  0x0000000000000CA4  XLS_ReadInt32
        public Int32 blockGhosts;                                   // 401  0x0000000000000CA8  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster;//index                                // 402  0x0000000000000CAC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTTRIGGERS")]
        public Int32 triggerType;//index                            // 403  0x0000000000000CB0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 operatorStatesTriggerProhibited;               // 404  0x0000000000000CB4  XLS_ReadIndexArray,01
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 sublevelDest;//index                           // 405  0x0000000000000CB8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 operateRequiredQuest;//index                   // 406  0x0000000000000CBC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_STATE")]
        public Int32 operateRequiredQuestState;//index              // 407  0x0000000000000CC0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_STATE_VALUE")]
        public Int32 operateRequiredQuestStateValue;//index         // 408  0x0000000000000CC4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_STATE_VALUE")]
        public Int32 operateProhibitedQuestStateValue;//index       // 409  0x0000000000000CC8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiresItemOfUnitType1;//index                // 66   0x0000000000000CCC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 spawnTreasureClassInLevel;//index              // 67   0x0000000000000CD0  XLS_ReadIndex
        public OneWayVisualPortalDir oneWayVisualPortalDir; // XLS_InternalIndex_OneWayVisualPortalDir (XLS_UNIT_DATA+445A), 0x04   // 410  0x0000000000000CD4  XLS_ReadInternalIndex
        public WarpResolveTime warpResolveTime; // XLS_InternalIndex_WarpResolveTime (XLS_UNIT_DATA+59FB), 0x05               // 473  0x0000000000000CD8  XLS_ReadInternalIndex
        public float labelScale;                                    // 413  0x0000000000000CDC  XLS_ReadFloat
        public float labelForwardOffset;                            // 414  0x0000000000000CE0  XLS_ReadFloat
        public float heightpercent;                                 // 469  0x0000000000000CE4  XLS_ReadFloat_0 ??
        public float weightPercent;                                 // 470  0x0000000000000CE8  XLS_ReadFloat_0 ??
        public Int32 hasAppearanceShape;//bool                      // 459  0x0000000000000CEC  XLS_ReadInt32,0B
        public byte apperanceHeightMin;                             // 460  0x0000000000000CF0  XLS_ReadByte
        public byte appearanceHeightMax;                            // 461  0x0000000000000CF1  XLS_ReadByte
        public byte appearanceWeightMin;                            // 462  0x0000000000000CF2  XLS_ReadByte
        public byte appearanceWeightMax;                            // 463  0x0000000000000CF3  XLS_ReadByte
        public Int32 appearanceUseLineBounds;//bool                 // 464  0x0000000000000CF4  XLS_ReadInt32,0B
        public byte appearanceShortSkinny;                          // 465  0x0000000000000CF5  XLS_ReadByte
        public byte appearanceTallSkinny;                           // 466  0x0000000000000CF6  XLS_ReadByte
        public byte appearanceShortFat;                             // 467  0x0000000000000CF7  XLS_ReadByte
        public byte appearanceTallFat;                              // 468  0x0000000000000CF8  XLS_ReadByte
        [ExcelOutput(IsTableIndex = true, TableStringId = "COLORSETS")]
        public Int32 colorSet;//index                               // 31   0x0000000000000CFC  XLS_ReadIndex
        public Int32 corpseExplodePoints;                           // 471  0x0000000000000D00  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiredAttackerUnitType;//index               // 472  0x0000000000000D04  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_AREAS")]
        public Int32 startingLevelArea;//index                      // 474  0x0000000000000D08  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_AREAS")]
        public Int32 warpToLevelArea;//index                        // 476  0x0000000000000D0C  XLS_ReadIndex
        public Int32 warpToFloor;                                   // 477  0x0000000000000D10  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableStringId = "GLOBAL_THEMES")]
        public Int32 globalThemeRequired;//index                    // 478  0x0000000000000D14  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 levelThemeRequired;//index                     // 479  0x0000000000000D18  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSCALING_FIELDLEVEL")]
        public Int32[] gameModeRequired;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 modeGroupOnClient;//index                      // 475  0x0000000000000D1C  XLS_ReadIndex
        public Int32 cameraTarget;                                  // 455  0x0000000000000D20  XLS_ReadInt32
        public Int32 null2;                                         // 456  0x0000000000000D24  XLS_ReadInt32
        public Int32 null3;                                         // 457  0x0000000000000D28  XLS_ReadInt32
        public Int32 null4;        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        Int32[] unknownBytes26;                                     // 0x0000000000000D2C to 0x0000000000000D84
        public Int32 null5;                                         // 458  0x0000000000000D84  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] unknownBytes27;                                     // 0x0000000000000D88 to 0x0000000000000DB8
        public bool noUseWithConsumableItem;        
        public bool noItemMode;
        public Bindability bindability;
        public Int32 defenceType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknownBytes28;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            spawn = 1,                                          // 472,1, 00
            spawnTypeExplicit = 2,                                // 472,2, 01
            spawnAtMerchant = 4,                              // 472,3, 02
            spawnAtPvpMerchant = 8,                                   // 472,4, 03
            forceIgnorescale = 16,                                  // 472,5, 04
            impactOnFuse = 32,                               // 472,6, 05
            impactOnFree = 64,                         // 472,7, 06
            impactOnHitUnit = 128,                        // 472,8, 07
            impactOnHitBackground = 256,                                // 472,9, 08
            havokIgnoresDirection = 512,                                    // 472,10, 09
            damagesOnFuse = 1024,                               // 472,11, 0A
            hitsUnits = 2048,                              // 472,12, 0B
            killOnUnitHit = 4096,                              // 472,13, 0C
            hitsBackground = 8192,                            // 472,14, 0D
            noRayCollsion = 16384,                                 // 472,15, 0E
            killOnBackground = 32768,                                // 472,16, 0F
            stickOnHit = 65536,                                       // 472,20, 10
            stickOnInit = 131072,                                // 472,21, 11
            sync = 262144,                                // 472,22, 12
            clientOnly = 524288,                              // 472,23, 13
            serverOnly = 1048576,                                  // 472,24, 14
            useSourceVel = 2097152,                         // 472,26, 15
            mustHit = 4194304,                // 472,27, 16
            prioritizeTarget = 8388608,               // 472,28, 17
            trailEffectsUseProjectile = 16777216,                    // 472,29, 18
            impactEffectUseProjectile = 33554432,                      // 472,30, 19
            destroyOtherMissiles = 67108864,                       // 472,31, 1A
            dontHitSkillTarget = 134217728,                   // 472,32, 1B
            flipFaceDirection = 268435456,                            // 472,33, 1C
            doNotUseRangeForSkill = 536870912,                       // 472,18, 1D
            pullsTarget = 1073741824,                  // 472,19, 1E
            damagesOnHitUnit = 2147483648                 // 472,17, 1F
        }

        [FlagsAttribute]
        public enum BitMask02 : uint
        {
            pulsesStatsOnHitUnit = 1,                       // 472,34, 20
            damagesOnHitBackground = 2,                            // 472,36, 21
            alwaysCheckForCollisions = 4,                            // 472,35, 22
            setShapePercentages = 8,                    // 472,37, 23
            useSourceAppearance = 16,                   // 472,38, 24
            doNotTransferRidersFromOwner = 32,                       // 472,40, 25
            doNotTransferDamagesOnClient = 64,                      // 472,41, 26
            missileIgnorePostlaunch = 128,                       // 472,42, 27
            attacksLocationOnHitUnit = 256,                         // 472,44, 28
            dontDeactivateWithRoom = 512,                           // 472,45, 29
            angerOthersOnDamaged = 1024,                       // 472,46, 2A
            angerOthersOnDeath = 2048,                      // 472,47, 2B
            alwaysFaceSkillTarget = 4096,           // 472,48, 2C
            setRopeEndWithNoTarget = 8192,                              // 472,49, 2D
            forceDrawDirectionToMoveDirection = 16384,                           // 472,50, 2E
            questNameColor = 32768,                      // 472,51, 2F
            doNotSortWeapons = 65536,                      // 472,52, 30
            ignoresEquipClassReqs = 131072,                         // 472,53, 31
            doNotUseSourceForTohit = 262144,                      // 472,54, 32
            angleWhilePathing = 524288,                // 472,55, 33
            doNotAddWardrobeLayer = 1048576,                           // 472,56, 34
            doNotUseContainerAppearance = 2097152,                  // 472,57, 35
            subscriberOnly = 4194304,                      // 472,58, 36
            computeLevelRequirement = 8388608,                              // 472,59, 37
            doNotFattenCollision = 16777216,             // 472,60, 38
            automapSave = 33554432,                    // 472,61, 39
            requiresCanOperateToBeKnown = 67108864,                              // 472,62, 3A
            forceFreeOnRoomReset = 134217728,              // 472,63, 3B
            canReflect = 268435456,                    // 472,64, 3C
            selectTargetIgnoresAimPos = 536870912,                 // 318,1, 3D
            canMeleeAboveHeight = 1073741824,           // 56,1, 3E
            getFlavortextFromQuest = 2147483648                     // 56,2, 3F
        }

        [FlagsAttribute]
        public enum BitMask03 : uint
        {
            unidentifiedNameFromBaseRow = 1,                            // 56,3, 40
            noRandomProperName = 2,                                        // 58,1, 41
            noNameModifications = 4,                                    // 58,2, 42
            preload = 8,                              // 58,3, 43
            ignoreInDat = 16,                        // 56,4, 44
            ignoreSavedStates = 32,                                        // 58,4, 45
            drawUsingCutUpWardrobe = 64,                                         // 58,5, 46
            isGood = 128,                                // 58,7, 47
            isNpc = 256,                                      // 106,1, 48
            cannotBeMoved = 512,                                   // 57,3, 49
            nolevel = 1024,                                  // 85,7, 4A
            usesSkills = 2048,                                     // 57,2, 4B
            autopickup = 4096,                          // 57,1, 4C
            trigger = 8192,                            // 57,4, 4D
            dieOnClientTrigger = 16384,                            // 57,5, 4E
            neverDestroyDead = 32768,                                  // 57,6, 4F
            collideWhenDead = 65536,                                  // 57,7, 50
            StartDead = 131072,                    // 57,8, 51
            givesLoot = 262144,                        // 57,10, 52
            doNotTriggerByProximity = 524288,                              // 57,11, 53
            triggerOnEnterRoom = 1048576,                                    // 58,8, 54
            destructible = 2097152,                                 // 58,9, 55
            inAir = 4194304,                          // 87,1, 56
            wallWalk = 8388608,                             // 69,1, 57
            startInTownIdle = 16777216,                         // 69,2, 58
            onDieDestroy = 33554432,                          // 69,3, 59
            onDieEndDestroy = 67108864,                   // 69,4, 5A
            onDieHideModel = 134217728,                            // 73,1, 5B
            selectableDeadOrDying = 268435456, // 28                   // 73,2, 5C
            interactive = 536870912,                             // 78,9, 5D
            HideDialogHead = 1073741824,                           // 78,2, 5E
            collideBad = 2147483648                          // 78,8, 5F
        }

        [FlagsAttribute]
        public enum BitMask04 : uint
        {
            collideGood = 1,                                       // 78,3, 60
            modesIgnoreAI = 2,                         // 78,4, 61
            dontPath = 4,                                   // 78,5, 62
            snapToPathnodeOnCreate = 8,                          // 78,6, 63
            untargetable = 16,                                        // 78,7, 64
            FaceDuringInteraction = 32,                                    // 58,11, 65
            noSynch = 64,                         // 58,12, 66
            cannotTurn = 128,                                     // 58,13, 67
            turnNeckInsteadOfBody = 256,                      // 58,14, 68
            Merchant = 512,                                       // 61,1, 69
            MerchantPvP = 1024,                                   // 61,6, 6A
            MerchantSharedInventory = 2048,                                     // 61,7, 6B
            Trader = 4096,                                   // 61,8, 6C
            Tradesman = 8192,                           // 61,11, 6D
            Gambler = 16384,                                    // 61,9, 6E
            MapVendor = 32768,                                     // 61,2, 6F
            GodQuestMessanger = 65536,                                // 61,3, 70
            Trainer = 131072,                                 // 61,4, 71
            healer = 262144,                           // 63,1, 72
            Gravekeeper = 524288,                           // 63,2, 73
            TaskGiver = 1048576,                  // 63,3, 74
            CanUpgradeItems = 2097152,                           // 63,6, 75
            CanAugmentItems = 4194304,                           // 63,7, 76
            AutoIdentifiesInventory = 8388608,                                  // 63,8, 77
            NPCDungeonWarp = 16777216,                             // 61,10, 78
            PvPSignerUpper = 33554432,                           // 470,1, 79
            Foreman = 67108864,               // 470,2, 7A
            Transporter = 134217728,                  // 57,9, 7B
            showsPortrait = 268435456,                 // 87,2, 7C
            petGetsStatPointsPerLevel = 536870912,                 // 87,3, 7D
            ignoresSkillPowerCost = 1073741824,                    // 63,9, 7E
            checkRadiusWhenPathing = 2147483648                           // 85,6, 7F
        }

        [FlagsAttribute]
        public enum BitMask05 : uint
        {
            checkHeightWhenPathing = 1,                            // 63,10, 80
            QuestImportantInfo = 2,                           // 63,13, 81
            ignoresTohit = 4,                     // 63,14, 82
            AskQuestsForOperate = 8,                                     // 63,15, 83
            AskFactionForOperate = 16,                             // 63,11, 84
            AskPvPCensorshipForOperate = 32,                           // 63,12, 85
            Structural = 64,                            // 63,16, 86
            AskQuestsForKnown = 128,                       // 63,17, 87
            AskQuestsForVisible = 256,                          // 63,18, 88
            informQuestsOnInit = 512,                                      // 64,1, 89
            InformQuestsOfLootDrop = 1024,                           // 67,2, 8A
            informQuestsOnDeath = 2048,                       // 67,3, 8B
            noTrade = 4096,                     // 68,1, 8C
            noAuction = 8192,                                   // 73,3, 8D
            FlagRoomAsNoSpawn = 16384,                          // 73,4, 8E
            MonitorPlayerApproach = 32768,                         // 226,1, 8F
            MonitorApproachClearLOS = 65536,                           // 226,2, 90
            canFizzle = 131072,                         // 226,3, 91
            inheritsDirection = 262144,                          // 226,4, 92
            cannotBeDismantled = 524288,            // 222,1, 93
            cannotBeUpgraded = 1048576,                          // 117,1, 94
            cannotBeAugmented = 2097152,                   // 117,3, 95
            cannotBeDemodded = 4194304,                            // 117,2, 96
            ignoreSellWithInventoryConfirm = 8388608,
            wardrobePerUnit = 16777216,                                  // 259,1, 98
            wardrobeSharesModelDef = 33554432,               // 259,2, 99
            noWeaponModel = 67108864,                      // 259,3, 9A
            undefined_9B = 134217728,                   // 259,4, 9B
            NoDrop = 268435456,                             // 260,1, 9C
            NoDropExceptForDuplicates = 536870912,                      // 260,3, 9D
            AskQuestsForPickup = 1073741824,                       // 260,4, 9E
            InformQuestsOnPickup = 2147483648                       // 176,1, 9F
        }

        [FlagsAttribute]
        public enum BitMask06 : uint
        {
            Examinable = 1,                                // 220,1, A0
            InformQuestsToUse = 2,                                  // 104,1, A1
            ConsumeWhenUsed = 4,                             // 125,1, A2
            immuneToCritical = 8,                                   // 255,1, A3
            noRandomAffixes = 16,                           // 320,1, A4
            canBeChampion = 32,                   // 341,1, A5
            noQualityDowngrade = 64,                        // 472,65, A6
            noDrawOnInit = 128,               // 406,1, A7
            ignoreDontDrawWeapons = 256,                       // 410,1, A8
            mustFaceMeleeTarget = 512,                                // 410,2, A9
            doNotDestroyIfVelocityIsZero = 1024,                         // 410,3, AA
            ignoreInteractDistance = 2048,                             // 412,1, AB
            operateRequiresGoodQuestStatus = 4096,//12
            reverseArriveDirection = 8192,                                       // 454,1, AD
            faceAfterWarp = 16384,//14
            neverAStartLocation = 32768,                         // 472,39, AF
            alwaysShowLabel = 65536,                           // 472,66, B0
            _undefined2 = 131072,                   // 472,68, B1
            null0 = 262144,                                    // 472,67, B2
            _undefined3 = 524288,//19
            isNonweaponMissile = 1048576,                 // 472,69, B4
            cullByScreensize = 2097152,                     // 472,70, B5
            linkWarpDestByLevelType = 4194304,                  // 472,25, B6
            isBoss = 8388608,                          // 64,2, B7
            _undefined4 = 16777216,            // 472,71, B8
            takeResponsibilityOnKill = 33554432,                        // 472,72, B9
            alwaysKnowForSounds = 67108864,                       // 472,73, BA
            ignoreTargetOnRepeatDmg = 134217728,                         // 472,74, BB
            bindToLevelArea = 268435456,                          // 470,3, BC
            doNotCollideWithDestructibles = 536870912,                          // 472,75, BD
            blocksEverything = 1073741824,                      // 78,1, BE
            everyoneCanTarget = 2147483648                 // 141,1, BF
        }

        [FlagsAttribute]
        public enum BitMask07 : uint
        {
            missilePlotArc = 1,
            petDiesOnWarp = 2,
            missileIsGore = 4,
            CanAttackFriends = 8,
            ignoreItemRequirements = 16,
            lowLodInTown = 32,
            treasureClassBeforeRoom = 64,
            TaskGiverNoStartingIcon = 128,
            assignGuid = 256,
            MerchantDoesNotRefresh = 512,
            dontDepopulate = 1024,
            doNotShrinkBones = 2048,
            multiplayerOnly = 4096,
            hasQuestInfo = 8192,
            NoSpin = 16384,
            NPCGuildMaster = 32768,
            autoIdentifyAffixs = 65536,
            NPCRespeccer = 131072,
            allowObjectStepping = 262144,
            alwaysUseFallback = 524288,
            cannotSpawnRandomLevelTreasure = 1048576,
            xferMissileStats = 2097152,
            specificToDifficulty = 4194304,
            isFieldMissile = 8388608,
            ignoreFuseMsStat = 16777216,
            usesPetlevel = 33554432,
            doesNotCountTowardsMaxPets = 67108864,
            mirrorInLeftHand = 134217728,
            warpOutRandomDirection = 268435456,
            cannon = 536870912,
            couponEvent = 1073741824,
            informQuestsOnSpawntreasure = 2147483648 
        }
        [FlagsAttribute]
        public enum BitMask08 : uint
        {
            merchantEvent = 1,
            spawnAtEventMerchant = 2,
            PvP_Shrine_health = 4,
            PvP_Shrine_Power = 8,
            PvP_Shrine_Shields = 16,
            PvP_Team_Red = 32,
            PvP_Team_Blue = 64,
            onlyUseInPvp = 128,
            merchantBonus = 256,
            spawnAtBonusMerchant = 512
        }

        //these are probably only for missiles.t.c
        [FlagsAttribute]
        public enum BitMask09 : uint
        {
            bounceOnUnitHit = 1,                                // 85,1, 0
            bounceOnBackGroundHit = 2,                          // 85,2, 1
            newDirectionOnBounce = 4,                           // 85,3, 2
            canNotRicochet = 8,                                 // 85,4, 3
            reTargetOnBounce = 16                               // 85,5, 4
        }

        public enum Sex
        {
            Null = -1,
            Male = 0,
            Female = 1
        }

        public enum WeaponBoneIndex
        {
            Null = -1,
            Generic = 0,
            Focus = 1
        }

        public enum RoomPopulatePass
        {
            Null = -1,
            SublevelPopulate = 0,
            Setup = 1,
            Content = 2
        }
        public enum QualityName
        {
            Null = -1,
            ItemQuality = 0,
            Affix = 1
        }

        public enum OneWayVisualPortalDir
        {
            Null = -1,
            From = 0,
            To = 1
        }
        public enum WarpResolveTime
        {
            Null = -1,
            SublevelPopulate = 0,//Not entirely sure about this one.
            LevelActivate = 1,
            OnUse = 2
        }

        public enum Bindability
        {
            None = -1,
            Disable = 0,
            Enable = 1
        }

        /* Unknown/Not Added
        XLS_UNIT_DATA+22BE 0A8 and     dword ptr [rsp+0A8h+var_80], 0
        XLS_UNIT_DATA+22C3 0A8 and     dword ptr [rsp+0A8h+var_88], 0
        XLS_UNIT_DATA+22C8 0A8 mov     r9d, 373h
        XLS_UNIT_DATA+22CE 0A8 lea     r8, aSfx_fire_damag             ; "sfx_fire_damage_percent"
        XLS_UNIT_DATA+22D5 0A8 mov     edx, edi
        XLS_UNIT_DATA+22D7 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+22DA 0A8 call    XLS_Unknown_0

        XLS_UNIT_DATA+25FD 0A8 and     dword ptr [rsp+0A8h+var_80], 0
        XLS_UNIT_DATA+2602 0A8 and     dword ptr [rsp+0A8h+var_88], 0
        XLS_UNIT_DATA+2607 0A8 lea     r8, aStealthRevealD             ; "stealth reveal duration"
        XLS_UNIT_DATA+260E 0A8 mov     r9d, 266h
        XLS_UNIT_DATA+2614 0A8 mov     edx, edi
        XLS_UNIT_DATA+2616 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+2619 0A8 call    XLS_Unknown_0

        XLS_UNIT_DATA+261E 0A8 and     dword ptr [rsp+0A8h+var_80], 0
        XLS_UNIT_DATA+2623 0A8 and     dword ptr [rsp+0A8h+var_88], 0
        XLS_UNIT_DATA+2628 0A8 lea     r8, aStealthRevealR             ; "stealth reveal radius"
        XLS_UNIT_DATA+262F 0A8 mov     r9d, 269h
        XLS_UNIT_DATA+2635 0A8 mov     edx, edi
        XLS_UNIT_DATA+2637 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+263A 0A8 call    XLS_Unknown_0

        XLS_UNIT_DATA+2AB7 0A8 lea     r8, aMaxSlots                   ; "max slots"
        XLS_UNIT_DATA+2ABE 0A8 lea     edx, [rdi-1]
        XLS_UNIT_DATA+2AC1 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+2AC4 0A8 call    XLS_Unknown_4

        XLS_UNIT_DATA+45BB 0A8 mov     [rsp+0A8h+var_78], r12
        XLS_UNIT_DATA+45C0 0A8 mov     dword ptr [rsp+0A8h+var_80], r12d
        XLS_UNIT_DATA+45C5 0A8 lea     r12d, [rbp-1Bh]
        XLS_UNIT_DATA+45C9 0A8 lea     r15d, [rbp+11h]
        XLS_UNIT_DATA+45CD 0A8 lea     r8, aAi_1                       ; "ai"
        XLS_UNIT_DATA+45D4 0A8 mov     r9d, esi                        ; AA
        XLS_UNIT_DATA+45D7 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+45DA 0A8 mov     edx, r12d                       ; 1A = AICOMMON_STATE excel table
        XLS_UNIT_DATA+45DD 0A8 mov     dword ptr [rsp+0A8h+var_88], r15d ; 46 = AI_INIT
        XLS_UNIT_DATA+45E2 0A8 call    XLS_ReadFromExtraIndexInt

        XLS_UNIT_DATA+45E7 0A8 xor     esi, esi
        XLS_UNIT_DATA+45E9 0A8 mov     [rsp+0A8h+var_78], rsi
        XLS_UNIT_DATA+45EE 0A8 lea     r8, aAiUnarmed                  ; "ai unarmed"
        XLS_UNIT_DATA+45F5 0A8 mov     r9d, edi                        ; AB
        XLS_UNIT_DATA+45F8 0A8 mov     edx, r12d                       ; 1A = AICOMMON_STATE excel table
        XLS_UNIT_DATA+45FB 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+45FE 0A8 mov     dword ptr [rsp+0A8h+var_80], esi
        XLS_UNIT_DATA+4602 0A8 mov     dword ptr [rsp+0A8h+var_88], r15d ; 46 = AI_INIT
        XLS_UNIT_DATA+4607 0A8 call    XLS_ReadFromExtraIndexInt

        XLS_UNIT_DATA+460C 0A8 lea     edi, [rbp-1]
        XLS_UNIT_DATA+460F 0A8 lea     r9d, [rbp+7Eh]                  ; B3
        XLS_UNIT_DATA+4613 0A8 lea     r8, aAiPeriod                   ; "ai period"
        XLS_UNIT_DATA+461A 0A8 mov     edx, edi                        ; 34
        XLS_UNIT_DATA+461C 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+461F 0A8 mov     dword ptr [rsp+0A8h+var_80], esi
        XLS_UNIT_DATA+4623 0A8 mov     dword ptr [rsp+0A8h+var_88], esi
        XLS_UNIT_DATA+4627 0A8 call    XLS_Unknown_0

        XLS_UNIT_DATA+462C 0A8 mov     dword ptr [rsp+0A8h+var_80], esi
        XLS_UNIT_DATA+4630 0A8 lea     r8, aAim_height                 ; "aim_height"
        XLS_UNIT_DATA+4637 0A8 mov     r9d, 10Ah                       ; 10A
        XLS_UNIT_DATA+463D 0A8 mov     edx, ebp                        ; 35
        XLS_UNIT_DATA+463F 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+4642 0A8 movss   dword ptr [rsp+0A8h+var_88], xmm9
        XLS_UNIT_DATA+4649 0A8 call    XLS_ReadFromExtraIndexFloat

        XLS_UNIT_DATA+464E 0A8 lea     ebp, [rsi+12h]
        XLS_UNIT_DATA+4651 0A8 lea     r9d, [r15+74h]                  ; BA
        XLS_UNIT_DATA+4655 0A8 lea     r8, aAi_awake_range             ; "ai_awake_range"
        XLS_UNIT_DATA+465C 0A8 mov     edx, edi                        ; 34
        XLS_UNIT_DATA+465E 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+4661 0A8 mov     dword ptr [rsp+0A8h+var_80], esi
        XLS_UNIT_DATA+4665 0A8 mov     dword ptr [rsp+0A8h+var_88], ebp

        XLS_UNIT_DATA+4669 0A8 call    XLS_Unknown_0
        XLS_UNIT_DATA+466E 0A8 mov     dword ptr [rsp+0A8h+var_80], esi
        XLS_UNIT_DATA+4672 0A8 lea     esi, [r15+76h]
        XLS_UNIT_DATA+4676 0A8 lea     r8, aAi_follow_rang             ; "ai_follow_range"
        XLS_UNIT_DATA+467D 0A8 mov     edx, edi                        ; 34
        XLS_UNIT_DATA+467F 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+4682 0A8 mov     r9d, esi                        ; BC
        XLS_UNIT_DATA+4685 0A8 mov     dword ptr [rsp+0A8h+var_88], 0Ah

        XLS_UNIT_DATA+468D 0A8 call    XLS_Unknown_0
        XLS_UNIT_DATA+4692 0A8 and     dword ptr [rsp+0A8h+var_80], 0
        XLS_UNIT_DATA+4697 0A8 lea     r15d, [rsi-1]
        XLS_UNIT_DATA+469B 0A8 lea     r8, aAi_sight_range             ; "ai_sight_range"
        XLS_UNIT_DATA+46A2 0A8 mov     edx, edi                        ; 34
        XLS_UNIT_DATA+46A4 0A8 mov     rcx, rbx
        XLS_UNIT_DATA+46A7 0A8 mov     r9d, r15d                       ; BB
        XLS_UNIT_DATA+46AA 0A8 mov     dword ptr [rsp+0A8h+var_88], 8
        XLS_UNIT_DATA+46B2 0A8 call    XLS_Unknown_0
        */
    }
}
