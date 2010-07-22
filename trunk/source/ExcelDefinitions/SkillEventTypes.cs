using System;
using System.Runtime.InteropServices;

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
        public Int32 bitmask1;/*0 bit uses laser turns
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
        public Int32 bitmask2;/*0 bit uses aim with weapon zero
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
}