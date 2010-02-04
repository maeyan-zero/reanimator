using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Items : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class ItemsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] header;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 name;//pchar: pointer to relevant text in the front of the file.
            Int32 buffer1;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 folder;//pchar
            Int32 buffer2;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 appearance;//pchar
            Int32 buffer3;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 appearenceFirst;//pchar
            Int32 buffer4;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 icon;//pchar
            Int32 buffer5;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 holyRadius;//pchar
            Int32 buffer6;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tinyHitParticle;//pchar
            Int32 buffer7;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 lightHitParticle;//pchar
            Int32 buffer8;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mediumHitParticle;//pchar
            Int32 buffer9;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 hardHitParticle;//pchar
            Int32 buffer10;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 killedParticle;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] buffer11;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 fizzleParticle;//pchar
            Int32 buffer12;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 reflectParticle;//pchar
            Int32 buffer13;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 restoreVitalsParticle;//pchar
            Int32 buffer14;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 diffuse;//pchar
            Int32 buffer15;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 normal;//pchar
            Int32 buffer16;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 specular;//pchar
            Int32 buffer17;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 lightmap;//pchar
            Int32 buffer18;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideSource1;//pchar
            Int32 buffer19;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideDest1;//pchar
            Int32 buffer20;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideSource2;//pchar
            Int32 buffer21;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideDest2;//pchar
            Int32 buffer22;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideSource3;//pchar
            Int32 buffer23;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideDest3;//pchar
            Int32 buffer24;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideSource4;//pchar
            Int32 buffer25;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideDest4;//pchar
            Int32 buffer26;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideSource5;//pchar
            Int32 buffer27;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 overrideDest5;//pchar
            Int32 buffer28;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 particleFolder;//pchar
            Int32 buffer29;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 pickupFunction;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] buffer30;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 triggerString1;//pchar
            Int32 buffer31;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 triggerString2;//pchar
            Int32 buffer32;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 charSelectFont;//pchar
            Int32 buffer33;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 tooltipDamageIcon;//pchar
            Int32 buffer34;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 damagingMeleeParticle;//pchar
            Int32 buffer35;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 muzzleFlash;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown02;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfDefault;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown03;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfPhysical;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown04;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfFire;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown05;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfElectric;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown06;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfSpectral;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown07;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfToxic;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] unknown08;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 mfPoison;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            byte[] unknown09;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trail_RopeNoTarget;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown10;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailDefault;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown11;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailPhysical;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown12;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailFire;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown13;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailElectric;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown14;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailSpectral;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown15;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailToxic;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] unknown16;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 trailPoison;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            byte[] unknown17;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projectile_RopeWithTarget;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown18;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projDefault;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown19;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projPhysical;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown20;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projFire;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown21;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projElectric;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown22;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projSpectral;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown23;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projToxic;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] unknown24;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 projPoison;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            byte[] unknown25;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactWall;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown26;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallDefault;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown27;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallPhysical;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown28;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallFire;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown29;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallElectric;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown30;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallSpectral;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown31;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallToxic;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
            byte[] unknown32;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impWallPoison;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            byte[] unknown33;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnit;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown34;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnitDefault;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown35;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnitPhysical;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown36;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnitFire;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown37;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnitElectric;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown38;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnitSpectral;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown39;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 impactUnitToxic;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
            byte[] unknown40;
            public Int32 bitmask01;/**'spawn',0
      'spawn at merchant',1
      'ForceIgnoresScale',2
      'impact on fuse',3
      'impact on free',4
      'impact on hit unit',5
      'impact on hit background',6
      'havok ignores direction',7
      'damages on fuse',8
      'hits units',9
      'kill on unit hit',10
      'hits background',11
      'no ray collision',12
      'kill on background',13
      'stick on hit',14
      'stick on init',15
      'Sync',16
      'Client only',17
      'Server only',18
      'use source vel',19
      'must hit',20
      'prioritize target',21
      'TrailEffectsUseProjectile',22
      'ImpactEffectsUseProjectile',23
      'destroy other missiles',24
      'dont hit skill target',25
      'flip face direction',26
      'don''t use range for skill',27
      'pulls target',28
	  'damages on hit unit',29
	  'pulses stats on hit unit',30
	  'damages on hit background',31*/
            public Int32 bitmask02;/**'always check for collisions',0
      'set shape percentages',1
      'use source appearance',2
      'don''t transfer riders from owner',3
      'don''t transfer damages on client',4
      'missile ignore postlaunch',5
      'attacks location on hit unit',6
      'dont deactivate with room',7
      'anger others on damaged',8
      'anger others on death',9
      'always face skill target',10
      'Set Rope End with No Target',11
      'force draw direction to move direction',12
      'quest name color',13
      'do not sort weapons',14
      'Ignores equip class reqs',15
      'do not use sorce for tohit',16
      'angle while pathing',17
      'don''t add wardrobe layer',18
      'don''t use container appearance',19
      'subscriber only',20
      'compute level requirement',21
      'don''t fatten collision',22
      'automap save',23
      'requires can operate to be known',24
      'force free on room reset',25
      'can reflect',26
      'select target ignores aim pos',27
      'can melee above height',28
	  'get flavortext from Quest',29
	  'unidentified name from base row',30
      'no random proper name',31*/
            public Int32 bitmask03;/**'no name modifications',0
	  'preload',1
      'ignore in dat',2
      'ignore saved states',3
      'draw using cut up wardrobe',4
	  'is good',5
      'is npc',6
      'cannot be moved',7
	  'nolevel',8
	  'uses skills',9
	  'autopickup',10
	  'trigger',11
      'die on client trigger',12
      'never destroy dead',13
      'collide when dead',14
      'Start dead',15
      'gives loot',16
      'donÆt trigger by proximity',17
      'trigger on enter room',18
      'destructible',19
      'in air',20
      'wall walk',21
	  'start in town idle',22
	  'on die destroy',23
      'on die end destroy',24
      'on die hide model',25
      'selectable dead or dying',26
	  'interactive',27
	  'Merchant Does Not Refresh',28
      'HideDialogHead',28
	  'collide bad',29
	  'collide good',30
	  'modes ignore AI',31*/
            public Int32 bitmask04;/**'dont path',0
      'snap to pathnode on create',1
      'untargetable',2
      'FaceDuringInteraction',3
      'no sync',4
	  'cannot turn',5
      'turn neck instead of body',6
      'Merchant',7
      'Merchant Shared Inventory',8
	  'Trader',9
	  'Tradesman',10
      'Gambler',11
      'MapVendor',12
	  'GodQuestMessanger',13
	  'Trainer',14
	  'Healer',15
      'Gravekeeper',16
      'TaskGiver',17
	  'Can Upgrade Items',18
      'Can Augment Items',19
      'Auto Identifies Inventory',20
	  'NPCDungeonWarp',21
      'PvPSignerUpper',22
      'Foreman',23
	  'Transporter',24
	  'Shows Portrait',25
	  'Pet Gets Stat Points per Level',26
	  'Ignores Skill Power Cost',27
	  'check radius when pathing',28
      'check height when pathing',29
	  'QuestImportantInfo',30
	  'ignores tohit',31*/
            public Int32 bitmask05;/**'AskQuestsForOperate',0
	  'AskFactionForOperate',1
	  'AskPvPCensorshipForOperate',2
      'Structural',3
      'AskQuestsForKnown',4
      'AskQuestsForVisible',5
      'inform quests on init',6
      'InformQuestsOfLootDrop',7
      'inform quests on death',8
      'no trade',9
	  'Flag Room As No Spawn',10
      'MonitorPlayerApproach',11
      'MonitorApproachClearLOS',12
	  'can fizzle',13
      'inherits direction',14
	  'cannot be dismantled',15
      'cannot be upgraded',16
      'cannot be augmented',17
      'cannot be de-modded',18
	  'ignore sell with inventory confirm',19
	  'wardrobe per unit',20
	  'wardrobe shares model def',21
      'no weapon model',22
      'NoDrop',24
      'NoDropExceptForDuplicates',25
      'AskQuestsForPickup',26
      'InformQuestsOnPickup',27
	  'Examinable',28
	  'InformQuestsToUse',29
      'Consume When Used',30
	  'immune to critical',31*/
            public Int32 bitmask06;/**'no random affixes',0
	  'can be champion',1
	  'no quality downgrade',2
	  'no draw on init',3
	  'must face melee target',4
	  'don''t destroy if velocity is zero',5
	  'ignore interact distance',6
	  'operate requires good quest status',7
	  'reverse arrive direction',8
      'face after warp',9
      'never a start location',10
	  'always show label',11
	  '',13 //no predefined name
	  'is nonweapon missile',15
      'cull by screensize',16
	  'link warp dest by level type',17
      'is boss',18
      'take responsibility on kill',20
      'always known for sounds',21
	  'ignore target on repeat dmg',22
	  'bind to level area',23
      'don''t collide with destructibles',24
      'blocks everything',25
      'everyone can target',26
      'missile plot arc',27
      'missile is gore',29
	  'Can Attack Friends',30
	  'ignore item requirements',31*/
            public Int32 bitmask07;/**'low lod in town',0
	  'treasure class before room',1
	  'TaskGiver No Starting Icon',2
	  'dont depopulate',5
      'don''t shrink bones',6
	  'has quest info',8
	  'multiplayer only',9
	  'No Spin',10
	  'NPCGuildMaster',11
      'auto identify affixs',12
	  'NPCRespeccer',13
      'allow object stepping',14
	  'always use fallback',15
      'cannot spawn random level treasure',16
	  'xfer missile stats',17
      'specific to difficulty',18
      'is field missile',19
      'ignore fuse ms stat',20
	  'uses petlevel',21*/
            public Int32 String;//string index
            public Int32 typeDescription;//string index
            public Int32 flavorText;//string index
            public Int32 additionalDescription;//string index
            public Int32 additionalRaceDescription;//string index
            public Int32 analyze;//string index
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
            [ExcelTables.ExcelOutput(IsIntOffset = true, FieldNames = new String[] {"name1", "name2", "name3"})]
            public Int32 fixedLevel;//intptr
            public Int32 hpMin;
            public Int32 hpMax;
            public Int32 powerMax;
            public Int32 experience;
            public Int32 attackRating;
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
            public Int32 bitmask08;/*'bounce on unit hit',0);
      'bounce on background hit',1);
      'New Direction on Bounce',2);
      'cannot ricochet',3);
      'retarget on bounce',4);*/
            public float impactCameraShakeDuration;
            public float impactCameraShakeMagnitude;
            public float impactCameraShakeDegrade;
            public Int32 maximumImpactFrequency;
            public Int32 onlyCollideWithUnitType;//index
            public Int32 questDescription;//index
            public Int32 pickUpCondition;//intptr
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknownBytes04;
            public Int32 scriptOnUse;//intptr
            public Int32 stackSize;//intptr
            public Int32 maxPickUp;
            public Int32 baseCost;
            public Int32 realWorldCost;
            public Int32 buyPriceMult;//intptr
            public Int32 buyPriceAdd;//intptr
            public Int32 sellPriceMult;//intptr
            public Int32 sellPriceAdd;//intptr
            public Int32 inventoryWardrobe;//index
            Int32 characterScreenWardrobe;      // always -1
            Int32 characterScreenState;      // always -1
            Int32 wardrobeBody;      // always -1
            Int32 wardrobeFallback;      // always -1
            Int32 wardrobeUnkown1;      // always -1
            Int32 wardrobeMip;      // always 0
            Int32 wardrobeAppearanceGroup;      // always -1
            Int32 wardrobeAppearanceGroup1st;      // always -1
            Int32 startingStance;      // always -1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] unknownBytes06;
            public Int32 containerUnitType1;
            public Int32 containerUnitType2;
            Int32 containerUnitType3;      // always 0
            Int32 containerUnitType4;      // always 0
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
            public Int32 minBaseDmg;//intptr
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
            public Int32 props1;//intptr
            public Int32 props2;//intptr
            public Int32 props3;//intptr
            public Int32 props4;//intptr
            public Int32 props5;//intptr
            public Int32 props1AppliesToUnitype;
            public Int32 props2AppliesToUnitype;
            public Int32 props3AppliesToUnitype;
            public Int32 props4AppliesToUnitype;
            public Int32 props5AppliesToUnitype;
            public Int32 perLevelProps1;//intptr
            public Int32 perLevelProps2;//intptr
            public Int32 propsElite;//intptr
            public Int32 affix1;
            public Int32 affix2;
            public Int32 affix3;
            public Int32 affix4;
            public Int32 affix5;
            public Int32 affix6;
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

        public Items(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ItemsTable>(data, ref offset, Count);
        }

