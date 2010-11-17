using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel.Tc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillsTCv4Row
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String skill;
        public Int32 code;
        public Int32 buffer;
        [ExcelOutput(IsBitmask = true)]
        public Skills.Bitmask01 bitmask1;
        [ExcelOutput(IsBitmask = true)]
        public Skills.Bitmask02 bitmask2;
        [ExcelOutput(IsBitmask = true)]
        public Skills.Bitmask03 bitmask3;
        [ExcelOutput(IsBitmask = true)]
        public Skills.Bitmask04 bitmask4;
        [ExcelOutput(IsBitmask = true)]
        public Skills.Bitmask05 bitmask5;
        [ExcelOutput(IsBitmask = true)]
        public Skills.Bitmask06 bitmask6;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Skills")]
        public Int32 displayName;//stridx
        public Int32 skillLevelIncludesSkills1_tcv4;
        public Int32 skillLevelIncludesSkills2_tcv4;
        public Int32 skillLevelIncludesSkills3_tcv4;
        public Int32 skillLevelIncludesSkills4_tcv4;
        public Int32 skillLevelIncludesSkills5_tcv4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String descriptionStringFunction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String effectStringFunction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
        public String skillBonusFunction;//doesn't appear to be used
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 192)]
        public String accumulationStringFunction;//doesn't appear to be used
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknown1;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Skills")]
        public Int32 descriptionString;//stridx
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Skills")]
        public Int32 effectString;//stridx
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Skills")]
        public Int32 skillBonusString;//stridx
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Skills")]
        public Int32 accumulationString;//stridx
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Skills")]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        Int32[] empty3_tcv4;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        Int32[] levels4_tcv4;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
        Int32[] perkPointCost_tcv4;
        public Int32 priority;
        public Int32 requiredStats1;
        public Int32 requiredStats2;
        Int32 requiredStatValuesA1;//these seem to be unused
        Int32 requiredStatValuesA2;
        Int32 requiredStatValuesA3;
        Int32 requiredStatValuesA4;
        Int32 requiredStatValuesA5;
        Int32 requiredStatValuesA6;
        Int32 requiredStatValuesA7;
        Int32 requiredStatValuesA8;
        Int32 requiredStatValuesA9;
        Int32 requiredStatValuesA10;
        Int32 requiredStatValuesA11;
        Int32 requiredStatValuesA12;
        Int32 requiredStatValuesA13;
        Int32 requiredStatValuesA14;
        Int32 requiredStatValuesA15;
        Int32 requiredStatValuesA16;
        Int32 requiredStatValuesA17;
        Int32 requiredStatValuesA18;
        Int32 requiredStatValuesA19;
        Int32 requiredStatValuesA20;
        Int32 requiredStatValuesA21;
        Int32 requiredStatValuesA22;
        Int32 requiredStatValuesA23;
        Int32 requiredStatValuesA24;
        Int32 requiredStatValuesA25;
        Int32 requiredStatValuesA26;
        Int32 requiredStatValuesA27;
        Int32 requiredStatValuesA28;
        Int32 requiredStatValuesA29;
        Int32 requiredStatValuesA30;
        Int32 requiredStatValuesB1;
        Int32 requiredStatValuesB2;
        Int32 requiredStatValuesB3;
        Int32 requiredStatValuesB4;
        Int32 requiredStatValuesB5;
        Int32 requiredStatValuesB6;
        Int32 requiredStatValuesB7;
        Int32 requiredStatValuesB8;
        Int32 requiredStatValuesB9;
        Int32 requiredStatValuesB10;
        Int32 requiredStatValuesB11;
        Int32 requiredStatValuesB12;
        Int32 requiredStatValuesB13;
        Int32 requiredStatValuesB14;
        Int32 requiredStatValuesB15;
        Int32 requiredStatValuesB16;
        Int32 requiredStatValuesB17;
        Int32 requiredStatValuesB18;
        Int32 requiredStatValuesB19;
        Int32 requiredStatValuesB20;
        Int32 requiredStatValuesB21;
        Int32 requiredStatValuesB22;
        Int32 requiredStatValuesB23;
        Int32 requiredStatValuesB24;
        Int32 requiredStatValuesB25;
        Int32 requiredStatValuesB26;
        Int32 requiredStatValuesB27;
        Int32 requiredStatValuesB28;
        Int32 requiredStatValuesB29;
        Int32 requiredStatValuesB30;
        public Int32 requiredSkills1;
        public Int32 requiredSkills2;
        public Int32 requiredSkills3;
        public Int32 requiredSkills4;
        public Int32 levelsOfRequiredSkills1;
        public Int32 levelsOfRequiredSkills2;
        public Int32 levelsOfRequiredSkills3;
        public Int32 levelsOfRequiredSkills4;
        [ExcelOutput(IsBool = true)]
        public Int32 bOnlyRequireOne;
        [ExcelOutput(IsBool = true)]
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
        public Int32 bonusSkills0;
        public Int32 bonusSkills1;
        public Int32 bonusSkills2;
        public Int32 bonusSkills3;
        public Int32 bonusSkills4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 bonusSkillScript0;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 bonusSkillScript1;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 bonusSkillScript2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 bonusSkillScript3;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 bonusSkillScript4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar0;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar1;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar3;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar5;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar7;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar8;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 skillVar9;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 craftingScript_tcv4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 craftingPropertiesScript_tcv4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 rangeMultScript;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 cost;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 coolDownPercentChange;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsSkillEvent;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsTransferOnAttack;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsSkillEventServer;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsSkillEventServerPostProcess;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsServerPostLaunch;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsServerPostLaunchPost;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsPostLaunch;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnStateSet;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsServerOnStateSet;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsServerOnStateSetPost;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnStateSetPost;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnPulseServerOnly;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnDeSelectServerOnly;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnPulse;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnLevelChange;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsOnSkillStart;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 statsScriptOnTarget;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 scriptFromScriptEvents;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 selectCost;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 startCondition;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 startConditionOntarget;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 stateDurationInTicks;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 stateDurationBonus;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 stateDurationPercentByTarget;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 activatorCondition;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 activatorConditionOnTarget;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 eventChance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 eventParam0;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 eventParam1;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 eventParam2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 eventParam3;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 infoScript;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 stateRemovedServer;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 powerCostScript;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 powerCostPctMultScript;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 coolDownSkillScript;
        public Int32 selectCondition;
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
        public Int32 prohibitingState0;//idx
        public Int32 prohibitingState1;//idx
        public Int32 prohibitingState2;//idx
        public Int32 prohibitingState3;//idx
        public Int32 stateOnSelect;//idx
        public Int32 clearStateOnSelect;//idx
        public Int32 clearStateOnDeselect;//idx
        public Int32 preventClearStateOnSelect;//idx
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
        public Int32 unlockPurchaseItem_tcv4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] unknown11;
        public Int32 undefined10_tcv4;

        public abstract class Skills
        {
            [FlagsAttribute]
            public enum Bitmask01 : uint
            {
                _undefined1 = (1 << 0),
                _undefined2 = (1 << 1),
                usesWeapon = (1 << 2),
                weaponIsRequired = (1 << 3),
                weaponTargeting = (1 << 4),
                usesWeaponSkill = (1 << 5),
                useWeaponCooldown = (1 << 6),
                cooldownUnitInsteadOfWeapon = (1 << 7),
                _undefined3 = (1 << 8),
                useWeaponIcon = (1 << 9),
                useAllWeapons = (1 << 10),
                combineWeaponDamage = (1 << 11),
                usesUnitFiringError = (1 << 12),
                displayFiringError = (1 << 13),
                _undefined4 = (1 << 14),
                checkLOS = (1 << 15),
                noLowAiming3rdPerson = (1 << 16),
                noHighAiming3rdPerson = (1 << 17),
                canTargetUnit = (1 << 18),
                findTargetUnit = (1 << 19),
                mustTargetUnit = (1 << 20),
                mustNotTargetUnit = (1 << 21),
                monsterMustTargetUnit = (1 << 22),
                cannotRetarget = (1 << 23),
                verifyTarget = (1 << 24),
                verifyTargetOnRequest = (1 << 25),
                targetsPosition = (1 << 26),
                keepTargetPositionOnRequest = (1 << 27),
                targetPosInStart = (1 << 28),
                targetDead = (1 << 29),
                targetDyingOnStart = (1 << 30),
                targetDyingAfterStart = ((uint)1 << 31)
            }

            [FlagsAttribute]
            public enum Bitmask02 : uint
            {
                targetSelectableDead = (1 << 0),
                targetFriend = (1 << 1),
                targetEnemy = (1 << 2),
                targetContainer = (1 << 3),
                targetDestructables = (1 << 4),
                targetOnlyDying = (1 << 5),
                dontTargetDestructables = (1 << 6),
                targetPets = (1 << 7),
                ignoreTeams = (1 << 8),
                allowUntargetables = (1 << 9),
                targetObjects_tcv4 = (1 << 10),
                uiUsesTarget = (1 << 11),
                dontFaceTarget = (1 << 12),
                mustFaceForward = (1 << 13),
                aimAtTarget = (1 << 14),
                unknownbit5 = (1 << 15),
                useMouseSkillsTargeting = (1 << 16),
                isMelee = (1 << 17),
                doMeleeItemEvents = (1 << 18),
                canDelayMelee = (1 << 19),
                deadCanDo = (1 << 20),
                stopall = (1 << 21),
                stopOnDead = (1 << 22),
                startOnSelect = (1 << 23),
                alwaysSelected = (1 << 24),
                unknownbit6 = (1 << 25),
                highlightWhenSelected = (1 << 26),
                repeatFire = (1 << 27),
                repeatAll = (1 << 28),
                hold = (1 << 29),
                loopMode = (1 << 30),
                stopHoldingSkills = ((uint)1 << 31)
            }

            [FlagsAttribute]
            public enum Bitmask03 : uint
            {
                holdOtherSkills = (1 << 0),
                preventOtherSkills = (1 << 1),
                preventSkillsByPriority = (1 << 2),
                runToTarget = (1 << 3),
                skillIsOn = (1 << 4),
                onGroundOnly = (1 << 5),
                learnable = (1 << 6),
                hotkeyable = (1 << 7),
                canGoInMouseButton = (1 << 8),
                canGoInLeftMouseButton = (1 << 9),
                usesPower = (1 << 10),
                drainsPower = (1 << 11),
                adjustPowerByLevel = (1 << 12),
                powerCostBoundedByMaxPower = (1 << 13),
                allowRequest = (1 << 14),
                trackRequest = (1 << 15),
                trackMetrics = (1 << 16),
                saveMissiles = (1 << 17),
                removeMissilesOnStop = (1 << 18),
                stopOnCollide = (1 << 19),
                noPlayerSkillInput = (1 << 20),
                noPlayerMovementInput = (1 << 21),
                noIdleOnStop = (1 << 22),
                doNotClearRemoveOnMoveStates = (1 << 23),
                useRange = (1 << 24),
                displayRange = (1 << 25),
                getHitCanDo = (1 << 26),
                movingCantDo = (1 << 27),
                playerStopMoving = (1 << 28),
                constantCooldown = (1 << 29),
                ignoreCooldownOnStart = (1 << 30),
                playCooldownOnWeapon = ((uint)1 << 31)
            }

            [FlagsAttribute]
            public enum Bitmask04 : uint
            {
                useUnitsCooldown = (1 << 0),
                addMonstersCooldown_tcv4 = (1 << 1),
                displayCooldown = (1 << 2),
                checkRangeOnStart = (1 << 3),
                checkMeleeRangeOnStart = (1 << 4),
                dontUseWeaponRange = (1 << 5),
                canStartInTown = (1 << 6),
                cantStartInPvp = (1 << 7),
                canStartInRts = (1 << 8),
                alwaysTestCanStartSkill = (1 << 9),
                isAggressive = (1 << 10),
                angersTargetOnExecute = (1 << 11),
                isRanged = (1 << 12),
                isSpell = (1 << 13),
                serverOnly = (1 << 14),
                clientOnly = (1 << 15),
                checkInventorySpace = (1 << 16),
                activatorWhileMoving = (1 << 17),
                activatorIgnoreMoving = (1 << 18),
                canNotDoInHellrift = (1 << 19),
                uiIsRedOnFallback = (1 << 20),
                impactUsesAimBone = (1 << 21),
                decoyCannotUse = (1 << 22),
                unknownbit7 = (1 << 23),
                unknownbit8 = (1 << 24),
                doNotPreferForMouse = (1 << 25),
                useHolyAuraForRange = (1 << 26),
                unknownbit9 = (1 << 27),
                disallowSameSkill = (1 << 28),
                dontUseRangeForMeleeEvents = (1 << 29),
                forceSkillRangeForMeleeEvents = (1 << 30),
                disabled = ((uint)1 << 31)
            }

            [FlagsAttribute]
            public enum Bitmask05 : uint
            {
                skillLevelFromStateTarget = (1 << 0),
                usesItemRequirements = (1 << 1),
                preventAnimationCutoff = (1 << 2),
                movesUnit = (1 << 3),
                selectsAMeleeSkill = (1 << 4),
                requiresSkillLevel = (1 << 5),
                disableClientsidePathing = (1 << 6),
                executeRequestedSkillOnMeleeAttack = (1 << 7),
                canBeExecutedFromMeleeAttack = (1 << 8),
                dontStopRequestAfterExecute = (1 << 9),
                lerpCameraWhileOn = (1 << 10),
                forceUseWeaponTargeting = (1 << 11),
                dontClearCooldownOnDeath = (1 << 12),
                dontCooldownOnStart = (1 << 13),
                powerOnEvent = (1 << 14),
                unknownbit10 = (1 << 15),
                defaultShiftSkillEnabled = (1 << 16),
                shiftSkillAlwaysEnabled = (1 << 17),
                canKillPetsForPowerCost = (1 << 18),
                reducePowerMaxByPowerCost = (1 << 19),
                skillFromUnitEvenTriggerNeedsDamageIncrement = (1 << 20),
                aiIsBusyWhileOn = (1 << 21),
                dontSelectOnPurchase = (1 << 22),
                unknownbit11 = (1 << 23),
                neverSetCooldown = (1 << 24),
                ignorePreventAllSkills = (1 << 25),
                mustStartInPortalSafeLevel = (1 << 26),
                unknownbit12 = (1 << 27),
                dontTargetPets = (1 << 28),
                requirePathToTarget = (1 << 29),
                fireToLocation = (1 << 30),
                defaultToPlayerLocation_tcv4 = ((uint)1 << 31)
            }

            [FlagsAttribute]
            public enum Bitmask06 : uint
            {
                testFiringConeOnStart = (1 << 0),
        		ignoreChampions = (1 << 1),
        		faceTargetPosition = (1 << 2),
        		targetDeadAndAlive	 = (1 << 3),
        		restartSkillOnUnitReactivate = (1 << 4),
        		subscriptionRequiredToLearn = (1 << 5),
        		subscriptionRequiredToUse = (1 << 6),
        		forceUiToShowEffect = (1 << 7),
        		dontIgnoreOwnedState = (1 << 8),
        		ghostCanDo = (1 << 9),
        		useBoneForMissilePosition = (1 << 10),
        		transferDamagesToPets = (1 << 11),
        		dontStagger = (1 << 12),
        		averageCombinedDamage_tcv4 = (1 << 13),
        		canNOTStartInRtsLevel_tcv4 = (1 << 14),
		        doesNotActivelyUseWeapon = (1 << 15),
        		canStartInPvpTown_tcv4 = (1 << 16),
		        doNotAllowRespec_tcv4 = (1 << 17),
                applySkillgroupDamagePercent_tcv4 = (1 << 18),
                requiresStatUnlockToPurchase_tcv4 = (1 << 19)
            }
        }
    }
}