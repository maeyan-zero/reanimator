using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Revival.Common;

namespace Hellgate
{
    public class ExcelScript
    {
        #region Type Definitions
        private class Argument
        {
            public String Name;             // name of argument
            public int Type;                // not used, just kept "just because"
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
            /*  0*/ new Function { Name = "setUnitTypeAreaFloorVis", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevelArea", Type = 1, TableIndex = 138 }, new Argument { Name = "nFloor", Type = 0 }, new Argument { Name = "nVis", Type = 0 } } },
            /*  1*/ new Function { Name = "setUnitTypeAreaFloorInteractive", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevelArea", Type = 1, TableIndex = 138 }, new Argument { Name = "nFloor", Type = 0 } } },
            /*  2*/ new Function { Name = "setUnitTypeAreaFloorDisabled", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevelArea", Type = 1, TableIndex = 138 }, new Argument { Name = "nFloor", Type = 0 } } },
            /*  3*/ new Function { Name = "showDialog", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nDialog", Type = 1, TableIndex = 53 } } },
            /*  4*/ new Function { Name = "setQuestBit", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nBit", Type = 0 } } },
            /*  5*/ new Function { Name = "getQuestBit", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nBit", Type = 0 } } },
            /*  6*/ new Function { Name = "isQuestTaskComplete", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nQuestTask", Type = 1, TableIndex = 165 } } },
            /*  7*/ new Function { Name = "isQuestTaskActive", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nQuestTask", Type = 1, TableIndex = 165 } } },
            /*  8*/ new Function { Name = "isTalkingTo", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nNPCID", Type = 1, TableIndex = 64 } } },
            /*  9*/ new Function { Name = "setTargetVisibility", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nVis", Type = 0 } } },
            /* 10*/ new Function { Name = "setTargetVisibilityOnFloor", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nVis", Type = 0 }, new Argument { Name = "nFloor", Type = 0 } } },
            /* 11*/ new Function { Name = "setStateOnTarget", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 12*/ new Function { Name = "setTargetInteractive", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nInteractive", Type = 0 } } },
            /* 13*/ new Function { Name = "setTargetToTeam", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nTeam", Type = 0 } } },
            /* 14*/ new Function { Name = "getIsTargetOfType", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nUnitType", Type = 1, TableIndex = 23 } } },
            /* 15*/ new Function { Name = "setMonsterInLevelToTarget", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nMonsterID", Type = 1, TableIndex = 115 } } },
            /* 16*/ new Function { Name = "setObjectInLevelToTarget", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nObjectID", Type = 1, TableIndex = 119 } } },
            /* 17*/ new Function { Name = "getIsTargetMonster", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nMonsterID", Type = 1, TableIndex = 115 } } },
            /* 18*/ new Function { Name = "getIsTargetObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nObjectID", Type = 1, TableIndex = 119 } } },
            /* 19*/ new Function { Name = "resetTargetObject", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 20*/ new Function { Name = "messageStatVal", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nStatId", Type = 1, TableIndex = 27 }, new Argument { Name = "nIndex", Type = 0 } } },
            /* 21*/ new Function { Name = "getStatVal", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nStatId", Type = 1, TableIndex = 27 }, new Argument { Name = "nIndex", Type = 0 } } },
            /* 22*/ new Function { Name = "createMap", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 23*/ new Function { Name = "randomizeMapSpawner", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 24*/ new Function { Name = "randomizeMapSpawnerEpic", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 25*/ new Function { Name = "setMapSpawnerByLevelAreaID", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevelAreaID", Type = 0 } } },
            /* 26*/ new Function { Name = "setMapSpawnerByLevelArea", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevelAreaID", Type = 1, TableIndex = 138 } } },
            /* 27*/ new Function { Name = "abs", Args = new[] { new Argument { Name = "a", Type = 0 } } },
            /* 28*/ new Function { Name = "min", Args = new[] { new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 29*/ new Function { Name = "max", Args = new[] { new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 30*/ new Function { Name = "pin", Args = new[] { new Argument { Name = "value", Type = 0 }, new Argument { Name = "min", Type = 0 }, new Argument { Name = "max", Type = 0 } } },
            /* 31*/ new Function { Name = "pct", Args = new[] { new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 32*/ new Function { Name = "pctFloat", Args = new[] { new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 }, new Argument { Name = "c", Type = 0 } } },
            /* 33*/ new Function { Name = "rand", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 34*/ new Function { Name = "randByUnitId", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 35*/ new Function { Name = "chance", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "nChance", Type = 0 }, new Argument { Name = "nChanceOutOf", Type = 0 } } },
            /* 36*/ new Function { Name = "chanceByStateMod", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nChance", Type = 0 }, new Argument { Name = "nChanceOutOf", Type = 0 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "nModBy", Type = 0 } } },
            /* 37*/ new Function { Name = "randSkillSeed", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 38*/ new Function { Name = "roll", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 } } },
            /* 39*/ new Function { Name = "divMult", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "a", Type = 0 }, new Argument { Name = "b", Type = 0 }, new Argument { Name = "c", Type = 0 } } },
            /* 40*/ new Function { Name = "distribute", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "numdie", Type = 0 }, new Argument { Name = "diesize", Type = 0 }, new Argument { Name = "start", Type = 0 } } },
            /* 41*/ new Function { Name = "roundstat", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 }, new Argument { Name = "value", Type = 0 } } },
            /* 42*/ new Function { Name = "getEventType", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nAffixID", Type = 1, TableIndex = 25 } } },
            /* 43*/ new Function { Name = "getAffixIDByName", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nAffixID", Type = 1, TableIndex = 52 } } },
            /* 44*/ new Function { Name = "get_skill_level", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 } } },
            /* 45*/ new Function { Name = "get_skill_level_object", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 } } },
            /* 46*/ new Function { Name = "pickskill", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stats", Type = 6 }, new Argument { Name = "nSkillLevel", Type = 0 } } },
            /* 47*/ new Function { Name = "pickskillbyunittype", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stats", Type = 6 }, new Argument { Name = "nUnitType", Type = 1, TableIndex = 23 }, new Argument { Name = "nSkillLevel", Type = 0 } } },
            /* 48*/ new Function { Name = "removeoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nUnitType", Type = 1, TableIndex = 23 } } },
            /* 49*/ new Function { Name = "killoldestpetoftype", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nUnitType", Type = 1, TableIndex = 23 } } },
            /* 50*/ new Function { Name = "pickskillbyskillgroup", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stats", Type = 6 }, new Argument { Name = "nSkillGroup", Type = 1, TableIndex = 39 }, new Argument { Name = "nSkillLevel", Type = 0 } } },
            /* 51*/ new Function { Name = "learnskill", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "skill", Type = 1, TableIndex = 41 } } },
            /* 52*/ new Function { Name = "getStatOwnerDivBySkillVar", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 }, new Argument { Name = "nVar", Type = 0 } } },
            /* 53*/ new Function { Name = "getStatOwnerDivBy", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 }, new Argument { Name = "nDivBy", Type = 0 } } },
            /* 54*/ new Function { Name = "switchUnitAndObject", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 55*/ new Function { Name = "getAchievementCompleteCount", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nAchievementID", Type = 1, TableIndex = 180 } } },
            /* 56*/ new Function { Name = "getVarRange", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 57*/ new Function { Name = "getVar", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nVariable", Type = 0 } } },
            /* 58*/ new Function { Name = "getAttackerSkillVarBySkill", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 }, new Argument { Name = "nVariable", Type = 0 } } },
            /* 59*/ new Function { Name = "getVarFromSkill", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 }, new Argument { Name = "nVariable", Type = 0 } } },
            /* 60*/ new Function { Name = "getVarFromSkillFromObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 }, new Argument { Name = "nVariable", Type = 0 } } },
            /* 61*/ new Function { Name = "hasStateObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 62*/ new Function { Name = "hasState", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 63*/ new Function { Name = "clearStateObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 64*/ new Function { Name = "clearState", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 65*/ new Function { Name = "clearStateClient", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 66*/ new Function { Name = "isDualWielding", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 67*/ new Function { Name = "getWieldingIsACount", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "unittype", Type = 1, TableIndex = 23 } } },
            /* 68*/ new Function { Name = "setState", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 69*/ new Function { Name = "setStateObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 } } },
            /* 70*/ new Function { Name = "setStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 71*/ new Function { Name = "addStateWithTimeMS", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 72*/ new Function { Name = "addStateWithTimeMSClient", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 73*/ new Function { Name = "setStateWithTimeMSOnObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 74*/ new Function { Name = "setStateWithTimeMSScriptOnObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 75*/ new Function { Name = "BroadcastEquipEvent", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 76*/ new Function { Name = "setAITargetToSkillTarget", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 77*/ new Function { Name = "setObjectAITargetToUnitAITarget", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 78*/ new Function { Name = "makeAIAwareOfObject", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 79*/ new Function { Name = "setAITargetToObject", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 80*/ new Function { Name = "hasSkillTarget", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /* 81*/ new Function { Name = "setStateOnSkillTargetWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 }, new Argument { Name = "clearFirst", Type = 0 } } },
            /* 82*/ new Function { Name = "runScriptParamOnStateClear", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "paramIndex", Type = 0 } } },
            /* 83*/ new Function { Name = "getCountOfUnitsInArea", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "area", Type = 0 } } },
            /* 84*/ new Function { Name = "runScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "scriptIndex", Type = 0 }, new Argument { Name = "area", Type = 0 }, new Argument { Name = "chance", Type = 0 }, new Argument { Name = "flag", Type = 0 } } },
            /* 85*/ new Function { Name = "doSkillAndScriptOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 }, new Argument { Name = "scriptIndex", Type = 0 }, new Argument { Name = "area", Type = 0 }, new Argument { Name = "chance", Type = 0 }, new Argument { Name = "flag", Type = 0 } } },
            /* 86*/ new Function { Name = "doSkillOnUnitsInAreaPCT", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkill", Type = 1, TableIndex = 41 }, new Argument { Name = "area", Type = 0 }, new Argument { Name = "chance", Type = 0 }, new Argument { Name = "flag", Type = 0 } } },
            /* 87*/ new Function { Name = "setStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 88*/ new Function { Name = "setStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 }, new Argument { Name = "paramIndex", Type = 0 } } },
            /* 89*/ new Function { Name = "setStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 }, new Argument { Name = "paramIndex", Type = 0 } } },
            /* 90*/ new Function { Name = "addStateWithTimeMSScriptParamObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 }, new Argument { Name = "paramIndex", Type = 0 } } },
            /* 91*/ new Function { Name = "addStateWithTimeMSScript", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 } } },
            /* 92*/ new Function { Name = "addStateWithTimeMSScriptParam", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nState", Type = 1, TableIndex = 73 }, new Argument { Name = "timerMS", Type = 0 }, new Argument { Name = "paramIndex", Type = 0 } } },
            /* 93*/ new Function { Name = "setDmgEffect", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nDmgEffect", Type = 1, TableIndex = 31 }, new Argument { Name = "nChance", Type = 0 }, new Argument { Name = "nTime", Type = 0 }, new Argument { Name = "nRoll", Type = 0 } } },
            /* 94*/ new Function { Name = "getStatOwner", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /* 95*/ new Function { Name = "getStatParent", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /* 96*/ new Function { Name = "addPCTStatOnOwner", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nStat", Type = 1, TableIndex = 27 }, new Argument { Name = "nValue", Type = 0 }, new Argument { Name = "nParam", Type = 0 } } },
            /* 97*/ new Function { Name = "setStatOnOwner", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nStat", Type = 1, TableIndex = 27 }, new Argument { Name = "nValue", Type = 0 }, new Argument { Name = "nParam", Type = 0 } } },
            /* 98*/ new Function { Name = "total", Args = new[] { new Argument { Name = "game3", Type = 3 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /* 99*/ new Function { Name = "basetotal", Args = new[] { new Argument { Name = "game3", Type = 3 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /*100*/ new Function { Name = "basestat", Args = new[] { new Argument { Name = "game3", Type = 3 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /*101*/ new Function { Name = "getcur", Args = new[] { new Argument { Name = "game3", Type = 3 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 }, new Argument { Name = "param", Type = 7 } } },
            /*102*/ new Function { Name = "statidx", Args = new[] { new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /*103*/ new Function { Name = "invcount", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "location", Type = 1, TableIndex = 24 } } },
            /*104*/ new Function { Name = "dmgrider", Args = new[] { new Argument { Name = "game3", Type = 3 } } },
            /*105*/ new Function { Name = "procrider", Args = new[] { new Argument { Name = "game3", Type = 3 } } },
            /*106*/ new Function { Name = "knockback", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "a", Type = 0 } } },
            /*107*/ new Function { Name = "colorcoderequirement", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 }, new Argument { Name = "param", Type = 7 } } },
            /*108*/ new Function { Name = "color_code_modunit_requirement", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 }, new Argument { Name = "param", Type = 7 } } },
            /*109*/ new Function { Name = "color_code_modunit_requirement2", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "stat1", Type = 1, TableIndex = 27 }, new Argument { Name = "param1", Type = 7 }, new Argument { Name = "stat2", Type = 1, TableIndex = 27 }, new Argument { Name = "param2", Type = 7 } } },
            /*110*/ new Function { Name = "feedchange", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /*111*/ new Function { Name = "feedcolorcode", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "stat", Type = 1, TableIndex = 27 } } },
            /*112*/ new Function { Name = "colorposneg", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*113*/ new Function { Name = "colorcodeprice", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*114*/ new Function { Name = "colorcodeclassreq", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*115*/ new Function { Name = "colorcodeskillslots", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*116*/ new Function { Name = "colorcodeskillusable", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*117*/ new Function { Name = "colorcodeskillgroupusable", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*118*/ new Function { Name = "meetsclassreqs", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*119*/ new Function { Name = "fontcolorrow", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nColorIndex", Type = 1, TableIndex = 7 } } },
            /*120*/ new Function { Name = "nodrop", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*121*/ new Function { Name = "notrade", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*122*/ new Function { Name = "BuyPriceByValue", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "valueType", Type = 0 } } },
            /*123*/ new Function { Name = "SellPriceByValue", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "valueType", Type = 0 } } },
            /*124*/ new Function { Name = "buyprice", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*125*/ new Function { Name = "buypriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*126*/ new Function { Name = "sellprice", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*127*/ new Function { Name = "sellpriceRealWorld", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*128*/ new Function { Name = "hitChance", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*129*/ new Function { Name = "dodgeChance", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*130*/ new Function { Name = "numaffixes", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*131*/ new Function { Name = "qualitypricemult", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*132*/ new Function { Name = "enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*133*/ new Function { Name = "visible_enemies_in_radius", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*134*/ new Function { Name = "champions_in_radius", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*135*/ new Function { Name = "distance_sq_to_champion", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*136*/ new Function { Name = "champion_hp_pct", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*137*/ new Function { Name = "bosses_in_radius", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*138*/ new Function { Name = "distance_sq_to_boss", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*139*/ new Function { Name = "boss_hp_pct", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*140*/ new Function { Name = "enemy_corpses_in_radius", Args = new[] { new Argument { Name = "game4", Type = 4 }, new Argument { Name = "unit", Type = 5 }, new Argument { Name = "radius", Type = 0 } } },
            /*141*/ new Function { Name = "monsters_killed", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "time", Type = 0 } } },
            /*142*/ new Function { Name = "monsters_killed_nonteam", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "time", Type = 0 } } },
            /*143*/ new Function { Name = "monsters_pct_left", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*144*/ new Function { Name = "hp_lost", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "time", Type = 0 } } },
            /*145*/ new Function { Name = "meters_moved", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "time", Type = 0 } } },
            /*146*/ new Function { Name = "attacks", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "time", Type = 0 } } },
            /*147*/ new Function { Name = "is_alive", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*148*/ new Function { Name = "monster_level", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*149*/ new Function { Name = "has_active_task", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*150*/ new Function { Name = "is_usable", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*151*/ new Function { Name = "is_examinable", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*152*/ new Function { Name = "is_operatable", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*153*/ new Function { Name = "isa", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "unittype", Type = 1, TableIndex = 23 } } },
            /*154*/ new Function { Name = "is_subscriber", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*155*/ new Function { Name = "is_hardcore", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*156*/ new Function { Name = "is_elite", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*157*/ new Function { Name = "get_difficulty", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*158*/ new Function { Name = "get_act", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*159*/ new Function { Name = "email_send_item_okay", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "context", Type = 2 } } },
            /*160*/ new Function { Name = "email_receive_item_okay", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "context", Type = 2 } } },
            /*161*/ new Function { Name = "colorcodesubscriber", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*162*/ new Function { Name = "item_requires_subscriber", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*163*/ new Function { Name = "colorcodenightmare", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*164*/ new Function { Name = "item_is_nightmare_specific", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*165*/ new Function { Name = "quality", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*166*/ new Function { Name = "meetsitemreqs", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*167*/ new Function { Name = "weapondps", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*168*/ new Function { Name = "SkillTargetIsA", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "unittype", Type = 1, TableIndex = 23 } } },
            /*169*/ new Function { Name = "GetObjectIsA", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "unittype", Type = 1, TableIndex = 23 } } },
            /*170*/ new Function { Name = "GetMissileSourceIsA", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "unittype", Type = 1, TableIndex = 23 } } },
            /*171*/ new Function { Name = "GetSkillHasReqWeapon", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*172*/ new Function { Name = "has_use_skill", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*173*/ new Function { Name = "hasdomname", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*174*/ new Function { Name = "dps", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "time", Type = 0 } } },
            /*175*/ new Function { Name = "ObjectCanUpgrade", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*176*/ new Function { Name = "use_state_duration", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*177*/ new Function { Name = "uses_missiles", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*178*/ new Function { Name = "uses_lasers", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*179*/ new Function { Name = "has_damage_radius", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*180*/ new Function { Name = "missile_count", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*181*/ new Function { Name = "laser_count", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*182*/ new Function { Name = "shots_per_minute", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*183*/ new Function { Name = "milliseconds_per_shot", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*184*/ new Function { Name = "player_crit_chance", Args = new[] { new Argument { Name = "pUnit", Type = 5 }, new Argument { Name = "nSlot", Type = 0 } } },
            /*185*/ new Function { Name = "player_crit_damage", Args = new[] { new Argument { Name = "pUnit", Type = 5 }, new Argument { Name = "nSlot", Type = 0 } } },
            /*186*/ new Function { Name = "add_item_level_armor", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevel", Type = 0 }, new Argument { Name = "nPercent", Type = 0 } } },
            /*187*/ new Function { Name = "player_level_skill_power_cost_percent", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*188*/ new Function { Name = "item_level_damage_mult", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*189*/ new Function { Name = "item_level_feed", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*190*/ new Function { Name = "item_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*191*/ new Function { Name = "item_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*192*/ new Function { Name = "item_level_shield_buffer", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*193*/ new Function { Name = "monster_level_sfx_defense", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*194*/ new Function { Name = "monster_level_sfx_attack", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*195*/ new Function { Name = "monster_level_damage", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*196*/ new Function { Name = "monster_level_damage_pct", Args = new[] { new Argument { Name = "nLevel", Type = 0 }, new Argument { Name = "nPCT", Type = 0 } } },
            /*197*/ new Function { Name = "monster_level_shields", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*198*/ new Function { Name = "monster_level_armor", Args = new[] { new Argument { Name = "level", Type = 0 } } },
            /*199*/ new Function { Name = "unit_ai_changer_attack", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*200*/ new Function { Name = "does_field_damage", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*201*/ new Function { Name = "distance_to_player", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*202*/ new Function { Name = "has_container", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*203*/ new Function { Name = "monster_armor", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "nDamageType", Type = 1, TableIndex = 30 }, new Argument { Name = "nPercent", Type = 0 } } },
            /*204*/ new Function { Name = "getSkillDmgMult", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillLvl", Type = 0 }, new Argument { Name = "a", Type = 0 } } },
            /*205*/ new Function { Name = "getSkillArmorMult", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillLvl", Type = 0 }, new Argument { Name = "a", Type = 0 } } },
            /*206*/ new Function { Name = "getSkillAttackSpeedMult", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillLvl", Type = 0 }, new Argument { Name = "a", Type = 0 } } },
            /*207*/ new Function { Name = "getSkillToHitMult", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillLvl", Type = 0 }, new Argument { Name = "a", Type = 0 } } },
            /*208*/ new Function { Name = "getSkillPctDmgMult", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillLvl", Type = 0 }, new Argument { Name = "a", Type = 0 } } },
            /*209*/ new Function { Name = "getPetCountOfType", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nUnitType", Type = 1, TableIndex = 23 } } },
            /*210*/ new Function { Name = "runScriptOnPetsOfType", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nUnitType", Type = 1, TableIndex = 23 }, new Argument { Name = "a", Type = 0 } } },
            /*211*/ new Function { Name = "randaffixtype", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "affixType", Type = 1, TableIndex = 50 } } },
            /*212*/ new Function { Name = "randaffixgroup", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "3h", Type = 1, TableIndex = 52 } } },
            /*213*/ new Function { Name = "applyaffix", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "affix", Type = 1, TableIndex = 52 }, new Argument { Name = "bForce", Type = 0 } } },
            /*214*/ new Function { Name = "getBonusValue", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "a", Type = 0 } } },
            /*215*/ new Function { Name = "getBonusAll", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*216*/ new Function { Name = "getDMGAugmentation", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillVar", Type = 0 }, new Argument { Name = "nLevel", Type = 0 }, new Argument { Name = "nPercentOfLevel", Type = 0 }, new Argument { Name = "nSkillPointsInvested", Type = 0 } } },
            /*217*/ new Function { Name = "getDMGAugmentationPCT", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillVar", Type = 0 }, new Argument { Name = "nLevel", Type = 0 }, new Argument { Name = "nPercentOfLevel", Type = 0 }, new Argument { Name = "nSkillPointsInvested", Type = 0 } } },
            /*218*/ new Function { Name = "getMonsterHPAtLevel", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevel", Type = 0 } } },
            /*219*/ new Function { Name = "getMonsterHPAtLevelByPCT", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nLevel", Type = 0 }, new Argument { Name = "nPCT", Type = 0 } } },
            /*220*/ new Function { Name = "display_dmg_absorbed_pct", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*221*/ new Function { Name = "dmg_percent_by_energy", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*222*/ new Function { Name = "weapon_range", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*223*/ new Function { Name = "IsObjectDestructable", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*224*/ new Function { Name = "GlobalThemeIsEnabled", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nTheme", Type = 1, TableIndex = 167 } } },
            /*225*/ new Function { Name = "SetRespawnPlayer", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*226*/ new Function { Name = "AddSecondaryRespawnPlayer", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*227*/ new Function { Name = "RemoveHPAndCheckForDeath", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "nRemove", Type = 0 } } },
            /*228*/ new Function { Name = "getSkillStat", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nSkillStat", Type = 1, TableIndex = 44 }, new Argument { Name = "nSkillLvl", Type = 0 } } },
            /*229*/ new Function { Name = "TownPortalIsAllowed", Args = new[] { new Argument { Name = "unit", Type = 5 } } },
            /*230*/ new Function { Name = "lowerManaCostOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 }, new Argument { Name = "nPctPower", Type = 0 } } },
            /*231*/ new Function { Name = "lowerCoolDownOnSkillByPct", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 }, new Argument { Name = "nPctCooldown", Type = 0 } } },
            /*232*/ new Function { Name = "skillIsOn", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*233*/ new Function { Name = "getSkillRange", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*234*/ new Function { Name = "setDmgEffectParams", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nDmgEffect", Type = 1, TableIndex = 31 }, new Argument { Name = "nParam0", Type = 0 }, new Argument { Name = "nParam1", Type = 0 }, new Argument { Name = "nParam2", Type = 0 } } },
            /*235*/ new Function { Name = "setDmgEffectSkill", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nDmgEffect", Type = 1, TableIndex = 31 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*236*/ new Function { Name = "setDmgEffectSkillOnTarget", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nDmgEffect", Type = 1, TableIndex = 31 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*237*/ new Function { Name = "getSkillID", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*238*/ new Function { Name = "fireMissileFromObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "missileID", Type = 1, TableIndex = 110 } } },
            /*239*/ new Function { Name = "caculateGemSockets", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*240*/ new Function { Name = "caculateRareGemSockets", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*241*/ new Function { Name = "caculateCraftingSlots", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*242*/ new Function { Name = "executeSkill", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*243*/ new Function { Name = "executeSkillOnObject", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*244*/ new Function { Name = "stopSkill", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*245*/ new Function { Name = "powercost", Args = new[] { new Argument { Name = "unit", Type = 5 }, new Argument { Name = "skillID", Type = 1, TableIndex = 41 } } },
            /*246*/ new Function { Name = "is_stash_ui_open" },
            /*247*/ new Function { Name = "setRecipeLearned", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*248*/ new Function { Name = "getRecipeLearned", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*249*/ new Function { Name = "createRecipe", Args = new[] { new Argument { Name = "context", Type = 2 } } },
            /*250*/ new Function { Name = "createSpecificRecipe", Args = new[] { new Argument { Name = "context", Type = 2 }, new Argument { Name = "nRecipeID", Type = 1, TableIndex = 108 } } },
            /*251*/ new Function { Name = "getCurrentGameTick", Args = new[] { new Argument { Name = "context", Type = 2 } } }
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
         *      666, 612368384, 0                                   -> return GetStatValue(612368384); // = GetStatValue(612368384 << 22 = 146) = GetStatValue("experience");
         * 
         * 669  0x29D       SetStatValue                            Sets the value of a stat.
         * e.g.
         *      26,10, 669,25165824, 0                              -> return SetStatValue(25165824); // = SetStatValue(25165824 << 22 = 6) = SetStatValue("hp_cur");
         * 
         * 
         */

        // op code array in client found at:  .data:00000001407161C8
        private enum ExcelOpCodes : uint
        {
            Return = 0,                     // 0x00
            CallPropery = 2,                // 0x02
            Call = 3,                       // 0x03
            Unknown4 = 4,                   // 0x04     // Ternary conditional?
            AllocateVar = 6,                // 0x06
            Unknown14 = 14,                 // 0x0E     // Ternary conditional?
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
            And = 516,                      // 0x204
            Or = 527,                       // 0x20F
            EndIf = 538,                    // 0x21A    // I think... Or something like that...
            GetStat666 = 666,               // 0x29A    // these are used for different arg/return types (int/uint/float/double/etc)
            GetStat667 = 667,               // 0x29B    // not sure which are which yet - do it later
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
            GlobalVarStats = 712,           // 0x2C8
            AssignContextVar = 713,         // 0x2C9
            NotEntirlySure714 = 714         // 0x2CA    // set type/size = 4??
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

        private enum ExcelTableCodes : uint
        {
            Stats = 23088
        }
        #endregion

        private const String DebugRoot = @"C:\excel_script_debug\";
        private static bool _debug;
        public static void EnableDebug(bool enableDebug)
        {
            _debug = enableDebug;

            if (!_debug) return;

            Directory.CreateDirectory(DebugRoot);
            String[] oldLogs = Directory.GetFiles(DebugRoot);
            foreach (String logPath in oldLogs)
            {
                File.Delete(logPath);
            }
        }

        private static FileManager _fileManager;
        public static void SetFileManager(FileManager fileManager)
        {
            if (fileManager == null) throw new ArgumentNullException("fileManager", "File manager cannot be null!");
            if (fileManager.DataFiles == null || fileManager.DataFiles.Count == 0) fileManager.LoadTableFiles();
            if (fileManager.DataFiles.Count == 0) throw new Exceptions.ExcelScriptNotInitialised("SetFileManager was unable to initialise the excel table files!");

            _fileManager = fileManager;

            ExcelFile propertiesTable = _fileManager.GetDataFile("PROPERTIES") as ExcelFile;
            ExcelFile skillsTable = _fileManager.GetDataFile("SKILLS") as ExcelFile;

            if (propertiesTable == null || skillsTable == null) throw new Exceptions.ExcelScriptNotInitialised("SetFileManager was unable to obtain the properties and skills excel tables!");

            _ParsePropertiesExcelFunctions(propertiesTable);
            _ParseSkillsExcelFunctions(skillsTable);
        }

        private class StackObject
        {
            public String Value;
            public int Precedence;          // using chart from http://en.wikipedia.org/wiki/Operators_in_C_and_C%2B%2B (with exponent = 4)
            public bool IsFunction;
            public uint ByteOffset;         // for local vars

            public override string ToString()
            {
                return Value;
            }
        }

        private static bool _havePropertiesFunctions;
        private static bool _haveSkillsFunctions;

        private String _script;
        private readonly Stack<StackObject> _stack;
        private readonly StackObject[] _vars;
        private ExcelFile.ExcelFunction _excelScriptFunction;
        private Function _excelFunction;

        public ExcelScript()
        {
            _script = String.Empty;
            _stack = new Stack<StackObject>();
            _vars = new StackObject[10];
        }

        /// <summary>
        /// Decompiles a script from byte codes to human readable text.
        /// </summary>
        /// <param name="scriptBytes">The script byte-code array.</param>
        /// <param name="offset">The starting offset within the byte code array.</param>
        /// <param name="scriptString">For debugging purposes only.</param>
        /// <param name="stringId">For debugging purposes only.</param>
        /// <param name="row">For debugging purposes only.</param>
        /// <param name="col">For debugging purposes only.</param>
        /// <param name="colName">For debugging purposes only.</param>
        /// <returns>Decompiled excel script byte codes as human readable script.</returns>
        public String Decompile(byte[] scriptBytes, int offset, String scriptString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            if (!_havePropertiesFunctions && !_debug) throw new Exceptions.ExcelScriptNotInitialised("Properties excel script functions have not been parsed; they must be loaded before any decompiling can be done.\nPlease ensure you have provided a valid FileManager for initialisation.");
            if (!_haveSkillsFunctions && !_debug) throw new Exceptions.ExcelScriptNotInitialised("Skills excel script functions have not been parsed; they must be loaded before any decompiling can be done.\nPlease ensure you have provided a valid FileManager for initialisation.");

            return _Decompile(scriptBytes, offset, scriptString, stringId, row, col, colName);
        }

        /// <summary>
        /// Decompiles an excel script function from byte codes to human readable text.
        /// </summary>
        /// <param name="excelScriptFunction">The excel script function to decompile.</param>
        /// <param name="excelFunction">The associated excel function to use for agrument reference.</param>
        /// <param name="scriptString">For debugging purposes only.</param>
        /// <param name="stringId">For debugging purposes only.</param>
        /// <param name="row">For debugging purposes only.</param>
        /// <param name="col">For debugging purposes only.</param>
        /// <param name="colName">For debugging purposes only.</param>
        /// <returns>Decompiled excel script byte codes as human readable script.</returns>
        private String Decompile(ExcelFile.ExcelFunction excelScriptFunction, Function excelFunction, String scriptString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            if (excelScriptFunction == null) throw new ArgumentNullException("excelScriptFunction", "The excel script function cannot be null!");
            if (excelFunction == null) throw new ArgumentNullException("excelFunction", "The excel function cannot be null!");

            _excelScriptFunction = excelScriptFunction;
            _excelFunction = excelFunction;
            return _Decompile(_excelScriptFunction.ScriptByteCode, 0, scriptString, stringId, row, col, colName);
        }

        private String _Decompile(byte[] scriptBytes, int offset, String scriptString = null, String stringId = null, int row = 0, int col = 0, String colName = null)
        {
            bool debug = (stringId != null) && _debug;
            bool debubShowParsed = false;
            bool debugOutputParsed = false;
            bool debugScriptParsed = true;
            String debugPos = null;

            if (debug)
            {
                debubShowParsed = false;
                debugOutputParsed = true;
                String rowName = String.Empty;
                if (_fileManager != null)
                {
                    int colIndex = 0;
                    switch (stringId)
                    {
                        case "ITEMDISPLAY":
                            colIndex = 1;
                            debubShowParsed = false;
                            break;

                        case "MUSICCONDITIONS":
                            colIndex = 4;
                            break;
                    }

                    rowName = _fileManager.GetExcelRowStringFromStringId(stringId, row, colIndex);
                }

                debugPos = String.Format("row({0}): '{1}', col({2}): '{3}', scriptBytes: {4}", row, rowName, col, colName, scriptString);
                if (debubShowParsed) Debug.WriteLine(debugPos);
            }

            try
            {
                int infCheck = 0;
                while (true)
                {
                    ExcelOpCodes opCode = (ExcelOpCodes)FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                    String value1;
                    String value2;
                    uint index;
                    int functionIndex;
                    uint unknown;
                    uint byteOffset;
                    StackObject stackObject1;
                    StackObject stackObject2;
                    String argName;

                    if (row == 21)
                    {
                        int bp = 0;
                    }

                    switch (opCode)
                    {
                        case ExcelOpCodes.Return:                   // 0    0x00
                            _CheckStack(1, opCode);

                            if (_stack.Count > 1)
                            {
                                while (_stack.Count > 1)
                                {
                                    StackObject stackObject = _stack.Pop();

                                    if (!stackObject.IsFunction)
                                    {
                                        String error = String.Format("Error: Stack has more than 1 value upon script return: script = \"{0}\"\n{1}", _script, _DumpStack(_stack));
                                        throw new Exceptions.ExcelScriptInvalidStackState(error);
                                    }

                                    _script = " " + stackObject.Value + ";" + _script;
                                }
                            }

                            _script = _stack.Pop().Value + ";" + _script;
                            if (debubShowParsed) Debug.WriteLine(_script);
                            return _script;
                            break;

                        case ExcelOpCodes.CallPropery:              // 2    0x02
                            functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref offset);

                            _CallFunction(functionIndex);
                            break;

                        case ExcelOpCodes.Call:                     // 3    0x03
                            functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref offset);

                            _CallFunction(functionIndex);
                            //if (functionIndex < 0 || functionIndex > CallFunctions.Count)
                            //{
                            //    throw new Exceptions.ExcelScriptUnknownFunctionCall("Unknown function index: " + functionIndex);
                            //}

                            //Function excelScriptFunction = CallFunctions[functionIndex];
                            //_CheckStack(excelScriptFunction.ArgCount, ExcelOpCodes.Call, excelScriptFunction);
                            //String argsString = String.Empty;

                            //for (int i = excelScriptFunction.ArgCount - 1; i >= 0; i--)
                            //{
                            //    StackObject argStackObject = _stack.Pop();
                            //    String argStr = argStackObject.Value;

                            //    int tableIndex = excelScriptFunction.Args[i].TableIndex;
                            //    if (tableIndex >= 0)
                            //    {
                            //        int rowIndex = int.Parse(argStr);
                            //        String excelString = _fileManager.GetExcelRowStringFromExcelIndex(tableIndex, rowIndex);

                            //        if (excelString == null)
                            //        {
                            //            Console.WriteLine(String.Format("Warning: Unknown row index '{0}' on table index '{1}'.", rowIndex, tableIndex));
                            //            argsString += rowIndex.ToString();
                            //        }
                            //        else
                            //        {
                            //            argsString = String.Format("'{0}'", excelString) + argsString;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        argsString = argStr + argsString;
                            //    }

                            //    if (i != 0) argsString = ", " + argsString;
                            //}

                            //String functionCallString = String.Format("{0}({1})", excelScriptFunction.Name, argsString);
                            //_stack.Push(new StackObject { Value = functionCallString, IsFunction = true });
                            break;

                        case ExcelOpCodes.Unknown4:                 // 4    0x04
                            _CheckStack(1, opCode);

                            unknown = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _stack.Push(new StackObject { Value = String.Format("Unknown4[{0}]", unknown) });
                            break;

                        case ExcelOpCodes.AllocateVar:              // 6    0x06
                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            Debug.Assert((byteOffset % 4) == 0);

                            value1 = String.Format(" var{0} = ", byteOffset);
                            _stack.Push(new StackObject { Value = value1, ByteOffset = byteOffset });
                            break;

                        case ExcelOpCodes.Unknown14:                // 14    0x0E
                            _CheckStack(1, opCode);

                            unknown = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _stack.Push(new StackObject { Value = String.Format("if ({0})[{1}] ", _stack.Pop().Value, unknown) });
                            break;

                        case ExcelOpCodes.Push:                     // 26   0x1A
                            int value = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                            _stack.Push(new StackObject { Value = value.ToString() });
                            break;

                        case ExcelOpCodes.PushLocalVarInt32:        // 50   0x32
                            Debug.Assert(_excelScriptFunction != null && _excelFunction != null);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            argName = (from arg in _excelFunction.Args
                                       where arg.ByteOffset == byteOffset
                                       select arg.Name).FirstOrDefault();

                            if (argName == null)
                            {
                                index = byteOffset / 4;
                                StackObject localVar = _vars[index];
                                Debug.Assert(localVar != null);

                                argName = "var" + byteOffset;
                            }

                            _stack.Push(new StackObject { Value = argName });
                            break;

                        case ExcelOpCodes.PushLocalVarPtr:          // 57   0x39
                            Debug.Assert(_excelScriptFunction != null && _excelFunction != null);

                            byteOffset = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            argName = (from arg in _excelFunction.Args
                                       where arg.ByteOffset == byteOffset
                                       select arg.Name).FirstOrDefault();

                            if (argName == null)
                            {
                                index = byteOffset / 4;
                                StackObject localVar = _vars[index];
                                Debug.Assert(localVar != null);

                                argName = "var" + byteOffset;
                            }

                            _stack.Push(new StackObject { Value = argName });
                            break;

                        case ExcelOpCodes.AssignLocalVarInt32:      // 98   0x62
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            index = stackObject1.ByteOffset / 4;
                            Debug.Assert(index >= 0 && index < _vars.Length);

                            // todo: check ensure stackObject2 is correct type??
                            _vars[index] = stackObject2;
                            value1 = String.Format("int{0}{1}", stackObject1.Value, stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsFunction = true });
                            break;

                        case ExcelOpCodes.AssignLocalVarPtr:        // 105   0x69
                            _CheckStack(2, opCode);

                            stackObject2 = _stack.Pop();
                            stackObject1 = _stack.Pop();

                            index = stackObject1.ByteOffset / 4;
                            Debug.Assert(index >= 0 && index < _vars.Length);

                            // todo: check ensure stackObject2 is correct type??
                            _vars[index] = stackObject2;
                            value1 = String.Format("void*{0}{1}", stackObject1.Value, stackObject2.Value);
                            _stack.Push(new StackObject { Value = value1, IsFunction = true });
                            break;

                        case ExcelOpCodes.Complement:               // 320  0x140
                            _CheckStack(1, opCode);

                            StackObject toComplementObject = _stack.Pop();
                            if (toComplementObject.Value[0] == '-') // if already negative, remove sign
                            {
                                toComplementObject.Value = toComplementObject.Value.Substring(1, toComplementObject.Value.Length - 1);
                            }
                            else
                            {
                                toComplementObject.Value = "-" + toComplementObject.Value;
                            }

                            _stack.Push(toComplementObject);
                            break;

                        case ExcelOpCodes.Not:                      // 339  0x153
                            _CheckStack(1, opCode);

                            _stack.Push(new StackObject { Value = String.Format("!{0}", _stack.Pop().Value) });
                            break;

                        case ExcelOpCodes.Pow:                      // 347  0x15B
                            _DoOperator("^", 4, opCode);
                            break;

                        case ExcelOpCodes.Mult:                     // 358  0x166
                            _DoOperator("*", 5, opCode);
                            break;

                        case ExcelOpCodes.Div:                      // 369  0x171
                            _DoOperator("/", 5, opCode);
                            break;

                        case ExcelOpCodes.Add:                      // 388  0x184
                            _DoOperator("+", 6, opCode);
                            break;

                        case ExcelOpCodes.Sub:                      // 399  0x18F
                            _DoOperator("-", 6, opCode);
                            break;

                        case ExcelOpCodes.LessThan:                 // 426  0x1AA
                            _DoOperator("<", 8, opCode);
                            break;

                        case ExcelOpCodes.GreaterThan:              // 437  0x1B5
                            _DoOperator(">", 8, opCode);
                            break;

                        case ExcelOpCodes.LessThanOrEqual:          // 448  0x1C0
                            _DoOperator("<=", 8, opCode);
                            break;

                        case ExcelOpCodes.GreaterThanOrEqual:       // 459  0x1CB
                            _DoOperator(">=", 8, opCode);
                            break;

                        case ExcelOpCodes.EqualTo:                  // 470  0x1D6
                            _DoOperator("==", 9, opCode);
                            break;

                        case ExcelOpCodes.NotEqualTo:               // 481  0x1E1
                            _DoOperator("!=", 9, opCode);
                            break;

                        case ExcelOpCodes.And:                      // 516  0x204
                            _CheckStack(1, opCode);

                            unknown = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _stack.Push(new StackObject { Value = String.Format("{0} &&[{1}]", _stack.Pop().Value, unknown) });
                            break;

                        case ExcelOpCodes.Or:                       // 527  0x20F
                            _CheckStack(1, opCode);

                            unknown = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            _stack.Push(new StackObject { Value = String.Format("{0} ||[{1}]", _stack.Pop().Value, unknown) });
                            break;

                        case ExcelOpCodes.EndIf:                    // 538  0x21A
                            _CheckStack(2, opCode);

                            value2 = _stack.Pop().Value;
                            value1 = _stack.Pop().Value;
                            _stack.Push(new StackObject { Value = String.Format("({0} {1})", value1, value2) });
                            break;

                        case ExcelOpCodes.GetStat666:               // 666  0x29A
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;
                            // todo: we are losing bits with this shift - figure out what they're used for
                            _DoFunction("GetStat666", index, (uint)ExcelTableCodes.Stats, opCode, false);
                            break;

                        case ExcelOpCodes.GetStat667:               // 667  0x29B
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("GetStat667", index, (uint)ExcelTableCodes.Stats, opCode, false);
                            break;

                        case ExcelOpCodes.SetStat669:               // 669  0x29D
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("SetStat669", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ExcelOpCodes.SetStat673:               // 673  0x2A1
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("SetStat673", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ExcelOpCodes.SetStat674:               // 674  0x2A2
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("SetStat674", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ExcelOpCodes.GetStat680:               // 680  0x2A8
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("GetStat680", index, (uint)ExcelTableCodes.Stats, opCode, false);
                            break;

                        case ExcelOpCodes.SetStat683:               // 683  0x2AB
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("SetStat683", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ExcelOpCodes.SetStat687:               // 687  0x2AF
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("SetStat687", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ExcelOpCodes.SetStat688:               // 688  0x2B0
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                            index >>= 22;

                            _DoFunction("SetStat688", index, (uint)ExcelTableCodes.Stats, opCode, true);
                            break;

                        case ExcelOpCodes.PushContextVarInt32:      // 700 0x2BC
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarInt32", index);
                            break;

                        case ExcelOpCodes.PushContextVarUInt32:     // 701 0x2BD
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarUInt32", index);
                            break;

                        case ExcelOpCodes.PushContextVarInt64:      // 702 0x2BE
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarInt64", index);
                            break;

                        case ExcelOpCodes.PushContextVarUInt64:     // 703 0x2BF
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarUInt64", index);
                            break;

                        case ExcelOpCodes.PushContextVarFloat:      // 704 0x2C0
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarFloat", index);
                            break;

                        case ExcelOpCodes.PushContextVarDouble:     // 705 0x2C1
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarDouble", index);
                            break;

                        case ExcelOpCodes.PushContextVarDouble2:    // 706 0x2C2
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarDouble2", index);
                            break;

                        case ExcelOpCodes.PushContextVarPtr:        // 707 0x2C3
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            _PushContextVariable("PushContextVarPtr", index);
                            break;

                        case ExcelOpCodes.GlobalVarGame3:           // 708  0x2C4
                            _stack.Push(new StackObject { Value = "game3" });
                            break;

                        case ExcelOpCodes.GlobalVarContext:         // 709  0x2C5
                            _stack.Push(new StackObject { Value = "context" });
                            break;

                        case ExcelOpCodes.GlobalVarGame4:           // 710  0x2C6
                            _stack.Push(new StackObject { Value = "game4" });
                            break;

                        case ExcelOpCodes.GlobalVarUnit:            // 711  0x2C7
                            _stack.Push(new StackObject { Value = "unit" });
                            break;

                        case ExcelOpCodes.GlobalVarStats:           // 712  0x2C8
                            _stack.Push(new StackObject { Value = "stats" });
                            break;

                        case ExcelOpCodes.AssignContextVar:         // 713  0x2C9
                            _CheckStack(1, opCode);

                            stackObject1 = _stack.Pop();
                            index = FileTools.ByteArrayToUInt32(scriptBytes, ref offset);

                            value1 = String.Format("SetContextVar({0}, {1})", ((ContextVariables) index).ToString().ToLower(), stackObject1.Value);
                            _stack.Push(new StackObject { Value = value1, IsFunction = true });
                            break;

                        case ExcelOpCodes.NotEntirlySure714:        // 714  0x2CA
                            _stack.Pop();
                            //_stack.Push(new StackObject { Value = "(unknown714)" });
                            break;

                        default:
                            throw new Exceptions.ExcelScriptUnknownOpCode(opCode.ToString(), _DumpStack(_stack));
                    }

                    infCheck++;
                    if (infCheck >= 1000) throw new Exceptions.ExcelScriptInfiniteCheck(opCode.ToString(), _DumpStack(_stack));
                }
            }
            catch (Exception e)
            {
                if (debug)
                {
                    String debugOutputPath = String.Format("{0}{1}_scriptdebug.txt", DebugRoot, stringId);
                    String debugOutput = String.Format("{0}\n{1}\n\n\n", debugPos, e);
                    File.AppendAllText(debugOutputPath, debugOutput);
                    debugScriptParsed = false;
                }

                throw;
            }
            finally
            {
                if (debugOutputParsed && debugScriptParsed)
                {
                    String[] code = _script.Split(new[] { "; ", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    String codeStr = code.Aggregate(String.Empty, (current, line) => current + ("\t" + line + ";\n"));

                    String debugOutputPath = String.Format("{0}{1}_scriptdebug.txt", DebugRoot, stringId);
                    String debugOutput = String.Format("{0}\n{1}\n\n", debugPos, codeStr);
                    File.AppendAllText(debugOutputPath, debugOutput);
                }
            }
        }

        private void _CallFunction(int functionIndex)
        {
            if (functionIndex < 0 || functionIndex > CallFunctions.Count)
            {
                throw new Exceptions.ExcelScriptUnknownFunctionCall("Unknown function index: " + functionIndex);
            }

            Function excelScriptFunction = CallFunctions[functionIndex];
            _CheckStack(excelScriptFunction.ArgCount, ExcelOpCodes.Call, excelScriptFunction);
            String argsString = String.Empty;

            for (int i = excelScriptFunction.ArgCount - 1; i >= 0; i--)
            {
                StackObject argStackObject = _stack.Pop();
                String argStr = argStackObject.Value;

                int tableIndex = excelScriptFunction.Args[i].TableIndex;
                if (tableIndex >= 0)
                {
                    int rowIndex = int.Parse(argStr);
                    String excelString = _fileManager.GetExcelRowStringFromExcelIndex(tableIndex, rowIndex);

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

        private void _DoOperator(String op, int precedence, ExcelOpCodes opCode)
        {
            _CheckStack(2, opCode);

            StackObject value2Object = _stack.Pop();
            StackObject value1Object = _stack.Pop();

            const String operatorFormat = "{0} {1} {2}";
            const String operatorFormatParentheses = "({0}) {1} {2}";

            String opFormat = value1Object.Precedence > precedence ? operatorFormatParentheses : operatorFormat;

            StackObject newStackObject = new StackObject
            {
                Value = String.Format(opFormat, value1Object.Value, op, value2Object.Value),
                Precedence = precedence
            };

            _stack.Push(newStackObject);
        }

        private void _DoFunction(String name, uint index, uint tableCode, ExcelOpCodes opCode, bool isSet)
        {
            String argStr = String.Empty;
            if (isSet)
            {
                _CheckStack(1, opCode);

                argStr = ", " + _stack.Pop().Value;
            }

            String indexStr = index.ToString();
            if (tableCode > 0)
            {
                indexStr = _GetExcelValueFromCode(index, tableCode);
            }

            StackObject newStackObject = new StackObject
            {
                Value = String.Format("{0}({1}{2})", name, indexStr, argStr),
                IsFunction = true
            };

            _stack.Push(newStackObject);
        }

        private void _CheckStack(int requiredCount, ExcelOpCodes opCode, Function functionDetails = null)
        {
            if (requiredCount <= _stack.Count) return;

            String extra = String.Empty;
            if (functionDetails != null)
            {
                extra = String.Format(" Function Name: '{0}'", functionDetails.Name);
            }

            String error = String.Format("The OpCode {0} requires {1} values on the stack.{2}\n{3}", opCode, requiredCount, extra, _DumpStack(_stack));
            throw new Exceptions.ExcelScriptInvalidStackState(error);
        }

        private static String _DumpStack(Stack<StackObject> stack)
        {
            String stackDump = "Stack Dump : LIFO\nIndex\tPrecedence\tIsFunction\tValue\n";

            int stackCount = stack.Count;
            for (int i = 0; i < stackCount; i++)
            {
                StackObject stackObject = stack.Pop();
                stackDump += String.Format("{0}\t\t{1}\t\t\t{2}\t\t{3}\n", i, stackObject.Precedence, stackObject.IsFunction, stackObject.Value);
            }

            return stackDump;
        }

        private static String _GetExcelValueFromCode(uint index, uint code)
        {
            if (_fileManager == null) return index.ToString();

            String indexStr = _fileManager.GetExcelRowStringFromRowIndex(code, (int)index);
            return String.IsNullOrEmpty(indexStr) ? index.ToString() : String.Format("'{0}'", indexStr);
        }


        #region Excel Script Functions Stuff

        /// <summary>
        /// Parses and appends the properties excel script functions to the global call function array.
        /// Must be called before the skills table and before script decompiling can begin.
        /// </summary>
        /// <param name="propertiesTable">The properties excel file containing the excel script functions to parse.</param>
        private static int _ParsePropertiesExcelFunctions(ExcelFile propertiesTable)
        {
            if (propertiesTable == null) throw new ArgumentNullException("propertiesTable", "Properties excel file cannot be null!");

            int added = _ParseExcelFunctions(propertiesTable);
            if (added > 0) _havePropertiesFunctions = true;

            return added;
        }

        /// <summary>
        /// Parses and appends the skills excel script functions to the global call function array.
        /// Must be called after the properties table and before script decompiling can begin.
        /// </summary>
        /// <param name="skillsTable">The skills excel file containing the excel script functions to parse.</param>
        private static int _ParseSkillsExcelFunctions(ExcelFile skillsTable)
        {
            if (skillsTable == null) throw new ArgumentNullException("skillsTable", "Skills excel file cannot be null!");
            if (!_havePropertiesFunctions) throw new Exceptions.ExcelScriptNotInitialised("Properties excel script functions have not been parsed! They must be loaded before the skills excel script functions!");

            int added = _ParseExcelFunctions(skillsTable);
            if (added > 0) _haveSkillsFunctions = true;

            return added;
        }

        /// <summary>
        /// Main worker function that parses and appends excel script functions to the global call function array.
        /// </summary>
        /// <param name="excelFile">The  excel file containing the excel script functions to parse.</param>
        /// <returns>The number of functions added.</returns>
        private static int _ParseExcelFunctions(ExcelFile excelFile)
        {
            int startFuncCount = CallFunctions.Count;
            int voidFunctions = 0;

            foreach (ExcelFile.ExcelFunction excelFunction in excelFile.ExcelFunctions)
            {
                if (excelFunction.Parameters.Count == 1) // not sure what to do with them - they don't appear to be used
                {
                    voidFunctions++;
                    continue;
                }

                Function function = new Function();
                List<Argument> arguments = new List<Argument>();

                for (int i = 0; i < excelFunction.Parameters.Count; i++)
                {
                    ExcelFile.ExcelFunction.Parameter parameter = excelFunction.Parameters[i];

                    if (i == 0)
                    {
                        function.Name = parameter.Name;
                        int functionIndex = parameter.TypeValues[4];
                        Debug.Assert(functionIndex == CallFunctions.Count);

                        continue;
                    }

                    //if (function.Name == "mod_feed_strength")
                    //{
                    //    int bp = 0;
                    //}

                    Argument arg = new Argument
                    {
                        Name = parameter.Name,
                        Type = 0, // I think they're all int
                        ByteOffset = parameter.TypeValues[3],
                    };

                    arguments.Add(arg);
                }

                String byteCodeString = null;
                if (_debug)
                {
                    int offset = 0;
                    byteCodeString = FileTools.ByteArrayToInt32Array(excelFunction.ScriptByteCode, ref offset, excelFunction.ScriptByteCode.Length / 4).ToString(",");
                }

                function.Args = arguments.ToArray();
                try
                {
                    ExcelScript excelScript = new ExcelScript();
                    function.ExcelScript = excelScript.Decompile(excelFunction, function, byteCodeString, "PROPERTIES", 0, 0, function.Name);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    continue;
                }

                if (_debug)
                {
                    String[] code = function.ExcelScript.Split(new[] {"; ", ";"}, StringSplitOptions.RemoveEmptyEntries);
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

                CallFunctions.Add(function);
            }

            return CallFunctions.Count - startFuncCount + voidFunctions;
        }

        #endregion

        #region Get Functions From ASM Stuff

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
                            function.Args[argIndex++] = new Argument { Name = registers["r8"], Type = 1, TableIndex = Int32.Parse(tableIndex, NumberStyles.HexNumber) };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Context"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = "context", Type = 2 };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Game3"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = "game3", Type = 3 };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Game4"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = "game4", Type = 4 };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Unit"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            String strType = registers["r8"];
                            Debug.Assert(!String.IsNullOrEmpty(strType));
                            String typeValue = strType.Replace("h", "");
                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = Int32.Parse(typeValue, NumberStyles.HexNumber) };
                        }
                        else if (asmStr[1].Contains("Script_AddParam_Param"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = 7 };
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

                    const String argFormatStringBasic = "new Argument {{ Name = \"{0}\", Type = {1} }}";
                    const String argFormatStringIndex = "new Argument {{ Name = \"{0}\", Type = {1}, TableIndex = {2} }}";

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
