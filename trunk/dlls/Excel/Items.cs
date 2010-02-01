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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            public Int32 name;
            Int32 buffer1;
            public Int32 folder;
            Int32 buffer2;
            public Int32 appearance;
            Int32 buffer3;
            public Int32 appearenceFirst;
            Int32 buffer4;
            public Int32 icon;
            Int32 buffer5;
            public Int32 holyRadius;
            Int32 buffer6;
            public Int32 tinyHitParticle;
            Int32 buffer7;
            public Int32 lightHitParticle;
            Int32 buffer8;
            public Int32 mediumHitParticle;
            Int32 buffer9;
            public Int32 hardHitParticle;
            Int32 buffer10;
            public Int32 killedParticle;
            Int32 buffer11;
            public Int32 fizzleParticle;
            Int32 buffer12;
            public Int32 reflectParticle;
            Int32 buffer13;
            public Int32 restoreVitalsParticle;
            Int32 buffer14;
            public Int32 diffuse;
            Int32 buffer15;
            public Int32 normal;
            Int32 buffer16;
            public Int32 specular;
            Int32 buffer17;
            public Int32 lightmap;
            Int32 buffer18;
            public Int32 overrideSource1;
            Int32 buffer19;
            public Int32 overrideSource2;
            Int32 buffer20;
            public Int32 overrideSource3;
            Int32 buffer21;
            public Int32 overrideSource4;
            Int32 buffer22;
            public Int32 overrideSource5;
            Int32 buffer23;
            public Int32 overrideDest1;
            Int32 buffer24;
            public Int32 overrideDest2;
            Int32 buffer25;
            public Int32 overrideDest3;
            Int32 buffer26;
            public Int32 overrideDest4;
            Int32 buffer27;
            public Int32 overrideDest5;
            Int32 buffer28;
            public Int32 particleFolder;
            Int32 buffer29;
            public Int32 pickupFunction;
            Int32 buffer30;
            public Int32 triggerString1;
            Int32 buffer31;
            public Int32 triggerString2;
            Int32 buffer32;
            public Int32 charSelectFont;
            Int32 buffer33;
            public Int32 tooltipDamageIcon;
            Int32 buffer34;
            public Int32 damagingMeleeParticle;
            Int32 buffer35;
            public Int32 muzzleFlash;
            Int32 buffer36;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 904)]
            byte[] unknown02;
            public Int32 impUnitPhysical;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 184)]
            byte[] unknown03;
            public Int32 spawn;
            public Int32 properties1;
            public Int32 behaviour;
            public Int32 dontPath;
            public Int32 action;
            public Int32 affixes;
            public Int32 properties2;
            public Int32 titleString;
            public Int32 typeDescription;
            public Int32 flavorText;
            public Int32 additionalDescription;
            public Int32 additionalRaceDescription;
            public Int32 analyze;
            public Int32 recipeList;
            public Int32 recipeSingleUse;
            public Int32 paperdollBackgroundLevel;
            public Int32 paperdollWeapon1;
            public Int32 paperdollWeapon2;
            public Int32 paperdollSkill;
            public Int32 paperdollColorset;
            public Int32 respawnChance;
            public Int32 respawnSpawnclass;
            public Int32 code1;
            public Int32 code2;
            public Int32 densityValueOverride;
            public Int32 minionPackSize;
            public float spinSpeed;
            public float maxTurnRate;
            public Int32 unitType;
            public Int32 unitTypeForLeveling;
            public Int32 preferedByUnitType;
            public Int32 family;
            public Int32 censorClassNoHumans;
            public Int32 censorClassNoGore;
            public Int32 sex;
            public Int32 race;
            public Int32 rarity;
            public Int32 spawnChance;
            public Int32 minMonsterExperienceLevel;
            public Int32 level;
            public Int32 monsterQuality;
            public Int32 monsterClassAtUniqueQuality;
            public Int32 monsterClassAtChampionQuality;
            public Int32 minionClass;
            public Int32 monsterNameType;
            public Int32 quality;
            public Int32 angerRange;
            public Int32 baseLevel;
            public Int32 capLevel;
            public Int32 minMerchantLevel;
            public Int32 maxMerchantLevel;
            public Int32 minSpawnLevel;
            public Int32 maxSpawnLevel;
            public Int32 maxLevel;
            public Int32 fixedLevel;
            public Int32 hpMin;
            public Int32 hpMax;
            public Int32 powerMax;
            public Int32 experience;
            public Int32 attackRating;
            public Int32 luckBonus;
            public Int32 luckChanceToSpawn;
            public Int32 roomPopulatePass;
            public Int32 weaponBoneIndex;
            public Int32 requiresAffixOrSuffix;
            public float autoPickupDistance;
            public Int32 pickupPullState;
            public float extraDyingTimeInSeconds;
            public Int32 npcInfo;
            public Int32 balanceTestCount;
            public Int32 balanceTestGroup;
            public Int32 merchantStartingPane;
            public Int32 merchantFactionType;
            public Int32 merchantFactionValueNeeded;
            public Int32 questRequirement;
            public float noSpawnRadius;
            public float monitorApproachRadius;
            public Int32 tasksGeneratedStat;
            public float serverMissileOffset;
            public float homingTurnAngleRadians;
            public float homingModBasedDis;
            public float homeAfterUnitRadius;
            public float collidableAfterXSeconds;
            public float homeAfterXSeconds;
            public Int32 bounceOnUnitHit;
            public float impactCameraShakeDuration;
            public float impactCameraShakeMagnitude;
            public float impactCameraShakeDegrade;
            public Int32 maximumImpactFrequency;
            public Int32 onlyCollideWithUnitType;
            public Int32 questDescription;
            public Int32 pickUpCondition;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknown04;
            public Int32 scriptOnUse;
            public Int32 stackSize;
            public Int32 maxPickUp;
            public Int32 baseCost;
            public Int32 realWorldCost;
            public Int32 buyPriceMult;
            public Int32 buyPriceAdd;
            public Int32 sellPriceMult;
            public Int32 sellPriceAdd;
            public Int32 inventoryWardrobe;
            public Int32 characterScreenWardrobe;
            public Int32 characterScreenState;
            public Int32 wardrobeBody;
            public Int32 wardrobeFallback;
            public Int32 wardrobeUnkown1;
            public Int32 wardrobeMip;
            public Int32 wardrobeAppearanceGroup;
            public Int32 wardrobeAppearanceGroup1st;
            public Int32 startingStance;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] unknown06;
            public Int32 containerUnitType1;
            public Int32 containerUnitType2;
            public Int32 containerUnitType3;
            public Int32 containerUnitType4;
            public Int32 firingErrorIncrease;
            public Int32 firingErrorDecrease;
            public Int32 firingErrorMax;
            public Int32 accuracyBase;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] unknown07;
            public Int32 refillHotKey;
            public Int32 animGroup;
            public Int32 meleeWeapon;
            public Int32 cdTicks;
            public Int32 approxDps;
            public Int32 tooltipDamageString;
            public Int32 requiredAffixGroups1;
            public Int32 requiredAffixGroups2;
            public Int32 requiredAffixGroups3;
            public Int32 requiredAffixGroups4;
            public Int32 requiredAffixGroups5;
            public Int32 requiredAffixGroups6;
            public Int32 requiredAffixGroups7;
            public Int32 requiredAffixGroups8;
            public Int32 spawnMonsterClass;
            public Int32 safeState;
            public Int32 skillGhost;
            public Int32 skillRef;
            public Int32 dmgType;
            public Int32 weaponDamageScale;
            public Int32 dontUseWeaponDamage;
            public Int32 minBaseDmg;
            public Int32 maxBaseDmg;
            public Int32 sfxAttackFocus;
            public Int32 sfxUknown1;
            public Int32 sfxUknown2;
            public Int32 sfxUknown3;
            public Int32 sfxUknown4;
            public Int32 sfxUknown5;
            public Int32 sfxPhysicalAbility;
            public Int32 sfxPhysicalDefense;
            public Int32 sfxPhysicalKnockbackInCm;
            public Int32 sfxPhysicalStunDurationInSeconds;
            public Int32 sfxUknown6;
            public Int32 sfxFireAbility;
            public Int32 sfxFireDefense;
            public Int32 sfxFireDamagePercent1;
            public Int32 sfxFireDamagePercent2;
            public Int32 sfxFireDamagePercent3;
            public Int32 sfxElectricAbility;
            public Int32 sfxElectricDefense;
            public Int32 sfxElectricDamagePercentOfInitialPerShock;
            public Int32 sfxElectricDurationInSeconds;
            public Int32 sfxUknown7;
            public Int32 sfxSpectralAbility;
            public Int32 sfxSpectralDefense;
            public Int32 sfxSpectralSpecial;
            public Int32 sfxSpectralDurationInSeconds;
            public Int32 sfxUknown8;
            public Int32 sfxToxicAbility;
            public Int32 sfxToxicDefense;
            public Int32 sfxToxicDamageAsPercentageOfDamageDeliveredFromAttack;
            public Int32 sfxToxicDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            byte[] unknown12;
            public Int32 sfxPoisonAbility;
            public Int32 sfxPoisonDefense;
            public Int32 sfxPoisonDamageAsPercentageOfDamageDeliveredFromAttack;
            public Int32 sfxPoisonDurationInSeconds;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
            byte[] unknown13;
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
            byte[] unknown15;
            public Int32 maxArmor;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 44)]
            byte[] unknown16;
            public Int32 shields;
            public Int32 shieldPhys;
            public Int32 shieldFire;
            public Int32 shieldElec;
            public Int32 shieldSpec;
            public Int32 shieldToxic;
            public Int32 shieldUnknown1;
            public Int32 shieldPoison;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] unknown18;
            public Int32 strengthPercent;
            public Int32 dexterityPercent;
            public Int32 startingAccuracy;
            public Int32 startingStrength;
            public Int32 startingStamina;
            public Int32 startingWillpower;
            public Int32 props1;
            public Int32 props2;
            public Int32 props3;
            public Int32 props4;
            public Int32 props5;
            public Int32 props1AppliesToUnitype;
            public Int32 props2AppliesToUnitype;
            public Int32 props3AppliesToUnitype;
            public Int32 props4AppliesToUnitype;
            public Int32 props5AppliesToUnitype;
            public Int32 perLevelProps1;
            public Int32 perLevelProps2;
            public Int32 propsElite;
            public Int32 affix1;
            public Int32 affix2;
            public Int32 affix3;
            public Int32 affix4;
            public Int32 affix5;
            public Int32 affix6;
            public Int32 treasure;
            public Int32 championTreasure;
            public Int32 firstTimeTreasure;
            public Int32 inventory;
            public Int32 recipeIngredientInvLoc;
            public Int32 recipeResultInvLoc;
            public Int32 startingTreasure;
            public Int32 invWidth;
            public Int32 invHeight;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] unknown19;
            public Int32 requiredQuality;
            public Int32 qualityName;
            public Int32 fieldMissile;
            public Int32 skillHitUnit;
            public Int32 skillHitBackground;
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
            byte[] unknown20;
            public Int32 skillOnFuse;
            public Int32 skillOnDamageRepeat;
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
            public Int32 unitDieBeginSkill;
            public Int32 unitDieBeginSkillClient;
            public Int32 scriptOnUnitDieEnd;
            public Int32 unitDieEndSkill;
            public Int32 deadSkill;
            public Int32 kickSkill;
            public Int32 rightSkill;
            public Int32 initSkill;
            public Int32 skillLevelActive;
            public Int32 initState1;
            public Int32 initState2;
            public Int32 initState3;
            public Int32 initState4;
            public Int32 initStateTicks;
            public Int32 dyingState;
            public Int32 skillWhenUsed;
            public Int32 SkillTab;
            public Int32 SkillTab2;
            public Int32 SkillTab3;
            public Int32 SkillTab4;
            public Int32 SkillTab5;
            public Int32 SkillTab6;
            public Int32 SkillTab7;
            public Int32 levelUpState;
            public Int32 sounds;
            public Int32 flippySound;
            public Int32 invPickUpSound;
            public Int32 invPutDownSound;
            public Int32 invEquipSound;
            public Int32 pickUpSound;
            public Int32 useSound;
            public Int32 cantUseSound;
            public Int32 cantFireSound;
            public Int32 blockSound;
            public Int32 getHit0;
            public Int32 getHit1;
            public Int32 getHit2;
            public Int32 getHit3;
            public Int32 getHit4;
            public Int32 getHit5;
            public Int32 interactSound;
            public Int32 damagingSound;
            public Int32 forwardFootStepLeft;
            public Int32 forwardFootStepRight;
            public Int32 backwardFootStepLeft;
            public Int32 backwardFootStepRight;
            public Int32 firstPersonJumpFootStep;
            public Int32 firstPersonLandFootStep;
            public Int32 outOfManaSound;
            public Int32 inventoryFullSound;
            public Int32 cantSound;
            public Int32 CantUseSound;
            public Int32 cantUseYetSound;
            public Int32 cantCastSound;
            public Int32 cantCastYetSound;
            public Int32 cantCastHereSound;
            public Int32 lockedSound;
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
            public Int32 rangeMin;
            public Int32 rangeMax;
            public Int32 decoyMonster;
            public Int32 havokShape;
            public Int32 missileHitUnit;
            public Int32 missileHitBackground;
            public Int32 missileOnFreeOrFuse;
            public Int32 missileTag;
            public Int32 damageRepeatRate;
            public Int32 damageRepeatChance;
            public Int32 repeatDamageImmediately;
            public Int32 serverSrcDamage;
            public Int32 blockGhosts;
            public Int32 monster;
            public Int32 triggerType;
            public Int32 operatorStatesTriggerProhibited;
            public Int32 sublevelDest;
            public Int32 operateRequiredQuest;
            public Int32 operateRequiredQuestState;
            public Int32 operateRequiredQuestStateValue;
            public Int32 operateProhibitedQuestStateValue;
            public Int32 requiresItemOfUnitType1;
            public Int32 spawnTreasureClassInLevel;
            public Int32 oneWayVisualPortalDir;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknown21;
            public float labelScale;
            public float labelForwardOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] unknown22;
            public Int32 hasAppearanceShape;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            byte[] unknown23;
            public Int32 colorSet;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] unknown24;
            public Int32 globalThemeRequired;
            public Int32 levelThemeRequired;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknown25;
            public Int32 undefined1;
            public Int32 undefined2;
            public Int32 undefined3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)]
            byte[] unknown26;
            public Int32 undefined4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            byte[] unknown27;
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
