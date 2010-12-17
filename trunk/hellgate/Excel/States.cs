﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class States
    {
        TableHeader header;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 name;
        Int32 buffer;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        Int32 buffer1;              // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 file;
        Int32 buffer2;              // always 0
        public Int32 isA0;
        public Int32 isA1;
        public Int32 isA2;
        public Int32 isA3;
        public Int32 isA4;
        public Int32 isA5;
        public Int32 isA6;
        public Int32 isA7;
        public Int32 isA8;
        public Int32 isA9;
        public Int32 statePreventedBy;
        public Int32 duration;
        public Int32 onDeath;
        public Int32 skillScriptParam;
        Int32 unknown18;            // always 0
        public Int32 element;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 pulseRateInMs;
        public Int32 pulseRateInMsClient;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 pulseSkill;
        Int32 unknown23;            // always 0
        Int32 unknown24;            // always 0
        Int32 unknown25;            // always 0
        public Int32 iconOrder;            // always 0
        Int32 unknown27;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 uiIcon;
        Int32 unknown29;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 uiIconTexture;
        Int32 unknown31;            // always 0
        public Int32 unknown32;     //not defined, even though it's used.
        Int32 unknown33;            // always 0
        [ExcelOutput(IsStringOffset = true)]
        public Int32 unknown34;     //undefined as well.
        Int32 unknown35;            // always 0
        public Int32 iconBackColor;
        Int32 unknown37;            // always 0
        Int32 unknown38;            // always 0
        public Int32 iconTooltipStringHellgate;//stridx
        public Int32 iconTooltipStringMythos;
        public Int32 iconTooltipStringAll;
        public Int32 assocState1;            // always -1
        public Int32 assocState2;            // always -1
        public Int32 assocState3;            // always -1
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;
        public Int32 gameFlag;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask02 bitmask02;
        Int32 unknown48;            // always 0
        Int32 unknown49;            // always 0
    }

    [FlagsAttribute]
    public enum BitMask01 : uint
    {
        executeAttackScriptMelee = 1,
        executeAttackScriptRanged = 2,
        executeSkillScriptOnRemove = 4,
        executeScriptOnSource = 8,
        pulseOnClientToo = 16
    }
    [FlagsAttribute]
    public enum BitMask02 : uint
    {
        stacks = 2,
        stacksPerSource = 4,
        sendToAll = 8,
        sendToSelf = 16,
        sendStats = 32,
        clientNeedsDuration = 64,
        clientOnly = 128,
        executeParentEvents = 256,
        triggerNotargetOnSet = 512,
        savePositionOnSet = 1024,
        saveWithUnit = 2048,
        flagForLoad = 4096,
        sharingModState = 8192,
        usedInHellgate = 16384,
        usedInTugboat = 32768,
        isBad = 65536,
        pulseOnSource = 131072,
        onChangeRepaintItemUi = 262144,
        saveInUnitfileHeader = 524288,
        updateChatServerOnChange = 1048576,
        triggerDigestSave = 2097152
    }
}