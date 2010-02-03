using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Skills : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SkillsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String skill;
            public Int32 code;
            Int32 buffer;
            public Int32 bitmask1;/*0 bit unknown unknown
	2 bit uses weapon
	3 bit weapon is required
	4 bit use weapon targeting
	5 bit uses weapon skill
	6 bit use weapon cooldown
	7 bit cooldown unit instead of weapon
	9 bit use weapon icon
	10 bit use all weapons
	11 bit combine weapon damage
	12 bit uses unit firing error
	13 bit display firing error
	15 bit check LOS
	16 bit no low aiming 3rd person
	17 bit no high aiming 3rd person
	18 bit can target unit
	19 bit find target unit
	20 bit must target unit
	21 bit must not target unit
	22 bit monster must target unit
	23 bit cannot retarget
	24 bit verify target
	25 bit verify target on request
	26 bit targets position
	27 bit keep target position on request
	28 bit target pos in stat
	29 bit target dead
	30 bit target dying on start
	31 bit target dying after start*/
            public Int32 bitmask2;/*32 bit target selectable dead
	33 bit target friend
	34 bit target enemy
	35 bit target container
	36 bit target destructables
	37 bit target only dying
	38 bit don't target destructables
	39 bit target pets
	40 bit ignore teams
	41 bit allow untargetables
	42 bit ui uses target
	43 bit don't face target
	44 bit must face forward
	45 bit aim at target
	47 bit use mouse skill's targeting
	48 bit is melee
	49 bit do melee item events
	50 bit can delay melee
	51 bit dead can do
	52 bit stopall
	53 bit stop on dead
	54 bit start on select
	55 bit always selected
	57 bit highlight when selected
	58 bit repeat fire
	59 bit repeat all
	60 bit hold
	61 bit loop mode
	62 bit stop holding skills
	63 bit hold other skills*/
            public Int32 bitmask3;/*64 bit prevent other skills
	65 bit prevent skills by priority
	66 bit run to target
	67 bit skill is on
	68 bit on ground only
	69 bit learnable
	70 bit hotkeyable
	71 bit can go in mouse button
	72 bit can go in left mouse button
	73 bit uses power
	74 bit drains power
	75 bit adjust power by level
	76 bit power cost bounded by max power
	77 bit allow request
	78 bit track request
	79 bit track metrics
	80 bit save missiles
	81 bit remove missiles on stop
	82 bit stop on collied
	83 bit no player skill input
	84 bit no player movement input
	85 bit no idle on stop
	86 bit do not clear remove on move states
	87 bit use range
	88 bit display range
	89 bit get hit can do
	90 bit moving can't do
	91 bit player stop moving
	92 bit constant cooldown
	93 bit ignore cooldown on start
	94 bit play cooldown on weapon
	95 bit use unit's cooldown*/
            public Int32 bitmask4;/*96 bit display cooldown
	97 bit check range on start
	98 bit check melee range on start
	99 bit dont use weapon range
	100 bit can start in town
	101 bit can't start in pvp
	102 bit can start in rts
	103 bit always test can start skill
	104 bit is aggressive
	105 bit angers target on execute
	106 bit is ranged
	107 bit is spell
	108 bit server only
	109 bit client only
	110 bit check inventory space
	111 bit activator while moving
	112 bit activator ignore moving
	113 bit can not do in hellrift
	114 bit ui is red on falback
	115 bit impact uses aim bone
	116 bit decoy cannot use
	119 bit do not prefer for mouse
	120 bit use holy aura for range
	122 bit disallow same skill
	123 bit don't use range for melee events
	124 bit force skill range for melee events
	125 bit disabled
	126 bit skill level from state target
	127 bit uses item requirements*/
            public Int32 bitmask5;/*128 bit prevent animation cutoff
	129 bit moves unit
	130 bit selects a melee skill
	131 bit requires skill level
	132 bit disable clientside pathing
	133 bit execute requested skill on melee attack
	134 bit can be executed from melee attack
	135 bit don't stop request after execute
	136 bit lerp camera while on
	137 bit force use weapon targeting
	138 bit don't clear cooldown on death
	139 bit don't cooldown on start
	140 bit power on event
	142 bit default shift skill enabled
	143 bit shift skill always enabled
	144 bit can kill pets for power cost
	145 bit reduce power max by power cost
	146 bit skill from unit event trigger needs dam
	147 bit ai is busy while on
	148 bit don't select on purchase
	150 bit never set cooldown
	151 bit ignore prevent all skills
	152 bit must start in portal safe level
	154 bit don't target pets
	155 bit require path to target
	156 bit fire to location
	157 bit test firing cone on start
	158 bit ignore champions
	159 bit face target position*/
            public Int32 bitmask6;/*160 bit target dead and alive
	161 bit restart skill on unit reactivate
	162 bit subscription required to learn
	163 bit subscription required to use
	164 bit force ui to show effect
	165 bit don't ignore owned state
	166 bit ghost can do
	167 bit use bone for missile position
	168 bit transfer damage to pets
	169 bit dont stagger
	170 bit does not actively use weapon*/
            public Int32 displayName;//stridx
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String descriptionStringFunction;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String effectStringFunction;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
            public String descriptionStringFunction;//doesn't appear to be used
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
            public String effectStringFunction;//doesn't appear to be used
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] unknown1;
            public Int32 descriptionString;//stridx
            public Int32 effectString;//stridx
            public Int32 skillBonusString;//stridx
            public Int32 accumulationString;//stridx
            public Int32 stringAfterRequiredWeapon;//stridx
            public Int32 skillsToAccumulate1;
            public Int32 skillsToAccumulate2;
            public Int32 skillsToAccumulate3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public String events;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String activator;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String largeIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String smallIcon;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] unknown2;
            public Int32 iconColor;
            public Int32 iconBackgroundColor;
            public Int32 skillTab;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 124)]
            byte[] unknown3;
            public Int32 skillGroup1;
            public Int32 skillGroup2;
            public Int32 skillGroup3;
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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknown4;
            public Int32 summonedInvLocation1;
            public Int32 summonedInvLocation2;
            public Int32 summonedInvLocation3;
            public Int32 summonedInvLocation4;
            public Int32 summonedInvLocation5;
            public Int32 summonedInvLocation6;
            public Int32 maxSummonedMinorPetClasses;
            public Int32 uiThrobsOnState;//idx
            public float powerCost;
            public float powerCostPerLevel;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] unknown5;
            public Int32 priority;
            public Int32 requiredStats1;
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
            public Int32 requiredSkills1;
            public Int32 requiredSkills2;
            public Int32 requiredSkills3;
            public Int32 requiredSkills4;
            public Int32 levelsOfRequiredSkills1;
            public Int32 levelsOfRequiredSkills2;
            public Int32 levelsOfRequiredSkills3;
            public Int32 levelsOfRequiredSkills4;
            public Int32 bOnlyRequireOne;//bool
            public Int32 bUsesCraftingPoints;
            public Int32 weaponLocation1;
            public Int32 weaponLocation2;
            public Int32 fallBackSkills1;
            public Int32 fallBackSkills2;
            public Int32 fallBackSkills3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 68)]
            byte[] unknown6;
            public Int32 targetUnittype;//idx
            public Int32 ignoreUnitType;//idx
            public Int32 ignoreTargetsWithState1;
            public Int32 ignoreTargetsWithState2;
            public Int32 selectTargetsWithState1;
            public Int32 selectTargetsWithState2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] unknown7;
            public Int32 modeOverride;//idx
            public Int32 bonusSkills1;
            public Int32 bonusSkills2;
            public Int32 bonusSkills3;
            public Int32 bonusSkills4;
            public Int32 bonusSkills5;
            public Int32 bonusSkillScript0;
            public Int32 bonusSkillScript1;
            public Int32 bonusSkillScript2;
            public Int32 bonusSkillScript3;
            public Int32 bonusSkillScript4;
            public Int32 skillVar0;//from here down are all intptr
            public Int32 skillVar1;
            public Int32 skillVar2;
            public Int32 skillVar3;
            public Int32 skillVar4;
            public Int32 skillVar5;
            public Int32 skillVar6;
            public Int32 skillVar7;
            public Int32 skillVar8;
            public Int32 skillVar9;
            public Int32 rangeMultScript;//(gets div by 100)
            public Int32 cost;
            public Int32 coolDownPercentChange;
            public Int32 statsTransferOnAttack;
            public Int32 statsSkillEvent;
            public Int32 statsSkillEventServer;
            public Int32 statsSkillEventServerPostProcess;
            public Int32 statsServerPostLaunch;
            public Int32 statsServerPostLaunchPost;
            public Int32 statsPostLaunch;
            public Int32 statsOnStateSet;
            public Int32 statsServerOnStateSet;
            public Int32 statsServerOnStateSetPost;
            public Int32 statsOnStateSetPost;
            public Int32 statsOnPulseServerOnly;
            public Int32 statsOnDeSelectServerOnly;
            public Int32 statsOnPulse;
            public Int32 statsOnLevelChange;
            public Int32 statsOnSkillStart;
            public Int32 statsScriptOnTarget;
            public Int32 scriptFromScriptEvents;
            public Int32 selectCost;
            public Int32 startCondition;
            public Int32 startConditionOntarget;
            public Int32 stateDurationInTicks;
            public Int32 stateDurationBonus;
            public Int32 activatorCondition;
            public Int32 activatorConditionOnTarget;
            public Int32 eventChance;
            public Int32 eventParam0;
            public Int32 eventParam1;
            public Int32 eventParam2;
            public Int32 eventParam3;
            public Int32 infoScript;
            public Int32 stateRemovedServer;
            public Int32 powerCostScript;
            public Int32 coolDownSkillScript;
            public Int32 skillOnpulse;//idx
            public Int32 selectCheckStat;//idx
            public Int32 startFunc;
            public Int32 targetFunc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] unknown8;
            public Int32 givesSkill;//idx
            public Int32 extraSkillToTurnOn;//idx
            public Int32 playerInputOverride;
            public Int32 requiresUnitType;//idx
            public Int32 requiresWeaponUnitType;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] unknown9;
            public Int32 fuseMissilesOnStateClear;//idx
            public Int32 requiresState;//idx
            public Int32 prohibitingState;//idx
            public Int32 prohibitingState;//idx
            public Int32 prohibitingState;//idx
            public Int32 prohibitingState;//idx
            public Int32 stateOnSelect;//idx
            public Int32 clearStateOnSelect;//idx
            public Int32 holdTicks;
            public Int32 holdWithMode;//idx
            public Int32 warmUpTicks;
            public Int32 testTicks;
            public Int32 coolDown;
            public Int32 coolDownSkillGroup;//idx
            public Int32 coolDownForGroup;
            public Int32 coolDownFinishedSound;//idx
            public Int32 coolDownMinPercent;
            public Int32 activatorKey;
            public Int32 activateMode;//idx
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
            public Int32 damageTypeOverride;//idx
            public float damageMultiplier;
            public Int32 maxExtraSpreadBullets;
            public Int32 spreadBulletMultiplier;
            public float reflectiveLifeTimeInSeconds;
            public float Param1;
            public float Param2;
            public Int32 usage;
            public Int32 family;
            public Int32 unitEventTrigger0;
            public Int32 unitEventTrigger1;
            public Int32 unitEventTrigger2;
            public Int32 unitEventTrigger3;
            public Int32 unitEventTriggerChanceScript;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte[] unknown10;
            public Int32 linkedLevelSkill0;
            public Int32 linkedLevelSkill1;
            public Int32 linkedLevelSkill2;
            public Int32 skillParent;
            public Int32 fieldMissile;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] unknown11;
            
        }

        public Skills(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SkillsTable>(data, ref offset, Count);
        }
    }
}
