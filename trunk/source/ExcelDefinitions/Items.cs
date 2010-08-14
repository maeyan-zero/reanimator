using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    // size = 0x0000000000000DB8
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemsRow
    {
        ExcelFile.TableHeader header;                               // 

        [ExcelOutput(IsStringOffset = true, SortId = 1)]
        public Int32 name;//pchar                                   // 1    0x0000000000000000  XLS_ReadCharPtr1
        Int32 buffer1;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 folder;//pchar                                 // 2    0x0000000000000008  XLS_ReadCharPtr1
        Int32 buffer2;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearance;//pchar                             // 3    0x0000000000000010  XLS_ReadCharPtr1
        Int32 buffer3;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearenceFirst;//pchar                        // 4    0x0000000000000018  XLS_ReadCharPtr1
        Int32 buffer4;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 icon;//pchar                                   // 5    0x0000000000000020  XLS_ReadCharPtr1
        Int32 buffer5;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 holyRadius;//pchar                             // 20   0x0000000000000028  XLS_ReadCharPtr1
        Int32 buffer6;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyHitParticle;//pchar                        // 22   0x0000000000000030  XLS_ReadCharPtr2
        Int32 buffer7;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lightHitParticle;//pchar                       // 23   0x0000000000000038  XLS_ReadCharPtr2
        Int32 buffer8;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumHitParticle;//pchar                      // 24   0x0000000000000040  XLS_ReadCharPtr2
        Int32 buffer9;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardHitParticle;//pchar                        // 25   0x0000000000000048  XLS_ReadCharPtr2
        Int32 buffer10;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killedParticle;//pchar                         // 26   0x0000000000000050  XLS_ReadCharPtr2
        Int32 buffer11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown01;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fizzleParticle;//pchar                         // 27   0x0000000000000060  XLS_ReadCharPtr1
        Int32 buffer12;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 reflectParticle;//pchar                        // 28   0x0000000000000068  XLS_ReadCharPtr1
        Int32 buffer13;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 restoreVitalsParticle;//pchar                  // 29   0x0000000000000070  XLS_ReadCharPtr1
        Int32 buffer14;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 diffuse;//pchar                                // 6    0x0000000000000078  XLS_ReadCharPtr2
        Int32 buffer15;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 normal;//pchar                                 // 7    0x0000000000000080  XLS_ReadCharPtr2
        Int32 buffer16;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 specular;//pchar                               // 8    0x0000000000000088  XLS_ReadCharPtr2
        Int32 buffer17;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lightmap;//pchar                               // 9    0x0000000000000090  XLS_ReadCharPtr2
        Int32 buffer18;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource1;//pchar                               // 10   0x0000000000000098  XLS_ReadCharPtr2
        Int32 buffer19;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest1;//pchar                                 // 15   0x00000000000000A0  XLS_ReadCharPtr2
        Int32 buffer20;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource2;//pchar                               // 11   0x00000000000000A8  XLS_ReadCharPtr2
        Int32 buffer21;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest2;//pchar                                 // 16   0x00000000000000B0  XLS_ReadCharPtr2
        Int32 buffer22;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource3;//pchar                               // 12   0x00000000000000B8  XLS_ReadCharPtr2
        Int32 buffer23;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest3;//pchar                                 // 17   0x00000000000000C0  XLS_ReadCharPtr2
        Int32 buffer24;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource4;//pchar                               // 13   0x00000000000000C8  XLS_ReadCharPtr2
        Int32 buffer25;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest4;//pchar                                 // 18   0x00000000000000D0  XLS_ReadCharPtr2
        Int32 buffer26;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource5;//pchar                               // 14   0x00000000000000D8  XLS_ReadCharPtr2
        Int32 buffer27;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest5;//pchar                                 // 19   0x00000000000000E0  XLS_ReadCharPtr2
        Int32 buffer28;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 particleFolder;//pchar                         // 21   0x00000000000000E8  XLS_ReadCharPtr1
        Int32 buffer29;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 pickupFunction;//pchar                         // 259  0x00000000000000F0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] buffer30;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown08;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfPoison;//pchar                               // 420  0x00000000000001A8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown09;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailSlashRopeNoTarget;//pchar                 // 433  0x0000000000000200  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown16;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailPoison;//pchar                            // 436  0x0000000000000280  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown17;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projectileSlashRopeWithTarget;//pchar          // 425  0x00000000000002D8  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown24;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projPoison;//pchar                             // 428  0x0000000000000358  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown25;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactWall;//pchar                             // 441  0x00000000000003B0  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
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
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        //byte[] unknown32;

        //[ExcelOutput(IsStringOffset = true)]
        //private Int32 impWallPoison;//pchar // unknown!!
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 116)]
        byte[] unknown32;                                           // 0x0000000000000414 to 0x0000000000000487
        
        //public Int32 stealthRevealDuration;                         // 182  0x0000000000000266  XLS_Unknown_0   - is this a 3-byte reading function??
        //public Int32 stealthRevealRadius;                           // 183  0x0000000000000269  XLS_Unknown_0


        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnit;//pchar                             // 448  0x0000000000000488  XLS_ReadCharPtr1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 116)]
        byte[] unknown40;
        public Int32 unknown;                                       // ???  0x0000000000000560  ??? Appears to be Int32, but not seen in code?
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
        [ExcelOutput(IsStringId = true, TableStringId = "Strings_Items")]
        public Int32 String;                                        // 375  0x0000000000000580  XLS_ReadStringIndex
        [ExcelOutput(IsStringId = true, TableStringId = "Strings_Items")]
        public Int32 typeDescription;                               // 378  0x0000000000000584  XLS_ReadStringIndex
        [ExcelOutput(IsStringId = true, TableStringId = "Strings_Items")]
        public Int32 flavorText;                                    // 379  0x0000000000000588  XLS_ReadStringIndex
        [ExcelOutput(IsStringId = true, TableStringId = "Strings_Items")]
        public Int32 additionalDescription;                         // 380  0x000000000000058C  XLS_ReadStringIndex
        [ExcelOutput(IsStringId = true, TableStringId = "Strings_Items")]
        public Int32 additionalRaceDescription;                     // 381  0x0000000000000590  XLS_ReadStringIndex
        [ExcelOutput(IsStringId = true, TableStringId = "Strings_Items")]
        public Int32 analyze;                                       // 382  0x0000000000000594  XLS_ReadStringIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6D)]
        Int32 recipeList;//index                                    // 62   0x0000000000000598  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6C)]
        public Int32 recipeSingleUse;//index                        // 63   0x000000000000059C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x69)]
        public Int32 paperdollBackgroundLevel;//index               // 43   0x00000000000005A0  XLS_ReadIndex
        public Int32 paperdollWeapon1;                              // 44   0x00000000000005A4  XLS_ReadIntArray,2
        public Int32 paperdollWeapon2;                              // 44   0x00000000000005A8
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 paperdollSkill;//index                         // 45   0x00000000000005AC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x08)]
        public Int32 paperdollColorset;//index                      // 46   0x00000000000005B0  XLS_ReadIndex
        public Int32 respawnChance;                                 // 35   0x00000000000005B4  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x90)]
        Int32 respawnSpawnClass;//index                             // 34   0x00000000000005B8  XLS_ReadIndex,90
        [ExcelOutput(SortId = 2)]
        public Int32 code;                                          // 32   0x00000000000005BC  XLS_ReadCode
        Int32 unknown41;
        public Int32 densityValueOverride;                          // 54   0x00000000000005C4  XLS_ReadInt32
        [ExcelOutput(IsIntOffset = true)]
        public Int32 minionPackSize;                                // 55   0x00000000000005C8  XLS_ReadIntPtr
        public float spinSpeed;                                     // 303  0x00000000000005CC  XLS_ReadFloat
        public float maxTurnRate;                                   // 304  0x00000000000005D0  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 unitType;//index                               // 33   0x00000000000005D4  XLS_ReadIndex,17
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        Int32 unitTypeForLeveling;//index                           // 36   0x00000000000005D8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 preferedByUnitType;//index                     // 37   0x00000000000005DC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 family;//index                                 // 38   0x00000000000005E0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 censorClassNoHumans;//index                    // 39   0x00000000000005E4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 censorClassNoGore;//index                      // 40   0x00000000000005E8  XLS_ReadIndex
        public Int32 sex;                                           // 41   0x00000000000005EC  XLS_ReadInt32_0
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x8C)]
        public Int32 race;//index                                   // 42   0x00000000000005F0  XLS_ReadIndex
        public Int32 rarity;                                        // 107  0x00000000000005F4  XLS_ReadInt32
        public Int32 spawnChance;                                   // 109  0x00000000000005F8  XLS_ReadInt32
        public Int32 minMonsterExperienceLevel;                     // 88   0x00000000000005FC  XLS_ReadInt32
        public Int32 level;                                         // 89   0x0000000000000600  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x72)]
        public Int32 monsterQuality;//index                         // 48   0x0000000000000604  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 monsterClassAtUniqueQuality;//index            // 49   0x0000000000000608  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 monsterClassAtChampionQuality;//index          // 50   0x000000000000060C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 minionClass;//index                            // 52   0x0000000000000610  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x75)]
        public Int32 monsterNameType;//index                        // 53   0x0000000000000614  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x43)]
        public Int32 itemQuality;//index                            // 51   0x0000000000000618  XLS_ReadIndex
        public Int32 angerRange;                                    // 416  0x000000000000061C  XLS_ReadInt32
        public Int32 baseLevel;                                     // 90   0x0000000000000620  XLS_ReadInt32
        public Int32 capLevel;                                      // 91   0x0000000000000624  XLS_ReadInt32
        public Int32 minMerchantLevel;                              // 92   0x0000000000000628  XLS_ReadInt32
        public Int32 maxMerchantLevel;                              // 93   0x000000000000062C  XLS_ReadInt32
        public Int32 minSpawnLevel;                                 // 94   0x0000000000000630  XLS_ReadInt32
        public Int32 maxSpawnLevel;                                 // 95   0x0000000000000634  XLS_ReadInt32       .text:00000001402E7A4C mov     dword ptr [rsp+0A8h+var_88], 186A0h (186A0h = 100,000....)
        public Int32 maxLevel;                                      // 96   0x0000000000000638  XLS_ReadInt32
        [ExcelOutput(IsIntOffset = true)]
        public Int32 fixedLevel;//intptr                            // 97   0x000000000000063C  XLS_ReadIntPtr
        public Int32 hpMin;                                         // 98   0x0000000000000640  XLS_ReadInt32
        public Int32 hpMax;                                         // 99   0x0000000000000644  XLS_ReadInt32
        public Int32 powerMax;                                      // 100  0x0000000000000648  XLS_ReadInt32
        public Int32 experience;                                    // 101  0x000000000000064C  XLS_ReadInt32
        public Int32 attackRating;                                  // 143  0x0000000000000650  XLS_ReadInt32
        [ExcelOutput(IsIntOffset = true)]
        public Int32 luckBonus;//intptr                             // 102  0x0000000000000654  XLS_ReadIntPtr
        public Int32 luckChanceToSpawn;                             // 103  0x0000000000000658  XLS_ReadInt32
        public Int32 roomPopulatePass;                              // 58   0x000000000000065C  XLS_ReadInt32_0
        public Int32 weaponBoneIndex;                               // 57   0x0000000000000660  XLS_ReadInt32_0
        public Int32 requiresAffixOrSuffix;                         // 108  0x0000000000000664  XLS_ReadInt32
        public float autoPickupDistance;                            // 86   0x0000000000000668  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x49)]
        public Int32 pickupPullState;//index                        // 87   0x000000000000066C  XLS_ReadIndex
        public float extraDyingTimeInSeconds;                       // 70   0x0000000000000670  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x40)]
        public Int32 npcInfo;//index                                // 71   0x0000000000000674  XLS_ReadIndex
        public Int32 balanceTestCount;                              // 72   0x0000000000000678  XLS_ReadInt32
        public Int32 balanceTestGroup;                              // 73   0x000000000000067C  XLS_ReadInt32
        public Int32 merchantStartingPane;                          // 59   0x0000000000000680  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6B)]
        public Int32 merchantFactionType;//index                    // 60   0x0000000000000684  XLS_ReadIndex
        public Int32 merchantFactionValueNeeded;                    // 61   0x0000000000000688  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x99)]
        public Int32 questRequirement;//index                       // 64   0x000000000000068C  XLS_ReadIndex
        public float noSpawnRadius;                                 // 65   0x0000000000000690  XLS_ReadFloat
        public float monitorApproachRadius;                         // 68   0x0000000000000694  XLS_ReadFloat
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x1B)]
        public Int32 tasksGeneratedStat;//index                     // 69   0x0000000000000698  XLS_ReadIndex,1B       text:00000001402E7392 mov     dword ptr [rsp+0A8h+var_88], 1Bh      0x1B == 27 (27 = STATS table)
        public float serverMissileOffset;                           // 119  0x000000000000069C  XLS_ReadFloat
        public float homingTurnAngleRadians;                        // 74   0x00000000000006A0  XLS_ReadFloat
        public float homingModBasedDis;                             // 75   0x00000000000006A4  XLS_ReadFloat
        public float homeAfterUnitRadius;                           // 76   0x00000000000006A8  XLS_ReadFloat
        public float collidableAfterXSeconds;                       // 77   0x00000000000006AC  XLS_ReadFloat
        public float homeAfterXSeconds;                             // 78   0x00000000000006B0  XLS_ReadFloat
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask08 bitmask08;                           // 85   0x00000000000006B4  XLS_ReadBitmask
        public float impactCameraShakeDuration;                     // 80   0x00000000000006B8  XLS_ReadFloat
        public float impactCameraShakeMagnitude;                    // 81   0x00000000000006BC  XLS_ReadFloat
        public float impactCameraShakeDegrade;                      // 82   0x00000000000006C0  XLS_ReadFloat
        public Int32 maximumImpactFrequency;                        // 83   0x00000000000006C4  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 onlyCollideWithUnitType;//index                // 79   0x00000000000006C8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x35)]
        public Int32 questDescription;//index                       // 260  0x00000000000006CC  XLS_ReadIndex
        [ExcelOutput(IsIntOffset = true)]
        public Int32 pickUpCondition;                               // 258  0x00000000000006D0  XLS_ReadIntPtr
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes04;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 scriptOnUse;                                   // 248  0x00000000000006D8  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 stackSize;                                     // 227  0x00000000000006DC  XLS_ReadIntPtr
        public Int32 maxPickUp;                                     // 228  0x00000000000006E0  XLS_ReadInt32
        public Int32 baseCost;                                      // 221  0x00000000000006E4  XLS_ReadInt32
        public Int32 realWorldCost;                                 // 222  0x00000000000006E8  XLS_ReadInt32
        [ExcelOutput(IsIntOffset = true)]
        public Int32 buyPriceMult;                                  // 223  0x00000000000006EC  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 buyPriceAdd;                                   // 224  0x00000000000006F0  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 sellPriceMult;                                 // 225  0x00000000000006F4  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 sellPriceAdd;                                  // 226  0x00000000000006F8  XLS_ReadIntPtr
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x5E)]
        public Int32 inventoryWardrobe;                             // 110  0x00000000000006FC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x5E)]
        public Int32 characterScreenWardrobe;                       // 111  0x0000000000000700  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x49)]
        public Int32 characterScreenState;                          // 112  0x0000000000000704  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x5F)]
        Int32 wardrobeBody;                                         // 113  0x0000000000000708  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        Int32 wardrobeFallback;                                     // 114  0x000000000000070C  XLS_ReadIndex
        Int32 null0;                                                // 115  0x0000000000000710  XLS_ReadInt32
        Int32 wardrobeMip;                                          // 118  0x0000000000000714  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x56)]
        public Int32 wardrobeAppearanceGroup;                       // 116  0x0000000000000718  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x56)]
        public Int32 wardrobeAppearanceGroup1st;                    // 117  0x000000000000071C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x24)]
        public Int32 startingStance;                                // 120  0x0000000000000720  XLS_ReadIndex
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknownBytes06;
        public Int32 containerUnitType1;                            // 261  0x0000000000000738  XLS_Unknown_1,4         .text:00000001402E92DF mov     dword ptr [rsp+0A8h+var_88], 4
        public Int32 containerUnitType2;                            // 261  0x000000000000073C
        public Int32 containerUnitType3;// always 0                 // 261  0x0000000000000740
        public Int32 containerUnitType4;// but we could use them    // 261  0x0000000000000744
        public Int32 firingErrorIncrease;                           // 127  0x0000000000000748  XLS_ReadInt32
        public Int32 firingErrorDecrease;                           // 128  0x000000000000074C  XLS_ReadInt32
        public Int32 firingErrorMax;                                // 129  0x0000000000000750  XLS_ReadInt32
        public Int32 accuracyBase;                                  // 130  0x0000000000000754  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes07;
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x18)]
        public Int32 refillHotKey;//index                           // 262  0x0000000000000768  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x25)]
        public Int32 animGroup;//index                              // 121  0x000000000000076C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x60)]
        public Int32 meleeWeapon;//index                            // 122  0x0000000000000770  XLS_ReadIndex
        public Int32 cdTicks;                                       // 131  0x0000000000000774  XLS_ReadInt32
        public float approxDps;                                     // 132  0x0000000000000778  XLS_ReadFloat
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tooltipDamageString;//stridx                   // 137  0x000000000000077C  XLS_ReadStringIndex
        public Int32 requiredAffixGroups1;                          // 126  0x0000000000000780  XLS_ReadIntArray,8
        public Int32 requiredAffixGroups2;                          // 126  0x0000000000000784
        public Int32 requiredAffixGroups3;                          // 126  0x0000000000000788
        public Int32 requiredAffixGroups4;                          // 126  0x000000000000078C
        public Int32 requiredAffixGroups5;                          // 126  0x0000000000000790
        public Int32 requiredAffixGroups6;                          // 126  0x0000000000000794
        public Int32 requiredAffixGroups7;                          // 126  0x0000000000000798
        public Int32 requiredAffixGroups8;                          // 126  0x000000000000079C
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 spawnMonsterClass;//index                      // 133  0x00000000000007A0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x49)]
        public Int32 safeState;//index                              // 134  0x00000000000007A4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillGhost;//index                             // 135  0x00000000000007A8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillRef;//index                               // 233  0x00000000000007AC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x1E)]
        public Int32 dmgType;//index                                // 138  0x00000000000007B0  XLS_ReadIndex
        public Int32 weaponDamageScale;                             // 142  0x00000000000007B4  XLS_ReadInt32
        public Int32 dontUseWeaponDamage;                           // 141  0x00000000000007B8  XLS_ReadInt32
        [ExcelOutput(IsIntOffset = true)]
        public Int32 minBaseDmg;                                    // 139  0x00000000000007BC  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 maxBaseDmg;                                    // 140  0x00000000000007C0  XLS_ReadIntPtr
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        byte[] unknownBytes12;
        public Int32 sfxPoisonAbilityPct;                           // 164  0x0000000000000854  XLS_ReadInt32
        public Int32 sfxPoisonDefensePct;                           // 165  0x0000000000000858  XLS_ReadInt32
        public Int32 sfxPoisonDamageAsPctOfDamageDeliveredFromAttack;//166  0x000000000000085C  XLS_ReadInt32
        public float sfxPoisonDurationInSeconds;                    // 167  0x0000000000000860  XLS_ReadFloat
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknownBytes13;
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
        public Int32 armorUnknown1;
        public Int32 armorPoison;                                   // 193  0x0000000000000914  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes15;                                      // 0x0000000000000918 to 0x0000000000000928
        public Int32 maxArmor;                                      // 187  0x0000000000000928  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
        byte[] unknownBytes16;                                      // 0x000000000000092C to 0x0000000000000958
        public Int32 shields;                                       // 194  0x0000000000000958  XLS_ReadInt32
        public Int32 shieldPhys;                                    // 195  0x000000000000095C  XLS_ReadInt32
        public Int32 shieldFire;                                    // 196  0x0000000000000960  XLS_ReadInt32
        public Int32 shieldElec;                                    // 197  0x0000000000000964  XLS_ReadInt32
        public Int32 shieldSpec;                                    // 198  0x0000000000000968  XLS_ReadInt32
        public Int32 shieldToxic;                                   // 199  0x000000000000096C  XLS_ReadInt32
        public Int32 shieldUnknown1;
        public Int32 shieldPoison;                                  // 200  0x0000000000000974  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes18;                                      // 0x0000000000000978 to 0x0000000000000988
        public Int32 strengthPercent;                               // 201  0x0000000000000988  XLS_ReadInt32
        public Int32 dexterityPercent;                              // 202  0x000000000000098C  XLS_ReadInt32
        public Int32 startingAccuracy;                              // 203  0x0000000000000990  XLS_ReadInt32
        public Int32 startingStrength;                              // 204  0x0000000000000994  XLS_ReadInt32
        public Int32 startingStamina;                               // 205  0x0000000000000998  XLS_ReadInt32
        public Int32 startingWillpower;                             // 206  0x000000000000099C  XLS_ReadInt32
        [ExcelOutput(IsIntOffset = true)]
        public Int32 props1;                                        // 207  0x00000000000009A0  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 props2;                                        // 208  0x00000000000009A4  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 props3;                                        // 209  0x00000000000009A8  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 props4;                                        // 210  0x00000000000009AC  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 props5;                                        // 211  0x00000000000009B0  XLS_ReadIntPtr
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 props1AppliesToUnitype;//index                 // 212  0x00000000000009B4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 props2AppliesToUnitype;//index                 // 213  0x00000000000009B8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 props3AppliesToUnitype;//index                 // 214  0x00000000000009BC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 props4AppliesToUnitype;//index                 // 215  0x00000000000009C0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 props5AppliesToUnitype;//index                 // 216  0x00000000000009C4  XLS_ReadIndex
        [ExcelOutput(IsIntOffset = true)]
        public Int32 perLevelProps1;                                // 218  0x00000000000009C8  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 perLevelProps2;                                // 219  0x00000000000009CC  XLS_ReadIntPtr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 propsElite;                                    // 217  0x00000000000009D0  XLS_ReadIntPtr
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES", Column = "affix")]
        public Int32 affix1;//index                                 // 220  0x00000000000009D4  XLS_ReadIntArray,6
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES", Column = "affix")]
        public Int32 affix2;//index                                 // 220  0x00000000000009D8
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES", Column = "affix")]
        public Int32 affix3;//index                                 // 220  0x00000000000009DC
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES", Column = "affix")]
        public Int32 affix4;//index                                 // 220  0x00000000000009E0
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES", Column = "affix")]
        public Int32 affix5;//index                                 // 220  0x00000000000009E4
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXES", Column = "affix")]
        public Int32 affix6;//index                                 // 220  0x00000000000009E8
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE", Column = "treasureClass")]
        public Int32 treasure;//index                               // 104  0x00000000000009EC  XLS_ReadIndex,65
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE", Column = "treasureClass")]
        public Int32 championTreasure;//index                       // 105  0x00000000000009F0  XLS_ReadIndex,65
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE", Column = "treasureClass")]
        public Int32 firstTimeTreasure;//index                      // 106  0x00000000000009F4  XLS_ReadIndex,65
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x0A)]
        public Int32 inventory;//index                              // 229  0x00000000000009F8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x18)]
        public Int32 recipeIngredientInvLoc;//index                 // 230  0x00000000000009FC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x18)]
        public Int32 recipeResultInvLoc;//index                     // 231  0x0000000000000A00  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE", Column = "treasureClass")]
        public Int32 startingTreasure;//index                       // 232  0x0000000000000A04  XLS_ReadIndex,65
        public Int32 invWidth;                                      // 256  0x0000000000000A08  XLS_ReadInt32
        public Int32 invHeight;                                     // 257  0x0000000000000A0C  XLS_ReadInt32
        public Int64 qualities;                                     // 123  0x0000000000000A10  XLS_Unknown - I think this reads 8 bytes (64 bits); input = 0x40 = 64
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x43)]
        public Int32 requiredQuality;//index                        // 124  0x0000000000000A18  XLS_ReadIndex
        public Int32 qualityName;                                   // 125  0x0000000000000A1C  XLS_ReadInt32_0
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6E)]
        public Int32 fieldMissile;//index                           // 84   0x0000000000000A20  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillHitUnit;//index                           // 391  0x0000000000000A24  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillHitBackground;//index                     // 392  0x0000000000000A28  XLS_ReadIndex
        Int32 skillMissed1;                                         // 393  0x0000000000000A2C  XLS_ReadIntArray,0F
        Int32 skillMissed2;                                         // 393  0x0000000000000A30
        Int32 skillMissed3;                                         // 393  0x0000000000000A34
        Int32 skillMissed4;                                         // 393  0x0000000000000A38
        Int32 skillMissed5;                                         // 393  0x0000000000000A3C
        Int32 skillMissed6;                                         // 393  0x0000000000000A40
        Int32 skillMissed7;                                         // 393  0x0000000000000A44
        Int32 skillMissed8;                                         // 393  0x0000000000000A48
        Int32 skillMissed9;                                         // 393  0x0000000000000A4C
        Int32 skillMissed10;                                        // 393  0x0000000000000A50
        Int32 skillMissed11;                                        // 393  0x0000000000000A54
        Int32 skillMissed12;                                        // 393  0x0000000000000A58
        Int32 skillMissed13;                                        // 393  0x0000000000000A5C
        Int32 skillMissed14;                                        // 393  0x0000000000000A60
        Int32 skillMissed15;                                        // 393  0x0000000000000A64
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes20;                                      // 0x0000000000000A68 to 0x0000000000000A6C
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillOnFuse;//index                            // 394  0x0000000000000A6C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillOnDamageRepeat;//index                    // 395  0x0000000000000A70  XLS_ReadIndex
        public Int32 startingskills1;                               // 234  0x0000000000000A74  XLS_ReadIntArray,0C
        public Int32 startingskills2;                               // 234  0x0000000000000A78
        public Int32 startingskills3;                               // 234  0x0000000000000A7C
        public Int32 startingskills4;                               // 234  0x0000000000000A80
        public Int32 startingskills5;                               // 234  0x0000000000000A84
        public Int32 startingskills6;                               // 234  0x0000000000000A88
        public Int32 startingskills7;                               // 234  0x0000000000000A8C
        public Int32 startingskills8;                               // 234  0x0000000000000A90
        public Int32 startingskills9;                               // 234  0x0000000000000A94
        public Int32 startingskills10;                              // 234  0x0000000000000A98
        public Int32 startingskills11;                              // 234  0x0000000000000A9C
        public Int32 startingskills12;                              // 234  0x0000000000000AA0
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 unitDieBeginSkill;//index                      // 235  0x0000000000000AA4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 unitDieBeginSkillClient;//index                // 236  0x0000000000000AA8  XLS_ReadIndex
        [ExcelOutput(IsIntOffset = true)]
        public Int32 scriptOnUnitDieEnd;//intptr                    // 237  0x0000000000000AAC  XLS_ReadIntPtr
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 unitDieEndSkill;//index                        // 238  0x0000000000000AB0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 deadSkill;//index                              // 239  0x0000000000000AB4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 kickSkill;//index                              // 240  0x0000000000000AB8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 rightSkill;//index                             // 241  0x0000000000000ABC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 initSkill;//index                              // 242  0x0000000000000AC0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillLevelActive;//index                       // 243  0x0000000000000AC4  XLS_ReadIndex
        public Int32 initState1;                                    // 244  0x0000000000000AC8  XLS_ReadIntArray,4
        public Int32 initState2;                                    // 244  0x0000000000000ACC
        public Int32 initState3;                                    // 244  0x0000000000000AD0
        public Int32 initState4;                                    // 244  0x0000000000000AD4
        public Int32 initStateTicks;                                // 245  0x0000000000000AD8  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x49)]
        public Int32 dyingState;//index                             // 246  0x0000000000000ADC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x29)]
        public Int32 skillWhenUsed;//index                          // 247  0x0000000000000AE0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab;//index                               // 249  0x0000000000000AE4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab2;//index                              // 250  0x0000000000000AE8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab3;//index                              // 251  0x0000000000000AEC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab4;//index                              // 252  0x0000000000000AF0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab5;//index                              // 253  0x0000000000000AF4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab6;//index                              // 254  0x0000000000000AF8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x26)]
        public Int32 SkillTab7;//index                              // 255  0x0000000000000AFC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x49)]
        public Int32 levelUpState;//index                           // 263  0x0000000000000B00  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 sounds;//index                                 // 264  0x0000000000000B04  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 flippySound;//index                            // 265  0x0000000000000B08  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 invPickUpSound;//index                         // 266  0x0000000000000B0C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 invPutDownSound;//index                        // 267  0x0000000000000B10  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 invEquipSound;//index                          // 268  0x0000000000000B14  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 pickUpSound;//index                            // 269  0x0000000000000B18  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 useSound;//index                               // 270  0x0000000000000B1C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantUseSound;//index                           // 271  0x0000000000000B20  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantFireSound;//index                          // 272  0x0000000000000B24  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 blockSound;//index                             // 273  0x0000000000000B28  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 getHit0;//index                                // 274  0x0000000000000B2C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 getHit1;//index                                // 275  0x0000000000000B30  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 getHit2;//index                                // 276  0x0000000000000B34  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 getHit3;//index                                // 277  0x0000000000000B38  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 getHit4;//index                                // 278  0x0000000000000B3C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 getHit5;//index                                // 279  0x0000000000000B40  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 interactSound;//index                          // 280  0x0000000000000B44  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 damagingSound;//index                          // 281  0x0000000000000B48  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x3F)]
        public Int32 forwardFootStepLeft;//index                    // 282  0x0000000000000B4C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x3F)]
        public Int32 forwardFootStepRight;//index                   // 283  0x0000000000000B50  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x3F)]
        public Int32 backwardFootStepLeft;//index                   // 284  0x0000000000000B54  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x3F)]
        public Int32 backwardFootStepRight;//index                  // 285  0x0000000000000B58  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x3F)]
        public Int32 firstPersonJumpFootStep;//index                // 286  0x0000000000000B5C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x3F)]
        public Int32 firstPersonLandFootStep;//index                // 287  0x0000000000000B60  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 outOfManaSound;//index                         // 288  0x0000000000000B64  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 inventoryFullSound;//index                     // 289  0x0000000000000B68  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantSound;//index                              // 292  0x0000000000000B6C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 CantUseSound;//index                           // 290  0x0000000000000B70  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantUseYetSound;//index                        // 291  0x0000000000000B74  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantCastSound;//index                          // 293  0x0000000000000B78  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantCastYetSound;//index                       // 294  0x0000000000000B7C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
        public Int32 cantCastHereSound;//index                      // 295  0x0000000000000B80  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x14)]
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
        public float walkAndRunDelta;                               // 342  0x0000000000000C74  XLS_ReadFloat
        public Int32 rangeMin;                                      // 326  0x0000000000000C78  XLS_ReadInt32
        public Int32 rangeMax;                                      // 327  0x0000000000000C7C  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 decoyMonster;//index                           // 415  0x0000000000000C80  XLS_ReadIndex
        public Int32 havokShape;                                    // 374  0x0000000000000C84  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6E)]
        public Int32 missileHitUnit;//index                         // 388  0x0000000000000C88  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6E)]
        public Int32 missileHitBackground;//index                   // 389  0x0000000000000C8C  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x6E)]
        public Int32 missileOnFreeOrFuse;//index                    // 390  0x0000000000000C90  XLS_ReadIndex
        public Int32 missileTag;                                    // 400  0x0000000000000C94  XLS_ReadInt32
        public Int32 damageRepeatRate;                              // 396  0x0000000000000C98  XLS_ReadInt32
        public Int32 damageRepeatChance;                            // 397  0x0000000000000C9C  XLS_ReadInt32
        public Int32 repeatDamageImmediately;                       // 398  0x0000000000000CA0  XLS_ReadInt32
        public Int32 serverSrcDamage;                               // 399  0x0000000000000CA4  XLS_ReadInt32
        public Int32 blockGhosts;                                   // 401  0x0000000000000CA8  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x73)]
        public Int32 monster;//index                                // 402  0x0000000000000CAC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x76)]
        public Int32 triggerType;//index                            // 403  0x0000000000000CB0  XLS_ReadIndex
        public Int32 operatorStatesTriggerProhibited;               // 404  0x0000000000000CB4  XLS_ReadIntArray,01
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x68)]
        public Int32 sublevelDest;//index                           // 405  0x0000000000000CB8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x99)]
        public Int32 operateRequiredQuest;//index                   // 406  0x0000000000000CBC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x9A)]
        public Int32 operateRequiredQuestState;//index              // 407  0x0000000000000CC0  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x97)]
        public Int32 operateRequiredQuestStateValue;//index         // 408  0x0000000000000CC4  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x97)]
        public Int32 operateProhibitedQuestStateValue;//index       // 409  0x0000000000000CC8  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 requiresItemOfUnitType1;//index                // 66   0x0000000000000CCC  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x65)]
        public Int32 spawnTreasureClassInLevel;//index              // 67   0x0000000000000CD0  XLS_ReadIndex
        public Int32 oneWayVisualPortalDir;                         // 410  0x0000000000000CD4  XLS_ReadInt32_0
        public Int32 warpResolveTime;                               // 473  0x0000000000000CD8  XLS_ReadInt32_0
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
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x08)]
        public Int32 colorSet;//index                               // 31   0x0000000000000CFC  XLS_ReadIndex
        public Int32 corpseExplodePoints;                           // 471  0x0000000000000D00  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x17)]
        public Int32 requiredAttackerUnitType;//index               // 472  0x0000000000000D04  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x84)]
        public Int32 startingLevelArea;//index                      // 474  0x0000000000000D08  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x84)]
        public Int32 warpToLevelArea;//index                        // 476  0x0000000000000D0C  XLS_ReadIndex
        public Int32 warpToFloor;                                   // 477  0x0000000000000D10  XLS_ReadInt32
        [ExcelOutput(IsTableIndex = true, TableIndex = 0xA7)]
        public Int32 globalThemeRequired;//index                    // 478  0x0000000000000D14  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x55)]
        public Int32 levelThemeRequired;//index                     // 479  0x0000000000000D18  XLS_ReadIndex
        [ExcelOutput(IsTableIndex = true, TableIndex = 0x21)]
        public Int32 modeGroupOnClient;//index                      // 475  0x0000000000000D1C  XLS_ReadIndex
        public Int32 null1;                                         // 455  0x0000000000000D20  XLS_ReadInt32
        public Int32 null2;                                         // 456  0x0000000000000D24  XLS_ReadInt32
        public Int32 null3;                                         // 457  0x0000000000000D28  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)]
        byte[] unknownBytes26;                                      // 0x0000000000000D2C to 0x0000000000000D84
        public Int32 null4;                                         // 458  0x0000000000000D84  XLS_ReadInt32
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        byte[] unknownBytes27;                                      // 0x0000000000000D88 to 0x0000000000000DB8

        public abstract class Items
        {
            [FlagsAttribute]
            public enum BitMask01 : uint
            {
                spawn = 1,                                          // 472,1, 00
                spawnAtMerchant = 2,                                // 472,2, 01
                forceIgnoresScale = 4,                              // 472,3, 02
                impactOnFuse = 8,                                   // 472,4, 03
                impactOnFree = 16,                                  // 472,5, 04
                impactOnHitUnit = 32,                               // 472,6, 05
                impactOnHitBackground = 64,                         // 472,7, 06
                havokIgnoresDirection = 128,                        // 472,8, 07
                damagesOnFuse = 256,                                // 472,9, 08
                hitsUnits = 512,                                    // 472,10, 09
                killOnUnitHit = 1024,                               // 472,11, 0A
                hitsBackground = 2048,                              // 472,12, 0B
                noRayCollision = 4096,                              // 472,13, 0C
                killOnBackground = 8192,                            // 472,14, 0D
                stickOnHit = 16384,                                 // 472,15, 0E
                stickOnInit = 32768,                                // 472,16, 0F
                sync = 65536,                                       // 472,20, 10
                clientOnly = 131072,                                // 472,21, 11
                serverOnly = 262144,                                // 472,22, 12
                useSourceVel = 524288,                              // 472,23, 13
                mustHit = 1048576,                                  // 472,24, 14
                prioritizeTarget = 2097152,                         // 472,26, 15
                trailEffectsUseProjectile = 4194304,                // 472,27, 16
                impactEffectsUseProjectile = 8388608,               // 472,28, 17
                destroyOtherMissiles = 16777216,                    // 472,29, 18
                dontHitSkillTarget = 33554432,                      // 472,30, 19
                flipFaceDirection = 67108864,                       // 472,31, 1A
                dontUseRangeForSkill = 134217728,                   // 472,32, 1B
                pullsTarget = 268435456,                            // 472,33, 1C
                damagesOnHitUnit = 536870912,                       // 472,18, 1D
                pulsesStatsOnHitUnit = 1073741824,                  // 472,19, 1E
                damagesOnHitBackground = 2147483648                 // 472,17, 1F
            }

            [FlagsAttribute]
            public enum BitMask02 : uint
            {
                alwaysCheckForCollisions = 1,                       // 472,34, 20
                setShapePercentages = 2,                            // 472,36, 21
                useSourceAppearance = 4,                            // 472,35, 22
                dontTransferRidersFromOwner = 8,                    // 472,37, 23
                dontTransferDamagesOnClient = 16,                   // 472,38, 24
                missileIgnorePostLaunch = 32,                       // 472,40, 25
                attacksLocationOnHitUnit = 64,                      // 472,41, 26
                dontDeactivateWithRoom = 128,                       // 472,42, 27
                angerOthersOnDamaged = 256,                         // 472,44, 28
                angerOthersOnDeath = 512,                           // 472,45, 29
                alwaysFaceSkillTarget = 1024,                       // 472,46, 2A
                setRopeEndWithNoTarget = 2048,                      // 472,47, 2B
                forceDrawDirectionToMoveDirection = 4096,           // 472,48, 2C
                questNameColor = 8192,                              // 472,49, 2D
                doNotSortWeapons = 16384,                           // 472,50, 2E
                ignoresEquipClassReqs = 32768,                      // 472,51, 2F
                doNotUseSorceForToHit = 65536,                      // 472,52, 30
                angleWhilePathing = 131072,                         // 472,53, 31
                dontAddWardrobeLayer = 262144,                      // 472,54, 32
                dontUseContainerAppearance = 524288,                // 472,55, 33
                subscriberOnly = 1048576,                           // 472,56, 34
                computeLevelRequirement = 2097152,                  // 472,57, 35
                dontFattenCollision = 4194304,                      // 472,58, 36
                automapSave = 8388608,                              // 472,59, 37
                requiresCanOperateToBeKnown = 16777216,             // 472,60, 38
                forceFreeOnRoomReset = 33554432,                    // 472,61, 39
                canReflect = 67108864,                              // 472,62, 3A
                selectTargetIgnoresAimPos = 134217728,              // 472,63, 3B
                canMeleeAboveHeight = 268435456,                    // 472,64, 3C
                getFlavorTextFromQuest = 536870912,                 // 318,1, 3D
                unIdentifiedNameFromBaseRow = 1073741824,           // 56,1, 3E
                noRandomProperName = 2147483648                     // 56,2, 3F
            }

            [FlagsAttribute]
            public enum BitMask03 : uint
            {
                noNameModifications = 1,                            // 56,3, 40
                preLoad = 2,                                        // 58,1, 41
                ignoreInDat = 4,                                    // 58,2, 42
                ignoreSavedStates = 8,                              // 58,3, 43
                drawUsingCutUpWardrobe = 16,                        // 56,4, 44
                isGood = 32,                                        // 58,4, 45
                isNpc = 64,                                         // 58,5, 46
                canNotBeMoved = 128,                                // 58,7, 47
                noLevel = 256,                                      // 106,1, 48
                usesSkills = 512,                                   // 57,3, 49
                autoPickup = 1024,                                  // 85,7, 4A
                trigger = 2048,                                     // 57,2, 4B
                dieOnClientTrigger = 4096,                          // 57,1, 4C
                neverDestroyDead = 8192,                            // 57,4, 4D
                collideWhenDead = 16384,                            // 57,5, 4E
                startDead = 32768,                                  // 57,6, 4F
                givesLoot = 65536,                                  // 57,7, 50
                dontTriggerByProximity = 131072,                    // 57,8, 51
                triggerOnEnterRoom = 262144,                        // 57,10, 52
                destructible = 524288,                              // 57,11, 53
                inAir = 1048576,                                    // 58,8, 54
                wallWalk = 2097152,                                 // 58,9, 55
                startInTownIdle = 4194304,                          // 87,1, 56
                onDieDestroy = 8388608,                             // 69,1, 57
                onDieEndDestroy = 16777216,                         // 69,2, 58
                onDieHideModel = 33554432,                          // 69,3, 59
                selectableDeadOrDying = 67108864,                   // 69,4, 5A
                interactive = 134217728,                            // 73,1, 5B
                hideDialogHead = 268435456, // 28                   // 73,2, 5C
                collideBad = 536870912,                             // 78,9, 5D
                collideGood = 1073741824,                           // 78,2, 5E
                modesIgnoreAI = 2147483648                          // 78,8, 5F
            }

            [FlagsAttribute]
            public enum BitMask04 : uint
            {
                dontPath = 1,                                       // 78,3, 60
                snapToPathnodeOnCreate = 2,                         // 78,4, 61
                unTargetable = 4,                                   // 78,5, 62
                FaceDuringInteraction = 8,                          // 78,6, 63
                noSync = 16,                                        // 78,7, 64
                canNotTurn = 32,                                    // 58,11, 65
                turnNeckInsteadOfBody = 64,                         // 58,12, 66
                merchant = 128,                                     // 58,13, 67
                merchantSharedInventory = 256,                      // 58,14, 68
                trader = 512,                                       // 61,1, 69
                tradesman = 1024,                                   // 61,6, 6A
                gambler = 2048,                                     // 61,7, 6B
                mapVendor = 4096,                                   // 61,8, 6C
                godQuestMessanger = 8192,                           // 61,11, 6D
                trainer = 16384,                                    // 61,9, 6E
                healer = 32768,                                     // 61,2, 6F
                graveKeeper = 65536,                                // 61,3, 70
                taskGiver = 131072,                                 // 61,4, 71
                canUpgradeItems = 262144,                           // 63,1, 72
                canAugmentItems = 524288,                           // 63,2, 73
                autoIdentifiesInventory = 1048576,                  // 63,3, 74
                npcDungeonWarp = 2097152,                           // 63,6, 75
                PvPSignerUpper = 4194304,                           // 63,7, 76
                foreman = 8388608,                                  // 63,8, 77
                transporter = 16777216,                             // 61,10, 78
                showsPortrait = 33554432,                           // 470,1, 79
                petGetsStatPointsPerLevel = 67108864,               // 470,2, 7A
                ignoresSkillPowerCost = 134217728,                  // 57,9, 7B
                checkRadiusWhenPathing = 268435456,                 // 87,2, 7C
                checkHeightWhenPathing = 536870912,                 // 87,3, 7D
                questImportantInfo = 1073741824,                    // 63,9, 7E
                ignoresToHit = 2147483648                           // 85,6, 7F
            }

            [FlagsAttribute]
            public enum BitMask05 : uint
            {
                askQuestsForOperate = 1,                            // 63,10, 80
                askFactionForOperate = 2,                           // 63,13, 81
                askPvPCensorshipForOperate = 4,                     // 63,14, 82
                structural = 8,                                     // 63,15, 83
                askQuestsForKnown = 16,                             // 63,11, 84
                askQuestsForVisible = 32,                           // 63,12, 85
                informQuestsOnInit = 64,                            // 63,16, 86
                informQuestsOfLootDrop = 128,                       // 63,17, 87
                informQuestsOnDeath = 256,                          // 63,18, 88
                noTrade = 512,                                      // 64,1, 89
                flagRoomAsNoSpawn = 1024,                           // 67,2, 8A
                monitorPlayerApproach = 2048,                       // 67,3, 8B
                monitorApproachClearLOS = 4096,                     // 68,1, 8C
                canFizzle = 8192,                                   // 73,3, 8D
                inheritsDirection = 16384,                          // 73,4, 8E
                canNotBeDismantled = 32768,                         // 226,1, 8F
                canNotBeUpgraded = 65536,                           // 226,2, 90
                canNotBeAugmented = 131072,                         // 226,3, 91
                canNotBeDeModded = 262144,                          // 226,4, 92
                ignoreSellWithInventoryConfirm = 524288,            // 222,1, 93
                wardrobePerUnit = 1048576,                          // 117,1, 94
                wardrobeSharesModelDef = 2097152,                   // 117,3, 95
                noWeaponModel = 4194304,                            // 117,2, 96
                _undefined1 = 8388608,
                noDrop = 16777216,                                  // 259,1, 98
                noDropExceptForDuplicates = 33554432,               // 259,2, 99
                askQuestsForPickup = 67108864,                      // 259,3, 9A
                informQuestsOnPickup = 134217728,                   // 259,4, 9B
                examinable = 268435456,                             // 260,1, 9C
                informQuestsToUse = 536870912,                      // 260,3, 9D
                consumeWhenUsed = 1073741824,                       // 260,4, 9E
                immuneToCritical = 2147483648                       // 176,1, 9F
            }

            [FlagsAttribute]
            public enum BitMask06 : uint
            {
                NoRandomAffixes = 1,                                // 220,1, A0
                canBeChampion = 2,                                  // 104,1, A1
                noQualityDowngrade = 4,                             // 125,1, A2
                noDrawOnInit = 8,                                   // 255,1, A3
                mustFaceMeleeTarget = 16,                           // 320,1, A4
                dontDestroyIfVelocityIsZero = 32,                   // 341,1, A5
                ignoreInteractDistance = 64,                        // 472,65, A6
                operateRequiresGoodQuestStatus = 128,               // 406,1, A7
                reverseArriveDirection = 256,                       // 410,1, A8
                faceAfterWarp = 512,                                // 410,2, A9
                neverAStartLocation = 1024,                         // 410,3, AA
                alwaysShowLabel = 2048,                             // 412,1, AB
                _undefined2 = 4096,//12
                null0 = 8192,                                       // 454,1, AD
                _undefined3 = 16384,//14
                isNonweaponMissile = 32768,                         // 472,39, AF
                cullByScreensize = 65536,                           // 472,66, B0
                linkWarpDestByLevelType = 131072,                   // 472,68, B1
                isBoss = 262144,                                    // 472,67, B2
                _undefined4 = 524288,//19
                takeResponsibilityOnKill = 1048576,                 // 472,69, B4
                alwaysKnownForSounds = 2097152,                     // 472,70, B5
                ignoreTargetOnRepeatDmg = 4194304,                  // 472,25, B6
                bindToLevelArea = 8388608,                          // 64,2, B7
                dontCollideWithDestructibles = 16777216,            // 472,71, B8
                blocksEverything = 33554432,                        // 472,72, B9
                everyoneCanTarget = 67108864,                       // 472,73, BA
                missilePlotArc = 134217728,                         // 472,74, BB
                petDiesOnWarp = 268435456,                          // 470,3, BC
                missileIsGore = 536870912,                          // 472,75, BD
                canAttackFriends = 1073741824,                      // 78,1, BE
                ignoreItemRequirements = 2147483648                 // 141,1, BF
            }

            [FlagsAttribute]
            public enum BitMask07 : uint
            {
                lowLodInTown = 1,                                   // 472,76, C0
                treasureClassBeforeRoom = 2,                        // 67,1, C1
                taskGiverNoStartingIcon = 4,                        // 61,5, C2
                assignGUID = 8,                                     // 471,1, C3
                merchantDoesNotRefresh = 16,                        // 59,1, C4
                dontDepopulate = 32,                                // 472,43, C5
                dontShrinkBones = 64,                               // 472,77, C6
                _undefined4 = 128,//7
                hasQuestInfo = 256,                                 // 58,6, C8
                multiplayerOnly = 512,                              // 472,81, C9
                noSpin = 1024,                                      // 260,2, CA
                npcGuildMaster = 2048,                              // 63,4, CB
                autoIdentifyAffixs = 4096,                          // 472,78, CC
                npcRespeccer = 8192,                                // 63,5, CD
                allowObjectStepping = 16384,                        // 472,79, CE
                alwaysUseFallback = 32768,                          // 115,1, CF
                canNotSpawnRandomLevelTreasure = 65536,             // 472,80, D0
                xferMissileStats = 131072,                          // 472,82, D1
                specificToDifficulty = 262144,                      // 472,83, D2
                isFieldMissile = 524288,                            // 472,84, D3
                ignoreFuseMsStat = 1048576,                         // 472,85, D4
                usesPetLevel = 2097152                              // 58,10, D5
            }

            //these are probably only for missiles.t.c
            [FlagsAttribute]
            public enum BitMask08 : uint
            {
                bounceOnUnitHit = 1,                                // 85,1, 0
                bounceOnBackGroundHit = 2,                          // 85,2, 1
                newDirectionOnBounce = 4,                           // 85,3, 2
                canNotRicochet = 8,                                 // 85,4, 3
                reTargetOnBounce = 16                               // 85,5, 4
            }
        }
    }
}