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
        class ItemsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown01;
            public UInt64 name;
            public UInt64 folder;
            public UInt64 appearance;
            public UInt64 appearenceFirst;
            public UInt64 icon;
            public UInt64 holyRadius;
            public UInt64 tinyHitParticle;
            public UInt64 lightHitParticle;
            public UInt64 mediumHitParticle;
            public UInt64 hardHitParticle;
            public UInt64 killedParticle;
            public UInt64 fizzleParticle;
            public UInt64 reflectParticle;
            public UInt64 restoreVitalsParticle;
            public UInt64 diffuse;
            public UInt64 normal;
            public UInt64 specular;
            public UInt64 lightmap;
            public UInt64 overrideSource1;
            public UInt64 overrideSource2;
            public UInt64 overrideSource3;
            public UInt64 overrideSource4;
            public UInt64 overrideSource5;
            public UInt64 overrideDest1;
            public UInt64 overrideDest2;
            public UInt64 overrideDest3;
            public UInt64 overrideDest4;
            public UInt64 overrideDest5;
            public UInt64 particleFolder;
            public UInt64 pickupFunction;
            public UInt64 triggerString1;
            public UInt64 triggerString2;
            public UInt64 charSelectFont;
            public UInt64 tooltipDamageIcon;
            public UInt64 damagingMeleeParticle;
            public UInt64 muzzleFlash;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 904)]
            public byte[] unknown02;
            public UInt32 impUnitPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 184)]
            public byte[] unknown03;
            public UInt32 spawn;
            public UInt32 properties1;
            public UInt32 behaviour;
            public UInt32 dontPath;
            public UInt32 action;
            public UInt32 affixes;
            public UInt32 properties2;
            public UInt32 titleString;
            public UInt32 typeDescription;
            public UInt32 flavorText;
            public UInt32 additionalDescription;
            public UInt32 additionalRaceDescription;
            public UInt32 analyze;
            public UInt32 recipeList;
            public UInt32 recipeSingleUse;
            public UInt32 paperdollBackgroundLevel;
            public UInt32 paperdollWeapon1;
            public UInt32 paperdollWeapon2;
            public UInt32 paperdollSkill;
            public UInt32 paperdollColorset;
            public UInt32 respawnChance;
            public UInt32 respawnSpawnclass;
            public UInt32 code1;
            public UInt32 code2;
            public UInt32 densityValueOverride;
            public UInt32 minionPackSize;
            public float spinSpeed;
            public float maxTurnRate;
            public UInt32 unitType;
            public UInt32 unitTypeForLeveling;
            public UInt32 preferedByUnitType;
            public UInt32 family;
            public UInt32 censorClassNoHumans;
            public UInt32 censorClassNoGore;
            public UInt32 sex;
            public UInt32 race;
            public UInt32 rarity;
            public UInt32 spawnChance;
            public UInt32 minMonsterExperienceLevel;
            public UInt32 level;
            public UInt32 monsterQuality;
            public UInt32 monsterClassAtUniqueQuality;
            public UInt32 monsterClassAtChampionQuality;
            public UInt32 minionClass;
            public UInt32 monsterNameType;
            public UInt32 quality;
            public UInt32 angerRange;
            public UInt32 baseLevel;
            public UInt32 capLevel;
            public UInt32 minMerchantLevel;
            public UInt32 maxMerchantLevel;
            public UInt32 minSpawnLevel;
            public UInt32 maxSpawnLevel;
            public UInt32 maxLevel;
            public UInt32 fixedLevel;
            public UInt32 hpMin;
            public UInt32 hpMax;
            public UInt32 powerMax;
            public UInt32 experience;
            public UInt32 attackRating;
            public UInt32 luckBonus;
            public UInt32 luckChanceToSpawn;
            public UInt32 roomPopulatePass;
            public UInt32 weaponBoneIndex;
            public UInt32 requiresAffixOrSuffix;
            public float autoPickupDistance;
            public UInt32 pickupPullState;
            public float extraDyingTimeInSeconds;
            public UInt32 npcInfo;
            public UInt32 balanceTestCount;
            public UInt32 balanceTestGroup;
            public UInt32 merchantStartingPane;
            public UInt32 merchantFactionType;
            public UInt32 merchantFactionValueNeeded;
            public UInt32 questRequirement;
            public float noSpawnRadius;
            public float monitorApproachRadius;
            public UInt32 tasksGeneratedStat;
            public float serverMissileOffset;
            public float homingTurnAngleRadians;
            public float homingModBasedDis;
            public float homeAfterUnitRadius;
            public float collidableAfterXSeconds;
            public float homeAfterXSeconds;
            public UInt32 bounceOnUnitHit;
            public float impactCameraShakeDuration;
            public float impactCameraShakeMagnitude;
            public float impactCameraShakeDegrade;
            public UInt32 maximumImpactFrequency;
            public UInt32 onlyCollideWithUnitType;
            public UInt32 questDescription;
            public UInt32 pickUpCondition;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown04;
            public UInt32 scriptOnUse;
            public UInt32 stackSize;
            public UInt32 maxPickUp;
            public UInt32 baseCost;
            public UInt32 realWorldCost;
            public UInt32 buyPriceMult;
            public UInt32 buyPriceAdd;
            public UInt32 sellPriceMult;
            public UInt32 sellPriceAdd;
            public UInt32 inventoryWardrobe;
            public UInt32 characterScreenWardrobe;
            public UInt32 characterScreenState;
            public UInt32 wardrobeBody;
            public UInt32 wardrobeFallback;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown05;
            public UInt32 wardrobeMip;
            public UInt32 wardrobeAppearanceGroup;
            public UInt32 wardrobeAppearanceGroup1st;
            public UInt32 startingStance;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] unknown06;
            public UInt32 containerUnitType1;
            public UInt32 containerUnitType2;
            public UInt32 containerUnitType3;
            public UInt32 containerUnitType4;
            public UInt32 firingErrorIncrease;
            public UInt32 firingErrorDecrease;
            public UInt32 firingErrorMax;
            public UInt32 accuracyBase;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown07;
            public UInt32 refillHotKey;
            public UInt32 animGroup;
            public UInt32 meleeWeapon;
            public UInt32 cdTicks;
            public UInt32 approxDps;
            public UInt32 tooltipDamageString;
            public UInt32 requiredAffixGroups1;
            public UInt32 requiredAffixGroups2;
            public UInt32 requiredAffixGroups3;
            public UInt32 requiredAffixGroups4;
            public UInt32 requiredAffixGroups5;
            public UInt32 requiredAffixGroups6;
            public UInt32 requiredAffixGroups7;
            public UInt32 requiredAffixGroups8;
            public UInt32 spawnMonsterClass;
            public UInt32 safeState;
            public UInt32 skillGhost;
            public UInt32 skillRef;
            public UInt32 dmgType;
            public UInt32 weaponDamageScale;
            public UInt32 dontUseWeaponDamage;
            public UInt32 minBaseDmg;
            public UInt32 maxBaseDmg;
            public UInt32 sfxAttackFocus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] unknown08;
            public UInt32 sfxPhysicalAbility;
            public UInt32 sfxPhysicalDefense;
            public UInt32 sfxPhysicalKnockbackInCm;
            public UInt32 sfxPhysicalStunDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown09;
            public UInt32 sfxFireAbility;
            public UInt32 sfxFireDefense;
            public UInt32 sfxFireDamagePercent1;
            public UInt32 sfxFireDamagePercent2;
            public UInt32 sfxFireDamagePercent3;
            public UInt32 sfxElectricAbility;
            public UInt32 sfxElectricDefense;
            public UInt32 sfxElectricDamagePercentOfInitialPerShock;
            public UInt32 sfxElectricDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown10;
            public UInt32 sfxSpectralAbility;
            public UInt32 sfxSpectralDefense;
            public UInt32 sfxSpectralSpecial;
            public UInt32 sfxSpectralDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown11;
            public UInt32 sfxToxicAbility;
            public UInt32 sfxToxicDefense;
            public UInt32 sfxToxicDamageAsPercentageOfDamageDeliveredFromAttack;
            public UInt32 sfxToxicDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] unknown12;
            public UInt32 sfxPoisonAbility;
            public UInt32 sfxPoisonDefense;
            public UInt32 sfxPoisonDamageAsPercentageOfDamageDeliveredFromAttack;
            public UInt32 sfxPoisonDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            public byte[] unknown13;
            public UInt32 dmgIncrement;
            public UInt32 radialDmgIncrement;
            public UInt32 fieldDmgIncrement;
            public UInt32 dotDmgIncrement;
            public UInt32 aiChangerDmgIncrement;
            public UInt32 toHit;
            public UInt32 criticalPct;
            public UInt32 criticalMult;
            public UInt32 staminaDrainChancePercent;
            public UInt32 staminaDrain;
            public UInt32 interruptAttackPct;
            public UInt32 interruptDefensePct;
            public UInt32 interruptChanceOnAnyHit;
            public UInt32 stealthDefensePct;
            public UInt32 stealthAttackPct;
            public UInt32 aiChangeDefense;
            public UInt32 armor;
            public UInt32 armorPhys;
            public UInt32 armorFire;
            public UInt32 armorElec;
            public UInt32 armorSpec;
            public UInt32 armorToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown14;
            public UInt32 armorPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown15;
            public UInt32 maxArmor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
            public byte[] unknown16;
            public UInt32 shields;
            public UInt32 shieldPhys;
            public UInt32 shieldFire;
            public UInt32 shieldElec;
            public UInt32 shieldSpec;
            public UInt32 shieldToxic;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown17;
            public UInt32 shieldPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown18;
            public UInt32 strengthPercent;
            public UInt32 dexterityPercent;
            public UInt32 startingAccuracy;
            public UInt32 startingStrength;
            public UInt32 startingStamina;
            public UInt32 startingWillpower;
            public UInt32 props1;
            public UInt32 props2;
            public UInt32 props3;
            public UInt32 props4;
            public UInt32 props5;
            public UInt32 props1AppliesToUnitype;
            public UInt32 props2AppliesToUnitype;
            public UInt32 props3AppliesToUnitype;
            public UInt32 props4AppliesToUnitype;
            public UInt32 props5AppliesToUnitype;
            public UInt32 perLevelProps1;
            public UInt32 perLevelProps2;
            public UInt32 propsElite;
            public UInt32 affix1;
            public UInt32 affix2;
            public UInt32 affix3;
            public UInt32 affix4;
            public UInt32 affix5;
            public UInt32 affix6;
            public UInt32 treasure;
            public UInt32 championTreasure;
            public UInt32 firstTimeTreasure;
            public UInt32 inventory;
            public UInt32 recipeIngredientInvLoc;
            public UInt32 recipeResultInvLoc;
            public UInt32 startingTreasure;
            public UInt32 invWidth;
            public UInt32 invHeight;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] unknown19;
            public UInt32 requiredQuality;
            public UInt32 qualityName;
            public UInt32 fieldMissile;
            public UInt32 skillHitUnit;
            public UInt32 skillHitBackground;
            public UInt32 skillMissed1;
            public UInt32 skillMissed2;
            public UInt32 skillMissed3;
            public UInt32 skillMissed4;
            public UInt32 skillMissed5;
            public UInt32 skillMissed6;
            public UInt32 skillMissed7;
            public UInt32 skillMissed8;
            public UInt32 skillMissed9;
            public UInt32 skillMissed10;
            public UInt32 skillMissed11;
            public UInt32 skillMissed12;
            public UInt32 skillMissed13;
            public UInt32 skillMissed14;
            public UInt32 skillMissed15;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown20;
            public UInt32 skillOnFuse;
            public UInt32 skillOnDamageRepeat;
            public UInt32 startingskills1;
            public UInt32 startingskills2;
            public UInt32 startingskills3;
            public UInt32 startingskills4;
            public UInt32 startingskills5;
            public UInt32 startingskills6;
            public UInt32 startingskills7;
            public UInt32 startingskills8;
            public UInt32 startingskills9;
            public UInt32 startingskills10;
            public UInt32 startingskills11;
            public UInt32 startingskills12;
            public UInt32 unitDieBeginSkill;
            public UInt32 unitDieBeginSkillClient;
            public UInt32 scriptOnUnitDieEnd;
            public UInt32 unitDieEndSkill;
            public UInt32 deadSkill;
            public UInt32 kickSkill;
            public UInt32 rightSkill;
            public UInt32 initSkill;
            public UInt32 skillLevelActive;
            public UInt32 initState1;
            public UInt32 initState2;
            public UInt32 initState3;
            public UInt32 initState4;
            public UInt32 initStateTicks;
            public UInt32 dyingState;
            public UInt32 skillWhenUsed;
            public UInt32 SkillTab;
            public UInt32 SkillTab2;
            public UInt32 SkillTab3;
            public UInt32 SkillTab4;
            public UInt32 SkillTab5;
            public UInt32 SkillTab6;
            public UInt32 SkillTab7;
            public UInt32 levelUpState;
            public UInt32 sounds;
            public UInt32 flippySound;
            public UInt32 invPickUpSound;
            public UInt32 invPutDownSound;
            public UInt32 invEquipSound;
            public UInt32 pickUpSound;
            public UInt32 useSound;
            public UInt32 cantUseSound;
            public UInt32 cantFireSound;
            public UInt32 blockSound;
            public UInt32 getHit0;
            public UInt32 getHit1;
            public UInt32 getHit2;
            public UInt32 getHit3;
            public UInt32 getHit4;
            public UInt32 getHit5;
            public UInt32 interactSound;
            public UInt32 damagingSound;
            public UInt32 forwardFootStepLeft;
            public UInt32 forwardFootStepRight;
            public UInt32 backwardFootStepLeft;
            public UInt32 backwardFootStepRight;
            public UInt32 firstPersonJumpFootStep;
            public UInt32 firstPersonLandFootStep;
            public UInt32 outOfManaSound;
            public UInt32 inventoryFullSound;
            public UInt32 cantSound;
            public UInt32 CantUseSound;
            public UInt32 cantUseYetSound;
            public UInt32 cantCastSound;
            public UInt32 cantCastYetSound;
            public UInt32 cantCastHereSound;
            public UInt32 lockedSound;
            public float pathingCollisionRadius;
            public float collisionRadius;
            public float collisionRadiusHorizontal;
            public float collisionHeight;
            public float blockingRadiusOverride;
            public float warpPlaneForwardMultiplier;
            public float warpOutDistance;
            public UInt32 snapToAngleInDegrees;
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
            public float stopAfterXNumOfTicks;
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
            public UInt32 rangeMin;
            public UInt32 rangeMax;
            public UInt32 decoyMonster;
            public UInt32 havokShape;
            public UInt32 missileHitUnit;
            public UInt32 missileHitBackground;
            public UInt32 missileOnFreeOrFuse;
            public UInt32 missileTag;
            public UInt32 damageRepeatRate;
            public UInt32 damageRepeatChance;
            public UInt32 repeatDamageImmediately;
            public UInt32 serverSrcDamage;
            public UInt32 blockGhosts;
            public UInt32 monster;
            public UInt32 triggerType;
            public UInt32 operatorStatesTriggerProhibited;
            public UInt32 sublevelDest;
            public UInt32 operateRequiredQuest;
            public UInt32 operateRequiredQuestState;
            public UInt32 operateRequiredQuestStateValue;
            public UInt32 operateProhibitedQuestStateValue;
            public UInt32 requiresItemOfUnitType1;
            public UInt32 spawnTreasureClassInLevel;
            public UInt32 oneWayVisualPortalDir;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown21;
            public float labelScale;
            public float labelForwardOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] unknown22;
            public UInt32 hasAppearanceShape;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] unknown23;
            public UInt32 colorSet;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] unknown24;
            public UInt32 globalThemeRequired;
            public UInt32 levelThemeRequired;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown25;
            public UInt32 undefined1;
            public UInt32 undefined2;
            public UInt32 undefined3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)]
            public byte[] unknown26;
            public UInt32 undefined4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public byte[] unknown27;
        }

        List<ItemsTable> items;

        public Items(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return items.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            items = ExcelTables.ReadTables<ItemsTable>(data, ref offset, Count);
        }

        struct SpawnMask
        {
            bool enableSpawn; // 1
            bool spawnAtMerchant; // 2
        }

        struct ItemMask
        {
            bool questNameColor; //14
            bool subscriberOnly; //21
            bool levelRequirement; //22
            bool unidentified; //31
        }

        struct BehaviourMask
        {
            bool noNameModifications; //1
            bool drawUsingWardrobe; //5
            bool autoPickup; //11
        }

        struct ActionMask
        {
            bool noTrade; //10
            bool flagRoomAsNoSpawn; //(11),
            bool noDismantle;//(16),
            bool noUpgrade;//(17),
            bool noAugment;//(18),
            bool noDemod;//(19),
            bool noIdentify;//(20),
            bool wardrobePerUnit;//(21),
            bool wardrobeSharesModelDefinition;//(22),
            bool noDrop;//(25),                            // player can no drop the item
            bool noDropDupes;//(26), //player can only drop/destroy duplicates
            bool examinable;//(29),
            bool consumeOnUse;//(31);
        }

        struct AffixMask
        {
            bool noRandomAffixes;//(1),
            bool mustFaceMeleeTarget;//(5),
            bool alwaysShowLabel;//(12);
        }

        struct PropertyMask
        {
            bool hasQuestInfo;//(11),
            bool multiplayerOnly;//(10);
        }
    }
}
