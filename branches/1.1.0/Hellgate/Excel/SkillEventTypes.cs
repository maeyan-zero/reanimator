using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillEventTypesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
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
        public BitMask01 bitmask01;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask02 bitmask02;
        public Int32 attachmentTable;//tbl
        [ExcelOutput(IsBool = true)]
        public Int32 paramContainsCount;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 doesNotRequireTableEntry;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 applySkillStats;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesAttachment;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesBones;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesBoneWeights;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 serverOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clientOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 needsDuration;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 aimingEvent;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isMelee;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isRanged;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isSpell;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 subskillInherit;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 convertDegreesToDot;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesTargetIndex;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 testsItsCondition;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesLasers;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 usesMissiles;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 canMultiBullets;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 paramsAreUsedInSkillString;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 startsCoolingAndPowerCost;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 consumesItem;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 checkPetPowerCost;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string eventHandler;
        public Int32 eventStringFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        byte[] undefined1;

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