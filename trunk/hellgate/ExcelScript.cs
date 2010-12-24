using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Revival.Common;

namespace Hellgate
{
    public static class ExcelScript
    {
        #region Type Definitions
        private class Argument
        {
            public String Name;
            public String Type;
            public int TableIndex = -1;
        }

        private class Function
        {
            public String Name;
            public String ReturnType;
            public Argument[] Args;
            public int ArgCount { get { return Args == null ? 0 : Args.Length; } }
        }

        private static readonly Dictionary<int, Function> ClientFunctions = new Dictionary<int, Function>
        {
            { 0, new Function { Name = "setUnitTypeAreaFloorVis", ReturnType = "void", Args = new[] { new Argument { Name = "nLevelArea", Type = "index", TableIndex = 138 }, new Argument { Name = "nFloor", Type = "int" }, new Argument { Name = "nVis", Type = "int" } } }},
            { 1, new Function { Name = "setUnitTypeAreaFloorInteractive", ReturnType = "void", Args = new[] { new Argument { Name = "nLevelArea", Type = "index", TableIndex = 138 }, new Argument { Name = "nFloor", Type = "int" } } }},
            { 2, new Function { Name = "setUnitTypeAreaFloorDisabled", ReturnType = "void", Args = new[] { new Argument { Name = "nLevelArea", Type = "index", TableIndex = 138 }, new Argument { Name = "nFloor", Type = "int" } } }},
            { 3, new Function { Name = "showDialog", ReturnType = "void", Args = new[] { new Argument { Name = "nDialog", Type = "index", TableIndex = 53 } } }},
            { 4, new Function { Name = "setQuestBit", ReturnType = "void", Args = new[] { new Argument { Name = "nBit", Type = "int" } } }},
            { 5, new Function { Name = "getQuestBit", ReturnType = "void", Args = new[] { new Argument { Name = "nBit", Type = "int" } } }},
            { 6, new Function { Name = "isQuestTaskComplete", ReturnType = "void", Args = new[] { new Argument { Name = "nQuestTask", Type = "index", TableIndex = 165 } } }},
            { 7, new Function { Name = "isQuestTaskActive", ReturnType = "void", Args = new[] { new Argument { Name = "nQuestTask", Type = "index", TableIndex = 165 } } }},
            { 8, new Function { Name = "isTalkingTo", ReturnType = "void", Args = new[] { new Argument { Name = "nNPCID", Type = "index", TableIndex = 64 } } }},
            { 9, new Function { Name = "setTargetVisibility", ReturnType = "void", Args = new[] { new Argument { Name = "nVis", Type = "int" } } }},
            { 10, new Function { Name = "setTargetVisibilityOnFloor", ReturnType = "void", Args = new[] { new Argument { Name = "nVis", Type = "int" }, new Argument { Name = "nFloor", Type = "int" } } }},
            { 11, new Function { Name = "setStateOnTarget", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 12, new Function { Name = "setTargetInteractive", ReturnType = "void", Args = new[] { new Argument { Name = "nInteractive", Type = "int" } } }},
            { 13, new Function { Name = "setTargetToTeam", ReturnType = "void", Args = new[] { new Argument { Name = "nTeam", Type = "int" } } }},
            { 14, new Function { Name = "getIsTargetOfType", ReturnType = "void", Args = new[] { new Argument { Name = "nUnitType", Type = "index", TableIndex = 23 } } }},
            { 15, new Function { Name = "setMonsterInLevelToTarget", ReturnType = "void", Args = new[] { new Argument { Name = "nMonsterID", Type = "index", TableIndex = 115 } } }},
            { 16, new Function { Name = "setObjectInLevelToTarget", ReturnType = "void", Args = new[] { new Argument { Name = "nObjectID", Type = "index", TableIndex = 119 } } }},
            { 17, new Function { Name = "getIsTargetMonster", ReturnType = "void", Args = new[] { new Argument { Name = "nMonsterID", Type = "index", TableIndex = 115 } } }},
            { 18, new Function { Name = "getIsTargetObject", ReturnType = "void", Args = new[] { new Argument { Name = "nObjectID", Type = "index", TableIndex = 119 } } }},
            { 19, new Function { Name = "resetTargetObject", ReturnType = "void" }},
            { 20, new Function { Name = "messageStatVal", ReturnType = "void", Args = new[] { new Argument { Name = "nStatId", Type = "index", TableIndex = 27 }, new Argument { Name = "nIndex", Type = "int" } } }},
            { 21, new Function { Name = "getStatVal", ReturnType = "void", Args = new[] { new Argument { Name = "nStatId", Type = "index", TableIndex = 27 }, new Argument { Name = "nIndex", Type = "int" } } }},
            { 22, new Function { Name = "createMap", ReturnType = "void" }},
            { 23, new Function { Name = "randomizeMapSpawner", ReturnType = "void" }},
            { 24, new Function { Name = "randomizeMapSpawnerEpic", ReturnType = "void" }},
            { 25, new Function { Name = "setMapSpawnerByLevelAreaID", ReturnType = "void", Args = new[] { new Argument { Name = "nLevelAreaID", Type = "int" } } }},
            { 26, new Function { Name = "setMapSpawnerByLevelArea", ReturnType = "void", Args = new[] { new Argument { Name = "nLevelAreaID", Type = "index", TableIndex = 138 } } }},
            { 27, new Function { Name = "abs", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" } } }},
            { 28, new Function { Name = "min", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 29, new Function { Name = "max", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 30, new Function { Name = "pin", ReturnType = "int", Args = new[] { new Argument { Name = "value", Type = "int" }, new Argument { Name = "min", Type = "int" }, new Argument { Name = "max", Type = "int" } } }},
            { 31, new Function { Name = "pct", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 32, new Function { Name = "pctFloat", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" }, new Argument { Name = "c", Type = "int" } } }},
            { 33, new Function { Name = "rand", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 34, new Function { Name = "randByUnitId", ReturnType = "void", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 35, new Function { Name = "chance", ReturnType = "int", Args = new[] { new Argument { Name = "nChance", Type = "int" }, new Argument { Name = "nChanceOutOf", Type = "int" } } }},
            { 36, new Function { Name = "chanceByStateMod", ReturnType = "void", Args = new[] { new Argument { Name = "nChance", Type = "int" }, new Argument { Name = "nChanceOutOf", Type = "int" }, new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "nModBy", Type = "int" } } }},
            { 37, new Function { Name = "randSkillSeed", ReturnType = "void", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 38, new Function { Name = "roll", ReturnType = "int", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" } } }},
            { 39, new Function { Name = "divMult", ReturnType = "void", Args = new[] { new Argument { Name = "a", Type = "int" }, new Argument { Name = "b", Type = "int" }, new Argument { Name = "c", Type = "int" } } }},
            { 40, new Function { Name = "distribute", ReturnType = "int", Args = new[] { new Argument { Name = "numdie", Type = "int" }, new Argument { Name = "diesize", Type = "int" }, new Argument { Name = "start", Type = "int" } } }},
            { 41, new Function { Name = "roundstat", ReturnType = "int", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 }, new Argument { Name = "value", Type = "int" } } }},
            { 42, new Function { Name = "getEventType", ReturnType = "void", Args = new[] { new Argument { Name = "nAffixID", Type = "index", TableIndex = 25 } } }},
            { 43, new Function { Name = "getAffixIDByName", ReturnType = "void", Args = new[] { new Argument { Name = "nAffixID", Type = "index", TableIndex = 52 } } }},
            { 44, new Function { Name = "get_skill_level", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "nSkill", Type = "index", TableIndex = 41 } } }},
            { 45, new Function { Name = "get_skill_level_object", ReturnType = "void", Args = new[] { new Argument { Name = "nSkill", Type = "index", TableIndex = 41 } } }},
            { 46, new Function { Name = "pickskill", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stats", Type = "type6" }, new Argument { Name = "nSkillLevel", Type = "int" } } }},
            { 47, new Function { Name = "pickskillbyunittype", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stats", Type = "type6" }, new Argument { Name = "nUnitType", Type = "index", TableIndex = 23 }, new Argument { Name = "nSkillLevel", Type = "int" } } }},
            { 48, new Function { Name = "removeoldestpetoftype", ReturnType = "void", Args = new[] { new Argument { Name = "nUnitType", Type = "index", TableIndex = 23 } } }},
            { 49, new Function { Name = "killoldestpetoftype", ReturnType = "void", Args = new[] { new Argument { Name = "nUnitType", Type = "index", TableIndex = 23 } } }},
            { 50, new Function { Name = "pickskillbyskillgroup", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stats", Type = "type6" }, new Argument { Name = "nSkillGroup", Type = "index", TableIndex = 39 }, new Argument { Name = "nSkillLevel", Type = "int" } } }},
            { 51, new Function { Name = "learnskill", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "skill", Type = "index", TableIndex = 41 } } }},
            { 52, new Function { Name = "getStatOwnerDivBySkillVar", ReturnType = "void", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 }, new Argument { Name = "nVar", Type = "int" } } }},
            { 53, new Function { Name = "getStatOwnerDivBy", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stat", Type = "index", TableIndex = 27 }, new Argument { Name = "nDivBy", Type = "int" } } }},
            { 54, new Function { Name = "switchUnitAndObject", ReturnType = "void" }},
            { 55, new Function { Name = "getAchievementCompleteCount", ReturnType = "void", Args = new[] { new Argument { Name = "nAchievementID", Type = "index", TableIndex = 180 } } }},
            { 56, new Function { Name = "getVarRange", ReturnType = "void" }},
            { 57, new Function { Name = "getVar", ReturnType = "void", Args = new[] { new Argument { Name = "nVariable", Type = "int" } } }},
            { 58, new Function { Name = "getAttackerSkillVarBySkill", ReturnType = "void", Args = new[] { new Argument { Name = "nSkill", Type = "index", TableIndex = 41 }, new Argument { Name = "nVariable", Type = "int" } } }},
            { 59, new Function { Name = "getVarFromSkill", ReturnType = "void", Args = new[] { new Argument { Name = "nSkill", Type = "index", TableIndex = 41 }, new Argument { Name = "nVariable", Type = "int" } } }},
            { 60, new Function { Name = "getVarFromSkillFromObject", ReturnType = "void", Args = new[] { new Argument { Name = "nSkill", Type = "index", TableIndex = 41 }, new Argument { Name = "nVariable", Type = "int" } } }},
            { 61, new Function { Name = "hasStateObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 62, new Function { Name = "hasState", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 63, new Function { Name = "clearStateObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 64, new Function { Name = "clearState", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 65, new Function { Name = "clearStateClient", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 66, new Function { Name = "isDualWielding", ReturnType = "void" }},
            { 67, new Function { Name = "getWieldingIsACount", ReturnType = "void", Args = new[] { new Argument { Name = "unittype", Type = "index", TableIndex = 23 } } }},
            { 68, new Function { Name = "setState", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 69, new Function { Name = "setStateObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 } } }},
            { 70, new Function { Name = "setStateWithTimeMS", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 71, new Function { Name = "addStateWithTimeMS", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 72, new Function { Name = "addStateWithTimeMSClient", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 73, new Function { Name = "setStateWithTimeMSOnObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 74, new Function { Name = "setStateWithTimeMSScriptOnObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 75, new Function { Name = "BroadcastEquipEvent", ReturnType = "void" }},
            { 76, new Function { Name = "setAITargetToSkillTarget", ReturnType = "void" }},
            { 77, new Function { Name = "setObjectAITargetToUnitAITarget", ReturnType = "void" }},
            { 78, new Function { Name = "makeAIAwareOfObject", ReturnType = "void" }},
            { 79, new Function { Name = "setAITargetToObject", ReturnType = "void" }},
            { 80, new Function { Name = "hasSkillTarget", ReturnType = "void" }},
            { 81, new Function { Name = "setStateOnSkillTargetWithTimeMSScript", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" }, new Argument { Name = "clearFirst", Type = "int" } } }},
            { 82, new Function { Name = "runScriptParamOnStateClear", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "paramIndex", Type = "int" } } }},
            { 83, new Function { Name = "getCountOfUnitsInArea", ReturnType = "void", Args = new[] { new Argument { Name = "area", Type = "int" } } }},
            { 84, new Function { Name = "runScriptOnUnitsInAreaPCT", ReturnType = "void", Args = new[] { new Argument { Name = "scriptIndex", Type = "int" }, new Argument { Name = "area", Type = "int" }, new Argument { Name = "chance", Type = "int" }, new Argument { Name = "flag", Type = "int" } } }},
            { 85, new Function { Name = "doSkillAndScriptOnUnitsInAreaPCT", ReturnType = "void", Args = new[] { new Argument { Name = "nSkill", Type = "index", TableIndex = 41 }, new Argument { Name = "scriptIndex", Type = "int" }, new Argument { Name = "area", Type = "int" }, new Argument { Name = "chance", Type = "int" }, new Argument { Name = "flag", Type = "int" } } }},
            { 86, new Function { Name = "doSkillOnUnitsInAreaPCT", ReturnType = "void", Args = new[] { new Argument { Name = "nSkill", Type = "index", TableIndex = 41 }, new Argument { Name = "area", Type = "int" }, new Argument { Name = "chance", Type = "int" }, new Argument { Name = "flag", Type = "int" } } }},
            { 87, new Function { Name = "setStateWithTimeMSScript", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 88, new Function { Name = "setStateWithTimeMSScriptParam", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" }, new Argument { Name = "paramIndex", Type = "int" } } }},
            { 89, new Function { Name = "setStateWithTimeMSScriptParamObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" }, new Argument { Name = "paramIndex", Type = "int" } } }},
            { 90, new Function { Name = "addStateWithTimeMSScriptParamObject", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" }, new Argument { Name = "paramIndex", Type = "int" } } }},
            { 91, new Function { Name = "addStateWithTimeMSScript", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" } } }},
            { 92, new Function { Name = "addStateWithTimeMSScriptParam", ReturnType = "void", Args = new[] { new Argument { Name = "nState", Type = "index", TableIndex = 73 }, new Argument { Name = "timerMS", Type = "int" }, new Argument { Name = "paramIndex", Type = "int" } } }},
            { 93, new Function { Name = "setDmgEffect", ReturnType = "void", Args = new[] { new Argument { Name = "nDmgEffect", Type = "index", TableIndex = 31 }, new Argument { Name = "nChance", Type = "int" }, new Argument { Name = "nTime", Type = "int" }, new Argument { Name = "nRoll", Type = "int" } } }},
            { 94, new Function { Name = "getStatOwner", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 95, new Function { Name = "getStatParent", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 96, new Function { Name = "addPCTStatOnOwner", ReturnType = "void", Args = new[] { new Argument { Name = "nStat", Type = "index", TableIndex = 27 }, new Argument { Name = "nValue", Type = "int" }, new Argument { Name = "nParam", Type = "int" } } }},
            { 97, new Function { Name = "setStatOnOwner", ReturnType = "void", Args = new[] { new Argument { Name = "nStat", Type = "index", TableIndex = 27 }, new Argument { Name = "nValue", Type = "int" }, new Argument { Name = "nParam", Type = "int" } } }},
            { 98, new Function { Name = "total", ReturnType = "unknown4", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 99, new Function { Name = "basetotal", ReturnType = "unknown4", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 100, new Function { Name = "basestat", ReturnType = "unknown4", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 101, new Function { Name = "getcur", ReturnType = "unknown4", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 }, new Argument { Name = "param", Type = "type5" } } }},
            { 102, new Function { Name = "statidx", ReturnType = "index", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 103, new Function { Name = "invcount", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "location", Type = "index", TableIndex = 24 } } }},
            { 104, new Function { Name = "dmgrider", ReturnType = "unknown4" }},
            { 105, new Function { Name = "procrider", ReturnType = "unknown4" }},
            { 106, new Function { Name = "knockback", ReturnType = "void", Args = new[] { new Argument { Name = "a", Type = "int" } } }},
            { 107, new Function { Name = "colorcoderequirement", ReturnType = "void", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 }, new Argument { Name = "param", Type = "type5" } } }},
            { 108, new Function { Name = "color_code_modunit_requirement", ReturnType = "void", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 }, new Argument { Name = "param", Type = "type5" } } }},
            { 109, new Function { Name = "color_code_modunit_requirement2", ReturnType = "void", Args = new[] { new Argument { Name = "stat1", Type = "index", TableIndex = 27 }, new Argument { Name = "param1", Type = "type5" }, new Argument { Name = "stat2", Type = "index", TableIndex = 27 }, new Argument { Name = "param2", Type = "type5" } } }},
            { 110, new Function { Name = "feedchange", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 111, new Function { Name = "feedcolorcode", ReturnType = "void", Args = new[] { new Argument { Name = "stat", Type = "index", TableIndex = 27 } } }},
            { 112, new Function { Name = "colorposneg", ReturnType = "void" }},
            { 113, new Function { Name = "colorcodeprice", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 114, new Function { Name = "colorcodeclassreq", ReturnType = "void" }},
            { 115, new Function { Name = "colorcodeskillslots", ReturnType = "void" }},
            { 116, new Function { Name = "colorcodeskillusable", ReturnType = "void" }},
            { 117, new Function { Name = "colorcodeskillgroupusable", ReturnType = "void" }},
            { 118, new Function { Name = "meetsclassreqs", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 119, new Function { Name = "fontcolorrow", ReturnType = "void", Args = new[] { new Argument { Name = "nColorIndex", Type = "index", TableIndex = 7 } } }},
            { 120, new Function { Name = "nodrop", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 121, new Function { Name = "notrade", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 122, new Function { Name = "BuyPriceByValue", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "valueType", Type = "int" } } }},
            { 123, new Function { Name = "SellPriceByValue", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "valueType", Type = "int" } } }},
            { 124, new Function { Name = "buyprice", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 125, new Function { Name = "buypriceRealWorld", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 126, new Function { Name = "sellprice", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 127, new Function { Name = "sellpriceRealWorld", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 128, new Function { Name = "hitChance", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 129, new Function { Name = "dodgeChance", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 130, new Function { Name = "numaffixes", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 131, new Function { Name = "qualitypricemult", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 132, new Function { Name = "enemies_in_radius", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 133, new Function { Name = "visible_enemies_in_radius", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 134, new Function { Name = "champions_in_radius", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 135, new Function { Name = "distance_sq_to_champion", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 136, new Function { Name = "champion_hp_pct", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 137, new Function { Name = "bosses_in_radius", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 138, new Function { Name = "distance_sq_to_boss", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 139, new Function { Name = "boss_hp_pct", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 140, new Function { Name = "enemy_corpses_in_radius", ReturnType = "int", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "radius", Type = "int" } } }},
            { 141, new Function { Name = "monsters_killed", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "time", Type = "int" } } }},
            { 142, new Function { Name = "monsters_killed_nonteam", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "time", Type = "int" } } }},
            { 143, new Function { Name = "monsters_pct_left", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 144, new Function { Name = "hp_lost", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "time", Type = "int" } } }},
            { 145, new Function { Name = "meters_moved", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "time", Type = "int" } } }},
            { 146, new Function { Name = "attacks", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "time", Type = "int" } } }},
            { 147, new Function { Name = "is_alive", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 148, new Function { Name = "monster_level", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 149, new Function { Name = "has_active_task", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 150, new Function { Name = "is_usable", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 151, new Function { Name = "is_examinable", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 152, new Function { Name = "is_operatable", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 153, new Function { Name = "isa", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "unittype", Type = "index", TableIndex = 23 } } }},
            { 154, new Function { Name = "is_subscriber", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 155, new Function { Name = "is_hardcore", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 156, new Function { Name = "is_elite", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 157, new Function { Name = "get_difficulty", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 158, new Function { Name = "get_act", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 159, new Function { Name = "email_send_item_okay", ReturnType = "type6", Args = new[] { new Argument { Name = "", Type = "void" } } }},
            { 160, new Function { Name = "email_receive_item_okay", ReturnType = "type6", Args = new[] { new Argument { Name = "", Type = "void" } } }},
            { 161, new Function { Name = "colorcodesubscriber", ReturnType = "void" }},
            { 162, new Function { Name = "item_requires_subscriber", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 163, new Function { Name = "colorcodenightmare", ReturnType = "void" }},
            { 164, new Function { Name = "item_is_nightmare_specific", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 165, new Function { Name = "quality", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 166, new Function { Name = "meetsitemreqs", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 167, new Function { Name = "weapondps", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 168, new Function { Name = "SkillTargetIsA", ReturnType = "void", Args = new[] { new Argument { Name = "unittype", Type = "index", TableIndex = 23 } } }},
            { 169, new Function { Name = "GetObjectIsA", ReturnType = "void", Args = new[] { new Argument { Name = "unittype", Type = "index", TableIndex = 23 } } }},
            { 170, new Function { Name = "GetMissileSourceIsA", ReturnType = "void", Args = new[] { new Argument { Name = "unittype", Type = "index", TableIndex = 23 } } }},
            { 171, new Function { Name = "GetSkillHasReqWeapon", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 172, new Function { Name = "has_use_skill", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 173, new Function { Name = "hasdomname", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 174, new Function { Name = "dps", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "time", Type = "int" } } }},
            { 175, new Function { Name = "ObjectCanUpgrade", ReturnType = "void" }},
            { 176, new Function { Name = "use_state_duration", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 177, new Function { Name = "uses_missiles", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 178, new Function { Name = "uses_lasers", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 179, new Function { Name = "has_damage_radius", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 180, new Function { Name = "missile_count", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 181, new Function { Name = "laser_count", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 182, new Function { Name = "shots_per_minute", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 183, new Function { Name = "milliseconds_per_shot", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 184, new Function { Name = "player_crit_chance", ReturnType = "type6", Args = new[] { new Argument { Name = "pUnit", Type = "type6" }, new Argument { Name = "nSlot", Type = "int" } } }},
            { 185, new Function { Name = "player_crit_damage", ReturnType = "type6", Args = new[] { new Argument { Name = "pUnit", Type = "type6" }, new Argument { Name = "nSlot", Type = "int" } } }},
            { 186, new Function { Name = "add_item_level_armor", ReturnType = "void", Args = new[] { new Argument { Name = "nLevel", Type = "int" }, new Argument { Name = "nPercent", Type = "int" } } }},
            { 187, new Function { Name = "player_level_skill_power_cost_percent", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 188, new Function { Name = "item_level_damage_mult", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 189, new Function { Name = "item_level_feed", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 190, new Function { Name = "item_level_sfx_attack", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 191, new Function { Name = "item_level_sfx_defense", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 192, new Function { Name = "item_level_shield_buffer", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 193, new Function { Name = "monster_level_sfx_defense", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 194, new Function { Name = "monster_level_sfx_attack", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 195, new Function { Name = "monster_level_damage", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 196, new Function { Name = "monster_level_damage_pct", ReturnType = "int", Args = new[] { new Argument { Name = "nLevel", Type = "int" }, new Argument { Name = "nPCT", Type = "int" } } }},
            { 197, new Function { Name = "monster_level_shields", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 198, new Function { Name = "monster_level_armor", ReturnType = "int", Args = new[] { new Argument { Name = "level", Type = "int" } } }},
            { 199, new Function { Name = "unit_ai_changer_attack", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 200, new Function { Name = "does_field_damage", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 201, new Function { Name = "distance_to_player", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 202, new Function { Name = "has_container", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 203, new Function { Name = "monster_armor", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "nDamageType", Type = "index", TableIndex = 30 }, new Argument { Name = "nPercent", Type = "int" } } }},
            { 204, new Function { Name = "getSkillDmgMult", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillLvl", Type = "int" }, new Argument { Name = "a", Type = "int" } } }},
            { 205, new Function { Name = "getSkillArmorMult", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillLvl", Type = "int" }, new Argument { Name = "a", Type = "int" } } }},
            { 206, new Function { Name = "getSkillAttackSpeedMult", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillLvl", Type = "int" }, new Argument { Name = "a", Type = "int" } } }},
            { 207, new Function { Name = "getSkillToHitMult", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillLvl", Type = "int" }, new Argument { Name = "a", Type = "int" } } }},
            { 208, new Function { Name = "getSkillPctDmgMult", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillLvl", Type = "int" }, new Argument { Name = "a", Type = "int" } } }},
            { 209, new Function { Name = "getPetCountOfType", ReturnType = "void", Args = new[] { new Argument { Name = "nUnitType", Type = "index", TableIndex = 23 } } }},
            { 210, new Function { Name = "runScriptOnPetsOfType", ReturnType = "void", Args = new[] { new Argument { Name = "nUnitType", Type = "index", TableIndex = 23 }, new Argument { Name = "a", Type = "int" } } }},
            { 211, new Function { Name = "randaffixtype", ReturnType = "void", Args = new[] { new Argument { Name = "affixType", Type = "index", TableIndex = 50 } } }},
            { 212, new Function { Name = "randaffixgroup", ReturnType = "void", Args = new[] { new Argument { Name = "affixGroup", Type = "affixGroup" } } }},
            { 213, new Function { Name = "applyaffix", ReturnType = "void", Args = new[] { new Argument { Name = "affix", Type = "index", TableIndex = 52 }, new Argument { Name = "bForce", Type = "int" } } }},
            { 214, new Function { Name = "getBonusValue", ReturnType = "void", Args = new[] { new Argument { Name = "a", Type = "int" } } }},
            { 215, new Function { Name = "getBonusAll", ReturnType = "void" }},
            { 216, new Function { Name = "getDMGAugmentation", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillVar", Type = "int" }, new Argument { Name = "nLevel", Type = "int" }, new Argument { Name = "nPercentOfLevel", Type = "int" }, new Argument { Name = "nSkillPointsInvested", Type = "int" } } }},
            { 217, new Function { Name = "getDMGAugmentationPCT", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillVar", Type = "int" }, new Argument { Name = "nLevel", Type = "int" }, new Argument { Name = "nPercentOfLevel", Type = "int" }, new Argument { Name = "nSkillPointsInvested", Type = "int" } } }},
            { 218, new Function { Name = "getMonsterHPAtLevel", ReturnType = "void", Args = new[] { new Argument { Name = "nLevel", Type = "int" } } }},
            { 219, new Function { Name = "getMonsterHPAtLevelByPCT", ReturnType = "void", Args = new[] { new Argument { Name = "nLevel", Type = "int" }, new Argument { Name = "nPCT", Type = "int" } } }},
            { 220, new Function { Name = "display_dmg_absorbed_pct", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 221, new Function { Name = "dmg_percent_by_energy", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 222, new Function { Name = "weapon_range", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 223, new Function { Name = "IsObjectDestructable", ReturnType = "void" }},
            { 224, new Function { Name = "GlobalThemeIsEnabled", ReturnType = "void", Args = new[] { new Argument { Name = "nTheme", Type = "index", TableIndex = 167 } } }},
            { 225, new Function { Name = "SetRespawnPlayer", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 226, new Function { Name = "AddSecondaryRespawnPlayer", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 227, new Function { Name = "RemoveHPAndCheckForDeath", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "nRemove", Type = "int" } } }},
            { 228, new Function { Name = "getSkillStat", ReturnType = "void", Args = new[] { new Argument { Name = "nSkillStat", Type = "index", TableIndex = 44 }, new Argument { Name = "nSkillLvl", Type = "int" } } }},
            { 229, new Function { Name = "TownPortalIsAllowed", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" } } }},
            { 230, new Function { Name = "lowerManaCostOnSkillByPct", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 }, new Argument { Name = "nPctPower", Type = "int" } } }},
            { 231, new Function { Name = "lowerCoolDownOnSkillByPct", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 }, new Argument { Name = "nPctCooldown", Type = "int" } } }},
            { 232, new Function { Name = "skillIsOn", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 233, new Function { Name = "getSkillRange", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 234, new Function { Name = "setDmgEffectParams", ReturnType = "void", Args = new[] { new Argument { Name = "nDmgEffect", Type = "index", TableIndex = 31 }, new Argument { Name = "nParam0", Type = "int" }, new Argument { Name = "nParam1", Type = "int" }, new Argument { Name = "nParam2", Type = "int" } } }},
            { 235, new Function { Name = "setDmgEffectSkill", ReturnType = "void", Args = new[] { new Argument { Name = "nDmgEffect", Type = "index", TableIndex = 31 }, new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 236, new Function { Name = "setDmgEffectSkillOnTarget", ReturnType = "void", Args = new[] { new Argument { Name = "nDmgEffect", Type = "index", TableIndex = 31 }, new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 237, new Function { Name = "getSkillID", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 238, new Function { Name = "fireMissileFromObject", ReturnType = "void", Args = new[] { new Argument { Name = "missileID", Type = "index", TableIndex = 110 } } }},
            { 239, new Function { Name = "caculateGemSockets", ReturnType = "void" }},
            { 240, new Function { Name = "caculateRareGemSockets", ReturnType = "void" }},
            { 241, new Function { Name = "caculateCraftingSlots", ReturnType = "void" }},
            { 242, new Function { Name = "executeSkill", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 243, new Function { Name = "executeSkillOnObject", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 244, new Function { Name = "stopSkill", ReturnType = "void", Args = new[] { new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 245, new Function { Name = "powercost", ReturnType = "type6", Args = new[] { new Argument { Name = "unit", Type = "type6" }, new Argument { Name = "skillID", Type = "index", TableIndex = 41 } } }},
            { 246, new Function { Name = "is_stash_ui_open", ReturnType = "" }},
            { 247, new Function { Name = "setRecipeLearned", ReturnType = "void" }},
            { 248, new Function { Name = "getRecipeLearned", ReturnType = "void" }},
            { 249, new Function { Name = "createRecipe", ReturnType = "void" }},
            { 250, new Function { Name = "createSpecificRecipe", ReturnType = "void", Args = new[] { new Argument { Name = "nRecipeID", Type = "index", TableIndex = 108 } } }},
            { 251, new Function { Name = "getCurrentGameTick", ReturnType = "void" }},
        };

        /* OpCode           Function                                Action
         * 0    0x00        Return                                  Must(?) be at end of script.
         * 
         * 3    0x03        Call Function                           Calls a client function.
         * e.g.
         *      26,0, 3,185, 0                                      -> return player_crit_damage(0); // 0 = right hand
         *      
         * 
         * 26   0x1A        Push                                    Pushes a number onto the stack.
         * e.g.
         *      26,1000, 0                                          -> return 1000;
         * 
         * 
         * 358  0x166       Multiplication                          Product of the two most recently pushed numbers.
         * e.g.
         *      26,10, 26,20, 358, 0                                -> return 10 * 20; // = 200
         *      
         * 
         * 369  0x171       Division                                Divition of the two most recently pushed numbers.
         * e.g.
         *      26, 200, 26, 5, 369, 0                              -> return 200 / 5; // = 40
         *      
         * 
         * 388  0x184       Addition                                Sum of the two most recently pushed numbers.
         * e.g.
         *      26,5, 26,10, 388, 26,100, 358, 0                    -> return (5 + 10) * 100; // = 150
         *      
         * 
         * 399  0x18F       Substraction                            Difference of the two most recently pushed numbers.
         * e.g.
         *      26,13, 26,42, 388, 26,99, 358, 26,4108, 399, 0      -> return (13 + 42) * 99 - 4108; // = 1337
         *      
         * 
         * 666  0x29A       GetStatValue                            Gets the value of a stat at the following supplied index (from STATS table).
         * e.g.
         *      666, 612368384, 0                                   -> return GetStatValue(612368384); // = GetStatValue(612368384 << 22 = 146) = GetStatValue("experience");
         *      
         * 
         * 669  0x29D       SetStatValue                            Sets the value of a stat.
         * e.g.
         *      26,10, 669,25165824, 0                              -> return SetStatValue(25165824); // = SetStatValue(25165824 << 22 = 6) = SetStatValue("hp_cur");
         *      
         * 
         * 
         */

        private enum ExcelOpCodes : uint
        {
            Return = 0,         // 0x00
            Call = 3,           // 0x03
            Push = 26,          // 0x1A
            Unknown339 = 339,   // 0x153
            Mult = 358,         // 0x166
            Div = 369,          // 0x171
            Add = 388,          // 0x184
            Sub = 399,          // 0x18F
            Unknown437 = 437,   // 0x1B5
            GetStat = 666,      // 0x29A
            SetStat = 669,      // 0x29D
            Modifier707 = 707,  // 0x2C3
            Modifier708 = 708,  // 0x2C4    // String?
            Modifier709 = 709,  // 0x2C5    // Color?
            Modifier711 = 711   // 0x2C7
        }
        #endregion

        public static String Decompile(byte[] scriptBytes, int offset)
        {
            String script = String.Empty;
            Stack<String> stack = new Stack<String>();

            int infCheck = 0;
            while (true)
            {
                ExcelOpCodes opCode = (ExcelOpCodes)FileTools.ByteArrayToUInt32(scriptBytes, ref offset);
                String value1;
                String value2;
                int index;

                switch (opCode)
                {
                    case ExcelOpCodes.Return:               // 0    0x00
                        if (stack.Count == 1)
                        {
                            script += stack.Pop();
                        }
                        Debug.Assert(stack.Count == 0);

                        return script;

                    case ExcelOpCodes.Call:                 // 3    0x03
                        int functionIndex = FileTools.ByteArrayToInt32(scriptBytes, ref offset);

                        Function excelScriptFunction;
                        if (!ClientFunctions.TryGetValue(functionIndex, out excelScriptFunction))
                        {
                            throw new Exceptions.ExcelScriptUnknownFunctionCall("Unknown function index: " + functionIndex);
                        }

                        _CheckStack(stack, excelScriptFunction.ArgCount+1, ExcelOpCodes.Call); // +1 for modifier/return type
                        String argString = String.Empty;

                        // todo: is this backwards? doesn't really matter much I guess
                        for (int i = 0; i < excelScriptFunction.ArgCount; i++)
                        {
                            argString += stack.Pop();
                            if (i < excelScriptFunction.ArgCount - 1) argString += ", ";
                        }

                        String functionCallString = String.Format("{0}{1}({2})", stack.Pop(), excelScriptFunction.Name, argString);

                        //if (stack.Count == 0)
                        //{
                        //    script += functionCallString + ";";
                        //}
                        //else
                        //{
                            stack.Push(functionCallString);
                        //}
                        break;

                    case ExcelOpCodes.Push:                 // 26   0x1A
                        int value = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                        stack.Push(value.ToString());
                        break;

                    case ExcelOpCodes.Unknown339:           // 339  0x153
                        // unknown
                        break;

                    case ExcelOpCodes.Mult:                 // 358  0x166
                        _CheckStack(stack, 2, opCode);

                        value2 = stack.Pop();
                        value1 = stack.Pop();
                        stack.Push(String.Format("{0} * {1}", value1, value2));
                        break;

                    case ExcelOpCodes.Div:                  // 369  0x171
                        _CheckStack(stack, 2, opCode);

                        value2 = stack.Pop();
                        value1 = stack.Pop();
                        stack.Push(String.Format("{0} / {1}", value1, value2));
                        break;

                    case ExcelOpCodes.Add:                  // 388  0x184
                        _CheckStack(stack, 2, opCode);

                        value2 = stack.Pop();
                        value1 = stack.Pop();
                        stack.Push(String.Format("{0} + {1}", value1, value2));
                        break;

                    case ExcelOpCodes.Sub:                  // 399  0x18F
                        _CheckStack(stack, 2, opCode);

                        value2 = stack.Pop();
                        value1 = stack.Pop();
                        stack.Push(String.Format("{0} - {1}", value1, value2));
                        break;

                    case ExcelOpCodes.Unknown437:           // 437  0x1B5
                        // unknown - "==" or "!=" ???
                        _CheckStack(stack, 2, opCode);

                        value2 = stack.Pop();
                        value1 = stack.Pop();
                        stack.Push(String.Format("{0} == {1}", value1, value2)); // let's try ==
                        break;

                    case ExcelOpCodes.GetStat:              // 666  0x29A
                        index = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                        index >>= 22;

                        script += "GetStat(" + index + ")";
                        break;

                    case ExcelOpCodes.SetStat:              // 669  0x29D
                        _CheckStack(stack, 1, opCode);

                        index = FileTools.ByteArrayToInt32(scriptBytes, ref offset);
                        index >>= 22;

                        script += String.Format("SetStat({0}, {1})", index, stack.Pop());
                        break;

                    case ExcelOpCodes.Modifier707:          // 707  0x2C3
                        //stack.Push("(Modifier707)");
                        break;

                    case ExcelOpCodes.Modifier708:          // 708  0x2C4   // String
                        stack.Push("(String)");
                        break;

                    case ExcelOpCodes.Modifier709:          // 709  0x2C5   // Color
                        stack.Push("(Color)");
                        break;

                    case ExcelOpCodes.Modifier711:          // 711  0x2C7
                        stack.Push("(bool)");
                        break;

                    default:
                        throw new Exceptions.ExcelScriptUnknownOpCode("Unknown OpCode: " + opCode);
                }

                infCheck++;
                if (infCheck >= 1000) throw new Exceptions.ExcelScriptInfiniteCheck("");
            }
        }

        private static void _CheckStack(Stack<String> stack, int requiredCount, ExcelOpCodes opCode)
        {
            if (stack.Count >= requiredCount) return;

            String error = String.Format("The OpCode {0} requires {1} values on the stack.\n{2}", opCode, requiredCount, _DumpStack(stack));
            throw new Exceptions.ExcelScriptInvalidStackState(error);
        }

        private static String _DumpStack(Stack<String> stack)
        {
            String stackDump = "Stack Dump : LIFO\n";

            for (int i = 0; i < stack.Count; i++)
            {
                stackDump += String.Format("{0}\t{1}\n", i, stack.Pop());
            }

            return stackDump;
        }




        //// Get Functions From ASM Stuff ////
        /// 
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

                if (line == 2605)
                {
                    int bp = 0;
                }

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
                        if (asmStr[1].Contains("DoStringCryptHash "))
                        {
                            if (function != null) functions.Add(function);

                            function = new Function { Args = new Argument[5] };
                            argIndex = 0;
                            haveFuncName = true;
                            haveReturnType = false;
                            haveVoidArg = false;

                            function.Name = registers["rcx"];
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_0"))
                        {
                            Debug.Assert(function != null && haveFuncName);

                            if (!haveReturnType)
                            {
                                function.ReturnType = "void";
                                haveReturnType = true;
                            }
                            else
                            {
                                // if we've already go a return type/arg, but now adding void, then the first type we added wasn't also an argument
                                Debug.Assert(argIndex <= 1);
                                if (argIndex == 1)
                                {
                                    Debug.Assert(function.ReturnType == function.Args[0].Type);
                                }

                                Argument argument = new Argument
                                {
                                    Name = "",
                                    Type = "void"
                                };
                                function.Args = new[] { argument };
                                haveVoidArg = true;
                            }
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_1"))
                        {
                            Debug.Assert(function != null && haveFuncName && !haveVoidArg);

                            if (!haveReturnType)
                            {
                                function.ReturnType = "int";
                                haveReturnType = true;
                            }

                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = "int" };
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_2")) // arg type
                        {
                            Debug.Assert(function != null && haveFuncName && !haveVoidArg);

                            if (!haveReturnType)
                            {
                                function.ReturnType = "index";
                                haveReturnType = true;
                            }

                            String strIndex = registers["rdx"];
                            Debug.Assert(strIndex != null);
                            String tableIndex = strIndex.Replace("h", "");
                            function.Args[argIndex++] = new Argument { Name = registers["r8"], Type = "index", TableIndex = Int32.Parse(tableIndex, NumberStyles.HexNumber) };
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_3")) // return type
                        {
                            Debug.Assert(function != null && haveFuncName && !haveReturnType);

                            function.ReturnType = "int";
                            haveReturnType = true;
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_4")) // return type
                        {
                            Debug.Assert(function != null && haveFuncName && !haveReturnType);

                            function.ReturnType = "unknown4";
                            haveReturnType = true;
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_5")) // arg type
                        {
                            Debug.Assert(function != null && haveFuncName && !haveVoidArg && haveReturnType);

                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = "type5" };
                        }
                        else if (asmStr[1].Contains("DoStringCryptHash_6")) // arg type
                        {
                            Debug.Assert(function != null && haveFuncName && !haveVoidArg);

                            if (!haveReturnType)
                            {
                                function.ReturnType = "type6";
                                haveReturnType = true;
                            }

                            function.Args[argIndex++] = new Argument { Name = registers["rdx"], Type = "type6" };
                        }
                        else if (asmStr[1].Contains("sub_1402456F8")) // arg type
                        {
                            Debug.Assert(function != null && haveFuncName && !haveVoidArg && haveReturnType);

                            function.Args[argIndex++] = new Argument { Name = registers["r9"], Type = "affixGroup" };
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

            // {index, new Function { Name = "FunctionName", ReturnType = "Type", Args = new[] { new Argument { Name = "", Type = "" } } }},

            int index = 0;
            foreach (Function func in functions)
            {
                String argsString = String.Empty;
                String[] formattedArgStrings = new String[func.ArgCount];
                int i = 0;
                foreach (Argument arg in func.Args)
                {
                    if (arg == null) break;

                    const String argFormatStringBasic = "new Argument {{ Name = \"{0}\", Type = \"{1}\" }}";
                    const String argFormatStringIndex = "new Argument {{ Name = \"{0}\", Type = \"{1}\", TableIndex = {2} }}";

                    String argFormatString = arg.TableIndex >= 0 ? argFormatStringIndex : argFormatStringBasic;
                    formattedArgStrings[i++] = String.Format(argFormatString, arg.Name, arg.Type, arg.TableIndex);
                }
                String formattedArgString = String.Join(", ", formattedArgStrings, 0, i);
                if (formattedArgString.Length > 0)
                {
                    formattedArgString = ", Args = new[] { " + formattedArgString + " }";
                }

                const String dictionaryFormat = "{{ {0}, new Function {{ Name = \"{1}\", ReturnType = \"{2}\"{3} }}}},";
                String cSharpDicCode = String.Format(dictionaryFormat, index++, func.Name, func.ReturnType, formattedArgString);
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
    }
}