#pragma warning disable 0169
        struct BitMask1
        {
            bool spawn;//(0
            bool spawnAtMerchant;//(1
            bool ForceIgnoresScale;//(2
            bool impactOnFuse;//(3
            bool impactOnFree;//(4
            bool impactOnHitUnit;//(5
            bool impactOnHitBackground;//(6
            bool havokIgnoresDirection;//(7
            bool damagesOnFuse;//(8
            bool hitsUnits;//(9
            bool killOnUnitHit;//(10
            bool hitsBackground;//(11
            bool noRayCollision;//(12
            bool killOnBackground;//(13
            bool stickOnHit;//(14
            bool stickOnInit;//(15
            bool Sync;//(16
            bool ClientOnly;//(17
            bool ServerOnly;//(18
            bool useSourceVel;//(19
            bool mustHit;//(20
            bool prioritizeTarget;//(21
            bool TrailEffectsUseProjectile;//(22
            bool ImpactEffectsUseProjectile;//(23
            bool destroyOtherMissiles;//(24
            bool dontHitSkillTarget;//(25
            bool flipFaceDirection;//(26
            bool dontUseRangeForSkill;//(27
            bool pullsTarget;//(28
            bool damagesOnHitUnit;//(29
            bool pulsesStatsOnHitUnit;//(30
            bool damagesOnHitBackground;//(31
        }

        struct BitMask2
        {
            bool alwaysCheckForCollisions;//(0
            bool setShapePercentages;//(1
            bool useSourceAppearance;//(2
            bool dontTransferRidersFromOwner;//(3
            bool dontTransferDamagesOnClient;//(4
            bool missileIgnorePostLaunch;//(5
            bool attacksLocationOnHitUnit;//(6
            bool dontDeactivateWithRoom;//(7
            bool angerOthersOnDamaged;//(8
            bool angerOthersOnDeath;//(9
            bool alwaysFaceSkillTarget;//(10
            bool setRopeEndWithNoTarget;//(11
            bool forceDrawDirectionToMoveDirection;//(12
            bool questNameColor;//(13
            bool doNotSortWeapons;//(14
            bool ignoresEquipClassReqs;//(15
            bool doNotUseSorceForToHit;//(16
            bool angleWhilePathing;//(17
            bool dontAddWardrobeLayer;//(18
            bool dontUseContainerAppearance;//(19
            bool subscriberOnly;//(20
            bool computeLevelRequirement;//(21
            bool dontFattenCollision;//(22
            bool automapSave;//(23
            bool requiresCanOperateToBeKnown;//(24
            bool forceFreeOnRoomReset;//(25
            bool canReflect;//(26
            bool selectTargetIgnoresAimPos;//(27
            bool canMeleeAboveHeight;//(28
            bool getFlavorTextFromQuest;//(29
            bool unIdentifiedNameFromBaseRow;//(30
            bool noRandomProperName;//(31
        }

        struct BtMask3
        {
            bool noNameModifications;//(0
            bool preLoad;//(1
            bool ignoreInDat;//(2
            bool ignoreSavedStates;//(3
            bool drawUsingCutUpWardrobe;//(4
            bool isGood;//(5
            bool isNpc;//(6
            bool canNotBeMoved;//(7
            bool noLevel;//(8
            bool usesSkills;//(9
            bool autoPickup;//(10
            bool trigger;//(11
            bool dieOnClientTrigger;//(12
            bool neverDestroyDead;//(13
            bool collideWhenDead;//(14
            bool startDead;//(15
            bool givesLoot;//(16
            bool dontTriggerByProximity;//(17
            bool triggerOnEnterRoom;//(18
            bool destructible;//(19
            bool inAir;//(20
            bool wallWalk;//(21
            bool startInTownIdle;//(22
            bool onDieDestroy;//(23
            bool onDieEndDestroy;//(24
            bool onDieHideModel;//(25
            bool selectableDeadOrDying;//(26
            bool interactive;//(27
            bool merchantDoesNotRefresh;//(28
            bool hideDialogHead;//(28
            bool collideBad;//(29
            bool collideGood;//(30
            bool modesIgnoreAI;//(31
        }

        struct BitMask4
        {
            bool dontPath;//(0
            bool snapToPathnodeOnCreate;//(1
            bool unTargetable;//(2
            bool FaceDuringInteraction;//(3
            bool noSync;//(4
            bool canNotTurn;//(5
            bool turnNeckInsteadOfBody;//(6
            bool merchant;//(7
            bool merchantSharedInventory;//(8
            bool trader;//(9
            bool tradesman;//(10
            bool gambler;//(11
            bool mapVendor;//(12
            bool godQuestMessanger;//(13
            bool trainer;//(14
            bool healer;//(15
            bool graveKeeper;//(16
            bool taskGiver;//(17
            bool canUpgradeItems;//(18
            bool canAugmentItems;//(19
            bool autoIdentifiesInventory;//(20
            bool npcDungeonWarp;//(21
            bool PvPSignerUpper;//(22
            bool foreman;//(23
            bool transporter;//(24
            bool showsPortrait;//(25
            bool petGetsStatPointsPerLevel;//(26
            bool ignoresSkillPowerCost;//(27
            bool checkRadiusWhenPathing;//(28
            bool checkHeightWhenPathing;//(29
            bool questImportantInfo;//(30
            bool ignoresToHit;//(31
        }

        struct BitMask5
        {
            bool noRandomAffixes;//(1),
            bool mustFaceMeleeTarget;//(5),
            bool alwaysShowLabel;//(12);
            bool askQuestsForOperate;//(0
            bool askFactionForOperate;//(1
            bool askPvPCensorshipForOperate;//(2
            bool structural;//(3
            bool askQuestsForKnown;//(4
            bool askQuestsForVisible;//(5
            bool informQuestsOnInit;//(6
            bool informQuestsOfLootDrop;//(7
            bool informQuestsOnDeath;//(8
            bool noTrade;//(9
            bool flagRoomAsNoSpawn;//(10
            bool monitorPlayerApproach;//(11
            bool monitorApproachClearLOS;//(12
            bool canFizzle;//(13
            bool inheritsDirection;//(14
            bool canNotBeDismantled;//(15
            bool canNotBeUpgraded;//(16
            bool canNotBeAugmented;//(17
            bool canNotBeDeModded;//(18
            bool ignoreSellWithInventoryConfirm;//(19
            bool wardrobePerUnit;//(20
            bool wardrobeSharesModelDef;//(21
            bool noWeaponModel;//(22
            bool noDrop;//(24
            bool noDropExceptForDuplicates;//(25
            bool askQuestsForPickup;//(26
            bool informQuestsOnPickup;//(27
            bool examinable;//(28
            bool informQuestsToUse;//(29
            bool consumeWhenUsed;//(30
            bool immuneToCritical;//(31
        }

        struct BitMask6
        {
            bool hasQuestInfo;//(11),
            bool multiplayerOnly;//(10);
            bool NoRandomAffixes;//(0
            bool canBeChampion;//(1
            bool noQualityDowngrade;//(2
            bool noDrawOnInit;//(3
            bool mustFaceMeleeTarget;//(4
            bool dontDestroyIfVelocityIsZero;//(5
            bool ignoreInteractDistance;//(6
            bool operateRequiresGoodQuestStatus;//(7
            bool reverseArriveDirection;//(8
            bool faceAfterWarp;//(9
            bool neverAStartLocation;//(10
            bool alwaysShowLabel;//(11
            bool undefined13;//(13 //no predefined name
            bool isNonweaponMissile;//(15
            bool cullByScreensize;//(16
            bool linkWarpDestByLevelType;//(17
            bool isBoss;//(18
            bool takeResponsibilityOnKill;//(20
            bool alwaysKnownForSounds;//(21
            bool ignoreTargetOnRepeatDmg;//(22
            bool bindToLevelArea;//(23
            bool dontCollideWithDestructibles;//(24
            bool blocksEverything;//(25
            bool everyoneCanTarget;//(26
            bool missilePlotArc;//(27
            bool missileIsGore;//(29
            bool canAttackFriends;//(30
            bool ignoreItemRequirements;//(31
        }
        struct Bitmask7
        {
            bool lowLodInTown;//(0
            bool treasureClassBeforeRoom;//(1
            bool taskGiverNoStartingIcon;//(2
            bool dontDepopulate;//(5
            bool dontShrinkBones;//(6
            bool hasQuestInfo;//(8
            bool multiplayerOnly;//(9
            bool noSpin;//(10
            bool npcGuildMaster;//(11
            bool autoIdentifyAffixs;//(12
            bool npcRespeccer;//(13
            bool allowObjectStepping;//(14
            bool alwaysUseFallback;//(15
            bool canNotSpawnRandomLevelTreasure;//(16
            bool xferMissileStats;//(17
            bool specificToDifficulty;//(18
            bool isFieldMissile;//(19
            bool ignoreFuseMsStat;//(20
            bool usesPetLevel;//(21
        }
        struct BitMask8
        {
            bool bounceOnUnitHit;//(0);//these are probably only for missiles.t.c
            bool bounceOnBackGroundHit;//(1);
            bool newDirectionOnBounce;//(2);
            bool canNotRicochet;//(3)
            bool reTargetOnBounce;//(4)
        }

#pragma warning restore 0169
    }
}
