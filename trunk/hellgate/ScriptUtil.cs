using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Revival.Common;

namespace Hellgate
{
    partial class ExcelScript
    {
        #region Type Definitions

        private enum ArgType : uint
        {
            Int32,              // 0
            ExcelIndex,         // 1
            Context,            // 2
            Game3,              // 3
            Game4,              // 4
            Unit,               // 5
            StatsList,          // 6
            Param,              // 7
            Ptr,                // 8    used for PushLocalVarPtr
            ContextPtr          // 9    used for PushContextVarPtr
        }

        private class Argument
        {
            public String Name;             // name of argument
            public ArgType Type;            // not used, just kept "just because"
            public int TableIndex = -1;     // used for excel index arguments
            public int ByteOffset = -1;     // used for excel script functions
        }

        private class Function
        {
            public String Name;             // name of function
            public Argument[] Args;         // function arguments (null if none)
            public int ArgCount { get { return Args == null ? 0 : Args.Length; } }
            public String ExcelScript;      // used for excel script functions - contains the decompiled script
        }

        private static readonly List<Function> CallFunctions = new List<Function>
        {
            /*  0*/ new Function { Name = "setUnitTypeAreaFloorVis", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelArea", Type = ArgType.ExcelIndex, TableIndex = 138 }, new Argument { Name = "nFloor", Type = ArgType.Int32 }, new Argument { Name = "nVis", Type = ArgType.Int32 } } },
            /*  1*/ new Function { Name = "setUnitTypeAreaFloorInteractive", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelArea", Type = ArgType.ExcelIndex, TableIndex = 138 }, new Argument { Name = "nFloor", Type = ArgType.Int32 } } },
            /*  2*/ new Function { Name = "setUnitTypeAreaFloorDisabled", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelArea", Type = ArgType.ExcelIndex, TableIndex = 138 }, new Argument { Name = "nFloor", Type = ArgType.Int32 } } },
            /*  3*/ new Function { Name = "showDialog", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDialog", Type = ArgType.ExcelIndex, TableIndex = 53 } } },
            /*  4*/ new Function { Name = "setQuestBit", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nBit", Type = ArgType.Int32 } } },
            /*  5*/ new Function { Name = "getQuestBit", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nBit", Type = ArgType.Int32 } } },
            /*  6*/ new Function { Name = "isQuestTaskComplete", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nQuestTask", Type = ArgType.ExcelIndex, TableIndex = 165 } } },
            /*  7*/ new Function { Name = "isQuestTaskActive", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nQuestTask", Type = ArgType.ExcelIndex, TableIndex = 165 } } },
            /*  8*/ new Function { Name = "isTalkingTo", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nNPCID", Type = ArgType.ExcelIndex, TableIndex = 64 } } },
            /*  9*/ new Function { Name = "setTargetVisibility", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVis", Type = ArgType.Int32 } } },
            /* 10*/ new Function { Name = "setTargetVisibilityOnFloor", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVis", Type = ArgType.Int32 }, new Argument { Name = "nFloor", Type = ArgType.Int32 } } },
            /* 11*/ new Function { Name = "setStateOnTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 12*/ new Function { Name = "setTargetInteractive", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nInteractive", Type = ArgType.Int32 } } },
            /* 13*/ new Function { Name = "setTargetToTeam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nTeam", Type = ArgType.Int32 } } },
            /* 14*/ new Function { Name = "getIsTargetOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 15*/ new Function { Name = "setMonsterInLevelToTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nMonsterID", Type = ArgType.ExcelIndex, TableIndex = 115 } } },
            /* 16*/ new Function { Name = "setObjectInLevelToTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nObjectID", Type = ArgType.ExcelIndex, TableIndex = 119 } } },
            /* 17*/ new Function { Name = "getIsTargetMonster", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nMonsterID", Type = ArgType.ExcelIndex, TableIndex = 115 } } },
            /* 18*/ new Function { Name = "getIsTargetObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nObjectID", Type = ArgType.ExcelIndex, TableIndex = 119 } } },
            /* 19*/ new Function { Name = "resetTargetObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 20*/ new Function { Name = "messageStatVal", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStatId", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nIndex", Type = ArgType.Int32 } } },
            /* 21*/ new Function { Name = "getStatVal", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStatId", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nIndex", Type = ArgType.Int32 } } },
            /* 22*/ new Function { Name = "createMap", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 23*/ new Function { Name = "randomizeMapSpawner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 24*/ new Function { Name = "randomizeMapSpawnerEpic", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 25*/ new Function { Name = "setMapSpawnerByLevelAreaID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelAreaID", Type = ArgType.Int32 } } },
            /* 26*/ new Function { Name = "setMapSpawnerByLevelArea", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelAreaID", Type = ArgType.ExcelIndex, TableIndex = 138 } } },
            /* 27*/ new Function { Name = "abs", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 } } },
            /* 28*/ new Function { Name = "min", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 29*/ new Function { Name = "max", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 30*/ new Function { Name = "pin", Args = new[] { new Argument { Name = "value", Type = ArgType.Int32 }, new Argument { Name = "min", Type = ArgType.Int32 }, new Argument { Name = "max", Type = ArgType.Int32 } } },
            /* 31*/ new Function { Name = "pct", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 32*/ new Function { Name = "pctFloat", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 }, new Argument { Name = "c", Type = ArgType.Int32 } } },
            /* 33*/ new Function { Name = "rand", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 34*/ new Function { Name = "randByUnitId", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 35*/ new Function { Name = "chance", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nChanceOutOf", Type = ArgType.Int32 } } },
            /* 36*/ new Function { Name = "chanceByStateMod", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nChanceOutOf", Type = ArgType.Int32 }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "nModBy", Type = ArgType.Int32 } } },
            /* 37*/ new Function { Name = "randSkillSeed", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 38*/ new Function { Name = "roll", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 39*/ new Function { Name = "divMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 }, new Argument { Name = "c", Type = ArgType.Int32 } } },
            /* 40*/ new Function { Name = "distribute", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "numdie", Type = ArgType.Int32 }, new Argument { Name = "diesize", Type = ArgType.Int32 }, new Argument { Name = "start", Type = ArgType.Int32 } } },
            /* 41*/ new Function { Name = "roundstat", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "value", Type = ArgType.Int32 } } },
            /* 42*/ new Function { Name = "getEventType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAffixID", Type = ArgType.ExcelIndex, TableIndex = 25 } } },
            /* 43*/ new Function { Name = "getAffixIDByName", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAffixID", Type = ArgType.ExcelIndex, TableIndex = 52 } } },
            /* 44*/ new Function { Name = "get_skill_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /* 45*/ new Function { Name = "get_skill_level_object", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /* 46*/ new Function { Name = "pickskill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 47*/ new Function { Name = "pickskillbyunittype", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 48*/ new Function { Name = "removeoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 49*/ new Function { Name = "killoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 50*/ new Function { Name = "pickskillbyskillgroup", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nSkillGroup", Type = ArgType.ExcelIndex, TableIndex = 39 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 51*/ new Function { Name = "learnskill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skill", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /* 52*/ new Function { Name = "getStatOwnerDivBySkillVar", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nVar", Type = ArgType.Int32 } } },
            /* 53*/ new Function { Name = "getStatOwnerDivBy", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nDivBy", Type = ArgType.Int32 } } },
            /* 54*/ new Function { Name = "switchUnitAndObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 55*/ new Function { Name = "getAchievementCompleteCount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAchievementID", Type = ArgType.ExcelIndex, TableIndex = 180 } } },
            /* 56*/ new Function { Name = "getVarRange", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 57*/ new Function { Name = "getVar", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 58*/ new Function { Name = "getAttackerSkillVarBySkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 59*/ new Function { Name = "getVarFromSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 60*/ new Function { Name = "getVarFromSkillFromObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 61*/ new Function { Name = "hasStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 62*/ new Function { Name = "hasState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 63*/ new Function { Name = "clearStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 64*/ new Function { Name = "clearState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 65*/ new Function { Name = "clearStateClient", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 66*/ new Function { Name = "isDualWielding", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 67*/ new Function { Name = "getWieldingIsACount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 68*/ new Function { Name = "setState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 69*/ new Function { Name = "setStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 } } },
            /* 70*/ new Function { Name = "setStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 71*/ new Function { Name = "addStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 72*/ new Function { Name = "addStateWithTimeMSClient", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 73*/ new Function { Name = "setStateWithTimeMSOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 74*/ new Function { Name = "setStateWithTimeMSScriptOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 75*/ new Function { Name = "BroadcastEquipEvent", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 76*/ new Function { Name = "setAITargetToSkillTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 77*/ new Function { Name = "setObjectAITargetToUnitAITarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 78*/ new Function { Name = "makeAIAwareOfObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 79*/ new Function { Name = "setAITargetToObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 80*/ new Function { Name = "hasSkillTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 81*/ new Function { Name = "setStateOnSkillTargetWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "clearFirst", Type = ArgType.Int32 } } },
            /* 82*/ new Function { Name = "runScriptParamOnStateClear", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 83*/ new Function { Name = "getCountOfUnitsInArea", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "area", Type = ArgType.Int32 } } },
            /* 84*/ new Function { Name = "runScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "scriptIndex", Type = ArgType.Int32 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /* 85*/ new Function { Name = "doSkillAndScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "scriptIndex", Type = ArgType.Int32 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /* 86*/ new Function { Name = "doSkillOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /* 87*/ new Function { Name = "setStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 88*/ new Function { Name = "setStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 89*/ new Function { Name = "setStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 90*/ new Function { Name = "addStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 91*/ new Function { Name = "addStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 92*/ new Function { Name = "addStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 73 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 93*/ new Function { Name = "setDmgEffect", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nTime", Type = ArgType.Int32 }, new Argument { Name = "nRoll", Type = ArgType.Int32 } } },
            /* 94*/ new Function { Name = "getStatOwner", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 95*/ new Function { Name = "getStatParent", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 96*/ new Function { Name = "addPCTStatOnOwner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /* 97*/ new Function { Name = "setStatOnOwner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /* 98*/ new Function { Name = "total", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 99*/ new Function { Name = "basetotal", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /*100*/ new Function { Name = "basestat", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /*101*/ new Function { Name = "getcur", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*102*/ new Function { Name = "statidx", Args = new[] { new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /*103*/ new Function { Name = "invcount", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "location", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*104*/ new Function { Name = "dmgrider", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 } } },
            /*105*/ new Function { Name = "procrider", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 } } },
            /*106*/ new Function { Name = "knockback", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*107*/ new Function { Name = "colorcoderequirement", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*108*/ new Function { Name = "color_code_modunit_requirement", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*109*/ new Function { Name = "color_code_modunit_requirement2", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat1", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param1", Type = ArgType.Param }, new Argument { Name = "stat2", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param2", Type = ArgType.Param } } },
            /*110*/ new Function { Name = "feedchange", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /*111*/ new Function { Name = "feedcolorcode", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /*112*/ new Function { Name = "colorposneg", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*113*/ new Function { Name = "colorcodeprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*114*/ new Function { Name = "colorcodeclassreq", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*115*/ new Function { Name = "colorcodeskillslots", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*116*/ new Function { Name = "colorcodeskillusable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*117*/ new Function { Name = "colorcodeskillgroupusable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*118*/ new Function { Name = "meetsclassreqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*119*/ new Function { Name = "fontcolorrow", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nColorIndex", Type = ArgType.ExcelIndex, TableIndex = 7 } } },
            /*120*/ new Function { Name = "nodrop", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*121*/ new Function { Name = "notrade", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*122*/ new Function { Name = "BuyPriceByValue", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "valueType", Type = ArgType.Int32 } } },
            /*123*/ new Function { Name = "SellPriceByValue", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "valueType", Type = ArgType.Int32 } } },
            /*124*/ new Function { Name = "buyprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*125*/ new Function { Name = "buypriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*126*/ new Function { Name = "sellprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*127*/ new Function { Name = "sellpriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*128*/ new Function { Name = "hitChance", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*129*/ new Function { Name = "dodgeChance", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*130*/ new Function { Name = "numaffixes", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*131*/ new Function { Name = "qualitypricemult", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*132*/ new Function { Name = "enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*133*/ new Function { Name = "visible_enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*134*/ new Function { Name = "champions_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*135*/ new Function { Name = "distance_sq_to_champion", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*136*/ new Function { Name = "champion_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*137*/ new Function { Name = "bosses_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*138*/ new Function { Name = "distance_sq_to_boss", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*139*/ new Function { Name = "boss_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*140*/ new Function { Name = "enemy_corpses_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*141*/ new Function { Name = "monsters_killed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*142*/ new Function { Name = "monsters_killed_nonteam", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*143*/ new Function { Name = "monsters_pct_left", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*144*/ new Function { Name = "hp_lost", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*145*/ new Function { Name = "meters_moved", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*146*/ new Function { Name = "attacks", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*147*/ new Function { Name = "is_alive", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*148*/ new Function { Name = "monster_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*149*/ new Function { Name = "has_active_task", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*150*/ new Function { Name = "is_usable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*151*/ new Function { Name = "is_examinable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*152*/ new Function { Name = "is_operatable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*153*/ new Function { Name = "isa", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*154*/ new Function { Name = "is_subscriber", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*155*/ new Function { Name = "is_hardcore", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*156*/ new Function { Name = "is_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*157*/ new Function { Name = "get_difficulty", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*158*/ new Function { Name = "get_act", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*159*/ new Function { Name = "email_send_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*160*/ new Function { Name = "email_receive_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*161*/ new Function { Name = "colorcodesubscriber", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*162*/ new Function { Name = "item_requires_subscriber", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*163*/ new Function { Name = "colorcodenightmare", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*164*/ new Function { Name = "item_is_nightmare_specific", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*165*/ new Function { Name = "quality", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*166*/ new Function { Name = "meetsitemreqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*167*/ new Function { Name = "weapondps", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*168*/ new Function { Name = "SkillTargetIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*169*/ new Function { Name = "GetObjectIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*170*/ new Function { Name = "GetMissileSourceIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*171*/ new Function { Name = "GetSkillHasReqWeapon", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*172*/ new Function { Name = "has_use_skill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*173*/ new Function { Name = "hasdomname", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*174*/ new Function { Name = "dps", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*175*/ new Function { Name = "ObjectCanUpgrade", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*176*/ new Function { Name = "use_state_duration", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*177*/ new Function { Name = "uses_missiles", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*178*/ new Function { Name = "uses_lasers", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*179*/ new Function { Name = "has_damage_radius", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*180*/ new Function { Name = "missile_count", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*181*/ new Function { Name = "laser_count", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*182*/ new Function { Name = "shots_per_minute", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*183*/ new Function { Name = "milliseconds_per_shot", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*184*/ new Function { Name = "player_crit_chance", Args = new[] { new Argument { Name = "pUnit", Type = ArgType.Unit }, new Argument { Name = "nSlot", Type = ArgType.Int32 } } },
            /*185*/ new Function { Name = "player_crit_damage", Args = new[] { new Argument { Name = "pUnit", Type = ArgType.Unit }, new Argument { Name = "nSlot", Type = ArgType.Int32 } } },
            /*186*/ new Function { Name = "add_item_level_armor", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercent", Type = ArgType.Int32 } } },
            /*187*/ new Function { Name = "player_level_skill_power_cost_percent", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*188*/ new Function { Name = "item_level_damage_mult", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*189*/ new Function { Name = "item_level_feed", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*190*/ new Function { Name = "item_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*191*/ new Function { Name = "item_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*192*/ new Function { Name = "item_level_shield_buffer", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*193*/ new Function { Name = "monster_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*194*/ new Function { Name = "monster_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*195*/ new Function { Name = "monster_level_damage", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*196*/ new Function { Name = "monster_level_damage_pct", Args = new[] { new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPCT", Type = ArgType.Int32 } } },
            /*197*/ new Function { Name = "monster_level_shields", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*198*/ new Function { Name = "monster_level_armor", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*199*/ new Function { Name = "unit_ai_changer_attack", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*200*/ new Function { Name = "does_field_damage", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*201*/ new Function { Name = "distance_to_player", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*202*/ new Function { Name = "has_container", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*203*/ new Function { Name = "monster_armor", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nDamageType", Type = ArgType.ExcelIndex, TableIndex = 30 }, new Argument { Name = "nPercent", Type = ArgType.Int32 } } },
            /*204*/ new Function { Name = "getSkillDmgMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*205*/ new Function { Name = "getSkillArmorMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*206*/ new Function { Name = "getSkillAttackSpeedMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*207*/ new Function { Name = "getSkillToHitMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*208*/ new Function { Name = "getSkillPctDmgMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*209*/ new Function { Name = "getPetCountOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*210*/ new Function { Name = "runScriptOnPetsOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*211*/ new Function { Name = "randaffixtype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affixType", Type = ArgType.ExcelIndex, TableIndex = 50 } } },
            /*212*/ new Function { Name = "randaffixgroup", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "3h", Type = ArgType.ExcelIndex, TableIndex = 52 } } },
            /*213*/ new Function { Name = "applyaffix", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affix", Type = ArgType.ExcelIndex, TableIndex = 52 }, new Argument { Name = "bForce", Type = ArgType.Int32 } } },
            /*214*/ new Function { Name = "getBonusValue", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*215*/ new Function { Name = "getBonusAll", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*216*/ new Function { Name = "getDMGAugmentation", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillVar", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercentOfLevel", Type = ArgType.Int32 }, new Argument { Name = "nSkillPointsInvested", Type = ArgType.Int32 } } },
            /*217*/ new Function { Name = "getDMGAugmentationPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillVar", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercentOfLevel", Type = ArgType.Int32 }, new Argument { Name = "nSkillPointsInvested", Type = ArgType.Int32 } } },
            /*218*/ new Function { Name = "getMonsterHPAtLevel", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 } } },
            /*219*/ new Function { Name = "getMonsterHPAtLevelByPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPCT", Type = ArgType.Int32 } } },
            /*220*/ new Function { Name = "display_dmg_absorbed_pct", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*221*/ new Function { Name = "dmg_percent_by_energy", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*222*/ new Function { Name = "weapon_range", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*223*/ new Function { Name = "IsObjectDestructable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*224*/ new Function { Name = "GlobalThemeIsEnabled", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nTheme", Type = ArgType.ExcelIndex, TableIndex = 167 } } },
            /*225*/ new Function { Name = "SetRespawnPlayer", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*226*/ new Function { Name = "AddSecondaryRespawnPlayer", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*227*/ new Function { Name = "RemoveHPAndCheckForDeath", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nRemove", Type = ArgType.Int32 } } },
            /*228*/ new Function { Name = "getSkillStat", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillStat", Type = ArgType.ExcelIndex, TableIndex = 44 }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 } } },
            /*229*/ new Function { Name = "TownPortalIsAllowed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*230*/ new Function { Name = "lowerManaCostOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nPctPower", Type = ArgType.Int32 } } },
            /*231*/ new Function { Name = "lowerCoolDownOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nPctCooldown", Type = ArgType.Int32 } } },
            /*232*/ new Function { Name = "skillIsOn", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*233*/ new Function { Name = "getSkillRange", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*234*/ new Function { Name = "setDmgEffectParams", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nParam0", Type = ArgType.Int32 }, new Argument { Name = "nParam1", Type = ArgType.Int32 }, new Argument { Name = "nParam2", Type = ArgType.Int32 } } },
            /*235*/ new Function { Name = "setDmgEffectSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*236*/ new Function { Name = "setDmgEffectSkillOnTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*237*/ new Function { Name = "getSkillID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*238*/ new Function { Name = "fireMissileFromObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "missileID", Type = ArgType.ExcelIndex, TableIndex = 110 } } },
            /*239*/ new Function { Name = "caculateGemSockets", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*240*/ new Function { Name = "caculateRareGemSockets", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*241*/ new Function { Name = "caculateCraftingSlots", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*242*/ new Function { Name = "executeSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*243*/ new Function { Name = "executeSkillOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*244*/ new Function { Name = "stopSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*245*/ new Function { Name = "powercost", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*246*/ new Function { Name = "is_stash_ui_open" },
            /*247*/ new Function { Name = "setRecipeLearned", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*248*/ new Function { Name = "getRecipeLearned", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*249*/ new Function { Name = "createRecipe", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*250*/ new Function { Name = "createSpecificRecipe", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeID", Type = ArgType.ExcelIndex, TableIndex = 108 } } },
            /*251*/ new Function { Name = "getCurrentGameTick", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } }
        };

        private static readonly List<Function> CallFunctionsTestCenter = new List<Function>
        {
            /*  0*/ new Function { Name = "setUnitTypeAreaFloorVis", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelArea", Type = ArgType.ExcelIndex, TableIndex = 139 }, new Argument { Name = "nFloor", Type = ArgType.Int32 }, new Argument { Name = "nVis", Type = ArgType.Int32 } } },
            /*  1*/ new Function { Name = "setUnitTypeAreaFloorInteractive", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelArea", Type = ArgType.ExcelIndex, TableIndex = 139 }, new Argument { Name = "nFloor", Type = ArgType.Int32 } } },
            /*  2*/ new Function { Name = "setUnitTypeAreaFloorDisabled", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelArea", Type = ArgType.ExcelIndex, TableIndex = 139 }, new Argument { Name = "nFloor", Type = ArgType.Int32 } } },
            /*  3*/ new Function { Name = "showQuestItemDialog", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*  4*/ new Function { Name = "setQuestBit", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nBit", Type = ArgType.Int32 } } },
            /*  5*/ new Function { Name = "getQuestBit", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nBit", Type = ArgType.Int32 } } },
            /*  6*/ new Function { Name = "isClientQuestComplete", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nQuestId", Type = ArgType.ExcelIndex, TableIndex = 104 } } },
            /*  7*/ new Function { Name = "isQuestTaskComplete", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nQuestTask", Type = ArgType.ExcelIndex, TableIndex = 166 } } },
            /*  8*/ new Function { Name = "isQuestTaskActive", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nQuestTask", Type = ArgType.ExcelIndex, TableIndex = 166 } } },
            /*  9*/ new Function { Name = "isTalkingTo", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nNPCID", Type = ArgType.ExcelIndex, TableIndex = 65 } } },
            /* 10*/ new Function { Name = "setTargetVisibility", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVis", Type = ArgType.Int32 } } },
            /* 11*/ new Function { Name = "setTargetVisibilityOnFloor", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVis", Type = ArgType.Int32 }, new Argument { Name = "nFloor", Type = ArgType.Int32 } } },
            /* 12*/ new Function { Name = "setStateOnTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 13*/ new Function { Name = "setTargetInteractive", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nInteractive", Type = ArgType.Int32 } } },
            /* 14*/ new Function { Name = "setTargetToTeam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nTeam", Type = ArgType.Int32 } } },
            /* 15*/ new Function { Name = "getIsTargetOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /* 16*/ new Function { Name = "setMonsterInLevelToTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nMonsterID", Type = ArgType.ExcelIndex, TableIndex = 116 } } },
            /* 17*/ new Function { Name = "setObjectInLevelToTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nObjectID", Type = ArgType.ExcelIndex, TableIndex = 120 } } },
            /* 18*/ new Function { Name = "getIsTargetMonster", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nMonsterID", Type = ArgType.ExcelIndex, TableIndex = 116 } } },
            /* 19*/ new Function { Name = "getIsTargetObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nObjectID", Type = ArgType.ExcelIndex, TableIndex = 120 } } },
            /* 20*/ new Function { Name = "resetTargetObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 21*/ new Function { Name = "messageStatVal", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStatId", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nIndex", Type = ArgType.Int32 } } },
            /* 22*/ new Function { Name = "getStatVal", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStatId", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nIndex", Type = ArgType.Int32 } } },
            /* 23*/ new Function { Name = "CraftingGetParentSkillLvl", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nParentSkill", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /* 24*/ new Function { Name = "CraftingAddAffixes", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 } } },
            /* 25*/ new Function { Name = "CraftingAddDmgType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDamageType", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 26*/ new Function { Name = "CraftingAddResDmgType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDamageType", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 } } },
            /* 27*/ new Function { Name = "CraftingModifyQuality", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 } } },
            /* 28*/ new Function { Name = "CraftingModifyAttackSpeed", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 } } },
            /* 29*/ new Function { Name = "CraftingModifyDamage", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 } } },
            /* 30*/ new Function { Name = "CraftingModifyDuration", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 } } },
            /* 31*/ new Function { Name = "CraftingModifyModLevel", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 }, new Argument { Name = "nValueAmount", Type = ArgType.Int32 } } },
            /* 32*/ new Function { Name = "CraftingModifyAC", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 } } },
            /* 33*/ new Function { Name = "CraftingModifyMovementSpeed", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModAmount", Type = ArgType.Int32 } } },
            /* 34*/ new Function { Name = "RecipePropChance", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nMinValue", Type = ArgType.Int32 }, new Argument { Name = "nMaxValue", Type = ArgType.Int32 } } },
            /* 35*/ new Function { Name = "RecipePropValue", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nMinValue", Type = ArgType.Int32 }, new Argument { Name = "nMaxValue", Type = ArgType.Int32 } } },
            /* 36*/ new Function { Name = "CraftingAddPctChance", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nValue", Type = ArgType.Int32 } } },
            /* 37*/ new Function { Name = "CraftingSetPctChance", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nValue", Type = ArgType.Int32 } } },
            /* 38*/ new Function { Name = "CraftingSetPctOfValue", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nValue", Type = ArgType.Int32 } } },
            /* 39*/ new Function { Name = "CraftingSetValue", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nPctOfValue", Type = ArgType.Int32 } } },
            /* 40*/ new Function { Name = "Recipe_UnitDataMinDmg", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModValue", Type = ArgType.Int32 } } },
            /* 41*/ new Function { Name = "Recipe_UnitDataMaxDmg", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModValue", Type = ArgType.Int32 } } },
            /* 42*/ new Function { Name = "Recipe_UnitDataACMin", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModValue", Type = ArgType.Int32 } } },
            /* 43*/ new Function { Name = "Recipe_UnitDataACMax", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nModValue", Type = ArgType.Int32 } } },
            /* 44*/ new Function { Name = "Recipe_UnitDataStat", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /* 45*/ new Function { Name = "CraftingSetStatByRecipeProp", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /* 46*/ new Function { Name = "CraftingSetDamageEffectByRecipeProp", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nDamageEffect", Type = ArgType.ExcelIndex, TableIndex = 32 }, new Argument { Name = "nMinRange", Type = ArgType.Int32 }, new Argument { Name = "nMaxRange", Type = ArgType.Int32 }, new Argument { Name = "nDurationLenMS", Type = ArgType.Int32 } } },
            /* 47*/ new Function { Name = "CraftingSetDamageEffectParamByRecipePro", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nDamageEffect", Type = ArgType.ExcelIndex, TableIndex = 32 }, new Argument { Name = "nParam", Type = ArgType.Int32 }, new Argument { Name = "nMinRange", Type = ArgType.Int32 }, new Argument { Name = "nMaxRange", Type = ArgType.Int32 } } },
            /* 48*/ new Function { Name = "CraftingSetStatByRangeAndCatLvl", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nParam", Type = ArgType.Int32 }, new Argument { Name = "nMinRange", Type = ArgType.Int32 }, new Argument { Name = "nMaxRange", Type = ArgType.Int32 } } },
            /* 49*/ new Function { Name = "CraftingGetValRangeByRecipePropPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nMinRange", Type = ArgType.Int32 }, new Argument { Name = "nMaxRange", Type = ArgType.Int32 } } },
            /* 50*/ new Function { Name = "CraftingSetStatByRangeAndRecipePropPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeProp", Type = ArgType.ExcelIndex, TableIndex = 187 }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nParam", Type = ArgType.Int32 }, new Argument { Name = "nMinRange", Type = ArgType.Int32 }, new Argument { Name = "nMaxRange", Type = ArgType.Int32 } } },
            /* 51*/ new Function { Name = "createMap", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 52*/ new Function { Name = "randomizeMapSpawner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 53*/ new Function { Name = "randomizeMapSpawnerEpic", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 54*/ new Function { Name = "createRandomDungeonSeedRuneStone", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nZone", Type = ArgType.ExcelIndex, TableIndex = 138 }, new Argument { Name = "nLevelType", Type = ArgType.ExcelIndex, TableIndex = 106 }, new Argument { Name = "nMin", Type = ArgType.Int32 }, new Argument { Name = "nMax", Type = ArgType.Int32 }, new Argument { Name = "nEpic", Type = ArgType.Int32 } } },
            /* 55*/ new Function { Name = "setMapSpawnerByLevelAreaID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelAreaID", Type = ArgType.Int32 } } },
            /* 56*/ new Function { Name = "setMapSpawnerByLevelArea", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevelAreaID", Type = ArgType.ExcelIndex, TableIndex = 139 } } },
            /* 57*/ new Function { Name = "abs", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 } } },
            /* 58*/ new Function { Name = "min", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 59*/ new Function { Name = "max", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 60*/ new Function { Name = "pin", Args = new[] { new Argument { Name = "value", Type = ArgType.Int32 }, new Argument { Name = "min", Type = ArgType.Int32 }, new Argument { Name = "max", Type = ArgType.Int32 } } },
            /* 61*/ new Function { Name = "pct", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 62*/ new Function { Name = "pctFloat", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 }, new Argument { Name = "c", Type = ArgType.Int32 } } },
            /* 63*/ new Function { Name = "rand", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 64*/ new Function { Name = "randByUnitId", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 65*/ new Function { Name = "chance", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nChanceOutOf", Type = ArgType.Int32 } } },
            /* 66*/ new Function { Name = "chanceByStateMod", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nChanceOutOf", Type = ArgType.Int32 }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "nModBy", Type = ArgType.Int32 } } },
            /* 67*/ new Function { Name = "randSkillSeed", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 68*/ new Function { Name = "roll", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 69*/ new Function { Name = "divMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 }, new Argument { Name = "c", Type = ArgType.Int32 } } },
            /* 70*/ new Function { Name = "distribute", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "numdie", Type = ArgType.Int32 }, new Argument { Name = "diesize", Type = ArgType.Int32 }, new Argument { Name = "start", Type = ArgType.Int32 } } },
            /* 71*/ new Function { Name = "roundstat", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "value", Type = ArgType.Int32 } } },
            /* 72*/ new Function { Name = "getEventType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAffixID", Type = ArgType.ExcelIndex, TableIndex = 26 } } },
            /* 73*/ new Function { Name = "getAffixIDByName", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAffixID", Type = ArgType.ExcelIndex, TableIndex = 53 } } },
            /* 74*/ new Function { Name = "get_skill_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /* 75*/ new Function { Name = "get_skill_level_object", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /* 76*/ new Function { Name = "pickskill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 77*/ new Function { Name = "pickskillbyunittype", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 78*/ new Function { Name = "removeoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /* 79*/ new Function { Name = "killoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /* 80*/ new Function { Name = "scaleUnitToInMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nScale", Type = ArgType.Int32 }, new Argument { Name = "nTimeMS", Type = ArgType.Int32 } } },
            /* 81*/ new Function { Name = "pickskillbyskillgroup", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nSkillGroup", Type = ArgType.ExcelIndex, TableIndex = 40 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 82*/ new Function { Name = "learnskill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skill", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /* 83*/ new Function { Name = "getStatOwnerDivBySkillVar", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nVar", Type = ArgType.Int32 } } },
            /* 84*/ new Function { Name = "getStatOwnerDivBy", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nDivBy", Type = ArgType.Int32 } } },
            /* 85*/ new Function { Name = "switchUnitAndObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 86*/ new Function { Name = "getAchievementCompleteCount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAchievementID", Type = ArgType.ExcelIndex, TableIndex = 181 } } },
            /* 87*/ new Function { Name = "getVarRange", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 88*/ new Function { Name = "getVar", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 89*/ new Function { Name = "getAttackerSkillVarBySkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 90*/ new Function { Name = "getVarFromSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 91*/ new Function { Name = "getVarFromSkillWithLvl", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nVariable", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 } } },
            /* 92*/ new Function { Name = "getVarFromSkillFromObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 93*/ new Function { Name = "hasStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 94*/ new Function { Name = "hasState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 95*/ new Function { Name = "clearStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 96*/ new Function { Name = "clearState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 97*/ new Function { Name = "clearStateClient", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /* 98*/ new Function { Name = "isDualWielding", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 99*/ new Function { Name = "getWieldingIsACount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*100*/ new Function { Name = "setState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /*101*/ new Function { Name = "setStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /*102*/ new Function { Name = "setStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*103*/ new Function { Name = "addStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*104*/ new Function { Name = "addStateWithTimeMSClient", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*105*/ new Function { Name = "setStateWithTimeMSOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*106*/ new Function { Name = "setStateWithTimeMSScriptOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*107*/ new Function { Name = "BroadcastEquipEvent", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*108*/ new Function { Name = "setAITargetToSkillTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*109*/ new Function { Name = "setObjectAITargetToUnitAITarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*110*/ new Function { Name = "makeAIAwareOfObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*111*/ new Function { Name = "setAITargetToObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*112*/ new Function { Name = "getPossibleTargetCount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*113*/ new Function { Name = "hasSkillTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*114*/ new Function { Name = "setStateOnSkillTargetWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "clearFirst", Type = ArgType.Int32 } } },
            /*115*/ new Function { Name = "runScriptParamOnStateClear", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /*116*/ new Function { Name = "getCountOfUnitsInArea", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "area", Type = ArgType.Int32 } } },
            /*117*/ new Function { Name = "runScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "scriptIndex", Type = ArgType.Int32 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /*118*/ new Function { Name = "doSkillAndScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "scriptIndex", Type = ArgType.Int32 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /*119*/ new Function { Name = "doSkillOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /*120*/ new Function { Name = "setStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*121*/ new Function { Name = "setStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /*122*/ new Function { Name = "setStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /*123*/ new Function { Name = "addStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /*124*/ new Function { Name = "addStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /*125*/ new Function { Name = "addStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 74 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /*126*/ new Function { Name = "setDmgEffect", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 32 }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nTime", Type = ArgType.Int32 }, new Argument { Name = "nRoll", Type = ArgType.Int32 } } },
            /*127*/ new Function { Name = "getStatOwner", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*128*/ new Function { Name = "getStatParent", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*129*/ new Function { Name = "addPCTStatOnOwner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /*130*/ new Function { Name = "setStatOnOwner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /*131*/ new Function { Name = "total", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*132*/ new Function { Name = "basetotal", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*133*/ new Function { Name = "basestat", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*134*/ new Function { Name = "getcur", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*135*/ new Function { Name = "statidx", Args = new[] { new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*136*/ new Function { Name = "hasvisibleprops", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*137*/ new Function { Name = "setvisibleprops", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*138*/ new Function { Name = "founddisplayitem", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*139*/ new Function { Name = "invcount", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "location", Type = ArgType.ExcelIndex, TableIndex = 25 } } },
            /*140*/ new Function { Name = "is_in_invloc", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "location", Type = ArgType.ExcelIndex, TableIndex = 25 } } },
            /*141*/ new Function { Name = "dmgrider", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 } } },
            /*142*/ new Function { Name = "procrider", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 } } },
            /*143*/ new Function { Name = "knockback", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*144*/ new Function { Name = "colorcoderequirement", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*145*/ new Function { Name = "color_code_modunit_requirement", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*146*/ new Function { Name = "color_code_modunit_requirement2", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat1", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "param1", Type = ArgType.Param }, new Argument { Name = "stat2", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "param2", Type = ArgType.Param } } },
            /*147*/ new Function { Name = "feedchange", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "nCheckBonus", Type = ArgType.Int32 } } },
            /*148*/ new Function { Name = "hoverstatchange", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*149*/ new Function { Name = "feedcolorcode", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*150*/ new Function { Name = "color_code_pos_neg_val2", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*151*/ new Function { Name = "colorposneg", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*152*/ new Function { Name = "colorcodeprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*153*/ new Function { Name = "colorcodeclassreq", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*154*/ new Function { Name = "colorcodeskillslots", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*155*/ new Function { Name = "colorcodeskillusable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*156*/ new Function { Name = "colorcodeskillgroupusable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*157*/ new Function { Name = "meetsclassreqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*158*/ new Function { Name = "fontcolorrow", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nColorIndex", Type = ArgType.ExcelIndex, TableIndex = 7 } } },
            /*159*/ new Function { Name = "nodrop", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*160*/ new Function { Name = "notrade", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*161*/ new Function { Name = "objectNotrade", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*162*/ new Function { Name = "BuyPriceByValue", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "valueType", Type = ArgType.Int32 } } },
            /*163*/ new Function { Name = "SellPriceByValue", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "valueType", Type = ArgType.Int32 } } },
            /*164*/ new Function { Name = "buyprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*165*/ new Function { Name = "buypriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*166*/ new Function { Name = "sellprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*167*/ new Function { Name = "sellpriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*168*/ new Function { Name = "hitChance", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*169*/ new Function { Name = "dodgeChance", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*170*/ new Function { Name = "numaffixes", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*171*/ new Function { Name = "numupgrades", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*172*/ new Function { Name = "numaugments", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*173*/ new Function { Name = "maxupgrades", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*174*/ new Function { Name = "maxaugments", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*175*/ new Function { Name = "qualitypricemult", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*176*/ new Function { Name = "enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*177*/ new Function { Name = "visible_enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*178*/ new Function { Name = "champions_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*179*/ new Function { Name = "distance_sq_to_champion", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*180*/ new Function { Name = "champion_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*181*/ new Function { Name = "bosses_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*182*/ new Function { Name = "distance_sq_to_boss", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*183*/ new Function { Name = "boss_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*184*/ new Function { Name = "enemy_corpses_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*185*/ new Function { Name = "monsters_killed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*186*/ new Function { Name = "monsters_killed_nonteam", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*187*/ new Function { Name = "monsters_pct_left", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*188*/ new Function { Name = "hp_lost", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*189*/ new Function { Name = "meters_moved", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*190*/ new Function { Name = "attacks", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*191*/ new Function { Name = "is_alive", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*192*/ new Function { Name = "monster_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*193*/ new Function { Name = "has_active_task", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*194*/ new Function { Name = "is_usable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*195*/ new Function { Name = "is_examinable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*196*/ new Function { Name = "is_operatable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*197*/ new Function { Name = "UnitContainsUnitType", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*198*/ new Function { Name = "isa", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*199*/ new Function { Name = "is_subscriber", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*200*/ new Function { Name = "is_hardcore", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*201*/ new Function { Name = "is_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*202*/ new Function { Name = "get_difficulty", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*203*/ new Function { Name = "same_game_variant", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*204*/ new Function { Name = "player_is_in_guild", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*205*/ new Function { Name = "getTierInvestmentForPane", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nPaneIndex", Type = ArgType.ExcelIndex, TableIndex = 39 } } },
            /*206*/ new Function { Name = "GetStat", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /*207*/ new Function { Name = "get_act", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*208*/ new Function { Name = "email_send_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*209*/ new Function { Name = "email_receive_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*210*/ new Function { Name = "colorcodesubscriber", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*211*/ new Function { Name = "item_requires_subscriber", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*212*/ new Function { Name = "colorcodevariantnormal", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*213*/ new Function { Name = "colorcodevarianthardcore", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*214*/ new Function { Name = "colorcodevariantelite", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*215*/ new Function { Name = "colorcodevarianthardcoreelite", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*216*/ new Function { Name = "colorcodenightmare", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*217*/ new Function { Name = "item_is_nightmare_specific", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*218*/ new Function { Name = "item_is_variant_normal", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*219*/ new Function { Name = "item_is_variant_hardcore", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*220*/ new Function { Name = "item_is_variant_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*221*/ new Function { Name = "item_is_variant_hardcore_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*222*/ new Function { Name = "quality", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*223*/ new Function { Name = "meetsitemreqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*224*/ new Function { Name = "weapondps", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*225*/ new Function { Name = "SkillTargetIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*226*/ new Function { Name = "getStatFromExecutedUnit", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*227*/ new Function { Name = "parseExecutedItemSkillScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nScriptIndex", Type = ArgType.Int32 } } },
            /*228*/ new Function { Name = "getObjectClassID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*229*/ new Function { Name = "copyExecutedUnitProps", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nPropIndex", Type = ArgType.Int32 } } },
            /*230*/ new Function { Name = "getItemCoolDown", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*231*/ new Function { Name = "getItemDuration", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*232*/ new Function { Name = "getItemDurationMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*233*/ new Function { Name = "GetObjectIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*234*/ new Function { Name = "GetMissileSourceIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*235*/ new Function { Name = "GetSkillHasReqWeapon", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*236*/ new Function { Name = "has_use_skill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*237*/ new Function { Name = "hasdomname", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*238*/ new Function { Name = "dps", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*239*/ new Function { Name = "ObjectCanUpgrade", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*240*/ new Function { Name = "use_state_duration", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*241*/ new Function { Name = "uses_missiles", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*242*/ new Function { Name = "uses_lasers", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*243*/ new Function { Name = "has_damage_radius", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*244*/ new Function { Name = "missile_count", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*245*/ new Function { Name = "laser_count", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*246*/ new Function { Name = "shots_per_minute", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*247*/ new Function { Name = "milliseconds_per_shot", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*248*/ new Function { Name = "player_crit_chance", Args = new[] { new Argument { Name = "pUnit", Type = ArgType.Unit }, new Argument { Name = "nSlot", Type = ArgType.Int32 } } },
            /*249*/ new Function { Name = "player_crit_damage", Args = new[] { new Argument { Name = "pUnit", Type = ArgType.Unit }, new Argument { Name = "nSlot", Type = ArgType.Int32 } } },
            /*250*/ new Function { Name = "add_item_level_armor", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercent", Type = ArgType.Int32 } } },
            /*251*/ new Function { Name = "player_level_skill_power_cost_percent", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*252*/ new Function { Name = "item_level_damage_mult", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*253*/ new Function { Name = "item_level_feed", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*254*/ new Function { Name = "item_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*255*/ new Function { Name = "item_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*256*/ new Function { Name = "item_level_shield_buffer", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*257*/ new Function { Name = "monster_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*258*/ new Function { Name = "monster_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*259*/ new Function { Name = "monster_level_damage", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*260*/ new Function { Name = "monster_level_damage_pct", Args = new[] { new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPCT", Type = ArgType.Int32 } } },
            /*261*/ new Function { Name = "monster_level_shields", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*262*/ new Function { Name = "monster_level_armor", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*263*/ new Function { Name = "unit_ai_changer_attack", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*264*/ new Function { Name = "does_field_damage", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*265*/ new Function { Name = "distance_to_player", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*266*/ new Function { Name = "has_container", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*267*/ new Function { Name = "monster_armor", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nDamageType", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nPercent", Type = ArgType.Int32 } } },
            /*268*/ new Function { Name = "getProcID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nProc", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*269*/ new Function { Name = "getDamageEffectID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDamageEffect", Type = ArgType.ExcelIndex, TableIndex = 32 } } },
            /*270*/ new Function { Name = "getDmgType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDamageType", Type = ArgType.ExcelIndex, TableIndex = 31 } } },
            /*271*/ new Function { Name = "getUnitTypeID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*272*/ new Function { Name = "getSkillDmgMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*273*/ new Function { Name = "getSkillArmorMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*274*/ new Function { Name = "getSkillAttackSpeedMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*275*/ new Function { Name = "getSkillToHitMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*276*/ new Function { Name = "getSkillPctDmgMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*277*/ new Function { Name = "getPetCountOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*278*/ new Function { Name = "runScriptOnPetsOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*279*/ new Function { Name = "randaffixtype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affixType", Type = ArgType.ExcelIndex, TableIndex = 51 } } },
            /*280*/ new Function { Name = "randaffixgroup", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affixGroup", Type = ArgType.ExcelIndex, TableIndex = 197 } } },
            /*281*/ new Function { Name = "applyaffix", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affix", Type = ArgType.ExcelIndex, TableIndex = 53 }, new Argument { Name = "bForce", Type = ArgType.Int32 } } },
            /*282*/ new Function { Name = "getBonusValue", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*283*/ new Function { Name = "getBonusAll", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*284*/ new Function { Name = "getDMGAugmentation", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillVar", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercentOfLevel", Type = ArgType.Int32 }, new Argument { Name = "nSkillPointsInvested", Type = ArgType.Int32 } } },
            /*285*/ new Function { Name = "getDMGAugmentationPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillVar", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercentOfLevel", Type = ArgType.Int32 }, new Argument { Name = "nSkillPointsInvested", Type = ArgType.Int32 } } },
            /*286*/ new Function { Name = "getMonsterHPAtLevel", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 } } },
            /*287*/ new Function { Name = "getMonsterHPAtLevelByPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPCT", Type = ArgType.Int32 } } },
            /*288*/ new Function { Name = "display_dmg_absorbed_pct", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*289*/ new Function { Name = "dmg_percent_by_energy", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*290*/ new Function { Name = "weapon_range", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*291*/ new Function { Name = "IsObjectDestructable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*292*/ new Function { Name = "GlobalThemeIsEnabled", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nTheme", Type = ArgType.ExcelIndex, TableIndex = 168 } } },
            /*293*/ new Function { Name = "RemoveHPAndCheckForDeath", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nRemove", Type = ArgType.Int32 } } },
            /*294*/ new Function { Name = "getSkillStat", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillStat", Type = ArgType.ExcelIndex, TableIndex = 45 }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 } } },
            /*295*/ new Function { Name = "getItemStatByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillStat", Type = ArgType.ExcelIndex, TableIndex = 45 }, new Argument { Name = "nPct", Type = ArgType.Int32 } } },
            /*296*/ new Function { Name = "TownPortalIsAllowed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*297*/ new Function { Name = "lowerManaCostOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nPctPower", Type = ArgType.Int32 } } },
            /*298*/ new Function { Name = "lowerCoolDownOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nPctCooldown", Type = ArgType.Int32 } } },
            /*299*/ new Function { Name = "skillIsOn", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*300*/ new Function { Name = "getSkillRange", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*301*/ new Function { Name = "setDmgEffectParams", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 32 }, new Argument { Name = "nParam0", Type = ArgType.Int32 }, new Argument { Name = "nParam1", Type = ArgType.Int32 }, new Argument { Name = "nParam2", Type = ArgType.Int32 } } },
            /*302*/ new Function { Name = "setDmgEffectSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 32 }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*303*/ new Function { Name = "setDmgEffectSkillOnTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 32 }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*304*/ new Function { Name = "getSkillID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*305*/ new Function { Name = "fireMissileFromObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "missileID", Type = ArgType.ExcelIndex, TableIndex = 111 } } },
            /*306*/ new Function { Name = "caculateGemSockets", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*307*/ new Function { Name = "caculateRareGemSockets", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*308*/ new Function { Name = "executeSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*309*/ new Function { Name = "executeSkillOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*310*/ new Function { Name = "stopSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*311*/ new Function { Name = "powercost", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 } } },
            /*312*/ new Function { Name = "powercost_at_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 42 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /*313*/ new Function { Name = "is_stash_ui_open" },
            /*314*/ new Function { Name = "setRecipeLearned", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*315*/ new Function { Name = "getRecipeLearned", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*316*/ new Function { Name = "getRecipeCategoryLevel", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nCategory", Type = ArgType.ExcelIndex, TableIndex = 186 } } },
            /*317*/ new Function { Name = "createRecipe", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*318*/ new Function { Name = "createSpecificRecipe", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeID", Type = ArgType.ExcelIndex, TableIndex = 109 } } },
            /*319*/ new Function { Name = "getCurrentGameTick", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*320*/ new Function { Name = "getSkillMaxLevel", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*321*/ new Function { Name = "giveAllRecipes", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*322*/ new Function { Name = "getSkillPctInvested", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /*323*/ new Function { Name = "has_a", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /*324*/ new Function { Name = "AddToAnchorMakers", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*325*/ new Function { Name = "item_belongs_to_gambler", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*326*/ new Function { Name = "reevaluate_defense", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*327*/ new Function { Name = "player_level_attack_rating", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*328*/ new Function { Name = "pet_level_attack_rating", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nLevel", Type = ArgType.Int32 } } },
            /*329*/ new Function { Name = "pet_reevaluate_defense", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*330*/ new Function { Name = "reevaluate_feed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*331*/ new Function { Name = "reevaluate_affix_feed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList } } },
            /*332*/ new Function { Name = "shift", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 28 } } },
            /*333*/ new Function { Name = "combat_has_secondary_attacks", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*334*/ new Function { Name = "combat_is_primary_attack", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*335*/ new Function { Name = "skill_script_param", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "state", Type = ArgType.ExcelIndex, TableIndex = 74 } } },
            /*336*/ new Function { Name = "reevaluate_stacksize", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } }
        };

        private static readonly List<Function> CallFunctionsResurrection = new List<Function>
        {
            /*  0*/ new Function { Name = "abs", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*  1*/ new Function { Name = "min", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /*  2*/ new Function { Name = "max", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /*  3*/ new Function { Name = "pin", Args = new[] { new Argument { Name = "value", Type = ArgType.Int32 }, new Argument { Name = "min", Type = ArgType.Int32 }, new Argument { Name = "max", Type = ArgType.Int32 } } },
            /*  4*/ new Function { Name = "pct", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /*  5*/ new Function { Name = "pctFloat", Args = new[] { new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 }, new Argument { Name = "c", Type = ArgType.Int32 } } },
            /*  6*/ new Function { Name = "rand", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /*  7*/ new Function { Name = "randByUnitId", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /*  8*/ new Function { Name = "chance", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nChanceOutOf", Type = ArgType.Int32 } } },
            /*  9*/ new Function { Name = "chanceByStateMod", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nChanceOutOf", Type = ArgType.Int32 }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "nModBy", Type = ArgType.Int32 } } },
            /* 10*/ new Function { Name = "randSkillSeed", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 11*/ new Function { Name = "roll", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 } } },
            /* 12*/ new Function { Name = "divMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 }, new Argument { Name = "b", Type = ArgType.Int32 }, new Argument { Name = "c", Type = ArgType.Int32 } } },
            /* 13*/ new Function { Name = "distribute", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "numdie", Type = ArgType.Int32 }, new Argument { Name = "diesize", Type = ArgType.Int32 }, new Argument { Name = "start", Type = ArgType.Int32 } } },
            /* 14*/ new Function { Name = "roundstat", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "value", Type = ArgType.Int32 } } },
            /* 15*/ new Function { Name = "getEventType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAffixID", Type = ArgType.ExcelIndex, TableIndex = 25 } } },
            /* 16*/ new Function { Name = "getAffixIDByName", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAffixID", Type = ArgType.ExcelIndex, TableIndex = 53 } } },
            /* 17*/ new Function { Name = "get_skill_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /* 18*/ new Function { Name = "get_skill_level_object", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /* 19*/ new Function { Name = "pickskill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 20*/ new Function { Name = "pickskillbyunittype", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 21*/ new Function { Name = "removeoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 22*/ new Function { Name = "killoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 23*/ new Function { Name = "pickskillbyskillgroup", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stats", Type = ArgType.StatsList }, new Argument { Name = "nSkillGroup", Type = ArgType.ExcelIndex, TableIndex = 39 }, new Argument { Name = "nSkillLevel", Type = ArgType.Int32 } } },
            /* 24*/ new Function { Name = "learnskill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skill", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /* 25*/ new Function { Name = "getStatOwnerDivBySkillVar", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nVar", Type = ArgType.Int32 } } },
            /* 26*/ new Function { Name = "getStatOwnerDivBy", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nDivBy", Type = ArgType.Int32 } } },
            /* 27*/ new Function { Name = "switchUnitAndObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 28*/ new Function { Name = "getAchievementCompleteCount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nAchievementID", Type = ArgType.ExcelIndex, TableIndex = 183 } } },
            /* 29*/ new Function { Name = "getVarRange", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 30*/ new Function { Name = "getVar", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 31*/ new Function { Name = "getAttackerSkillVarBySkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 32*/ new Function { Name = "getVarFromSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 33*/ new Function { Name = "getVarFromSkillFromObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nVariable", Type = ArgType.Int32 } } },
            /* 34*/ new Function { Name = "hasStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 35*/ new Function { Name = "hasState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 36*/ new Function { Name = "clearStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 37*/ new Function { Name = "clearState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 38*/ new Function { Name = "clearStateClient", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 39*/ new Function { Name = "isDualWielding", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 40*/ new Function { Name = "getWieldingIsACount", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /* 41*/ new Function { Name = "setState", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 42*/ new Function { Name = "setStateObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /* 43*/ new Function { Name = "setStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 44*/ new Function { Name = "addStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 45*/ new Function { Name = "addStateWithTimeMSClient", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 46*/ new Function { Name = "setStateWithTimeMSOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 47*/ new Function { Name = "setStateWithTimeMSScriptOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 48*/ new Function { Name = "BroadcastEquipEvent", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 49*/ new Function { Name = "setAITargetToSkillTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 50*/ new Function { Name = "setObjectAITargetToUnitAITarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 51*/ new Function { Name = "makeAIAwareOfObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 52*/ new Function { Name = "setAITargetToObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 53*/ new Function { Name = "hasSkillTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 54*/ new Function { Name = "setStateOnSkillTargetWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "clearFirst", Type = ArgType.Int32 } } },
            /* 55*/ new Function { Name = "runScriptParamOnStateClear", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 56*/ new Function { Name = "getCountOfUnitsInArea", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "area", Type = ArgType.Int32 } } },
            /* 57*/ new Function { Name = "runScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "scriptIndex", Type = ArgType.Int32 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /* 58*/ new Function { Name = "doSkillAndScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "scriptIndex", Type = ArgType.Int32 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /* 59*/ new Function { Name = "doSkillOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkill", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "area", Type = ArgType.Int32 }, new Argument { Name = "chance", Type = ArgType.Int32 }, new Argument { Name = "flag", Type = ArgType.Int32 } } },
            /* 60*/ new Function { Name = "setStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 61*/ new Function { Name = "setStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 62*/ new Function { Name = "setStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 63*/ new Function { Name = "addStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 64*/ new Function { Name = "addStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 } } },
            /* 65*/ new Function { Name = "addStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nState", Type = ArgType.ExcelIndex, TableIndex = 75 }, new Argument { Name = "timerMS", Type = ArgType.Int32 }, new Argument { Name = "paramIndex", Type = ArgType.Int32 } } },
            /* 66*/ new Function { Name = "setDmgEffect", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nChance", Type = ArgType.Int32 }, new Argument { Name = "nTime", Type = ArgType.Int32 }, new Argument { Name = "nRoll", Type = ArgType.Int32 } } },
            /* 67*/ new Function { Name = "getStatOwner", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 68*/ new Function { Name = "getStatParent", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 69*/ new Function { Name = "addPCTStatOnOwner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /* 70*/ new Function { Name = "setStatOnOwner", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nValue", Type = ArgType.Int32 }, new Argument { Name = "nParam", Type = ArgType.Int32 } } },
            /* 71*/ new Function { Name = "total", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 72*/ new Function { Name = "basetotal", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 73*/ new Function { Name = "basestat", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 74*/ new Function { Name = "getcur", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /* 75*/ new Function { Name = "statidx", Args = new[] { new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 76*/ new Function { Name = "invcount", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "location", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /* 77*/ new Function { Name = "is_in_invloc", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "location", Type = ArgType.ExcelIndex, TableIndex = 24 } } },
            /* 78*/ new Function { Name = "dmgrider", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 } } },
            /* 79*/ new Function { Name = "procrider", Args = new[] { new Argument { Name = "game3", Type = ArgType.Game3 } } },
            /* 80*/ new Function { Name = "knockback", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /* 81*/ new Function { Name = "colorcoderequirement", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /* 82*/ new Function { Name = "color_code_modunit_requirement", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param", Type = ArgType.Param } } },
            /* 83*/ new Function { Name = "color_code_modunit_requirement2", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat1", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param1", Type = ArgType.Param }, new Argument { Name = "stat2", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "param2", Type = ArgType.Param } } },
            /* 84*/ new Function { Name = "feedchange", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nCheckBonus", Type = ArgType.Int32 } } },
            /* 85*/ new Function { Name = "hoverstatchange", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 86*/ new Function { Name = "hoverstatchangeSetItem", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 87*/ new Function { Name = "feedcolorcode", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /* 88*/ new Function { Name = "color_code_pos_neg_val2", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 89*/ new Function { Name = "colorposneg", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 90*/ new Function { Name = "colorcodeprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /* 91*/ new Function { Name = "colorcodeclassreq", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 92*/ new Function { Name = "colorcodeSexReq", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 93*/ new Function { Name = "colorcodeskillslots", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 94*/ new Function { Name = "colorcodeskillusable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 95*/ new Function { Name = "colorcodeskillgroupusable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /* 96*/ new Function { Name = "meetsclassreqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /* 97*/ new Function { Name = "meetsSexReqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /* 98*/ new Function { Name = "fontcolorrow", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nColorIndex", Type = ArgType.ExcelIndex, TableIndex = 7 } } },
            /* 99*/ new Function { Name = "nodrop", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*100*/ new Function { Name = "notrade", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*101*/ new Function { Name = "objectNotrade", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*102*/ new Function { Name = "BuyPriceByValue", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "valueType", Type = ArgType.Int32 } } },
            /*103*/ new Function { Name = "SellPriceByValue", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "valueType", Type = ArgType.Int32 } } },
            /*104*/ new Function { Name = "buyprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*105*/ new Function { Name = "buypriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nindex", Type = ArgType.Int32 } } },
            /*106*/ new Function { Name = "usetermRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nindex", Type = ArgType.Int32 } } },
            /*107*/ new Function { Name = "IsExistPackageContents", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*108*/ new Function { Name = "sellprice", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*109*/ new Function { Name = "sellpriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*110*/ new Function { Name = "hitChance", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*111*/ new Function { Name = "dodgeChance", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*112*/ new Function { Name = "numaffixes", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*113*/ new Function { Name = "qualitypricemult", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*114*/ new Function { Name = "enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*115*/ new Function { Name = "visible_enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*116*/ new Function { Name = "champions_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*117*/ new Function { Name = "mutant_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*118*/ new Function { Name = "mutant_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*119*/ new Function { Name = "distance_sq_to_champion", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*120*/ new Function { Name = "champion_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*121*/ new Function { Name = "bosses_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*122*/ new Function { Name = "distance_sq_to_boss", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*123*/ new Function { Name = "boss_hp_pct", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*124*/ new Function { Name = "enemy_corpses_in_radius", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "radius", Type = ArgType.Int32 } } },
            /*125*/ new Function { Name = "monsters_killed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*126*/ new Function { Name = "monsters_killed_nonteam", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*127*/ new Function { Name = "monsters_pct_left", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*128*/ new Function { Name = "hp_lost", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*129*/ new Function { Name = "meters_moved", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*130*/ new Function { Name = "attacks", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*131*/ new Function { Name = "is_alive", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*132*/ new Function { Name = "monster_level", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*133*/ new Function { Name = "has_active_task", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*134*/ new Function { Name = "is_usable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*135*/ new Function { Name = "is_examinable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*136*/ new Function { Name = "is_operatable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*137*/ new Function { Name = "isa", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*138*/ new Function { Name = "issuba", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*139*/ new Function { Name = "is_subscriber", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*140*/ new Function { Name = "has_value_state", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*141*/ new Function { Name = "has_standard_state", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*142*/ new Function { Name = "has_premium_state", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*143*/ new Function { Name = "has_inv_ext1_state", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*144*/ new Function { Name = "has_inv_ext2_state", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*145*/ new Function { Name = "has_inv_ext3_state", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*146*/ new Function { Name = "is_SharedStash", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*147*/ new Function { Name = "is_hardcore", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*148*/ new Function { Name = "is_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*149*/ new Function { Name = "get_difficulty", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*150*/ new Function { Name = "same_game_variant", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*151*/ new Function { Name = "player_is_in_guild", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*152*/ new Function { Name = "get_act", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*153*/ new Function { Name = "email_send_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*154*/ new Function { Name = "email_receive_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*155*/ new Function { Name = "colorcodesubscriber", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*156*/ new Function { Name = "item_requires_subscriber", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*157*/ new Function { Name = "colorcodevariantnormal", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*158*/ new Function { Name = "colorcodevarianthardcore", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*159*/ new Function { Name = "colorcodevariantelite", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*160*/ new Function { Name = "colorcodevarianthardcoreelite", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*161*/ new Function { Name = "colorcodenightmare", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*162*/ new Function { Name = "item_is_nightmare_specific", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*163*/ new Function { Name = "item_is_variant_normal", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*164*/ new Function { Name = "item_is_variant_hardcore", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*165*/ new Function { Name = "item_is_variant_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*166*/ new Function { Name = "item_is_variant_hardcore_elite", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*167*/ new Function { Name = "quality", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*168*/ new Function { Name = "meetsitemreqs", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*169*/ new Function { Name = "weapondps", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*170*/ new Function { Name = "SkillTargetIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*171*/ new Function { Name = "GetObjectIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*172*/ new Function { Name = "GetMissileSourceIsA", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "unittype", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*173*/ new Function { Name = "GetSkillHasReqWeapon", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*174*/ new Function { Name = "has_use_skill", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*175*/ new Function { Name = "hasdomname", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*176*/ new Function { Name = "dps", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "time", Type = ArgType.Int32 } } },
            /*177*/ new Function { Name = "ObjectCanUpgrade", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*178*/ new Function { Name = "use_state_duration", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*179*/ new Function { Name = "uses_missiles", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*180*/ new Function { Name = "uses_lasers", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*181*/ new Function { Name = "has_damage_radius", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*182*/ new Function { Name = "missile_count", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*183*/ new Function { Name = "laser_count", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*184*/ new Function { Name = "shots_per_minute", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*185*/ new Function { Name = "milliseconds_per_shot", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*186*/ new Function { Name = "player_crit_chance", Args = new[] { new Argument { Name = "pUnit", Type = ArgType.Unit }, new Argument { Name = "nSlot", Type = ArgType.Int32 } } },
            /*187*/ new Function { Name = "player_crit_damage", Args = new[] { new Argument { Name = "pUnit", Type = ArgType.Unit }, new Argument { Name = "nSlot", Type = ArgType.Int32 } } },
            /*188*/ new Function { Name = "add_item_level_armor", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercent", Type = ArgType.Int32 } } },
            /*189*/ new Function { Name = "player_level_skill_power_cost_percent", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*190*/ new Function { Name = "item_level_damage_mult", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*191*/ new Function { Name = "item_level_feed", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*192*/ new Function { Name = "item_level_upgprice", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*193*/ new Function { Name = "item_upgrade_feed", Args = new[] { new Argument { Name = "upgradeCnt", Type = ArgType.Int32 } } },
            /*194*/ new Function { Name = "item_quality_feed_weight", Args = new[] { new Argument { Name = "quality", Type = ArgType.Int32 } } },
            /*195*/ new Function { Name = "item_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*196*/ new Function { Name = "item_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*197*/ new Function { Name = "item_level_shield_buffer", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*198*/ new Function { Name = "monster_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*199*/ new Function { Name = "monster_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*200*/ new Function { Name = "monster_level_damage", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*201*/ new Function { Name = "monster_level_damage_pct", Args = new[] { new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPCT", Type = ArgType.Int32 } } },
            /*202*/ new Function { Name = "monster_level_shields", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*203*/ new Function { Name = "monster_level_armor", Args = new[] { new Argument { Name = "level", Type = ArgType.Int32 } } },
            /*204*/ new Function { Name = "unit_ai_changer_attack", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*205*/ new Function { Name = "does_field_damage", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*206*/ new Function { Name = "distance_to_player", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*207*/ new Function { Name = "has_container", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*208*/ new Function { Name = "monster_armor", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nDamageType", Type = ArgType.ExcelIndex, TableIndex = 30 }, new Argument { Name = "nPercent", Type = ArgType.Int32 } } },
            /*209*/ new Function { Name = "getSkillDmgMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*210*/ new Function { Name = "getSkillArmorMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*211*/ new Function { Name = "getSkillAttackSpeedMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*212*/ new Function { Name = "getSkillToHitMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*213*/ new Function { Name = "getSkillPctDmgMult", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*214*/ new Function { Name = "getPetCountOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*215*/ new Function { Name = "runScriptOnPetsOfType", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*216*/ new Function { Name = "randaffixtype", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affixType", Type = ArgType.ExcelIndex, TableIndex = 51 } } },
            /*217*/ new Function { Name = "randaffixgroup", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affixGroup", Type = ArgType.ExcelIndex, TableIndex = 53 } } },
            /*218*/ new Function { Name = "applyaffix", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "affix", Type = ArgType.ExcelIndex, TableIndex = 53 }, new Argument { Name = "bForce", Type = ArgType.Int32 } } },
            /*219*/ new Function { Name = "getBonusValue", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "a", Type = ArgType.Int32 } } },
            /*220*/ new Function { Name = "getBonusAll", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*221*/ new Function { Name = "getDMGAugmentation", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillVar", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercentOfLevel", Type = ArgType.Int32 }, new Argument { Name = "nSkillPointsInvested", Type = ArgType.Int32 } } },
            /*222*/ new Function { Name = "getDMGAugmentationPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillVar", Type = ArgType.Int32 }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPercentOfLevel", Type = ArgType.Int32 }, new Argument { Name = "nSkillPointsInvested", Type = ArgType.Int32 } } },
            /*223*/ new Function { Name = "getMonsterHPAtLevel", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 } } },
            /*224*/ new Function { Name = "getMonsterHPAtLevelByPCT", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nLevel", Type = ArgType.Int32 }, new Argument { Name = "nPCT", Type = ArgType.Int32 } } },
            /*225*/ new Function { Name = "display_dmg_absorbed_pct", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*226*/ new Function { Name = "dmg_percent_by_energy", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*227*/ new Function { Name = "weapon_range", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*228*/ new Function { Name = "IsObjectDestructable", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*229*/ new Function { Name = "GlobalThemeIsEnabled", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nTheme", Type = ArgType.ExcelIndex, TableIndex = 170 } } },
            /*230*/ new Function { Name = "SetRespawnPlayer", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*231*/ new Function { Name = "AddSecondaryRespawnPlayer", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*232*/ new Function { Name = "RemoveHPAndCheckForDeath", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "nRemove", Type = ArgType.Int32 } } },
            /*233*/ new Function { Name = "getSkillStat", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nSkillStat", Type = ArgType.ExcelIndex, TableIndex = 44 }, new Argument { Name = "nSkillLvl", Type = ArgType.Int32 } } },
            /*234*/ new Function { Name = "TownPortalIsAllowed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*235*/ new Function { Name = "RecallIsAllowed", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*236*/ new Function { Name = "lowerManaCostOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nPctPower", Type = ArgType.Int32 } } },
            /*237*/ new Function { Name = "lowerCoolDownOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 }, new Argument { Name = "nPctCooldown", Type = ArgType.Int32 } } },
            /*238*/ new Function { Name = "skillIsOn", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*239*/ new Function { Name = "getSkillRange", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*240*/ new Function { Name = "setDmgEffectParams", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "nParam0", Type = ArgType.Int32 }, new Argument { Name = "nParam1", Type = ArgType.Int32 }, new Argument { Name = "nParam2", Type = ArgType.Int32 } } },
            /*241*/ new Function { Name = "setDmgEffectSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*242*/ new Function { Name = "setDmgEffectSkillOnTarget", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nDmgEffect", Type = ArgType.ExcelIndex, TableIndex = 31 }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*243*/ new Function { Name = "getSkillID", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*244*/ new Function { Name = "fireMissileFromObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "missileID", Type = ArgType.ExcelIndex, TableIndex = 114 } } },
            /*245*/ new Function { Name = "caculateGemSockets", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*246*/ new Function { Name = "caculateRareGemSockets", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*247*/ new Function { Name = "caculateCraftingSlots", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*248*/ new Function { Name = "executeSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*249*/ new Function { Name = "executeSkillOnObject", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*250*/ new Function { Name = "stopSkill", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*251*/ new Function { Name = "powercost", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "skillID", Type = ArgType.ExcelIndex, TableIndex = 41 } } },
            /*252*/ new Function { Name = "is_stash_ui_open" },
            /*253*/ new Function { Name = "setRecipeLearned", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*254*/ new Function { Name = "getRecipeLearned", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*255*/ new Function { Name = "createRecipe", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*256*/ new Function { Name = "createSpecificRecipe", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nRecipeID", Type = ArgType.ExcelIndex, TableIndex = 112 } } },
            /*257*/ new Function { Name = "getCurrentGameTick", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*258*/ new Function { Name = "has_a", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nUnitType", Type = ArgType.ExcelIndex, TableIndex = 23 } } },
            /*259*/ new Function { Name = "item_belongs_to_gambler", Args = new[] { new Argument { Name = "context", Type = ArgType.Context } } },
            /*260*/ new Function { Name = "combat_has_secondary_attacks", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*261*/ new Function { Name = "combat_is_primary_attack", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*262*/ new Function { Name = "constrain", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nOldVersion", Type = ArgType.Int32 }, new Argument { Name = "nUpdateVersion", Type = ArgType.Int32 }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nParam", Type = ArgType.Param }, new Argument { Name = "nMin", Type = ArgType.Int32 }, new Argument { Name = "nMax", Type = ArgType.Int32 } } },
            /*263*/ new Function { Name = "modifyratio", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nOldVersion", Type = ArgType.Int32 }, new Argument { Name = "nUpdateVersion", Type = ArgType.Int32 }, new Argument { Name = "nStat", Type = ArgType.ExcelIndex, TableIndex = 27 }, new Argument { Name = "nParam", Type = ArgType.Param }, new Argument { Name = "nOldMin", Type = ArgType.Int32 }, new Argument { Name = "nOldMax", Type = ArgType.Int32 }, new Argument { Name = "nMin", Type = ArgType.Int32 }, new Argument { Name = "nMax", Type = ArgType.Int32 } } },
            /*264*/ new Function { Name = "max_item_augments", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*265*/ new Function { Name = "is_pvp", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*266*/ new Function { Name = "is_pvptdm", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*267*/ new Function { Name = "is_pvpctl", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*268*/ new Function { Name = "is_pvpelm", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 } } },
            /*269*/ new Function { Name = "drain_target_power", Args = new[] { new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "nValue", Type = ArgType.Int32 } } },
            /*270*/ new Function { Name = "display_pvp_exp_to_next", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*271*/ new Function { Name = "display_pvp_win_percentage", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*272*/ new Function { Name = "getStatsOnStateSet", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context }, new Argument { Name = "stat", Type = ArgType.ExcelIndex, TableIndex = 27 } } },
            /*273*/ new Function { Name = "getItemStateDuration", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*274*/ new Function { Name = "hasItem", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "itemID", Type = ArgType.ExcelIndex, TableIndex = 103 } } },
            /*275*/ new Function { Name = "skill_script_param", Args = new[] { new Argument { Name = "game4", Type = ArgType.Game4 }, new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "state", Type = ArgType.ExcelIndex, TableIndex = 75 } } },
            /*276*/ new Function { Name = "is_allow_sword", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } },
            /*277*/ new Function { Name = "is_bindable", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*278*/ new Function { Name = "getConditionRatio", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit } } },
            /*279*/ new Function { Name = "refund_regist_item_okay", Args = new[] { new Argument { Name = "unit", Type = ArgType.Unit }, new Argument { Name = "context", Type = ArgType.Context } } }
        };

        /* OpCode           Function                                Action
         * 0    0x00        Return                                  Must be at end of script.
         * 
         * 3    0x03        Call Function                           Calls a client function.
         * e.g.
         *      26,0, 3,185, 0                                      -> return player_crit_damage(0); // 0 = right hand, 1 = left hand
         *      
          * 26   0x1A        Push                                    Pushes a number onto the stack.
         * e.g.
         *      26,1000, 0                                          -> return 1000;
         * 
         * 339  0x166       Negation                                Performs a boolean-not operation.
         * e.g.
         *      26,42, 26,13, 470, 339, 0                           -> return !(42 == 13); // = true
         *      
         * 358  0x166       Multiplication                          Product of the two most recently pushed numbers.
         * e.g.
         *      26,10, 26,20, 358, 0                                -> return 10 * 20; // = 200
         *      
         * 369  0x171       Division                                Divition of the two most recently pushed numbers.
         * e.g.
         *      26, 200, 26, 5, 369, 0                              -> return 200 / 5; // = 40
         * 
         * 388  0x184       Addition                                Sum of the two most recently pushed numbers.
         * e.g.
         *      26,5, 26,10, 388, 26,100, 358, 0                    -> return (5 + 10) * 100; // = 150
         * 
         * 399  0x18F       Substraction                            Difference of the two most recently pushed numbers.
         * e.g.
         *      26,13, 26,42, 388, 26,99, 358, 26,4108, 399, 0      -> return (13 + 42) * 99 - 4108; // = 1337
         * 
         * 426  0x1AA       Less Than                               Performs a less-than comparison test.
         * e.g.
         *      26,55, 26,50, 470, 0                                -> return (55 < 50); // = false
         * 
         * 437  0x1B5       Greater Than                            Performs a greater-than comparison test.
         * e.g.
         *      26,42, 26,13, 470, 0                                -> return (42 > 12); // = true
         * 
         * 448  0x1C0       Less Than or Equal To                   Performs a less-than or equal to comparison test.
         * e.g.
         *      26,111, 26,111, 448, 0                              -> return (111 <= 111); // = true
         * 
         * 459  0x1CB       Greater Than or Equal To                Performs a greater-than or equal to comparison test.
         * e.g.
         *      26,7, 26,21, 459, 0                                -> return (7 >= 21); // = false
         * 
         * 470  0x1D6       Equal To                                Performs an equal-to comparison test.
         * e.g.
         *      26,50, 26,50, 470, 0                                -> return (50 == 50); // = true
         * 
         * 481  0x1E1       Not Equal To                            Performs a not equal-to comparison test.
         * e.g.
         *      26,130, 26,120, 470, 0                              -> return (130 != 120); // = true
         * 
         * 666  0x29A       GetStatValue                            Gets the value of a stat at the following supplied index (from STATS table).
         * e.g.
         *      666, 612368384, 0                                   -> return GetStat(612368384); // = GetStat(612368384 << 22 = 146) = GetStat("experience");
         * 
         * 669  0x29D       SetStatValue                            Sets the value of a stat.
         * e.g.
         *      26,10, 669,25165824, 0                              -> return SetStat(25165824); // = SetStat(25165824 << 22 = 6) = SetStat("hp_cur");
         * 
         * 
         */

        // op code array in client found at:  .data:00000001407161C8
        public enum ScriptOpCodes : uint
        {
            Return = 0,                     // 0x00
            CallPropery = 2,                // 0x02
            Call = 3,                       // 0x03
            TernaryFalse = 4,               // 0x04     the result/script returned of original (prior to TernaryTrue) stack object boolean evaluation result of false
            AllocateVar = 6,                // 0x06
            Unknown9 = 9,                   // 0x09     seen in properties function "holy_radius_enemies": 707,0,714,666,1228931072,26,8,707,0,714,666,1237319680,399,26,8,3,58,26,0,3,59,3,58,9,0
            TernaryTrue = 14,               // 0x0E     the result/script returned of the previous stack object boolean evaluation result of true
            Push = 26,                      // 0x1A
            PushLocalVarInt32 = 50,         // 0x32     .rdata:0000000140606B18     aPushLocalVar_3 db 'push local variable at offset %u  value = %d  type = int'
            PushLocalVarPtr = 57,           // 0x39     .rdata:0000000140606D08     aPushLocalVa_10 db 'push local variable at offset %u  value = %x  type = pointer'
            AssignLocalVarInt32 = 98,       // 0x62     .rdata:0000000140607988     aAssignLocalV_3 db 'assign local variable at offset %u  value = %d  type = int'
            AssignLocalVarPtr = 105,        // 0x69     .rdata:0000000140607B90     aAssignLocal_10 db 'assign local variable at offset %u  value = %x  type = pointer'
            Complement = 320,               // 0x140
            Not = 339,                      // 0x153
            Pow = 347,                      // 0x15B
            Mult = 358,                     // 0x166
            Div = 369,                      // 0x171
            Add = 388,                      // 0x184
            Sub = 399,                      // 0x18F
            LessThan = 426,                 // 0x1AA
            GreaterThan = 437,              // 0x1B5
            LessThanOrEqual = 448,          // 0x1C0
            GreaterThanOrEqual = 459,       // 0x1CB
            EqualTo = 470,                  // 0x1D6
            NotEqualTo = 481,               // 0x1E1
            And = 516,                      // 0x204    .rdata:000000014060AB50     aLandjmpOffse_6 db 'landjmp  offset = %u  type = int'
            Or = 527,                       // 0x20F    .rdata:000000014060AF00     aLorjmpOffset_6 db 'lorjmp  offset = %u  type = int'
            EndCond = 538,                  // 0x21A    .rdata:000000014060B198     aLogendTypeInt  db 'logend  type = int'
            GetStat666 = 666,               // 0x29A    these are used for different arg/return types (int/uint/float/double/etc)
            GetStat667 = 667,               // 0x29B    not sure which are which yet - do it later
            SetStat669 = 669,               // 0x29D
            SetStat673 = 673,               // 0x2A1
            SetStat674 = 674,               // 0x2A2
            GetStat680 = 680,               // 0x2A8
            SetStat683 = 683,               // 0x2AB
            SetStat687 = 687,               // 0x2AF
            SetStat688 = 688,               // 0x2B0
            PushContextVarInt32 = 700,      // 0x2BC    .rdata:000000014060C1A8     aPushContextV_3 db 'push context variable %s value = %d  type = int'
            PushContextVarUInt32 = 701,     // 0x2BD    .rdata:000000014060C1E0     aPushContextV_4 db 'push context variable %s value = %u  type = unsigned int'
            PushContextVarInt64 = 702,      // 0x2BE    .rdata:000000014060C220     aPushContextV_5 db 'push context variable %s value = %I64d  type = INT64'
            PushContextVarUInt64 = 703,     // 0x2BF    .rdata:000000014060C258     aPushContextV_6 db 'push context variable %s value = %I64u  type = UINT64'
            PushContextVarFloat = 704,      // 0x2C0    .rdata:000000014060C290     aPushContextV_7 db 'push context variable %s value = %f  type = float'
            PushContextVarDouble = 705,     // 0x2C1    .rdata:000000014060C2C8     aPushContextV_8 db 'push context variable %s value = %f  type = double'
            PushContextVarDouble2 = 706,    // 0x2C2    706 = 705 = sub_14023DF8C
            PushContextVarPtr = 707,        // 0x2C3    .rdata:000000014060C300     aPushContextV_9 db 'push context variable %s value = %x  type = pointer'
            GlobalVarGame3 = 708,           // 0x2C4    I don't think these are "global vars" as such (more like a "type" of local var... kind of)
            GlobalVarContext = 709,         // 0x2C5    but until I know more this will do for now
            GlobalVarGame4 = 710,           // 0x2C6
            GlobalVarUnit = 711,            // 0x2C7
            GlobalVarStatsList = 712,       // 0x2C8
            AssignContextVar = 713,         // 0x2C9
            UsePtrObjectReference = 714     // 0x2CA    // set type/size = 4??  // pointer object usage?? (let's try that) - seems to work
        }

        private enum ContextVariables : uint
        {
            Unit,           // 0        // not seen used
            Object,         // 1        // not seen used
            Source,         // 2        // not seen used
            Statslist,      // 3        // ptr
            Skill,          // 4        // not seen used
            StateId,        // 5        // not seen used
            SkLvl,          // 6        // int32
            Param1,         // 7        // seen used as int32
            Param2          // 8        // seen used as int32
        }

        private class StackObject
        {
            public String Value;
            public int Precedence;              // using chart from http://en.wikipedia.org/wiki/Operators_in_C_and_C%2B%2B (with exponent = 4)
            public bool IsIf;
            public bool IsFunction;
            public bool IsVarAssign;            // for local vars
            public uint ByteOffset;             // for local vars
            public ArgType Type;
            public int StatementCount = -1;
            public int OperatorCount = -1;
            public int TrueStatements = -1;
            public int FalseStatements = -1;
            public int IfLevel = -1;
            public bool IsPrecedenceFunc;
            public ScriptOpCodes OpCode;

            public override string ToString()
            {
                return Value;
            }
        }
        #endregion

        #region Static Debug Function

        public static void EnableDebug(bool enableDebug, bool isTCv4 = false)
        {
            _debug = enableDebug;

            if (!_debug) return;

            String debugRoot = isTCv4 ? DebugRootTestCenter : DebugRoot;
            Directory.CreateDirectory(debugRoot);
            String[] oldLogs = Directory.GetFiles(debugRoot);
            foreach (String logPath in oldLogs)
            {
                File.Delete(logPath);
            }
        }

        #endregion

        #region Compiler Helper Functions

        private StackObject _GetVar(String varName)
        {
            return _vars.FirstOrDefault(varObj => varObj.Value == varName);
        }

        private static int _GetByteOffset(ICollection scriptByteCode)
        {
            return scriptByteCode.Count * 4;
        }

        private void _CompileStatFunctions(List<Int32> scriptByteCode, String nameStr, int funcNameStartOffset, int functionStartOffset, Int32[] contextPtr)
        {
            ScriptOpCodes funcOpCode = _GetScriptOpCode(nameStr.ToLower(CultureInfo.InvariantCulture));
            if (funcOpCode == 0) throw new Exceptions.ScriptUnknownVarNameException(nameStr, funcNameStartOffset);

            _offset++;
            _SkipWhite();

            // do we have excel string or row index?
            String excelStr;
            Object statRow = null;
            int rowIndex = -1;
            if (_script[_offset] == '\'')
            {
                excelStr = _GetString();

                foreach (Object tableRow in _statsTable.Rows)
                {
                    rowIndex++;

                    if ((String)_statsDelegator["stat"](tableRow) != excelStr) continue;

                    statRow = tableRow;
                    break;
                }

                if (statRow == null) throw new Exceptions.UnknownExcelStringException(excelStr, _offset);
                _offset += excelStr.Length + 1;
            }
            else
            {
                rowIndex = _GetNumber();

                if (rowIndex < 0 || rowIndex > _statsTable.Rows.Count) throw new IndexOutOfRangeException(String.Format("Excel row index '{0}' out of range at script offset '{1}'", rowIndex, _offset - rowIndex.ToString().Length));

                statRow = _statsTable.Rows[rowIndex];
                excelStr = rowIndex.ToString(); // for exception below
            }

            _SkipWhite();

            int param1Table = (int)_statsDelegator["param1Table"](statRow);
            bool isSetStat = (nameStr.StartsWith("Set"));

            Int32[] callStatFunc = new[] { (Int32)funcOpCode, 0 };
            if (param1Table == -1 &&
                (isSetStat || _script[_offset] != ',')) // some excel calls have not-used(?) extra params
            {
                int param = rowIndex << 22;
                callStatFunc[1] = param;
            }
            else
            {
                if (_script[_offset] != ',') throw new Exceptions.ScriptFunctionArgumentCountException(nameStr, 2, String.Format("\nThe GetStat for stat '{0}' requires an extra parameter at offset '{1}'.", excelStr, _offset));

                _offset++;
                _SkipWhite();

                int paramRowIndex = -1;
                if (_script[_offset] == '\'') // excel string
                {
                    String paramStr = _GetString();
                    paramRowIndex = _fileManager.GetExcelRowIndexFromTableIndex(param1Table, paramStr);
                    if (paramRowIndex == -1) throw new Exceptions.UnknownExcelStringException(paramStr, _offset);
                    _offset += paramStr.Length + 1;
                }
                else // row index
                {
                    paramRowIndex = _GetNumber();
                    if (paramRowIndex == -1) throw new Exceptions.UnknownExcelStringException(paramRowIndex.ToString(), _offset);
                }

                int param = (rowIndex << 22) | paramRowIndex;
                callStatFunc[1] = param;
            }

            if (isSetStat)
            {
                if (_script[_offset] != ',') throw new Exceptions.ScriptFunctionArgumentCountException(nameStr, 2, String.Format("\nThe GetStat for stat '{0}' requires an value to set it to at offset '{1}'.", excelStr, _offset));
                _offset++;

                Int32[] getStatArgBytes = _Compile(')', null, false, _GetByteOffset(scriptByteCode));
                scriptByteCode.AddRange(getStatArgBytes);
            }

            if (_script[_offset] != ')') throw new Exceptions.ScriptFormatException(String.Format("Unexpected end of function '{0}' starting at offset '{1}'", nameStr, functionStartOffset), _offset);
            _offset++;

            if (contextPtr != null) scriptByteCode.AddRange(contextPtr);
            scriptByteCode.AddRange(callStatFunc);
        }

        private static ArgType _GetArgType(String argNameLower)
        {
            return Enum.GetValues(typeof(ArgType)).Cast<ArgType>().Where(type => type.ToString().ToLower() == argNameLower).FirstOrDefault();
        }

        private String _GetString()
        {
            int endIndex = _script.IndexOf('\'', ++_offset);
            int strLen = endIndex - _offset;
            if (strLen < 0) throw new Exceptions.ScriptFormatException("Unexpected end of string.", _offset);

            return _script.Substring(_offset, endIndex - _offset);
        }

        private int _GetNumber()
        {
            String numStr = _GetNumberStr();
            if (String.IsNullOrEmpty(numStr)) throw new Exceptions.ScriptFormatException("Unexpected string number format", _offset);

            int scriptNum;
            if (!int.TryParse(numStr, out scriptNum)) throw new Exceptions.ScriptFormatException(String.Format("Failed to convert script segment '{0}' to number.", numStr), _offset);

            return scriptNum;
        }

        private static ScriptOpCodes _GetScriptOpCode(String opCodeLower)
        {
            return Enum.GetValues(typeof(ScriptOpCodes)).Cast<ScriptOpCodes>().Where(scriptOpCodes => scriptOpCodes.ToString().ToLower() == opCodeLower).FirstOrDefault();
        }

        private bool _IsNumber()
        {
            int startOffset = _offset;
            while (_script[startOffset] == '-') startOffset++;
            return (_script[startOffset] >= '0' && _script[startOffset] <= '9');
        }

        private String _GetNumberStr()
        {
            int index = _offset;

            // remove/count negative chars
            while (_script[_offset] == '-') _offset++;
            int negativeCount = _offset - index;

            index += negativeCount;
            while (index < _script.Length && _script[index] >= '0' && _script[index] <= '9')
            {
                index++;
            }
            if (index == _script.Length) throw new Exceptions.ScriptFormatException("_GetNumberStr() failed, expected ';' terminator.", --index);

            if (_script[index] >= 'A' && _script[index] <= 'Z' ||
                _script[index] >= 'a' && _script[index] <= 'z' ||
                _script[index] == '_') return null;

            int numLen = index - _offset;
            String numStr = _script.Substring(_offset, numLen);
            _offset += numLen;

            if (negativeCount % 2 == 1) numStr = "-" + numStr;
            return numStr;
        }

        private void _SkipWhite()
        {
            if (_offset >= _script.Length) return;

            while (_offset < _script.Length && (_script[_offset] == ' ' || _script[_offset] == '\n' || _script[_offset] == '\t'))
            {
                _offset++;
            }
        }

        private String _GetNameStr()
        {
            int index = _offset;
            if (_IsNumber()) throw new Exceptions.ScriptFormatException("Unexpected number character in name string!", _offset);

            while (_script[index] >= '0' && _script[index] <= '9' ||
                   _script[index] >= 'A' && _script[index] <= 'Z' ||
                   _script[index] >= 'a' && _script[index] <= 'z' ||
                   _script[index] == '_')
            {
                index++;
            }

            int strLen = index - _offset;
            String nameStr = _script.Substring(_offset, strLen);

            _offset += strLen;
            return nameStr;
        }

        private Function _GetFunction(String functionName)
        {
            return _callFunctions.FirstOrDefault(function => function.Name == functionName);
        }

        private Function[] _GetFunctions(String functionName)
        {
            return (from function in _callFunctions
                    where function.Name == functionName
                    select function).ToArray();
        }

        #endregion

        #region Decomiler Helper Functions

        private void _DecompileCondion(byte[] scriptBytes, ScriptOpCodes opCode, int ifLevel)
        {
            _CheckStack(1, opCode);

            uint byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref _offset);
            Debug.Assert(byteOffset % 4 == 0 && FileTools.ByteArrayToUInt32(scriptBytes, _startOffset + (int)byteOffset - 4) == (uint)ScriptOpCodes.EndCond);

            StackObject conditionObject = _stack.Pop();
            conditionObject.OpCode = opCode;
            _stack.Push(conditionObject);

            int subMaxBytes = (int)byteOffset - (_offset - _startOffset);
            _Decompile(scriptBytes, subMaxBytes, ifLevel + 1);


            if (!_debug || !_debugFormatConditionalByteCounts) return;

            conditionObject = _stack.Pop();
            conditionObject.Value = String.Format("{0}[{1}]", conditionObject.Value, byteOffset);
            _stack.Push(conditionObject);
        }

        private void _PushLocalVar(int byteOffset, ArgType argType)
        {
            String argName = null;
            if (_excelFunction != null)
            {
                argName = (from arg in _excelFunction.Args
                           where arg.ByteOffset == byteOffset
                           select arg.Name).FirstOrDefault();
            }

            if (argName == null)
            {
                StackObject localVar = (from varObj in _vars
                                        where varObj.ByteOffset == byteOffset
                                        select varObj).FirstOrDefault();
                Debug.Assert(localVar != null);

                int index = byteOffset / 4;
                argName = "var" + index;
            }
            Debug.Assert(argName != null);


            _stack.Push(new StackObject { Value = argName, Type = argType });

        }

        private StackObject _Return(int startStackCount, uint bytesRead, bool processStackOnReturn, int ifLevel, bool debugShowParsed)
        {
            _CheckStack(1, ScriptOpCodes.Return);

            String script = String.Empty;
            StackObject stackObject = _stack.Peek();
            int statementCount = _stack.Count - startStackCount; // will be > 0 for if, else, ||, &&
            int stackCount = _stack.Count;

            for (int i = stackCount; processStackOnReturn && i > startStackCount; i--)
            {
                stackObject = _stack.Pop();

                if (statementCount != 1 || startStackCount <= 0) // if (statementCount == 1 && startStackCount > 0), then we don't want to check this - we're in an if/else block
                {
                    if (!stackObject.IsFunction && !stackObject.IsVarAssign && !stackObject.IsIf && _stack.Count != 0)
                    {
                        _stack.Push(stackObject); // re-add for stack dump
                        String error = String.Format("Error: Stack has more than 1 value upon script return: script = \"{0}\"\n{1}", _script, _DumpStack(_stack));
                        throw new Exceptions.ScriptInvalidStackStateException(error);
                    }
                }

                String semiColon = ";";
                if (statementCount == 1 && startStackCount > 0) semiColon = String.Empty; // if true, we're in an if/else/||/&& block, and only 1 statement, so no ;

                String newLine = (i == stackCount) ? String.Empty : "\n"; // if last object, then don't add new line

                if (stackObject.IsIf)
                {
                    script = stackObject.Value + newLine + script;
                }
                else
                {
                    script = stackObject.Value + semiColon + newLine + script;
                }
            }

            if (startStackCount == 0)
            {
                _script += script;
                if (debugShowParsed) Debug.WriteLine(_script);
            }

            StackObject returnStackObject = new StackObject
            {
                Value = script,
                ByteOffset = bytesRead,
                StatementCount = statementCount,
                TrueStatements = stackObject.TrueStatements,
                FalseStatements = stackObject.FalseStatements,
                IfLevel = ifLevel
            };

            // if we have a statementCount, but no true statement count, we had an if-block with no else-block
            if (returnStackObject.TrueStatements == -1 && statementCount > 0)
            {
                returnStackObject.TrueStatements = statementCount;
            }

            return returnStackObject;
        }

        private void _CallFunction(int functionIndex)
        {
            if (functionIndex < 0 || functionIndex > _callFunctions.Count)
            {
                throw new Exceptions.ScriptUnexpectedFunctionIndexException(functionIndex);
            }

            Function excelScriptFunction = _callFunctions[functionIndex];
            _CheckStack(excelScriptFunction.ArgCount, ScriptOpCodes.Call, excelScriptFunction);
            String argsString = String.Empty;

            for (int i = excelScriptFunction.ArgCount - 1; i >= 0; i--)
            {
                StackObject argStackObject = _stack.Pop();
                String argStr = argStackObject.Value;

                // todo: consider removing first arg if is "global" and change to @global->func(var1, var2) instead of func(@global, var1, var2)
                // this will "neaten" up functions like "isa()" from isa(@unit, 'type') to @unit->isa('type')
                //if (excelScriptFunction.Args[i].Type != ArgType.Int32 &&
                //    excelScriptFunction.Args[i].Type != ArgType.ExcelIndex &&
                //    excelScriptFunction.Args[i].Type != ArgType.Param && 
                //    excelScriptFunction.Args[i].Type != ArgType.Context)
                //{
                //    if (argStackObject.Type != excelScriptFunction.Args[i].Type)
                //    {
                //        int bp = 0;
                //    }

                //    if (i != 0)
                //    {
                //        int bp = 0;
                //    }
                //}

                int tableIndex = excelScriptFunction.Args[i].TableIndex;
                if (tableIndex >= 0)
                {
                    int rowIndex = int.Parse(argStr);
                    String excelString = _fileManager.GetExcelRowStringFromTableIndex(tableIndex, rowIndex);

                    if (excelString == null)
                    {
                        Console.WriteLine(String.Format("Warning: Unknown row index '{0}' on table index '{1}'.", rowIndex, tableIndex));
                        argsString += rowIndex.ToString();
                    }
                    else
                    {
                        argsString = String.Format("'{0}'", excelString) + argsString;
                    }
                }
                else
                {
                    argsString = argStr + argsString;
                }

                if (i != 0) argsString = ", " + argsString;
            }

            String functionCallString = String.Format("{0}({1})", excelScriptFunction.Name, argsString);
            _stack.Push(new StackObject { Value = functionCallString, IsFunction = true });
        }

        private void _PushContextVariable(String functionName, uint varIndex)
        {
            String funcString = String.Format("{0}('{1}')", functionName, ((ContextVariables)varIndex).ToString().ToLower());

            _stack.Push(new StackObject { Value = funcString, IsFunction = true });
        }

        private void _DoOperator(String op, int precedence, ScriptOpCodes opCode, int ifLevel)
        {
            _CheckStack(2, opCode);

            StackObject value2Object = _stack.Pop();
            StackObject value1Object = _stack.Pop();

            const String operatorFormat = "{0}{1}{2}";
            const String operatorFormatLParentheses = "({0}){1}{2}";
            const String operatorFormatRParentheses = "{0}{1}({2})";
            const String operatorFormat2Parentheses = "({0}){1}({2})";

            String opFormat = operatorFormat;
            bool debugTest = false;
            if (value1Object.Precedence > precedence)
            {
                opFormat = operatorFormatLParentheses;
                debugTest = true;
            }
            else if (value2Object.Precedence > precedence)
            {
                opFormat = operatorFormatRParentheses;
                debugTest = true;
            }

            // some of these extra conditions are purely to ensure complete fidelity when recompiling
            // in some cases they are completely unnecessary from an arithmetic perspective
            if (value1Object.Precedence == value2Object.Precedence && value1Object.IsPrecedenceFunc && value2Object.IsPrecedenceFunc)
            {
                opFormat = operatorFormat2Parentheses;
            }
            else if (value1Object.OperatorCount > 0 && value2Object.OperatorCount > 0) // e.g.   (a * b) * (c * d)
            {
                opFormat = operatorFormat2Parentheses;
            }
            else if (value2Object.OperatorCount > 0 && value2Object.Precedence >= precedence && !debugTest)
            {
                opFormat = operatorFormatRParentheses;
            }


            int operatorCount = (value2Object.OperatorCount == -1) ? 1 : value2Object.OperatorCount + 1;
            StackObject newStackObject = new StackObject
            {
                Value = String.Format(opFormat, value1Object.Value, op, value2Object.Value),
                Precedence = precedence,
                IsPrecedenceFunc = true,
                OperatorCount = operatorCount,
                ByteOffset = (uint)_offset,
                IfLevel = ifLevel
            };

            _stack.Push(newStackObject);
        }

        private void _StatsFunction(String name, uint value, ScriptOpCodes opCode, bool isSet)
        {
            uint rowIndex = value >> 22;
            uint param = value & 0x3FFFFF;

            // check if we have an object ptr reference
            if ((!isSet && _stack.Count > 0) || (isSet && _stack.Count > 1))
            {
                StackObject stackObjectPeek = _stack.Peek();
                if (stackObjectPeek.IsVarAssign && stackObjectPeek.Type == ArgType.ContextPtr)
                {
                    name = stackObjectPeek.Value + name;
                    _stack.Pop();
                }
            }

            // get index string details
            String indexStr;
            String paramStr;
            if (_statsTable == null)
            {
                indexStr = rowIndex.ToString();
                paramStr = param.ToString();
            }
            else
            {
                Debug.Assert(rowIndex >= 0 && rowIndex < _statsTable.Rows.Count);
                Object statsRow = _statsTable.Rows[(int)rowIndex];

                indexStr = (String)_statsDelegator["stat"](statsRow);

                int param1Table = (int)_statsDelegator["param1Table"](statsRow);
                if (param1Table != -1)
                {
                    paramStr = _fileManager.GetExcelRowStringFromTableIndex(param1Table, (int)param);
                }
                else
                {
                    paramStr = null;
                }

                // A couple of TCv4 stats have this - don't think it matters though...
                Debug.Assert((int)_statsDelegator["param2Table"](statsRow) == -1);
            }

            // generate the arg string
            String argStr = String.Empty;
            if (isSet)
            {
                _CheckStack(1, opCode);
                StackObject varStackObject = _stack.Pop();

                argStr = ", " + varStackObject.Value;
            }

            // format paramStr if needed
            if (!String.IsNullOrEmpty(paramStr))
            {
                paramStr = String.Format(", '{0}'", paramStr);
            }
            else if (param != 0) // we have an excel index, but no excel row string
            {
                paramStr = ", " + param;
            }

            // create new stack object
            StackObject newStackObject = new StackObject
            {
                Value = String.Format("{0}('{1}'{2}{3})", name, indexStr, paramStr, argStr),
                IsFunction = true
            };

            _stack.Push(newStackObject);
        }

        private void _CheckStack(int requiredCount, ScriptOpCodes opCode, Function functionDetails = null)
        {
            if (requiredCount <= _stack.Count) return;

            String extra = String.Empty;
            if (functionDetails != null)
            {
                extra = String.Format(" Function Name: '{0}'", functionDetails.Name);
            }

            String error = String.Format("The OpCode {0} requires {1} values on the stack.{2}\n{3}", opCode, requiredCount, extra, _DumpStack(_stack));
            throw new Exceptions.ScriptInvalidStackStateException(error);
        }

        private static String _DumpStack(Stack<StackObject> stack)
        {
            String stackDump = "Stack Dump : LIFO\nIndex\tPrecedence\tIsFunction\tIsVarAssign\tIsIf\tValue\n";

            int stackCount = stack.Count;
            for (int i = 0; i < stackCount; i++)
            {
                StackObject stackObject = stack.Pop();
                stackDump += String.Format("{0}\t\t{1}\t\t\t{2}\t\t{3}\t\t{4}\t{5}\n", i, stackObject.Precedence, stackObject.IsFunction, stackObject.IsVarAssign, stackObject.IsIf, stackObject.Value);
            }

            return stackDump;
        }

        #endregion

        #region Excel Script Functions Stuff

        /// <summary>
        /// Initialiser function to generate the excel script functions.
        /// </summary>
        private void _GenerateExcelScriptFunctions()
        {
            ExcelFile propertiesTable = _fileManager.GetDataFile("PROPERTIES") as ExcelFile;
            ExcelFile skillsTable = _fileManager.GetDataFile("SKILLS") as ExcelFile;

            if (propertiesTable == null || skillsTable == null) throw new Exceptions.ScriptNotInitialisedException("_GenerateExcelScriptFunctions() was unable to obtain the Properties and Skills excel tables!");

            _ParseExcelFunctions(propertiesTable);
            _ParseExcelFunctions(skillsTable);
            _haveExcelFunctions = true;
        }

        /// <summary>
        /// Main worker function that parses and appends excel script functions to the global call function array.
        /// </summary>
        /// <param name="excelFile">The  excel file containing the excel script functions to parse.</param>
        /// <returns>The number of functions added.</returns>
        private void _ParseExcelFunctions(ExcelFile excelFile)
        {
            const bool debugOutputLikeFunction = true;

            foreach (ExcelFile.ExcelFunction excelFunction in excelFile.ExcelFunctions)
            {
                //if (excelFunction.Parameters[0].Name == "holy_radius_enemies")
                //{
                //    int bp = 0;
                //}

                // not sure what to do with them - they don't appear to be used
                if (excelFunction.Parameters[0].TypeValues.Length <= 4) continue;

                Function function = new Function();
                List<Argument> arguments = new List<Argument>();

                for (int i = 0; i < excelFunction.Parameters.Count; i++)
                {
                    ExcelFile.ExcelFunction.Parameter parameter = excelFunction.Parameters[i];

                    if (i == 0)
                    {
                        function.Name = parameter.Name;
                        int functionIndex = parameter.TypeValues[4];
                        Debug.Assert(functionIndex == _callFunctions.Count);

                        continue;
                    }

                    Argument arg = new Argument
                    {
                        Name = parameter.Name,
                        Type = 0, // I think they're all int
                        ByteOffset = parameter.TypeValues[3],
                    };

                    arguments.Add(arg);
                }

                String byteCodeString = null;
                if (_debug && debugOutputLikeFunction)
                {
                    int offset = 0;
                    byteCodeString = FileTools.ByteArrayToInt32Array(excelFunction.ScriptByteCode, ref offset, excelFunction.ScriptByteCode.Length / 4).ToString(",");
                }

                function.Args = arguments.ToArray();

                function.ExcelScript = _Decompile(excelFunction, function, byteCodeString, "PROPERTIES", 0, 0, function.Name);

                if (_debug && debugOutputLikeFunction)
                {
                    String[] code = function.ExcelScript.Split(new[] { "; ", ";", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    String codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));

                    String args = String.Empty;
                    for (int i = 0; i < function.ArgCount; i++)
                    {
                        args += "int " + function.Args[i].Name;

                        if (i != function.ArgCount - 1) args += ", ";
                    }

                    String funcString = String.Format("void {0}({1})\n{{\n{2}}}\n", function.Name, args, codeStr);
                    Debug.WriteLine(funcString);
                }

                _callFunctions.Add(function);
            }
        }

        #endregion

        #region Get Functions From ASM Stuff

        public static void ExtractFunctionListCStyle(IEnumerable<String> functionCode)
        {
            Regex argsRegex = new Regex(@"([^ \f\n\r\t\v]*)\((.*)\)");
            Regex stringRegex = new Regex("\"(.*)\"");
            List<Function> functions = new List<Function>();
            List<Argument> arguments = new List<Argument>();
            Function function = null;

            foreach (String call in functionCode)
            {
                Match match = argsRegex.Match(call);
                String callFunc = match.Groups[1].Value;
                String[] funcArgs = match.Groups[2].Value.Split(new[] { ',' });
                Debug.Assert(!String.IsNullOrEmpty(callFunc));

                switch (callFunc)
                {
                    case "Script_AddFunction":
                        {
                            String funcName = funcArgs[0];
                            Debug.Assert(!String.IsNullOrEmpty(funcName));
                            funcName = stringRegex.Match(funcName).Groups[1].Value;
                            Debug.Assert(!String.IsNullOrEmpty(funcName));

                            if (function != null)
                            {
                                function.Args = arguments.ToArray();
                                functions.Add(function);
                            }

                            function = new Function { Name = funcName };
                            arguments = new List<Argument>();
                        }
                        break;

                    case "Script_AddParam_Int":
                        {
                            Debug.Assert(function != null);

                            String argName = funcArgs[0];
                            Debug.Assert(!String.IsNullOrEmpty(argName));
                            argName = stringRegex.Match(argName).Groups[1].Value;
                            Debug.Assert(!String.IsNullOrEmpty(argName));

                            arguments.Add(new Argument { Name = argName, Type = ArgType.Int32 });
                        }
                        break;

                    case "Script_AddParam_Param":
                        {
                            Debug.Assert(function != null);

                            String argName = funcArgs[0];
                            Debug.Assert(!String.IsNullOrEmpty(argName));
                            argName = stringRegex.Match(argName).Groups[1].Value;
                            Debug.Assert(!String.IsNullOrEmpty(argName));

                            arguments.Add(new Argument { Name = argName, Type = ArgType.Param });
                        }
                        break;

                    case "Script_AddParam_Unit":
                        {
                            Debug.Assert(function != null);
                            Debug.Assert(funcArgs.Length >= 2);

                            String argName = funcArgs[0];
                            Debug.Assert(!String.IsNullOrEmpty(argName));
                            argName = stringRegex.Match(argName).Groups[1].Value;
                            Debug.Assert(!String.IsNullOrEmpty(argName));

                            String argType = funcArgs[1];
                            Debug.Assert(!String.IsNullOrEmpty(argType));
                            int argTypeInt = int.Parse(argType);

                            arguments.Add(new Argument { Name = argName, Type = (ArgType)argTypeInt });
                        }
                        break;

                    case "Script_AddParam_ExcelIndex":
                        {
                            Debug.Assert(function != null);
                            Debug.Assert(funcArgs.Length >= 2);

                            String excelIndex = funcArgs[0];
                            Debug.Assert(!String.IsNullOrEmpty(excelIndex));
                            int excelIndexInt = int.Parse(excelIndex);

                            String argName = funcArgs[1];
                            Debug.Assert(!String.IsNullOrEmpty(argName));
                            argName = stringRegex.Match(argName).Groups[1].Value;
                            Debug.Assert(!String.IsNullOrEmpty(argName));

                            arguments.Add(new Argument { Name = argName, Type = ArgType.ExcelIndex, TableIndex = excelIndexInt });
                        }
                        break;

                    case "Script_AddParam_ExcelIndex_0": // only used on "randaffixgroup();"
                        Debug.Assert(function != null);
                        arguments.Add(new Argument { Name = "affixGroup", Type = ArgType.ExcelIndex, TableIndex = 0x35 });
                        break;

                    case "Script_AddParam_Context":
                        Debug.Assert(function != null);
                        arguments.Add(new Argument { Name = "context", Type = ArgType.Context });
                        break;

                    case "Script_AddParam_Game3":
                        Debug.Assert(function != null);
                        arguments.Add(new Argument { Name = "game3", Type = ArgType.Game3 });
                        break;

                    case "Script_AddParam_Game4":
                        Debug.Assert(function != null);
                        arguments.Add(new Argument { Name = "game4", Type = ArgType.Game4 });
                        break;

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            Debug.Assert(function != null);
            function.Args = arguments.ToArray();
            functions.Add(function);

            int index = 0;
            foreach (Function func in functions)
            {
                String argsString = String.Empty;
                String[] formattedArgStrings = new String[func.ArgCount];
                int i = 0;
                foreach (Argument arg in func.Args)
                {
                    if (arg == null) break;

                    const String argFormatStringBasic = "new Argument {{ Name = \"{0}\", Type = ArgType.{1} }}";
                    const String argFormatStringIndex = "new Argument {{ Name = \"{0}\", Type = ArgType.{1}, TableIndex = {2} }}";

                    String argFormatString = arg.TableIndex >= 0 ? argFormatStringIndex : argFormatStringBasic;
                    formattedArgStrings[i++] = String.Format(argFormatString, arg.Name, arg.Type, arg.TableIndex);
                }
                String formattedArgString = String.Join(", ", formattedArgStrings, 0, i);
                if (formattedArgString.Length > 0)
                {
                    formattedArgString = ", Args = new[] { " + formattedArgString + " }";
                }

                const String dictionaryFormat = "/*{0,3}*/ new Function {{ Name = \"{1}\"{2} }},";
                String cSharpDicCode = String.Format(dictionaryFormat, index++, func.Name, formattedArgString);
                Debug.WriteLine(cSharpDicCode);
            }
        }

        /// <summary>
        /// Normalises a register, removing the double word char for rx registers, and converting the 32 bit registers to their 64 bit code (e.g. eax -> rax).
        /// </summary>
        /// <param name="reg">The register string to normalise.</param>
        /// <returns>The normalised register.</returns>
        private static String _NormReg(String reg)
        {
            reg = reg.Replace(" ", "");
            if (reg[0] == 'e') reg = reg.Replace('e', 'r');
            if (reg.Length == 3 && reg[2] == 'd') reg = reg.Substring(0, 2);
            if (reg.Length == 4) reg = reg.Substring(0, 3);

            return reg;
        }

        public static void ExtractFunctionList(IEnumerable<String> functionCode)
        {
            Dictionary<String, String> registers = new Dictionary<String, String>
            {
                { "rax", "0" },
                { "rbp", "0" },
                { "rbx", "0" },
                { "rcx", "0" },
                { "rdi", "0" },
                { "rdx", "0" },
                { "rsi", "0" },
                { "r8", "0" },
                { "r9", "0" },
                { "r12", "0" },
                { "r13", "0" },
                { "r14", "0" },
                { "r15", "0" },
            };
            int startRegCount = registers.Count;

            // Function                       		Type
            // Script_AddParam_Int                  0
            // Script_AddParam_ExcelIndex           1
            // Script_AddParam_AffixGroup			1	// ??
            // Script_AddParam_Context              2
            // Script_AddParam_Game3                3	// not sure what's the difference with these two - is one a pointer and one by value? (random guess)
            // Script_AddParam_Game4                4
            // Script_AddParam_Unit                 r8	// always 5 or 6
            // Script_AddParam_Param				7	// not sure what type

            List<Function> functions = new List<Function>();
            Function function = null;
            int argIndex = 0;
            int line = 0;
            bool haveFuncName = false;
            bool haveReturnType = false;
            bool haveVoidArg = false;
            foreach (String asmString in functionCode)
            {
                line++;

                String[] asmStr = asmString.Split(new[] { '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (asmStr.Length == 0) continue;

                String opCode = asmStr[0];

                //if (line == 2605)
                //{
                //    int bp = 0;
                //}

                switch (opCode)
                {
                    case "mov":
                        String val;
                        if (registers.TryGetValue(_NormReg(asmStr[2]), out val))
                        {
                            registers[_NormReg(asmStr[1])] = val;
                        }
                        else
                        {
                            registers[_NormReg(asmStr[1])] = asmStr[2];
                        }
                        break;

                    case "lea":
                        if (asmString.Contains("Load Effective Address"))
                        {
                            if (asmString.IndexOf('[') == -1) continue; // ptr to function

                            String leaStr = asmStr[2];
                            int start = leaStr.IndexOf('[');
                            int end = leaStr.IndexOf(']');
                            String calcStr = leaStr.Substring(start + 1, end - start - 1);

                            bool isSum = false;
                            bool isDiff = false;
                            if (calcStr.IndexOf('+') > 0) isSum = true;
                            else if (calcStr.IndexOf('-') > 0) isDiff = true;

                            String[] args = calcStr.Split(new[] { '+', '-' });

                            String reg = _NormReg(args[0]);
                            String num = args[1].Replace("h", "");
                            int numInt = int.Parse(num, NumberStyles.HexNumber);

                            String origVal;
                            if (registers.TryGetValue(_NormReg(reg), out origVal))
                            {
                                int origValInt = int.Parse(origVal.Replace("h", ""), NumberStyles.HexNumber);

                                if (isSum) registers[_NormReg(asmStr[1])] = (origValInt + numInt).ToString("X") + "h";
                                else if (isDiff) registers[_NormReg(asmStr[1])] = (origValInt - numInt).ToString("X") + "h";
                                else Debug.Assert(false, "bad calc");
                            }
                            else
                            {
                                registers[_NormReg(asmStr[1])] = asmStr[2];
                            }
                        }
                        else
                        {
                            registers[_NormReg(asmStr[1])] = _GetStringFromAsm(asmStr[asmStr.Length - 1]);
                        }
                        break;

                    case "call":
                        if (asmStr[1].Contains("Script_AddFunction"))
                        {
                            if (function != null) functions.Add(function);

                            function = new Function { Args = new Argument[10] };
                            argIndex = 0;
                            haveFuncName = true;

                            function.Name = registers["rcx"];
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Int"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = 0 };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_ExcelIndex") || asmStr[1].Contains("Script_AddParam_AffixGroup"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            String strIndex = registers["rdx"];
                            Debug.Assert(!String.IsNullOrEmpty(strIndex));
                            String tableIndex = strIndex.Replace("h", "");
                            function.Args[argIndex++] = new Argument { Name = registers["r8"], Type = ArgType.ExcelIndex, TableIndex = Int32.Parse(tableIndex, NumberStyles.HexNumber) };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Context"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = "context", Type = ArgType.Context };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Game3"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = "game3", Type = ArgType.Game3 };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Game4"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = "game4", Type = ArgType.Game4 };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Unit"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            String strType = registers["r8"];
                            Debug.Assert(!String.IsNullOrEmpty(strType));
                            String typeValue = strType.Replace("h", "");
                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = (ArgType)Int32.Parse(typeValue, NumberStyles.HexNumber) };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Param"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = ArgType.Param };
                        }
                        else
                        {
                            int bp2 = 0;
                        }

                        break;

                    default:
                        int b3p = 0;
                        break;
                }
            }
            if (function != null) functions.Add(function);

            Debug.Assert(startRegCount == registers.Count);


            // new Function { Name = "FunctionName", ReturnType = "Type", Args = new[] { new Argument { Name = "", Type = "" } } },

            int index = 0;
            foreach (Function func in functions)
            {
                String argsString = String.Empty;
                String[] formattedArgStrings = new String[func.ArgCount];
                int i = 0;
                foreach (Argument arg in func.Args)
                {
                    if (arg == null) break;

                    const String argFormatStringBasic = "new Argument {{ Name = \"{0}\", Type = ArgType.{1} }}";
                    const String argFormatStringIndex = "new Argument {{ Name = \"{0}\", Type = ArgType.{1}, TableIndex = {2} }}";

                    String argFormatString = arg.TableIndex >= 0 ? argFormatStringIndex : argFormatStringBasic;
                    formattedArgStrings[i++] = String.Format(argFormatString, arg.Name, arg.Type, arg.TableIndex);
                }
                String formattedArgString = String.Join(", ", formattedArgStrings, 0, i);
                if (formattedArgString.Length > 0)
                {
                    formattedArgString = ", Args = new[] { " + formattedArgString + " }";
                }

                const String dictionaryFormat = "/*{0,3}*/ new Function {{ Name = \"{1}\"{2} }},";
                String cSharpDicCode = String.Format(dictionaryFormat, index++, func.Name, formattedArgString);
                Debug.WriteLine(cSharpDicCode);
            }
        }

        private static String _GetStringFromAsm(String asmCode)
        {
            // probably could be done with Regex, meh

            int strStart = asmCode.IndexOf('"');
            int strEnd = asmCode.IndexOf('"', strStart + 1);
            int strLen = strEnd - strStart - 1;

            return strLen <= 0 ? null : asmCode.Substring(strStart + 1, strLen);
        }

        #endregion
    }
}
