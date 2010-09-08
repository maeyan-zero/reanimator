using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public abstract class CharacterProperty
    {
        protected UnitWrapper _wrapper;
        protected Unit _hero;
        protected TableDataSet _dataSet;
        protected DataTable _statsTable;

        public CharacterProperty(UnitWrapper heroUnit, TableDataSet dataSet)
        {
            _wrapper = heroUnit;
            _hero = _wrapper.HeroUnit;
            _dataSet = dataSet;

            _statsTable = _dataSet.GetExcelTableFromStringId("STATS");
        }

        public int GetBitCount(string value)
        {
            try
            {
                DataRow[] row = _statsTable.Select("stat = " + value);
                int bitCount = (int)row[0]["valbits"];
                return bitCount;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public int GetBitCount(int code)
        {
            DataRow[] row = _statsTable.Select("code = " + code);
            int bitCount = (int)row[0]["valbits"];
            return bitCount;
        }
    }

    public class UnitWrapper
    {
        Unit _hero;
        string _savePath;
        TableDataSet _dataSet;
        int _unitType;
        bool _isMale;
        CharacterClass _class;
        CharacterGameMode _mode;
        CharacterValues _values;
        CharacterSkills _skills;
        CharacterItems _items;
        CharacterEquippment _equippment;
        CharacterAppearance _appearance;
        EngineerDrone _drone;


        public string SavePath
        {
            get { return _savePath; }
            set { _savePath = value; }
        }

        public bool IsMale
        {
            get { return _isMale; }
            set { _isMale = value; }
        }

        public CharacterClass Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public string ClassName
        {
            get
            {
                string charClass = _class.ToString();
                string[] split = charClass.Split(new string[]{"_"}, StringSplitOptions.RemoveEmptyEntries);
                return split[0];
            }
        }

        public CharacterGameMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public CharacterValues Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public CharacterSkills Skills
        {
            get { return _skills; }
            set { _skills = value; }
        }

        public CharacterItems Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public CharacterEquippment Equippment
        {
            get { return _equippment; }
            set { _equippment = value; }
        }

        public CharacterAppearance Appearance
        {
            get { return _appearance; }
            set { _appearance = value; }
        }

        public EngineerDrone Drone
        {
            get { return _drone; }
            set { _drone = value; }
        }

        public int UnitType
        {
            get
            {
                return _unitType;
            }
        }

        public UnitWrapper(TableDataSet dataSet, Unit heroUnit)
        {
            _hero = heroUnit;
            _dataSet = dataSet;
            DataTable players = _dataSet.GetExcelTableFromStringId("PLAYERS");
            DataRow[] playerRow = players.Select("code = " + _hero.unitCode);
            int playerType = (int)playerRow[0]["unitType"];

            List<int> skillTabs = new List<int>();
                    
            for(int counter = 1; counter < 8; counter++)
            {
                int skillTab = (int)playerRow[0]["SkillTab" + counter];
                if (skillTab >= 0)
                {
                    skillTabs.Add(skillTab);
                }
            }

            _unitType = playerType - 3;
            //the following code also retrieves the unitType from the tables, but is VERY prone to modifications
            //DataTable unitTypes = _dataSet.GetExcelTableFromStringId("UNITTYPES");
            //DataRow[] unitTypeRow = unitTypes.Select("code = " + playerType);
            //_unitType = (int)unitTypeRow[0]["isA0"];
            

            _class = GetCharacterClass(_hero);
            _isMale = _class.ToString().EndsWith("_Male");
            _mode = new CharacterGameMode(this, _dataSet);
            _values = new CharacterValues(this, _dataSet);
            _skills = new CharacterSkills(this, _dataSet, skillTabs.ToArray());
            _items = new CharacterItems(this, _dataSet);
            _equippment = new CharacterEquippment(this, _dataSet);
            _appearance = new CharacterAppearance(this, _dataSet);

            if (_class == CharacterClass.Engineer_Male || _class == CharacterClass.Engineer_Female)
            {
                _drone = new EngineerDrone(this, _dataSet);
            }

            //int level = _values.Level;
            //int palladium = _values.Palladium;
            //int skills = _values.SkillPoints;
            //int attribute = _values.AttributePoints;
        }

        private CharacterClass GetCharacterClass(Unit _hero)
        {
            return (CharacterClass)Enum.Parse(typeof(CharacterClass), _hero.unitCode.ToString());
        }

        public Unit HeroUnit
        {
            get { return _hero; }
        }

        public string Name
        {
            get { return _hero.Name; }
            set { _hero.Name = value; }
        }
    }

    public enum CharacterClass
    {
        Guardian_Male = 31354,
        Guardian_Female = 31098,
        Blademaster_Male = 30842,
        Blademaster_Female = 30586,
        Engineer_Male = 30840,
        Engineer_Female = 30584,
        Marksman_Male = 30328,
        Marksman_Female = 30072,
        Evoker_Male = 30841,
        Evoker_Female = 30585,
        Summoner_Male = 30329,
        Summoner_Female = 30073,
        Drone = 29797
    }

    public enum GameMode
    {
        Elite = 21062,
        Hardcore = 18243,
        HardcoreDead = 18499
    }

    public class CharacterGameMode : CharacterProperty
    {
        public CharacterGameMode(UnitWrapper heroUnit, TableDataSet dataSet) : base(heroUnit, dataSet)
        {
        }

        public bool IsElite
        {
            get
            {
                return _hero.PlayerFlags1.Contains((int)GameMode.Elite) || _hero.PlayerFlags2.Contains((int)GameMode.Elite);
            }
            set
            {
                if (value)
                {
                    if (!_hero.PlayerFlags1.Contains((int)GameMode.Elite))
                    {
                        _hero.PlayerFlags1.Add((int)GameMode.Elite);
                    }
                    if (!_hero.PlayerFlags2.Contains((int)GameMode.Elite))
                    {
                        _hero.PlayerFlags2.Add((int)GameMode.Elite);
                    }
                }
                else
                {
                    _hero.PlayerFlags1.Remove((int)GameMode.Elite);
                    _hero.PlayerFlags2.Remove((int)GameMode.Elite);
                }
            }
        }

        public bool IsHardcore
        {
            get
            {
                return _hero.PlayerFlags1.Contains((int)GameMode.Hardcore) || _hero.PlayerFlags2.Contains((int)GameMode.Hardcore);
            }
            set
            {
                if (value)
                {
                    if (!_hero.PlayerFlags1.Contains((int)GameMode.Hardcore))
                    {
                        _hero.PlayerFlags1.Add((int)GameMode.Hardcore);
                    }
                    if (!_hero.PlayerFlags2.Contains((int)GameMode.Hardcore))
                    {
                        _hero.PlayerFlags2.Add((int)GameMode.Hardcore);
                    }
                }
                else
                {
                    _hero.PlayerFlags1.Remove((int)GameMode.Hardcore);
                    _hero.PlayerFlags2.Remove((int)GameMode.Hardcore);
                }
            }
        }

        public bool IsHardcoreDead
        {
            get
            {
                return _hero.PlayerFlags1.Contains((int)GameMode.HardcoreDead) || _hero.PlayerFlags2.Contains((int)GameMode.HardcoreDead);
            }
            set
            {
                if (value)
                {
                    if (!_hero.PlayerFlags1.Contains((int)GameMode.HardcoreDead))
                    {
                        _hero.PlayerFlags1.Add((int)GameMode.HardcoreDead);
                    }
                    if (!_hero.PlayerFlags2.Contains((int)GameMode.HardcoreDead))
                    {
                        _hero.PlayerFlags2.Add((int)GameMode.HardcoreDead);
                    }
                }
                else
                {
                    _hero.PlayerFlags1.Remove((int)GameMode.HardcoreDead);
                    _hero.PlayerFlags2.Remove((int)GameMode.HardcoreDead);
                }
            }
        }
    }

    public class CharacterValues : CharacterProperty
    {
        public CharacterValues(UnitWrapper heroUnit, TableDataSet dataSet)
            : base(heroUnit, dataSet)
        {
        }

        public int Level
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.level) - GetBitCount((int)ItemValueNames.level);
                //return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.level) - GetBitCount(12336);
            }
            set
            {
                if (value > MaxLevel)
                {
                    value = MaxLevel;
                }
                if(value < 0)
                {
                    value = 0;
                }

                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.level, value + GetBitCount((int)ItemValueNames.level));
            }
        }

        public int MaxLevel
        {
            get
            {
                DataTable playersTable = _dataSet.GetExcelTableFromStringId("PLAYERS");
                DataRow[] playerRows = playersTable.Select("code = " + _hero.unitCode);
                return (int)playerRows[0]["maxLevel"];
            }
        }

        public int Palladium
        {
            get
            {
                int palladium = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.gold);

                //if the character doesn't have palladium on him, there's also no palladium entry, so let's add it
                if(palladium == 0)
                {
                    //maximum Palladium value is 9.999.999 which is 100110001001011001111111 = 24 bit = bitCount
                    UnitHelpFunctions.AddSimpleValue(_hero, ItemValueNames.gold, 0, GetBitCount((int)ItemValueNames.gold));
                }

                return palladium;
            }
            set
            {
                if (value > MaxPalladium)
                {
                    value = MaxPalladium;
                }
                if (value < 0)
                {
                    value = 0;
                }

                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.gold, value);
            }
        }

        public int MaxPalladium
        {
            get
            {
                return 9999999;
            }
        }

        public int AttributePoints
        {
            get
            {
                int attributePoints = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.stat_points);

                //if the character doesn't have palladium on him, there's also no palladium entry, so let's add it
                if (attributePoints == 0)
                {
                    //bitCount = 10 taken from other saves => max value = 1023
                    UnitHelpFunctions.AddSimpleValue(_hero, ItemValueNames.stat_points, 0, GetBitCount((int)ItemValueNames.stat_points));
                }

                return attributePoints;
            }
            set
            {
                if (value > MaxAttributePoints)
                {
                    value = MaxAttributePoints;
                }
                if (value < 0)
                {
                    value = 0;
                }

                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.stat_points, value);
            }
        }

        public int MaxAttributePoints
        {
            get
            {
                return 1000;
            }
        }

        public int SkillPoints
        {
            get
            {
                int skillPoints = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.skill_points);

                //if the character doesn't have palladium on him, there's also no palladium entry, so let's add it
                if (skillPoints == 0)
                {
                    //bitCount = 12 taken from other saves => max value = 4095
                    UnitHelpFunctions.AddSimpleValue(_hero, ItemValueNames.skill_points, 0, GetBitCount((int)ItemValueNames.skill_points));
                }

                return skillPoints;
            }
            set
            {
                if (value > MaxSkillPoints)
                {
                    value = MaxSkillPoints;
                }
                if (value < 0)
                {
                    value = 0;
                }

                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.skill_points, value);
            }
        }

        public int MaxSkillPoints
        {
            get
            {
                return 4000;
            }
        }

        public int Accuracy
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.accuracy);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.accuracy, value);
            }
        }

        public int Stamina
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.stamina);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.stamina, value);
            }
        }

        public int Strength
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.strength);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.strength, value);
            }
        }

        public int Willpower
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.willpower);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.willpower, value);
            }
        }

        public int PlayTime
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.played_time_in_seconds);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.played_time_in_seconds, value);
            }
        }

        public int AchievementPointsCur
        {
            get
            {
                int achievementPointsCur = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.achievement_points_cur);

                if (achievementPointsCur == 0)
                {
                    //bitCount = 12 taken from other saves => max value = 4095
                    UnitHelpFunctions.AddSimpleValue(_hero, ItemValueNames.skill_points, 0, GetBitCount((int)ItemValueNames.achievement_points_cur));
                }

                return achievementPointsCur;
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.achievement_points_cur, value);
            }
        }

        public int AchievementPointsTotal
        {
            get
            {
                int achievementPointsTotal = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.achievement_points_total);

                if (achievementPointsTotal == 0)
                {
                    //bitCount = 12 taken from other saves => max value = 4095
                    UnitHelpFunctions.AddSimpleValue(_hero, ItemValueNames.skill_points, 0, GetBitCount((int)ItemValueNames.achievement_points_total));
                }

                return achievementPointsTotal;
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.achievement_points_cur, value);
            }
        }

        public int Experience
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.experience);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.experience, value);
            }
        }

        public int Experience_Prev
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.experience_prev);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.experience_prev, value);
            }
        }

        public int Experience_Next
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.experience_next);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.experience_next, value);
            }
        }
    }

    public class CharacterSkills : CharacterProperty
    {
        List<SkillTab> _skillTabs;
        SkillTab _generalSkills;

        public SkillTab GeneralSkills
        {
            get { return _generalSkills; }
        }

        public List<SkillTab> SkillTabs
        {
            get { return _skillTabs; }
        }

        public CharacterSkills(UnitWrapper heroUnit, TableDataSet dataSet, int[] skillTabs)
            : base(heroUnit, dataSet)
        {
            _skillTabs = new List<SkillTab>();

            //to make things easier, let's add all available character skills to the list
            List<Unit.StatBlock.Stat.Values> availableSkills = new List<Unit.StatBlock.Stat.Values>();
            ////get the skills the character already knows
            Unit.StatBlock.Stat skills = UnitHelpFunctions.GetComplexValue(_hero, ItemValueNames.skill_level);
            ////add them to the complete skill list
            availableSkills.AddRange(skills.values);

            DataTable skillTable = dataSet.GetExcelTableFromStringId("SKILLS");

            //let's add all the skills the character doesn't know yet
            foreach (int skillTab in skillTabs)
            {
                DataRow[] skillRows = skillTable.Select("skillTab = " + skillTab);

                SkillTab skillsInSkillTab = CreateSkillsFromRow(availableSkills, skillTable, skillRows);

                if (skillsInSkillTab.Skills.Count > 0)
                {
                    _skillTabs.Add(skillsInSkillTab);
                }
            }


            // select the general skill tab
            DataRow[] generalSkillRows = skillTable.Select("skillTab = " + 0);
            _generalSkills = CreateSkillsFromRow(availableSkills, skillTable, generalSkillRows);

            //add all skills back to the savegame
            availableSkills.Clear();

            foreach (Skill skill in _generalSkills.Skills)
            {
                availableSkills.Add(skill.SkillBlock);
            }

            foreach (SkillTab skillTab in _skillTabs)
            {
                foreach (Skill skill in skillTab.Skills)
                {
                    availableSkills.Add(skill.SkillBlock);
                }
            }

            skills.repeatCount = availableSkills.Count;
            skills.values = availableSkills.ToArray();
        }

        private SkillTab CreateSkillsFromRow(List<Unit.StatBlock.Stat.Values> availableSkills, DataTable skillTable, DataRow[] skillRows)
        {
            List<Unit.StatBlock.Stat.Values> values = new List<Unit.StatBlock.Stat.Values>();
            SkillTab skillInSkillTab = new SkillTab();

            //iterate through all available skills
            foreach (DataRow row in skillRows)
            {
                //get the skill id
                int skillId = (int)row["code"];
                //if the skill is already present, use that one
                Unit.StatBlock.Stat.Values tmpSkill = availableSkills.Find(tmp => tmp.Attribute1 == skillId);
                if (tmpSkill != null)
                {
                    values.Add(tmpSkill);
                }
                //if not, add a new one
                else
                {
                    Unit.StatBlock.Stat.Values skillEntry = new Unit.StatBlock.Stat.Values();
                    skillEntry.Attribute1 = skillId;

                    values.Add(skillEntry);
                }
            }
            //_hero.Stats.statCount

            //and finally... initialize all skills :)
            foreach (Unit.StatBlock.Stat.Values skillBlock in values)
            {
                Skill skill = InitializeSkill(skillTable, skillBlock);
                skillInSkillTab.Skills.Add(skill);
            }
            return skillInSkillTab;
        }

        private Skill InitializeSkill(DataTable table, Unit.StatBlock.Stat.Values skillBlock)
        {
            DataRow[] availableSkillRows = table.Select("code = " + skillBlock.Attribute1);

            string name = (string)availableSkillRows[0]["displayName_string"];
            string description = (string)availableSkillRows[0]["descriptionString_string"];
            string iconName = (string)availableSkillRows[0]["smallIcon"];
            int maxLevel = (int)availableSkillRows[0]["maxLevel"];
            int row = (int)availableSkillRows[0]["skillPageRow"];
            int column = (int)availableSkillRows[0]["skillPageColumn"];

            List<int> requiredSkills = new List<int>();
            List<int> levelsOfRequiredSkills = new List<int>();

            for(int counter = 1; counter < 5; counter++)
            {
                int requiredSkill = (int)availableSkillRows[0]["requiredSkills" + counter];
                if (requiredSkill >= 0)
                {
                    requiredSkills.Add(requiredSkill);
                }
                int requiredLevel = (int)availableSkillRows[0]["levelsOfrequiredSkills" + counter];
                if (requiredLevel >= 0)
                {
                    levelsOfRequiredSkills.Add(requiredLevel);
                }
            }
            return new Skill(name, description, iconName, maxLevel, new Point(column, row), requiredSkills.ToArray(), levelsOfRequiredSkills.ToArray(), skillBlock);
        }
    }

    public class CharacterItems : CharacterProperty
    {
        public CharacterItems(UnitWrapper heroUnit, TableDataSet dataSet)
            : base(heroUnit, dataSet)
        {
        }

        public List<Unit> Items
        {
            get
            {
                return _hero.Items;
            }
            set
            {
                _hero.Items = value;
            }
        }

        public int Quantity
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.item_quantity);
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if (value > MaxQuantity)
                {
                    value = MaxQuantity;
                }

                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.item_quantity, value);
            }
        }

        public int MaxQuantity
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.item_quantity_max);
            }
        }

        public InventoryTypes InventoryType
        {
            get
            {
                return (InventoryTypes)Enum.Parse(typeof(InventoryTypes), _hero.unitCode.ToString());
            }
            set
            {
                _hero.inventoryType = (int)value;
            }
        }

        public Point InventoryPosition
        {
            get
            {
                return new Point(_hero.inventoryPositionX, _hero.inventoryPositionY);
            }
            set
            {
                _hero.inventoryPositionX = value.X;
                _hero.inventoryPositionY = value.Y;
            }
        }

        public Size InventorySize
        {
            get
            {
                int width = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.inventory_width);
                int height =  UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.inventory_height);

                if (width < 1)
                {
                    width = 1;
                }
                if (height < 1)
                {
                    height = 1;
                }

                return new Size(width, height);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.inventory_width, value.Width);
                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.inventory_height, value.Height);
            }
        }

        public ItemQuality Quality
        {
            get
            {
                return (ItemQuality)UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.item_quality);
            }
        }

        public List<Unit> GetItemsOfQuality(ItemQuality quality)
        {
            List<Unit> tmp = new List<Unit>();

            foreach (Unit item in _hero.Items)
            {
                if (UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.item_quality) == (int)quality)
                {
                    tmp.Add(item);
                }
            }
            return tmp;
        }

        public List<Unit> GetItemsOfInventoryType(InventoryTypes type)
        {
            List<Unit> tmp = new List<Unit>();

            foreach (Unit item in _hero.Items)
            {
                if (item.inventoryType == (int)type)
                {
                    tmp.Add(item);
                }
            }

            return tmp;
        }

        public int ItemAugmentedCount
        {
            get
            {
                int augmentedCount = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.item_augmented_count);

                if (augmentedCount == 0)
                {
                    //maximum Palladium value is 9.999.999 which is 100110001001011001111111 = 24 bit = bitCount
                    UnitHelpFunctions.AddSimpleValue(_hero, ItemValueNames.item_augmented_count, 0, GetBitCount((int)ItemValueNames.item_augmented_count));
                }

                return augmentedCount;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                //if (value > 10)
                //{
                //    value = 10;
                //}

                UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.item_augmented_count, value);
            }
        }

        public void RemoveItem(Unit item)
        {
            _hero.RemoveItem(item);
        }

        public void AddItem(Unit item)
        {
            _hero.AddItem(item);
        }
    }

    //public class CharacterItemList : CharacterProperty
    //{
    //    public CharacterItemList(UnitWrapper heroUnit, TableDataSet dataSet)
    //        : base(heroUnit, dataSet)
    //    {
    //    }

    //    public List<Unit> Items
    //    {
    //        get
    //        {
    //            return _hero.Items;
    //        }
    //    }

    //    public List<Unit> GetItemsOfInventoryType(InventoryTypes type)
    //    {
    //        List<Unit> tmp = new List<Unit>();

    //        foreach (Unit item in _hero.Items)
    //        {
    //            if (item.inventoryType == (int)type)
    //            {
    //                tmp.Add(item);
    //            }
    //        }

    //        return tmp;
    //    }

    //    public void RemoveItem(Unit item)
    //    {
    //        _hero.Items.Remove(item);
    //    }

    //    public void AddItem(Unit item)
    //    {
    //        _hero.Items.Add(item);
    //    }
    //}

    //public class CharacterItem : CharacterProperty
    //{
    //    public CharacterItem(UnitWrapper heroUnit, TableDataSet dataSet)
    //        : base(heroUnit, dataSet)
    //    {
    //    }

    //    public List<Unit> Items
    //    {
    //        get
    //        {
    //            return _hero.Items;
    //        }
    //        set
    //        {
    //            _hero.Items = value;
    //        }
    //    }

    //    public InventoryTypes InventoryType
    //    {
    //        get
    //        {
    //            return (InventoryTypes)Enum.Parse(typeof(InventoryTypes), _hero.unitCode.ToString());
    //        }
    //        set
    //        {
    //            _hero.inventoryType = (int)value;
    //        }
    //    }

    //    public Point InventoryPosition
    //    {
    //        get
    //        {
    //            return new Point(_hero.inventoryPositionX, _hero.inventoryPositionY);
    //        }
    //        set
    //        {
    //            _hero.inventoryPositionX = value.X;
    //            _hero.inventoryPositionY = value.Y;
    //        }
    //    }

    //    public Size InventorySize
    //    {
    //        get
    //        {
    //            int width = UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.inventory_width);
    //            int height =  UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.inventory_height);

    //            if (width < 1)
    //            {
    //                width = 1;
    //            }
    //            if (height < 1)
    //            {
    //                height = 1;
    //            }

    //            return new Size(width, height);
    //        }
    //        set
    //        {
    //            UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.inventory_width, value.Width);
    //            UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.inventory_height, value.Height);
    //        }
    //    }
    //}

    public class CharacterAppearance : CharacterProperty
    {
        public CharacterAppearance(UnitWrapper heroUnit, TableDataSet dataSet)
            : base(heroUnit, dataSet)
        {
        }

        public Size CharacterSize
        {
            get
            {
                return new Size(_hero.CharacterWidth, _hero.CharacterHeight);
            }
            set
            {
                _hero.CharacterWidth = value.Width;
                _hero.CharacterHeight = value.Height;
            }
        }
    }

    public class CharacterEquippment : CharacterProperty
    {
        public CharacterEquippment(UnitWrapper heroUnit, TableDataSet dataSet)
            : base(heroUnit, dataSet)
        {
        }
    }

    public class EngineerDrone : CharacterProperty
    {
        public EngineerDrone(UnitWrapper heroUnit, TableDataSet dataSet)
            : base(heroUnit, dataSet)
        {
        }

        public Unit Drone
        {
            get
            {
                return _hero.Items.Find(item => item.unitCode == (int)CharacterClass.Drone);
            }
            set
            {
                _hero.Items.Remove(Drone);
                _hero.Items.Add(value);
            }
        }
    }

    public class WeaponSlots : CharacterProperty
    {
        public WeaponSlots(UnitWrapper heroUnit, TableDataSet dataSet)
            : base(heroUnit, dataSet)
        {
        }

        public Unit[] WeaponSlot1
        {
            get
            {
                List<Unit> _weapons = new List<Unit>();

                foreach(Unit item in _hero.Items)
                {
                    if (item.inventoryType == (int)InventoryTypes.CurrentWeaponSet)
                    {
                        _weapons.Add(item);
                    }
                }

                return _weapons.ToArray();
            }
        }

        public Unit[] WeaponSlot2
        {
            get
            {
                List<Unit> _weapons = new List<Unit>();

                foreach (Unit item in _hero.Items)
                {
                    if (item.inventoryType == (int)InventoryTypes.CurrentWeaponSet)
                    {
                        _weapons.Add(item);
                    }
                }

                return _weapons.ToArray();
            }
        }

        public Unit[] WeaponSlot3
        {
            get
            {
                List<Unit> _weapons = new List<Unit>();

                foreach (Unit item in _hero.Items)
                {
                    if (item.inventoryType == (int)InventoryTypes.CurrentWeaponSet)
                    {
                        _weapons.Add(item);
                    }
                }

                return _weapons.ToArray();
            }
        }
    }

    public class SkillTab
    {
        List<Skill> _skills;

        public List<Skill> Skills
        {
            get { return _skills; }
        }

        public SkillTab()
        {
            _skills = new List<Skill>();
        }

        public Skill GetSkillByName(string name)
        {
            return _skills.Find(tmp => tmp.Name == name);
        }
    }

    public class Skill
    {
        Unit.StatBlock.Stat.Values _skillBlock;
        string _name;
        string _description;
        int _maxLevel;
        string _iconName;
        int[] _requiredSkills;
        int[] _levelsOfRequiredSkills;
        Point _position;

        public Unit.StatBlock.Stat.Values SkillBlock
        {
            get { return _skillBlock; }
            set { _skillBlock = value; }
        }

        public int[] RequiredSkills
        {
            get { return _requiredSkills; }
        }

        public int[] LevelsOfRequiredSkills
        {
            get { return _levelsOfRequiredSkills; }
        }

        public int SkillID
        {
            get { return _skillBlock.Attribute1; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int MaxLevel
        {
            get { return _maxLevel; }
        }

        public string IconName
        {
            get { return _iconName; }
        }

        public int CurrentLevel
        {
            get { return _skillBlock.Stat; }
            set
            {
                if (value > _maxLevel)
                {
                    value = _maxLevel;
                }

                _skillBlock.Stat = value;
            }
        }

        public Point Position
        {
            get { return _position; }
        }

        public bool Learned
        {
            get
            {
                return CurrentLevel > 0;
            }
        }

        public Skill(string name, string description, string iconName, int maxLevel, Point position, int[] requiredSkills, int[] levelsOfRequiredSkills, Unit.StatBlock.Stat.Values skillBlock)
        {
            _name = name;
            _description = description;
            _iconName = iconName;
            _maxLevel = maxLevel;
            _position = position;
            _requiredSkills = requiredSkills;
            _levelsOfRequiredSkills = levelsOfRequiredSkills;
            _skillBlock = skillBlock;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
