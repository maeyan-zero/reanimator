﻿using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AnimationGroupsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 playRightLeft;//bool
        public Int32 playLegs;//bool
        public Int32 onlyPlaySubGroups;//bool
        public Int32 showInHammer;//bool
        public Int32 copyFootSteps;//bool
        public Int32 defaultStance;//idx
        public Int32 defaultStanceInTown;//idx
        public Int32 canStartSkillWithLeftweapon;//bool
        public float secondsToHoldStanceInTown;
        public Int32 undefined;
        public Int32 fallback;//idx
        public Int32 rightWeapon;//idx
        public Int32 leftWeapon;//idx
        public Int32 rightAnims;//idx
        public Int32 leftAnims;//idx
        public Int32 legAnims;//idx
    }
}
