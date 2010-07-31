using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillEventTypesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramDesc4;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public SkillEventTypes.BitMask01 bitmask01;
        /*public Int32 bitmask1;/*0 bit uses laser turns
	1 bit uses requires target
	2 bit uses force new
	3 bit uses laser seeks surfaces
	4 bit uses face target
	5 bit uses use unit target
	6 bit uses use event offset
	7 bit uses loop
	8 bit uses use event offset absolute
	9 bit uses place on target
	10 bit unknown
	11 bit uses transfer stats
	12 bit uses when target in range
	13 bit uses add to center
	14 bit uses 360 targeting
	15 bit uses place on skill target
	16 bit uses use ai target
	17 bit uses use offhand weapon
	18 bit uses float
	19 bit uses don't validate target
	20 bit uses random firing direction
	21 bit uses autoaim projectile
	22 bit uses target weapon
	23 bit unknown
	24 bit unknown
	25 bit uses use holy radius for range
	26 bit unk
	27 bit unk
	28 bit unk
	29 bit uses laser attacks location
	30 bit uses at next cooldown
	31 bit uses aim with weapon*/
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public SkillEventTypes.BitMask02 bitmask02;
        /*public Int32 bitmask2;/*0 bit uses aim with weapon zero
	1 bit unk
	2 bit unk
	3 bit unk
	4 bit unk
	5 bit uses use ultimate owner
	6 bit unk
	7 bit unk
	8 bit uses include in ui count*/
        public Int32 attachmentTable;//tbl
        public Int32 paramContainsCount;//bool
        public Int32 doesNotRequireTableEntry;//bool
        public Int32 applySkillStats;//bool
        public Int32 usesAttachment;//bool
        public Int32 usesBones;//bool
        public Int32 usesBoneWeights;//bool
        public Int32 serverOnly;//bool
        public Int32 clientOnly;//bool
        public Int32 needsDuration;//bool
        public Int32 aimingEvent;//bool
        public Int32 isMelee;//bool
        public Int32 isRanged;//bool
        public Int32 isSpell;//bool
        public Int32 subskillInherit;//bool
        public Int32 convertDegreesToDot;//bool
        public Int32 usesTargetIndex;//bool
        public Int32 testsItsCondition;//bool
        public Int32 usesLasers;//bool
        public Int32 usesMissiles;//bool
        public Int32 canMultiBullets;//bool
        public Int32 paramsAreUsedInSkillString;//bool
        public Int32 startsCoolingAndPowerCost;//bool
        public Int32 consumesItem;//bool
        public Int32 checkPetPowerCost;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string eventHandler;
        public Int32 eventStringFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        byte[] undefined1;
    }
    public abstract class SkillEventTypes
    {
        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            usesLaserTurns = 1,
            usesRequiresTarget = 2,
            usesForceNew = 4,
            usesLaserSeeksSurfaces = 8,
            usesFaceTarget = 16,
            usesUseUnitTarget = 32,
            usesUseEventOffset = 64,
            usesLoop = 128,
            usesUseEventOffsetAbsolute = 256,
            usesPlaceOnTarget = 512,
            unknown = 1024,
            usesTransferStats = 2048,
            usesWhenTargetInRange = 4096,
            usesAddToCenter = 8192,
            uses360Targeting = 16384,
            usesPlaceOnSkillTarget = 32768,
            usesUseAiTarget = 65536,
            usesUseOffhandWeapon = 131072,
            usesFloat = 262144,
            usesDontValidateTarget = 524288,
            usesRandomFiringDirection = 1048576,
            usesAutoaimProjectile = 2097152,
            usesTargetWeapon = 4194304,
            unkA = 8388608,
            unkB = 16777216,
            usesUseHolyRadiusForRange = 33554432,
            unkC = 67108864,
            unkD = 134217728,
            unkE = 268435456,
            usesLaserAttacksLocation = 536870912,
            usesAtNextCooldown = 1073741824,
            usesAimWithWeapon = 2147483648
        }
        [FlagsAttribute]
        public enum BitMask02 : uint
        {
            usesAimWithWeaponZero = 1,
            unk01 = 2,
            unk02 = 4,
            unk03 = 8,
            unk04 = 16,
            usesUseUltimateOwner = 32,
            unk05 = 64,
            unk06 = 128,
            usesIncludeInUiCount = 256
        }
    }
}