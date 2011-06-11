using System;
using System.Runtime.InteropServices;
using Hellgate.Xml;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SkillsRow
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String skill;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 buffer;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask01 bitmask1;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask02 bitmask2;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask03 bitmask3;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask04 bitmask4;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask05 bitmask5;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask06 bitmask6;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayName;//stridx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String descriptionStringFunction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String effectStringFunction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
        public String skillBonusFunction;//doesn't appear to be used
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
        public String accumulationStringFunction;//doesn't appear to be used
        public Int32 unknown101;
        public Int32 unknown102;
        public Int32 unknown103;
        public Int32 unknown104;
        public Int32 unknown105;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 descriptionString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 effectString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 skillBonusString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 accumulationString;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringAfterRequiredWeapon;//stridx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillsToAccumulate1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillsToAccumulate2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillsToAccumulate3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String events;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String activator;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String largeIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String smallIcon;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] unknown2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 iconColor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 iconBackgroundColor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLTABS")]
        public Int32 skillTab;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 124)]
        byte[] unknown3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 skillGroup4;
        public Int32 skillPageColumn;
        public Int32 skillPageRow;
        public Int32 level1;
        public Int32 level2;
        public Int32 level3;
        public Int32 level4;
        public Int32 level5;
        public Int32 level6;
        public Int32 level7;
        public Int32 level8;
        public Int32 level9;
        public Int32 level10;
        public Int32 level11;
        public Int32 level12;
        public Int32 level13;
        public Int32 level14;
        public Int32 level15;
        public Int32 maxLevel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String summonedAi;
        public Int32 unknown201;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 summonedInvLocation6;
        public Int32 maxSummonedMinorPetClasses;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 uiThrobsOnState;//idx
        public float powerCost;
        public float powerCostPerLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown5;
        public Int32 perkPointCost1;
		public Int32 perkPointCost2;
		public Int32 perkPointCost3;
		public Int32 perkPointCost4;
		public Int32 perkPointCost5;
		public Int32 perkPointCost6;
		public Int32 perkPointCost7;
		public Int32 perkPointCost8;
		public Int32 perkPointCost9;
		public Int32 perkPointCost10;
		public Int32 perkPointCost11;
		public Int32 perkPointCost12;
		public Int32 perkPointCost13;
		public Int32 perkPointCost14;
		public Int32 perkPointCost15;		
        public Int32 priority;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 requiredStats1;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 requiredStats2;
        public Int32 requiredStatValuesA1;//these seem to be unused
        public Int32 requiredStatValuesA2;
        public Int32 requiredStatValuesA3;
        public Int32 requiredStatValuesA4;
        public Int32 requiredStatValuesA5;
        public Int32 requiredStatValuesA6;
        public Int32 requiredStatValuesA7;
        public Int32 requiredStatValuesA8;
        public Int32 requiredStatValuesA9;
        public Int32 requiredStatValuesA10;
        public Int32 requiredStatValuesA11;
        public Int32 requiredStatValuesA12;
        public Int32 requiredStatValuesA13;
        public Int32 requiredStatValuesA14;
        public Int32 requiredStatValuesA15;
        public Int32 requiredStatValuesB1;
        public Int32 requiredStatValuesB2;
        public Int32 requiredStatValuesB3;
        public Int32 requiredStatValuesB4;
        public Int32 requiredStatValuesB5;
        public Int32 requiredStatValuesB6;
        public Int32 requiredStatValuesB7;
        public Int32 requiredStatValuesB8;
        public Int32 requiredStatValuesB9;
        public Int32 requiredStatValuesB10;
        public Int32 requiredStatValuesB11;
        public Int32 requiredStatValuesB12;
        public Int32 requiredStatValuesB13;
        public Int32 requiredStatValuesB14;
        public Int32 requiredStatValuesB15;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 requiredSkills4;
        public Int32 levelsOfRequiredSkills1;
        public Int32 levelsOfRequiredSkills2;
        public Int32 levelsOfRequiredSkills3;
        public Int32 levelsOfRequiredSkills4;
        [ExcelOutput(IsBool = true)]
        public Int32 bOnlyRequireOne;
        [ExcelOutput(IsBool = true)]
        public Int32 bUsesCraftingPoints;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 weaponLocation1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 weaponLocation2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 fallBackSkills1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 fallBackSkills2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 fallBackSkills3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 68)]
        byte[] unknown6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 targetUnittype;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ignoreUnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 ignoreTargetsWithState1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 ignoreTargetsWithState2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 selectTargetsWithState1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 selectTargetsWithState2;
        public Int32 unknown301;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 modeOverride;//idx
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills0;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills1;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 bonusSkills4;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript0;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript1;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript2;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript3;
        [ExcelOutput(IsScript = true)]
        public Int32 bonusSkillScript4;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar0;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar1;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar2;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar3;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar4;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar5;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar6;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar7;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar8;
        [ExcelOutput(IsScript = true)]
        public Int32 skillVar9;
        [ExcelOutput(IsScript = true)]
        public Int32 rangeMultScript;
        [ExcelOutput(IsScript = true)]
        public Int32 cost;
        [ExcelOutput(IsScript = true)]
        public Int32 coolDownPercentChange;
        [ExcelOutput(IsScript = true)]
        public Int32 statsSkillEvent;
        [ExcelOutput(IsScript = true)]
        public Int32 statsTransferOnAttack;
        [ExcelOutput(IsScript = true)]
        public Int32 statsSkillEventServer;
        [ExcelOutput(IsScript = true)]
        public Int32 statsSkillEventServerPostProcess;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerPostLaunch;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerPostLaunchPost;
        [ExcelOutput(IsScript = true)]
        public Int32 statsPostLaunch;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnStateSet;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerOnStateSet;
        [ExcelOutput(IsScript = true)]
        public Int32 statsServerOnStateSetPost;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnStateSetPost;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnPulseServerOnly;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnDeSelectServerOnly;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnPulse;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnLevelChange;
        [ExcelOutput(IsScript = true)]
        public Int32 statsOnSkillStart;
        [ExcelOutput(IsScript = true)]
        public Int32 statsScriptOnTarget;
        [ExcelOutput(IsScript = true)]
        public Int32 scriptFromScriptEvents;
        [ExcelOutput(IsScript = true)]
        public Int32 selectCost;
        [ExcelOutput(IsScript = true)]
        public Int32 startCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 startConditionOntarget;
        [ExcelOutput(IsScript = true)]
        public Int32 stateDurationInTicks;
        [ExcelOutput(IsScript = true)]
        public Int32 stateDurationBonus;
        [ExcelOutput(IsScript = true)]
        public Int32 activatorCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 activatorConditionOnTarget;
        [ExcelOutput(IsScript = true)]
        public Int32 eventChance;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam0;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam1;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam2;
        [ExcelOutput(IsScript = true)]
        public Int32 eventParam3;
        [ExcelOutput(IsScript = true)]
        public Int32 infoScript;
        [ExcelOutput(IsScript = true)]
        public Int32 stateRemovedServer;
        [ExcelOutput(IsScript = true)]
        public Int32 powerCostScript;
        [ExcelOutput(IsScript = true)]
        public Int32 coolDownSkillScript;
        [ExcelOutput(IsScript = true)]
        public Int32 selectCondition;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillOnpulse;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 selectCheckStat;//idx
        public Int32 startFunc;
        public Int32 targetFunc;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 givesSkill;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 extraSkillToTurnOn;//idx
        public Int32 playerInputOverride;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiresUnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiresWeaponUnitType;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 fuseMissilesOnStateClear;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 requiresState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState0;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 prohibitingState3;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 stateOnSelect;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 clearStateOnSelect;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 preventClearStateOnSelect;//idx
        public Int32 holdTicks;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 holdWithMode;//idx
        public Int32 warmUpTicks;
        public Int32 testTicks;
        public Int32 coolDown;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLGROUPS")]
        public Int32 coolDownSkillGroup;//idx
        public Int32 coolDownForGroup;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 coolDownFinishedSound;//idx
        public Int32 coolDownMinPercent;
        public Int32 activatorKey;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 activateMode;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 activateSkill;//idx
        public Int32 activatePriority;
        public float rangeMin;
        public float rangeMax;
        public float firingCone;
        public float rangeDesired;
        public float rangePercentPerLevel;
        public float weaponRangeMultiplier;
        public float impactForwardBias;
        public float modeSpeed;
        [ExcelOutput(IsTableIndex = true, TableStringId = "DAMAGETYPES")]
        public Int32 damageTypeOverride;//idx
        public float damageMultiplier;
        public Int32 maxExtraSpreadBullets;
        public Int32 spreadBulletMultiplier;
        public float reflectiveLifeTimeInSeconds;
        public float Param1;
        public float Param2;
        public Int32 usage;
        public Int32 family;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNIT_EVENT_TYPES")]
        public Int32 unitEventTrigger3;
        public Int32 unitEventTriggerChanceScript;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] unknown10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 linkedLevelSkill0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 linkedLevelSkill1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 linkedLevelSkill2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 skillParent;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 fieldMissile;
        public Int32 termForSharedStash;
        public XmlSkillEventsDefinition SkillEvents; // custom row - was called unknown11
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] unknown11;

        [FlagsAttribute]
        public enum Bitmask01 : uint
        {
            byte_934961	= 1,//0
			_undefined1 = 2,
			usesWeapon = 4,	//2
			usesWeaponBuffsOnly = 8,	//3
			weaponIsRequired = 16,	//4
			useWeaponTargeting = 32,	//5
			usesWeaponSkill = 64,	//6
			useWeaponCooldown = 128,	//7
			cooldownUnitInsteadOfWeapon = 256,	//8
			_undefined9 = 512,
			useWeaponIcon = 1024,	//A
			useAllWeapons = 2048,	//B
			combineWeaponDamage = 4096,	//C
			usesUnitFiringError = 8192,	//D
			displayFiringError = 16384,	//E
			_undefinedF = 32768,
			checkLOS = 65536,	//10
			_undefined11 = 131072,
			_undefined12 = 262144,
			canTargetUnit = 524288,	//13
			findTargetUnit = 1048576,	//14
			mustTargetUnit = 2097152,	//15
			mustNotTargetUnit = 4194304,	//16
			monsterMustTargetUnit = 8388608,	//17
			cannotRetarget = 16777216,	//18
			verifyTarget = 33554432,	//19
			verifyTargetOnRequest = 67108864,	//1A
			targetsPosition = 134217728,	//1B
			keepTargetPositionOnRequest = 268435456,	//1C
			targetPosInStat = 536870912,	//1D
			targetDead = 1073741824,	//1E
			targetDyingOnStart = 2147483648	//1F
        }

        [FlagsAttribute]
        public enum Bitmask02 : uint
        {
            targetDyingAfterStart = 1,	//20
			targetSelectableDead = 2,	//21
			targetFriend = 4,	//22
			targetEnemy = 8,	//23
			targetContainer = 16,	//24
			targetDestructables = 32,	//25
			targetOnlyDying = 64,	//26
			doNotTargetDestructables = 128,	//27
			targetPets = 256,	//28
			ignoreTeams = 512,	//29
			allowUntargetables = 1024,	//2A
			uiUsesTarget = 2048,	//2B
			doNotFaceTarget = 4096,	//2C
			mustFaceForward = 8192,	//2D
			aimAtTarget = 16384,	//2E
			_undefined2F = 32768,
			useMouseSkillTargeting = 65536,	//30
			isMelee = 131072,	//31
			doMeleeItemEvents = 262144,	//32
			_undefined33 = 524288,
			canDelayMelee = 1048576,	//34
			deadCanDo = 2097152,	//35
			stopall = 4194304,	//36
			stopOnDead = 8388608,	//37
			startOnSelect = 16777216,	//38
			alwaysSelected = 33554432,	//39
			_undefined3A = 67108864,
			highlightWhenSelected = 134217728,	//3B
			repeatFire = 268435456,	//3C
			repeatAll = 536870912,	//3D
			hold = 1073741824,	//3E
			loopMode = 2147483648	//3F
        }

        [FlagsAttribute]
        public enum Bitmask03 : uint
        {
            stopHoldingSkills = 1,	//40
			holdOtherSkills = 2,	//41
			preventOtherSkills = 4,	//42
			preventSkillsByPriority = 8,	//43
			runToTarget = 16,	//44
			skillIsOn = 32,	//45
			onGroundOnly = 64,	//46
			learnable = 128,	//47
			hotkeyable = 256,	//48
			canGoInMouseButton = 512,	//49
			canGoInLeftMouseButton = 1024,	//4A
			usesPower = 2048,	//4B
			drainsPower = 4096,	//4C
			adjustPowerByLevel = 8192,	//4D
			powerCostBoundedByMaxPower = 16384,	//4E
			allowRequest = 32768,	//4F
			trackRequest = 65536,	//50
			trackMetrics = 131072,	//51
			saveMissiles = 262144,	//52
			removeMissilesOnStop = 524288,	//53
			stopOnCollide = 1048576,	//54
			noPlayerSkillInput = 2097152,	//55
			noPlayerMovementInput = 4194304,	//56
			noIdleOnStop = 8388608,	//57
			doNotClearRemoveOnMoveStates = 16777216,	//58
			useRange = 33554432,	//59
			displayRange = 67108864,	//5A
			getHitCanDo = 134217728,	//5B
			movingCanNotDo = 268435456,	//5C
			playerStopMoving = 536870912,	//5D
			constantCooldown = 1073741824,	//5E
			ignoreCooldownOnStart = 2147483648	//5F
        }

        [FlagsAttribute]
        public enum Bitmask04 : uint
        {
            playCooldownOnWeapon = 1,	//60
			useUnitCooldown = 2,	//61
			addMonstersCooldown = 4,	//62
			displayCooldown = 8,	//63
			checkRangeOnStart = 16,	//64
			checkMeleeRangeOnStart = 32,	//65
			dontUseWeaponRange = 64,	//66
			canStartInTown = 128,	//67
			canNotStartInPvp = 256,	//68
			_undefined69 = 512,
			canStartInRts = 1024,	//6A
			alwaysTestCanStartSkill = 2048,	//6B
			isAggressive = 4096,	//6C
			angersTargetOnExecute = 8192,	//6D
			isRanged = 16384,	//6E
			isNoneWeaponSkill = 32768,	//6F
			serverOnly = 65536,	//70
			clientOnly = 131072, //71
			checkInventorySpace = 262144,	//72
			activatorWhileMoving = 524288,	//73
			activatorIgnoreMoving = 1048576,	//74
			canNotDoInHellrift = 2097152,	//75
			uiIsRedOnFallback = 4194304,	//76
			impactUsesAimBone = 8388608,	//77
			decoyCannotUse = 16777216,	//78
			_undefined79 = 33554432,
			_undefined7a = 67108864,
			doNotPreferForMouse = 134217728,	//7B
			useHolyAuraForRange = 268435456,	//7C
			_undefined7D = 536870912,
			disallowSameSkill = 1073741824,	//7E
			doNotUseRangeForMeleeEvents = 2147483648	//7F
        }

        [FlagsAttribute]
        public enum Bitmask05 : uint
        {
            forceSkillRangeForMeleeEvents = 1,	//80
			disabled = 2,	//81
			skillLevelFromStateTarget = 4,	//82
			usesItemRequirements = 8,	//83	
			preventAnimationCutoff = 16,	//84
			movesUnit = 32,	//85
			selectsAMeleeSkill = 64,	//86
			requiresSkillLevel = 128,	//87
			disableClientsidePathing = 256,	//88
			executeRequestedSkillOnMeleeAttack = 512,	//89
			canBeExecutedFromMeleeAttack = 1024,	//8A
			doNotStopRequestAfterExecute = 2048,	//8B
			lerpCameraWhileOn = 4096,	//8C
			forceUseWeaponTargeting = 8192,	//8D
			doNotClearCooldownOnDeath = 16384,	//8E
			doNotCooldownOnStart = 32768,	//8F
			powerOnEvent = 65536,	//90
			_undefined91 = 131072,
			defaultShiftSkillEnabled = 262144,	//92
			shiftSkillAlwaysEnabled = 524288,	//93
			canKillPetsForPowerCost = 1048576,	//94
			reducePowerMaxByPowerCost = 2097152,	//95
			skillFromUnitEventTriggerNeedsDamageIncrement = 4194304,	//96
			aiIsBusyWhileOn = 8388608,	//97
			doNotSelectOnPurchase = 16777216,	//98
			_undefined99 = 33554432,
			neverSetCooldown = 67108864,	//9A
			ignorePreventAllSkills = 134217728,	//9B
			mustStartInPortalSafeLevel = 268435456,	//9C
			_undefined9D = 536870912,
			doNotTargetPets = 1073741824,	//9E
			requirePathToTarget = 2147483648	//9F
        }

        [FlagsAttribute]
        public enum Bitmask06 : uint
        {
            fireToLocation = 1,	//A0
			testFiringConeOnStart = 2,	//A1
			ignoreChampions = 4,	//A2
			faceTargetPosition = 8,	//A3
			targetDeadAndAlive = 16,	//A4
			restartSkillOnUnitReactivate = 32,	//A5
			subscriptionRequiredToLearn = 64,	//A6
			subscriptionRequiredToUse = 128,	//A7
			forceUiToShowEffect = 256,	//A8
			doNotIgnoreOwnedState = 512,	//A9
			ghostCanDo = 1024,	//AA
			useBoneForMissilePosition = 2048,	//AB
			transferDamagesToPets = 4096,	//AC
			doNotStagger = 8192,	//AD
			doesNotActivelyUseWeapon = 16384,	//AE
			isPlayerSSkill = 32768,	//AF
        }

        public static class Mysh
        {
            public static readonly byte[] Data = new byte[]
            {
                0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x78, 0x78, 0x00, 0x00, 0x00, 0x39, 0x00, 0x00,
                0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x6D, 0x79, 0x73, 0x68, 0x03, 0x00, 0x00,
                0x00, 0x03, 0x00, 0x00, 0x00, 0x73, 0x65, 0x6C,
                0x00, 0x76, 0x02, 0x00, 0x39, 0x00, 0x00, 0x00,
                0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x6D, 0x79, 0x73, 0x68, 0x03, 0x00, 0x00, 0x00,
                0x07, 0x00, 0x00, 0x00, 0x64, 0x6D, 0x67, 0x74,
                0x79, 0x70, 0x65, 0xF6, 0x03, 0x4F, 0x8B, 0x39,
                0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04,
                0x00, 0x00, 0x00 
            };
        }
    }
}
