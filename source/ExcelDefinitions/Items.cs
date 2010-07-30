using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ItemsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(IsStringOffset = true)]
        public Int32 name;//pchar: pointer to relevant text in the front of the file.
        Int32 buffer1;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 folder;//pchar
        Int32 buffer2;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearance;//pchar
        Int32 buffer3;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 appearenceFirst;//pchar
        Int32 buffer4;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 icon;//pchar
        Int32 buffer5;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 holyRadius;//pchar
        Int32 buffer6;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tinyHitParticle;//pchar
        Int32 buffer7;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lightHitParticle;//pchar
        Int32 buffer8;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mediumHitParticle;//pchar
        Int32 buffer9;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 hardHitParticle;//pchar
        Int32 buffer10;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 killedParticle;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] buffer11;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 fizzleParticle;//pchar
        Int32 buffer12;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 reflectParticle;//pchar
        Int32 buffer13;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 restoreVitalsParticle;//pchar
        Int32 buffer14;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 diffuse;//pchar
        Int32 buffer15;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 normal;//pchar
        Int32 buffer16;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 specular;//pchar
        Int32 buffer17;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 lightmap;//pchar
        Int32 buffer18;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource1;//pchar
        Int32 buffer19;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest1;//pchar
        Int32 buffer20;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource2;//pchar
        Int32 buffer21;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest2;//pchar
        Int32 buffer22;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource3;//pchar
        Int32 buffer23;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest3;//pchar
        Int32 buffer24;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource4;//pchar
        Int32 buffer25;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest4;//pchar
        Int32 buffer26;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideSource5;//pchar
        Int32 buffer27;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 overrideDest5;//pchar
        Int32 buffer28;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 particleFolder;//pchar
        Int32 buffer29;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 pickupFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] buffer30;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 triggerString1;//pchar
        Int32 buffer31;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 triggerString2;//pchar
        Int32 buffer32;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 charSelectFont;//pchar
        Int32 buffer33;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 tooltipDamageIcon;//pchar
        Int32 buffer34;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 damagingMeleeParticle;//pchar
        Int32 buffer35;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 muzzleFlash;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown02;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfDefault;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown03;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfPhysical;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown04;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfFire;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown05;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfElectric;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown06;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfSpectral;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown07;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfToxic;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown08;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 mfPoison;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown09;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trail_RopeNoTarget;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown10;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailDefault;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown11;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailPhysical;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown12;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailFire;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown13;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailElectric;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown14;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailSpectral;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown15;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailToxic;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown16;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 trailPoison;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown17;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projectile_RopeWithTarget;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown18;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projDefault;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown19;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projPhysical;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown20;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projFire;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown21;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projElectric;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown22;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projSpectral;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown23;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projToxic;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown24;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 projPoison;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown25;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactWall;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown26;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallDefault;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown27;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallPhysical;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown28;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallFire;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown29;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallElectric;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown30;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallSpectral;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown31;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impWallToxic;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown32;
        //[ExcelOutput(IsStringOffset = true)]
        private Int32 impWallPoison;//pchar // unknown!!
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknown33;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnit;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown34;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitDefault;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown35;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitPhysical;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown36;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitFire;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown37;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitElectric;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown38;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitSpectral;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        byte[] unknown39;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 impactUnitToxic;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        byte[] unknown40;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask01 bitmask01;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask02 bitmask02;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask03 bitmask03;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask04 bitmask04;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask05 bitmask05;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask06 bitmask06;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public Items.BitMask07 bitmask07;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 String;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 typeDescription;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 flavorText;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 additionalDescription;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 additionalRaceDescription;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 analyze;
        public Int32 recipeList;//index
        public Int32 recipeSingleUse;//index
        public Int32 paperdollBackgroundLevel;//index
        public Int32 paperdollWeapon1;
        public Int32 paperdollWeapon2;
        public Int32 paperdollSkill;//index
        public Int32 paperdollColorset;//index
        public Int32 respawnChance;
        public Int32 respawnSpawnclass;//index
        public Int32 code1;
        public Int32 unknown41;
        public Int32 densityValueOverride;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 minionPackSize;//intptr: points to the last section of the file.
        public float spinSpeed;
        public float maxTurnRate;
        public Int32 unitType;//index
        public Int32 unitTypeForLeveling;//index
        public Int32 preferedByUnitType;//index
        public Int32 family;//index
        public Int32 censorClassNoHumans;//index
        public Int32 censorClassNoGore;//index
        public Int32 sex;
        public Int32 race;//index
        public Int32 rarity;
        public Int32 spawnChance;
        public Int32 minMonsterExperienceLevel;
        public Int32 level;
        public Int32 monsterQuality;//index
        public Int32 monsterClassAtUniqueQuality;//index
        public Int32 monsterClassAtChampionQuality;//index
        public Int32 minionClass;//index
        public Int32 monsterNameType;//index
        public Int32 quality;//index
        public Int32 angerRange;
        public Int32 baseLevel;
        public Int32 capLevel;
        public Int32 minMerchantLevel;
        public Int32 maxMerchantLevel;
        public Int32 minSpawnLevel;
        public Int32 maxSpawnLevel;
        public Int32 maxLevel;
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 1)]
        public Int32 fixedLevel;//intptr
        public Int32 hpMin;
        public Int32 hpMax;
        public Int32 powerMax;
        public Int32 experience;
        public Int32 attackRating;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 luckBonus;//intptr
        public Int32 luckChanceToSpawn;
        public Int32 roomPopulatePass;
        public Int32 weaponBoneIndex;
        public Int32 requiresAffixOrSuffix;
        public float autoPickupDistance;
        public Int32 pickupPullState;//index
        public float extraDyingTimeInSeconds;
        public Int32 npcInfo;//index
        public Int32 balanceTestCount;
        public Int32 balanceTestGroup;
        public Int32 merchantStartingPane;
        public Int32 merchantFactionType;//index
        public Int32 merchantFactionValueNeeded;
        public Int32 questRequirement;//index
        public float noSpawnRadius;
        public float monitorApproachRadius;
        public Int32 tasksGeneratedStat;//index
        public float serverMissileOffset;
        public float homingTurnAngleRadians;
        public float homingModBasedDis;
        public float homeAfterUnitRadius;
        public float collidableAfterXSeconds;
        public float homeAfterXSeconds;
        public Int32 bitmask08;
        public float impactCameraShakeDuration;
        public float impactCameraShakeMagnitude;
        public float impactCameraShakeDegrade;
        public Int32 maximumImpactFrequency;
        public Int32 onlyCollideWithUnitType;//index
        public Int32 questDescription;//index
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 6)]
        public Int32 pickUpCondition;//intptr
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes04;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 scriptOnUse;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 17)]
        public Int32 stackSize;//intptr
        public Int32 maxPickUp;
        public Int32 baseCost;
        public Int32 realWorldCost;
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 4)]
        public Int32 buyPriceMult;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 5)]
        public Int32 buyPriceAdd;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 2)]
        public Int32 sellPriceMult;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 3)]
        public Int32 sellPriceAdd;//intptr
        public Int32 inventoryWardrobe;//index
        public Int32 characterScreenWardrobe;      // always -1
        public Int32 characterScreenState;      // always -1
        public Int32 wardrobeBody;      // always -1
        public Int32 wardrobeFallback;      // always -1
        public Int32 wardrobeUnkown1;      // always -1
        public Int32 wardrobeMip;      // always 0
        public Int32 wardrobeAppearanceGroup;      // always -1
        public Int32 wardrobeAppearanceGroup1st;      // always -1
        public Int32 startingStance;      // always -1
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknownBytes06;
        public Int32 containerUnitType1;
        public Int32 containerUnitType2;
        public Int32 containerUnitType3;      // always 0 but we could use them
        public Int32 containerUnitType4;      // always 0
        public Int32 firingErrorIncrease;
        public Int32 firingErrorDecrease;
        public Int32 firingErrorMax;
        public Int32 accuracyBase;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes07;
        public Int32 refillHotKey;//index
        public Int32 animGroup;//index
        public Int32 meleeWeapon;//index
        public Int32 cdTicks;
        public float approxDps;
        public Int32 tooltipDamageString;//stridx
        public Int32 requiredAffixGroups1;
        public Int32 requiredAffixGroups2;
        public Int32 requiredAffixGroups3;
        public Int32 requiredAffixGroups4;
        public Int32 requiredAffixGroups5;
        public Int32 requiredAffixGroups6;
        public Int32 requiredAffixGroups7;
        public Int32 requiredAffixGroups8;
        public Int32 spawnMonsterClass;//index
        public Int32 safeState;//index
        public Int32 skillGhost;//index
        public Int32 skillRef;//index
        public Int32 dmgType;//index
        public Int32 weaponDamageScale;
        public Int32 dontUseWeaponDamage;
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 7)]
        public Int32 minBaseDmg;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 8)]
        public Int32 maxBaseDmg;//intptr
        public Int32 sfxAttackFocus;
        public Int32 sfxUknown1;
        public Int32 sfxUknown2;
        public Int32 sfxUknown3;
        public Int32 sfxUknown4;
        public Int32 sfxUknown5;
        public Int32 sfxPhysicalAbility;
        public Int32 sfxPhysicalDefense;
        public Int32 sfxPhysicalKnockbackInCm;
        public float sfxPhysicalStunDurationInSeconds;
        public Int32 sfxUknown6;
        public Int32 sfxFireAbility;
        public Int32 sfxFireDefense;
        public Int32 sfxFireDamagePercent1;
        public Int32 sfxFireDamagePercent2;
        public Int32 sfxFireDamagePercent3;
        public Int32 sfxElectricAbility;
        public Int32 sfxElectricDefense;
        public Int32 sfxElectricDamagePercentOfInitialPerShock;
        public float sfxElectricDurationInSeconds;
        public Int32 sfxUknown7;
        public Int32 sfxSpectralAbility;
        public Int32 sfxSpectralDefense;
        public Int32 sfxSpectralSpecial;
        public float sfxSpectralDurationInSeconds;
        public Int32 sfxUknown8;
        public Int32 sfxToxicAbility;
        public Int32 sfxToxicDefense;
        public Int32 sfxToxicDamageAsPercentageOfDamageDeliveredFromAttack;
        public float sfxToxicDurationInSeconds;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        byte[] unknownBytes12;
        public Int32 sfxPoisonAbility;
        public Int32 sfxPoisonDefense;
        public Int32 sfxPoisonDamageAsPercentageOfDamageDeliveredFromAttack;
        public float sfxPoisonDurationInSeconds;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        byte[] unknownBytes13;
        public Int32 dmgIncrement;
        public Int32 radialDmgIncrement;
        public Int32 fieldDmgIncrement;
        public Int32 dotDmgIncrement;
        public Int32 aiChangerDmgIncrement;
        public Int32 toHit;
        public Int32 criticalPct;
        public Int32 criticalMult;
        public Int32 staminaDrainChancePercent;
        public Int32 staminaDrain;
        public Int32 interruptAttackPct;
        public Int32 interruptDefensePct;
        public Int32 interruptChanceOnAnyHit;
        public Int32 stealthDefensePct;
        public Int32 stealthAttackPct;
        public Int32 aiChangeDefense;
        public Int32 armor;
        public Int32 armorPhys;
        public Int32 armorFire;
        public Int32 armorElec;
        public Int32 armorSpec;
        public Int32 armorToxic;
        public Int32 armorUnknown1;
        public Int32 armorPoison;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes15;
        public Int32 maxArmor;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
        byte[] unknownBytes16;
        public Int32 shields;
        public Int32 shieldPhys;
        public Int32 shieldFire;
        public Int32 shieldElec;
        public Int32 shieldSpec;
        public Int32 shieldToxic;
        public Int32 shieldUnknown1;
        public Int32 shieldPoison;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknownBytes18;
        public Int32 strengthPercent;
        public Int32 dexterityPercent;
        public Int32 startingAccuracy;
        public Int32 startingStrength;
        public Int32 startingStamina;
        public Int32 startingWillpower;
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 9)]
        public Int32 props1;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 10)]
        public Int32 props2;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 11)]
        public Int32 props3;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 12)]
        public Int32 props4;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 16)]
        public Int32 props5;//intptr
        public Int32 props1AppliesToUnitype;//index
        public Int32 props2AppliesToUnitype;//index
        public Int32 props3AppliesToUnitype;//index
        public Int32 props4AppliesToUnitype;//index
        public Int32 props5AppliesToUnitype;//index
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 13)]
        public Int32 perLevelProps1;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 14)]
        public Int32 perLevelProps2;//intptr
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 15)]
        public Int32 propsElite;//intptr
        public Int32 affix1;//index
        public Int32 affix2;//index
        public Int32 affix3;//index
        public Int32 affix4;//index
        public Int32 affix5;//index
        public Int32 affix6;//index
        public Int32 treasure;//index
        public Int32 championTreasure;//index
        public Int32 firstTimeTreasure;//index
        public Int32 inventory;//index
        public Int32 recipeIngredientInvLoc;//index
        public Int32 recipeResultInvLoc;//index
        public Int32 startingTreasure;//index
        public Int32 invWidth;
        public Int32 invHeight;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknownBytes19;
        public Int32 requiredQuality;//index
        public Int32 qualityName;//index
        public Int32 fieldMissile;//index
        public Int32 skillHitUnit;//index
        public Int32 skillHitBackground;//index
        public Int32 skillMissed1;
        public Int32 skillMissed2;
        public Int32 skillMissed3;
        public Int32 skillMissed4;
        public Int32 skillMissed5;
        public Int32 skillMissed6;
        public Int32 skillMissed7;
        public Int32 skillMissed8;
        public Int32 skillMissed9;
        public Int32 skillMissed10;
        public Int32 skillMissed11;
        public Int32 skillMissed12;
        public Int32 skillMissed13;
        public Int32 skillMissed14;
        public Int32 skillMissed15;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes20;
        public Int32 skillOnFuse;//index
        public Int32 skillOnDamageRepeat;//index
        public Int32 startingskills1;
        public Int32 startingskills2;
        public Int32 startingskills3;
        public Int32 startingskills4;
        public Int32 startingskills5;
        public Int32 startingskills6;
        public Int32 startingskills7;
        public Int32 startingskills8;
        public Int32 startingskills9;
        public Int32 startingskills10;
        public Int32 startingskills11;
        public Int32 startingskills12;
        public Int32 unitDieBeginSkill;//index
        public Int32 unitDieBeginSkillClient;//index
        [ExcelOutput(IsIntOffset = true)]
        public Int32 scriptOnUnitDieEnd;//intptr
        public Int32 unitDieEndSkill;//index
        public Int32 deadSkill;//index
        public Int32 kickSkill;//index
        public Int32 rightSkill;//index
        public Int32 initSkill;//index
        public Int32 skillLevelActive;//index
        public Int32 initState1;
        public Int32 initState2;
        public Int32 initState3;
        public Int32 initState4;
        public Int32 initStateTicks;
        public Int32 dyingState;//index
        public Int32 skillWhenUsed;//index
        public Int32 SkillTab;//index
        public Int32 SkillTab2;//index
        public Int32 SkillTab3;//index
        public Int32 SkillTab4;//index
        public Int32 SkillTab5;//index
        public Int32 SkillTab6;//index
        public Int32 SkillTab7;//index
        public Int32 levelUpState;//index
        public Int32 sounds;//index
        public Int32 flippySound;//index
        public Int32 invPickUpSound;//index
        public Int32 invPutDownSound;//index
        public Int32 invEquipSound;//index
        public Int32 pickUpSound;//index
        public Int32 useSound;//index
        public Int32 cantUseSound;//index
        public Int32 cantFireSound;//index
        public Int32 blockSound;//index
        public Int32 getHit0;//index
        public Int32 getHit1;//index
        public Int32 getHit2;//index
        public Int32 getHit3;//index
        public Int32 getHit4;//index
        public Int32 getHit5;//index
        public Int32 interactSound;//index
        public Int32 damagingSound;//index
        public Int32 forwardFootStepLeft;//index
        public Int32 forwardFootStepRight;//index
        public Int32 backwardFootStepLeft;//index
        public Int32 backwardFootStepRight;//index
        public Int32 firstPersonJumpFootStep;//index
        public Int32 firstPersonLandFootStep;//index
        public Int32 outOfManaSound;//index
        public Int32 inventoryFullSound;//index
        public Int32 cantSound;//index
        public Int32 CantUseSound;//index
        public Int32 cantUseYetSound;//index
        public Int32 cantCastSound;//index
        public Int32 cantCastYetSound;//index
        public Int32 cantCastHereSound;//index
        public Int32 lockedSound;//index
        public float pathingCollisionRadius;
        public float collisionRadius;
        public float collisionRadiusHorizontal;
        public float collisionHeight;
        public float blockingRadiusOverride;
        public float warpPlaneForwardMultiplier;
        public float warpOutDistance;
        public Int32 snapToAngleInDegrees;
        public float offsetUp;
        public float scale;
        public float weaponScale;
        public float scaleDelta;
        public float championScale;
        public float championScaleDelta;
        public float scaleMultiplier;
        public float ragdollForceMultiplier;
        public float meleeImpactOffset;
        public float meleeRangeMax;
        public float meleeRangeDesired;
        public float maxAutoMapRadius;
        public float fuse;
        public float clientMinimumLifetime;
        public float rangeBase;
        public float rangeDesiredMult;
        public float force;
        public float horizontalAccuracy;
        public float verticleAccuracy;
        public float hitBackup;
        public float jumpVelocity;
        public float velocityForImpact;
        public float acceleration;
        public float bounce;
        public float dampening;
        public float friction;
        public float missileArcHeight;
        public float missileArcDelta;
        public float spawnRandomizeLengthPercent;
        public Int32 stopAfterXNumOfTicks;
        public float walkSpeed;
        public float walkMin;
        public float walkMax;
        public float strafeSpeed;
        public float strafeMin;
        public float strafeMax;
        public float jumpSpeed;
        public float jumpMin;
        public float jumpMax;
        public float runSpeed;
        public float runMin;
        public float runMax;
        public float backupSpeed;
        public float backupMin;
        public float backupMax;
        public float knockBackSpeed;
        public float knockBackMin;
        public float knockBackMax;
        public float meleeSpeed;
        public float meleeMin;
        public float meleeMax;
        public float walkAndRunDelta;
        public float rangeMin;
        public float rangeMax;
        public Int32 decoyMonster;//index
        public Int32 havokShape;
        public Int32 missileHitUnit;//index
        public Int32 missileHitBackground;//index
        public Int32 missileOnFreeOrFuse;//index
        public Int32 missileTag;
        public Int32 damageRepeatRate;
        public Int32 damageRepeatChance;
        public Int32 repeatDamageImmediately;//bool
        public Int32 serverSrcDamage;
        public Int32 blockGhosts;//bool
        public Int32 monster;//index
        public Int32 triggerType;//index
        public Int32 operatorStatesTriggerProhibited;
        public Int32 sublevelDest;//index
        public Int32 operateRequiredQuest;//index
        public Int32 operateRequiredQuestState;//index
        public Int32 operateRequiredQuestStateValue;//index
        public Int32 operateProhibitedQuestStateValue;//index
        public Int32 requiresItemOfUnitType1;//index
        public Int32 spawnTreasureClassInLevel;//index
        public Int32 oneWayVisualPortalDir;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes21;
        public float labelScale;
        public float labelForwardOffset;
        public float heightpercent;
        public float weightPercent;
        public Int32 hasAppearanceShape;
        public byte apperanceHeightMin;
        public byte appearanceHeightMax;
        public byte appearanceWeightMin;
        public byte appearanceWeightMax;
        public Int32 appearanceUseLineBounds;//bool
        public byte appearanceShortSkinny;
        public byte appearanceTallSkinny;
        public byte appearanceShortFat;
        public byte appearanceTallFat;
        public Int32 colorSet;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknownBytes24;
        public Int32 globalThemeRequired;//index
        public Int32 levelThemeRequired;//index
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknownBytes25;
        public Int32 undefined1;
        public Int32 undefined2;
        public Int32 undefined3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)]
        byte[] unknownBytes26;
        public Int32 undefined4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        byte[] unknownBytes27;
    }

    public abstract class Items
    {
        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            spawn = 1,
            spawnAtMerchant = 2,
            forceIgnoresScale = 4,
            impactOnFuse = 8,
            impactOnFree = 16,
            impactOnHitUnit = 32,
            impactOnHitBackground = 64,
            havokIgnoresDirection = 128,
            damagesOnFuse = 256,
            hitsUnits = 512,
            killOnUnitHit = 1024,
            hitsBackground = 2048,
            noRayCollision = 4096,
            killOnBackground = 8192,
            stickOnHit = 16384,
            stickOnInit = 32768,
            sync = 65536,
            clientOnly = 131072,
            serverOnly = 262144,
            useSourceVel = 524288,
            mustHit = 1048576,
            prioritizeTarget = 2097152,
            trailEffectsUseProjectile = 4194304,
            impactEffectsUseProjectile = 8388608,
            destroyOtherMissiles = 16777216,
            dontHitSkillTarget = 33554432,
            flipFaceDirection = 67108864,
            dontUseRangeForSkill = 134217728,
            pullsTarget = 268435456,
            damagesOnHitUnit = 536870912,
            pulsesStatsOnHitUnit = 1073741824,
            damagesOnHitBackground = 2147483648
        }

        [FlagsAttribute]
        public enum BitMask02 : uint
        {
            alwaysCheckForCollisions = 1,
            setShapePercentages = 2,
            useSourceAppearance = 4,
            dontTransferRidersFromOwner = 8,
            dontTransferDamagesOnClient = 16,
            missileIgnorePostLaunch = 32,
            attacksLocationOnHitUnit = 64,
            dontDeactivateWithRoom = 128,
            angerOthersOnDamaged = 256,
            angerOthersOnDeath = 512,
            alwaysFaceSkillTarget = 1024,
            setRopeEndWithNoTarget = 2048,
            forceDrawDirectionToMoveDirection = 4096,
            questNameColor = 8192,
            doNotSortWeapons = 16384,
            ignoresEquipClassReqs = 32768,
            doNotUseSorceForToHit = 65536,
            angleWhilePathing = 131072,
            dontAddWardrobeLayer = 262144,
            dontUseContainerAppearance = 524288,
            subscriberOnly = 1048576,
            computeLevelRequirement = 2097152,
            dontFattenCollision = 4194304,
            automapSave = 8388608,
            requiresCanOperateToBeKnown = 16777216,
            forceFreeOnRoomReset = 33554432,
            canReflect = 67108864,
            selectTargetIgnoresAimPos = 134217728,
            canMeleeAboveHeight = 268435456,
            getFlavorTextFromQuest = 536870912,
            unIdentifiedNameFromBaseRow = 1073741824,
            noRandomProperName = 2147483648
        }

        [FlagsAttribute]
        public enum BitMask03 : uint
        {
            noNameModifications = 1,
            preLoad = 2,
            ignoreInDat = 4,
            ignoreSavedStates = 8,
            drawUsingCutUpWardrobe = 16,
            isGood = 32,
            isNpc = 64,
            canNotBeMoved = 128,
            noLevel = 256,
            usesSkills = 512,
            autoPickup = 1024,
            trigger = 2048,
            dieOnClientTrigger = 4096,
            neverDestroyDead = 8192,
            collideWhenDead = 16384,
            startDead = 32768,
            givesLoot = 65536,
            dontTriggerByProximity = 131072,
            triggerOnEnterRoom = 262144,
            destructible = 524288,
            inAir = 1048576,
            wallWalk = 2097152,
            startInTownIdle = 4194304,
            onDieDestroy = 8388608,
            onDieEndDestroy = 16777216,
            onDieHideModel = 33554432,
            selectableDeadOrDying = 67108864,
            interactive = 134217728,
            hideDialogHead = 268435456, // 28
            collideBad = 536870912,
            collideGood = 1073741824,
            modesIgnoreAI = 2147483648
        }

        [FlagsAttribute]
        public enum BitMask04 : uint
        {
            dontPath = 1,
            snapToPathnodeOnCreate = 2,
            unTargetable = 4,
            FaceDuringInteraction = 8,
            noSync = 16,
            canNotTurn = 32,
            turnNeckInsteadOfBody = 64,
            merchant = 128,
            merchantSharedInventory = 256,
            trader = 512,
            tradesman = 1024,
            gambler = 2048,
            mapVendor = 4096,
            godQuestMessanger = 8192,
            trainer = 16384,
            healer = 32768,
            graveKeeper = 65536,
            taskGiver = 131072,
            canUpgradeItems = 262144,
            canAugmentItems = 524288,
            autoIdentifiesInventory = 1048576,
            npcDungeonWarp = 2097152,
            PvPSignerUpper = 4194304,
            foreman = 8388608,
            transporter = 16777216,
            showsPortrait = 33554432,
            petGetsStatPointsPerLevel = 67108864,
            ignoresSkillPowerCost = 134217728,
            checkRadiusWhenPathing = 268435456,
            checkHeightWhenPathing = 536870912,
            questImportantInfo = 1073741824,
            ignoresToHit = 2147483648
        }

        [FlagsAttribute]
        public enum BitMask05 : uint
        {
            askQuestsForOperate = 1,
            askFactionForOperate = 2,
            askPvPCensorshipForOperate = 4,
            structural = 8,
            askQuestsForKnown = 16,
            askQuestsForVisible = 32,
            informQuestsOnInit = 64,
            informQuestsOfLootDrop = 128,
            informQuestsOnDeath = 256,
            noTrade = 512,
            flagRoomAsNoSpawn = 1024,
            monitorPlayerApproach = 2048,
            monitorApproachClearLOS = 4096,
            canFizzle = 8192,
            inheritsDirection = 16384,
            canNotBeDismantled = 32768,
            canNotBeUpgraded = 65536,
            canNotBeAugmented = 131072,
            canNotBeDeModded = 262144,
            ignoreSellWithInventoryConfirm = 524288,
            wardrobePerUnit = 1048576,
            wardrobeSharesModelDef = 2097152,
            noWeaponModel = 4194304,
            //23
            noDrop = 16777216,
            noDropExceptForDuplicates = 33554432,
            askQuestsForPickup = 67108864,
            informQuestsOnPickup = 134217728,
            examinable = 268435456,
            informQuestsToUse = 536870912,
            consumeWhenUsed = 1073741824,
            immuneToCritical = 2147483648
        }

        [FlagsAttribute]
        public enum BitMask06 : uint
        {
            NoRandomAffixes = 1,
            canBeChampion = 2,
            noQualityDowngrade = 4,
            noDrawOnInit = 8,
            mustFaceMeleeTarget = 16,
            dontDestroyIfVelocityIsZero = 32,
            ignoreInteractDistance = 64,
            operateRequiresGoodQuestStatus = 128,
            reverseArriveDirection = 256,
            faceAfterWarp = 512,
            neverAStartLocation = 1024,
            alwaysShowLabel = 2048,
            //12
            undefined13 = 8192, //no predefined name
            //14
            isNonweaponMissile = 32768,
            cullByScreensize = 65536,
            linkWarpDestByLevelType = 131072,
            isBoss = 262144,
            //19
            takeResponsibilityOnKill = 1048576,
            alwaysKnownForSounds = 2097152,
            ignoreTargetOnRepeatDmg = 4194304,
            bindToLevelArea = 8388608,
            dontCollideWithDestructibles = 16777216,
            blocksEverything = 33554432,
            everyoneCanTarget = 67108864,
            missilePlotArc = 134217728,
            petDiesOnWarp = 268435456,
            missileIsGore = 536870912,
            canAttackFriends = 1073741824,
            ignoreItemRequirements = 2147483648
        }

        [FlagsAttribute]
        public enum BitMask07 : uint
        {
            lowLodInTown = 1,
            treasureClassBeforeRoom = 2,
            taskGiverNoStartingIcon = 4,
            assignGUID = 8,
            merchantDoesNotRefresh = 16,
            dontDepopulate = 32,
            dontShrinkBones = 64,
            //7
            hasQuestInfo = 256,
            multiplayerOnly = 512,
            noSpin = 1024,
            npcGuildMaster = 2048,
            autoIdentifyAffixs = 4096,
            npcRespeccer = 8192,
            allowObjectStepping = 16384,
            alwaysUseFallback = 32768,
            canNotSpawnRandomLevelTreasure = 65536,
            xferMissileStats = 131072,
            specificToDifficulty = 262144,
            isFieldMissile = 524288,
            ignoreFuseMsStat = 1048576,
            usesPetLevel = 2097152
        }

        [FlagsAttribute]
        public enum BitMask08 : uint
        {
            bounceOnUnitHit = 1,//these are probably only for missiles.t.c
            bounceOnBackGroundHit = 2,
            newDirectionOnBounce = 4,
            canNotRicochet = 8,
            reTargetOnBounce = 16
        }
    }
}