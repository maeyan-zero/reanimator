using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public abstract class CharacterProperty
    {
        protected UnitObject unitObject;

        protected FileManager fileManager;
        protected DataTable statsTable;

        public CharacterProperty()
        {
        }

        public CharacterProperty(UnitObject unitObject, FileManager fileManager)
        {
            this.unitObject = unitObject;
            this.fileManager = fileManager;

            statsTable = fileManager.GetDataTable("STATS");
        }

        public UnitObject UnitObject
        {
            get { return unitObject; }
            set { unitObject = value; }
        }

        public int GetBitCount(string value)
        {
            try
            {
                DataRow[] row = statsTable.Select("stat = " + value);
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
            DataRow[] row = statsTable.Select("code = " + code);
            int bitCount = (int)row[0]["valbits"];
            return bitCount;
        }
    }

    public class UnitWrapper2
    {
        CharacterFile baseCharacter;
        FileManager fileManager;
        CharacterWrapper characterWrapper;

        public CharacterFile BaseCharacter
        {
            get { return baseCharacter; }
            set { baseCharacter = value; }
        }

        public CharacterWrapper CharacterWrapper
        {
            get { return characterWrapper; }
            private set { characterWrapper = value; }
        }

        public UnitWrapper2(string savegamePath, FileManager fileManager)
        {
            baseCharacter.ParseFileBytes(File.ReadAllBytes(savegamePath));
            this.fileManager = fileManager;

            ParseCharacter(baseCharacter.Character);
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name
        {
            get
            {
                return baseCharacter.Name.Replace("\0", "");
            }
        }

        public string Path
        {
            get
            {
                return baseCharacter.Path;
            }
        }

        private void ParseCharacter(UnitObject unitObject)
        {
            UnitHelpFunctions unitHelp = new UnitHelpFunctions(fileManager);
            unitHelp.LoadCharacterValues(baseCharacter.Character);

            characterWrapper = new CharacterWrapper(baseCharacter.Character, fileManager); 
        }
    }

    public class CharacterWrapper : CharacterProperty
    {
        DataTable itemsTable;
        int unitType;

        EngineerDrone drone;
        CharacterClass characterClass;
        CharacterGameMode characterGameMode;
        CharacterValues characterValues;
        CharacterSkills characterSkills;
        CharacterInventory characterInventory;
        WeaponSlots weaponSlots;
        Gender gender;

        public EngineerDrone Drone
        {
            get { return drone; }
            set { drone = value; }
        }

        public CharacterSkills CharacterSkills
        {
            get { return characterSkills; }
            set { characterSkills = value; }
        }

        public WeaponSlots WeaponSlots
        {
            get { return weaponSlots; }
            set { weaponSlots = value; }
        }

        public CharacterValues CharacterValues
        {
            get { return characterValues; }
            set { characterValues = value; }
        }

        public CharacterGameMode CharacterGameMode
        {
            get { return characterGameMode; }
            set { characterGameMode = value; }
        }

        public CharacterClass CharacterClass
        {
            get { return characterClass; }
            set { characterClass = value; }
        }

        public CharacterInventory CharacterInventory
        {
            get { return characterInventory; }
            set { characterInventory = value; }
        }

        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public CharacterWrapper(UnitObject character, FileManager fileManager)
            : base(character, fileManager)
        {
            itemsTable = fileManager.GetDataTable("ITEMS");

            DataTable players = fileManager.GetDataTable("PLAYERS");
            DataRow[] playerRow = players.Select("code = " + character.UnitCode);

            if (playerRow.Length > 0)
            {
                int playerType = (int)playerRow[0]["unitType"];

                List<int> skillTabs = new List<int>();

                for (int counter = 1; counter < 8; counter++)
                {
                    int skillTab = (int)playerRow[0]["SkillTab" + counter];
                    if (skillTab >= 0)
                    {
                        skillTabs.Add(skillTab);
                    }
                }

                unitType = playerType - 3;

                characterSkills = new CharacterSkills(character, fileManager, skillTabs.ToArray());
            }

            characterClass = GetCharacterClass(character);
            characterGameMode = new CharacterGameMode(character, fileManager);
            characterValues = new CharacterValues(character, fileManager);
            gender = characterClass.ToString().EndsWith("_Male") ? Gender.Male : Gender.Female;
            weaponSlots = new WeaponSlots(character, fileManager);
            characterInventory = new CharacterInventory(character, fileManager);

            if (characterClass == CharacterClass.Engineer_Male || characterClass == CharacterClass.Engineer_Female)
            {
                //drone = new EngineerDrone(character, fileManager);
            }
        }

        private CharacterClass GetCharacterClass(UnitObject character)
        {
            return (CharacterClass)Enum.Parse(typeof(CharacterClass), character.UnitCode.ToString());
        }

        public string ClassName
        {
            get
            {
                string charClass = characterClass.ToString();
                string[] split = charClass.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                return split[0];
            }
        }
    }

    public class CharacterGameMode : CharacterProperty
    {
        public CharacterGameMode(UnitObject unitObject, FileManager fileManager)
            : base(unitObject, fileManager)
        {
        }

        public bool IsElite
        {
            get
            {
                return UnitObject.StateCodes1.Contains((int)GameMode.Elite) || unitObject.StateCodes2.Contains((int)GameMode.Elite);
            }
            set
            {
                if (value)
                {
                    if (!UnitObject.StateCodes1.Contains((int)GameMode.Elite))
                    {
                        UnitObject.StateCodes1.Add((int)GameMode.Elite);
                    }
                    if (!UnitObject.StateCodes2.Contains((int)GameMode.Elite))
                    {
                        UnitObject.StateCodes2.Add((int)GameMode.Elite);
                    }
                }
                else
                {
                    UnitObject.StateCodes1.Remove((int)GameMode.Elite);
                    UnitObject.StateCodes2.Remove((int)GameMode.Elite);
                }
            }
        }

        public bool IsHardcore
        {
            get
            {
                return UnitObject.StateCodes1.Contains((int)GameMode.Hardcore) || UnitObject.StateCodes2.Contains((int)GameMode.Hardcore);
            }
            set
            {
                if (value)
                {
                    if (!UnitObject.StateCodes1.Contains((int)GameMode.Hardcore))
                    {
                        UnitObject.StateCodes1.Add((int)GameMode.Hardcore);
                    }
                    if (!UnitObject.StateCodes2.Contains((int)GameMode.Hardcore))
                    {
                        UnitObject.StateCodes2.Add((int)GameMode.Hardcore);
                    }
                }
                else
                {
                    UnitObject.StateCodes1.Remove((int)GameMode.Hardcore);
                    UnitObject.StateCodes2.Remove((int)GameMode.Hardcore);
                }
            }
        }

        public bool IsHardcoreDead
        {
            get
            {
                return UnitObject.StateCodes1.Contains((int)GameMode.HardcoreDead) || UnitObject.StateCodes2.Contains((int)GameMode.HardcoreDead);
            }
            set
            {
                if (value)
                {
                    if (!UnitObject.StateCodes1.Contains((int)GameMode.HardcoreDead))
                    {
                        UnitObject.StateCodes1.Add((int)GameMode.HardcoreDead);
                    }
                    if (!UnitObject.StateCodes2.Contains((int)GameMode.HardcoreDead))
                    {
                        UnitObject.StateCodes2.Add((int)GameMode.HardcoreDead);
                    }
                }
                else
                {
                    UnitObject.StateCodes1.Remove((int)GameMode.HardcoreDead);
                    UnitObject.StateCodes2.Remove((int)GameMode.HardcoreDead);
                }
            }
        }
    }

    public class CharacterValues : CharacterProperty
    {
        int _maxPalladium;
        int _maxLevel;

        public CharacterValues(UnitObject unitObject, FileManager fileManager)
            : base(unitObject, fileManager)
        {
            try
            {
                //could also use "stat" column and "gold" entry
                DataRow[] goldRow = statsTable.Select("code = " + (int)ItemValueNames.gold);
                int maxPalladium = (int)goldRow[0]["maxSet"];

                _maxPalladium = maxPalladium;

                DataTable playersTable = fileManager.GetDataTable("PLAYERS");
                DataRow[] playerRows = playersTable.Select("code = " + UnitObject.UnitCode);
                int maxLevel = (int)playerRows[0]["maxLevel"];

                _maxLevel = maxLevel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CharacterValues");
            }
        }

        public int Level
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.level) - GetBitCount((int)ItemValueNames.level);
                //return UnitHelpFunctions.GetSimpleValue(_hero, (int)ItemValueNames.level) - GetBitCount(12336);
            }
            set
            {
                if (value > MaxLevel)
                {
                    value = MaxLevel;
                }
                if (value < 0)
                {
                    value = 0;
                }

                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.level, value + GetBitCount((int)ItemValueNames.level));
            }
        }

        public int MaxLevel
        {
            get
            {
                return _maxLevel;
            }
        }

        public int Palladium
        {
            get
            {
                int palladium = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.gold);

                //if the character doesn't have palladium on him, there's also no palladium entry, so let's add it
                if (palladium == 0)
                {
                    //maximum Palladium value is 9.999.999 which is 100110001001011001111111 = 24 bit = bitCount
                    UnitHelpFunctions.AddSimpleValue(UnitObject, ItemValueNames.gold, 0, GetBitCount((int)ItemValueNames.gold));
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

                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.gold, value);
            }
        }

        public int MaxPalladium
        {
            get
            {
                return _maxPalladium;
            }
        }

        /// <summary>
        /// The current number of unused attribute points
        /// </summary>
        public int AttributePoints
        {
            get
            {
                int attributePoints = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.stat_points);

                //if the character doesn't have palladium on him, there's also no palladium entry, so let's add it
                if (attributePoints == 0)
                {
                    //bitCount = 10 taken from other saves => max value = 1023
                    UnitHelpFunctions.AddSimpleValue(UnitObject, ItemValueNames.stat_points, 0, GetBitCount((int)ItemValueNames.stat_points));
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

                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.stat_points, value);
            }
        }

        /// <summary>
        /// The maximum number of unused attribute points a character can have
        /// </summary>
        public int MaxAttributePoints
        {
            get
            {
                return 1000;
            }
        }

        /// <summary>
        /// The current number of unused skill pointse
        /// </summary>
        public int SkillPoints
        {
            get
            {
                int skillPoints = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.skill_points);

                //if the character doesn't have palladium on him, there's also no palladium entry, so let's add it
                if (skillPoints == 0)
                {
                    //bitCount = 12 taken from other saves => max value = 4095
                    UnitHelpFunctions.AddSimpleValue(UnitObject, ItemValueNames.skill_points, 0, GetBitCount((int)ItemValueNames.skill_points));
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

                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.skill_points, value);
            }
        }

        /// <summary>
        /// The maximum number of unused skill points a character can have
        /// </summary>
        public int MaxSkillPoints
        {
            get
            {
                return 4000;
            }
        }

        /// <summary>
        /// The current accuracy value
        /// </summary>
        public int Accuracy
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.accuracy);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.accuracy, value);
            }
        }

        /// <summary>
        /// The current stamina value
        /// </summary>
        public int Stamina
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.stamina);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.stamina, value);
            }
        }

        /// <summary>
        /// The current strength value
        /// </summary>
        public int Strength
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.strength);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.strength, value);
            }
        }

        /// <summary>
        /// The current willpower value
        /// </summary>
        public int Willpower
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.willpower);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.willpower, value);
            }
        }

        /// <summary>
        /// The current amount of achievement points
        /// </summary>
        public int AchievementPointsCur
        {
            get
            {
                int achievementPointsCur = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.achievement_points_cur);

                if (achievementPointsCur == 0)
                {
                    //bitCount = 12 taken from other saves => max value = 4095
                    UnitHelpFunctions.AddSimpleValue(UnitObject, ItemValueNames.skill_points, 0, GetBitCount((int)ItemValueNames.achievement_points_cur));
                }

                return achievementPointsCur;
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.achievement_points_cur, value);
            }
        }

        /// <summary>
        /// The total amount of achievement points that were collected up till now accuracy value
        /// </summary>
        public int AchievementPointsTotal
        {
            get
            {
                int achievementPointsTotal = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.achievement_points_total);

                if (achievementPointsTotal == 0)
                {
                    //bitCount = 12 taken from other saves => max value = 4095
                    UnitHelpFunctions.AddSimpleValue(UnitObject, ItemValueNames.skill_points, 0, GetBitCount((int)ItemValueNames.achievement_points_total));
                }

                return achievementPointsTotal;
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.achievement_points_cur, value);
            }
        }

        /// <summary>
        /// The total amount of experience points a character has collected
        /// </summary>
        public int Experience
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.experience);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.experience, value);
            }
        }

        /// <summary>
        /// The experience needed for the previous level up
        /// </summary>
        public int Experience_Prev
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.experience_prev);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.experience_prev, value);
            }
        }

        /// <summary>
        /// The experience needed for the next level up
        /// </summary>
        public int Experience_Next
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.experience_next);
            }
            set
            {
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.experience_next, value);
            }
        }

        /// <summary>
        /// The total playtime of the character
        /// </summary>
        public int PlayTime
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.played_time_in_seconds);
            }
            //set
            //{
            //    UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.played_time_in_seconds, value);
            //}
        }

        /// <summary>
        /// The total playtime of the character as string
        /// </summary>
        public string PlayTimeString
        {
            get
            {
                int playTime = PlayTime;
                TimeSpan timeSpan = TimeSpan.FromSeconds(playTime);
                string timeString = string.Format("{0:00}d {1:00}h {2:00}m {3:00}s", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                return timeString;
            }
        }
    }

    public class WeaponSlots : CharacterProperty
    {
        public WeaponSlots(UnitObject unitObject, FileManager fileManager)
            : base(unitObject, fileManager)
        {
        }

        public UnitObject[] WeaponSlot1
        {
            get
            {
                List<UnitObject> _weapons = new List<UnitObject>();

                foreach (UnitObject item in UnitObject.Items)
                {
                    if (item.InventoryLocationIndex == (int)InventoryTypes.CurrentWeaponSet)
                    {
                        _weapons.Add(item);
                    }
                }

                return _weapons.ToArray();
            }
        }

        public UnitObject[] WeaponSlot2
        {
            get
            {
                List<UnitObject> _weapons = new List<UnitObject>();

                foreach (UnitObject item in UnitObject.Items)
                {
                    if (item.InventoryLocationIndex == (int)InventoryTypes.CurrentWeaponSet)
                    {
                        _weapons.Add(item);
                    }
                }

                return _weapons.ToArray();
            }
        }

        public UnitObject[] WeaponSlot3
        {
            get
            {
                List<UnitObject> _weapons = new List<UnitObject>();

                foreach (UnitObject item in UnitObject.Items)
                {
                    if (item.InventoryLocationIndex == (int)InventoryTypes.CurrentWeaponSet)
                    {
                        _weapons.Add(item);
                    }
                }

                return _weapons.ToArray();
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

        public CharacterSkills(UnitObject unitObject, FileManager fileManager, IEnumerable<int> skillTabs) : base(unitObject, fileManager)
        {
            _skillTabs = new List<SkillTab>();
            
            //to make things easier, let's add all available character skills to the list
            List<UnitObjectStats.Stat.StatValue> availableSkills = new List<UnitObjectStats.Stat.StatValue>();
            ////get the skills the character already knows
            UnitObjectStats.Stat skills = UnitHelpFunctions.GetComplexValue(unitObject, ItemValueNames.skill_level);
            ////add them to the complete skill list
            availableSkills.AddRange(skills.Values);

            DataTable skillTable = fileManager.GetDataTable("SKILLS");

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

            //skills.repeatCount = availableSkills.Count;

            //skills.Values = availableSkills;
            skills.Values.Clear();
            skills.Values.AddRange(availableSkills);
        }

        private SkillTab CreateSkillsFromRow(List<UnitObjectStats.Stat.StatValue> availableSkills, DataTable skillTable, DataRow[] skillRows)
        {
            List<UnitObjectStats.Stat.StatValue> values = new List<UnitObjectStats.Stat.StatValue>();
            SkillTab skillInSkillTab = new SkillTab();

            //iterate through all available skills
            foreach (DataRow row in skillRows)
            {
                //get the skill id
                int skillId = (int)row["code"];
                //if the skill is already present, use that one
                UnitObjectStats.Stat.StatValue tmpSkill = availableSkills.Find(tmp => tmp.Param1 == skillId);
                if (tmpSkill != null)
                {
                    values.Add(tmpSkill);
                }
                //if not, add a new one
                else
                {
                    UnitObjectStats.Stat.StatValue skillEntry = new UnitObjectStats.Stat.StatValue();
                    skillEntry.Param1 = skillId;

                    values.Add(skillEntry);
                }
            }
            //_hero.Stats.statCount

            //and finally... initialize all skills :)
            foreach (UnitObjectStats.Stat.StatValue skillBlock in values)
            {
                Skill skill = InitializeSkill(skillTable, skillBlock);
                skillInSkillTab.Skills.Add(skill);
            }
            return skillInSkillTab;
        }

        private Skill InitializeSkill(DataTable table, UnitObjectStats.Stat.StatValue skillBlock)
        {
            DataRow[] availableSkillRows = table.Select("code = " + skillBlock.Param1);

            string name = availableSkillRows[0]["displayName_string"].ToString();
            string description = availableSkillRows[0]["descriptionString_string"].ToString();
            string iconName = (string)availableSkillRows[0]["smallIcon"];
            int maxLevel = (int)availableSkillRows[0]["maxLevel"];
            int row = (int)availableSkillRows[0]["skillPageRow"];
            int column = (int)availableSkillRows[0]["skillPageColumn"];

            List<int> requiredSkills = new List<int>();
            List<int> levelsOfRequiredSkills = new List<int>();

            for (int counter = 1; counter < 5; counter++)
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
        UnitObjectStats.Stat.StatValue _skillBlock;
        string _name;
        string _description;
        int _maxLevel;
        string _iconName;
        int[] _requiredSkills;
        int[] _levelsOfRequiredSkills;
        Point _position;

        public UnitObjectStats.Stat.StatValue SkillBlock
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
            get { return _skillBlock.Param1; }
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
            get { return _skillBlock.Value; }
            set
            {
                if (value > _maxLevel)
                {
                    value = _maxLevel;
                }

                _skillBlock.Value = value;
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

        public Skill(string name, string description, string iconName, int maxLevel, Point position, int[] requiredSkills, int[] levelsOfRequiredSkills, UnitObjectStats.Stat.StatValue skillBlock)
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

    public class CharacterInventory : CharacterProperty
    {
        List<CharacterInventoryType> _inventoryList;

        public CharacterInventory(UnitObject unitObject, FileManager fileManager)
            : base(unitObject, fileManager)
        {
            _inventoryList = new List<CharacterInventoryType>();

            foreach (UnitObject unit in unitObject.Items)
            {
                CharacterItems item = new CharacterItems(unit, fileManager);

                // get the matching inventory entry
                CharacterInventoryType inv = _inventoryList.Find(tmp => tmp.InventoryType == (int)item.InventoryType);

                if (inv == null)
                {
                    inv = new CharacterInventoryType((int)item.InventoryType);
                    _inventoryList.Add(inv);
                }

                inv.Items.Add(item);
            }
        }

        public CharacterInventoryType GetInventoryById(int inventoryId)
        {
            CharacterInventoryType inv = _inventoryList.Find(tmp => tmp.InventoryType == inventoryId);

            if (inv == null)
            {
                inv = new CharacterInventoryType(inventoryId);
                _inventoryList.Add(inv);
            }

            return inv;
        }

        public bool CheckIfInventoryIsPopulated(int inventory)
        {
            CharacterInventoryType inventorySlot = GetInventoryById(inventory);
            if (inventorySlot != null && inventorySlot.Items.Count > 0)
            {
                return true;
            }
            return false;
        }

        public List<CharacterInventoryType> InventoryType
        {
            get { return _inventoryList; }
        }

        public void Set(CharacterInventoryType inventory)
        {
            int index = _inventoryList.FindIndex(tmp => tmp.InventoryType == inventory.InventoryType);
            _inventoryList[index] = inventory;
        }

        public void Apply()
        {
            UnitObject.Items.Clear();

            foreach (CharacterInventoryType type in _inventoryList)
            {
                foreach (CharacterItems item in type.Items)
                {
                    UnitObject.Items.Add(item.UnitObject);
                }
            }
        }
    }

    public class CharacterInventoryType
    {
        int _inventoryType;
        List<CharacterItems> _items;

        public CharacterInventoryType(int inventoryType)
        {
            _inventoryType = inventoryType;
            _items = new List<CharacterItems>();
        }

        public List<CharacterItems> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public int InventoryType
        {
            get { return _inventoryType; }
            set { _inventoryType = value; }
        }

        public override string ToString()
        {
            return _inventoryType.ToString();
        }
    }


    public class CharacterItems : CharacterProperty
    {
        DataTable _itemTable;
        List<CharacterItems> _items;
        bool _isQuestItem;
        Color _qualityColor;
        Bitmap _itemImage;
        string _itemImagePath;
        bool _isItem;
        bool _isConsumable;
        int _numberOfAugmentations;
        int _numberOfAugmentationsLeft;
        int _maxNumberOfAugmentations;
        int _maxNumberOfAffixes;
        int _numberOfAffixes;
        int _numberOfUpgrades;
        int _maxNumberOfUpgrades;
        int _stackSize;
        int _maxStackSize;

        public CharacterItems(UnitObject unitObject, FileManager fileManager)
            : base(unitObject, fileManager)
        {
            _itemTable = fileManager.GetDataTable("ITEMS");
            DataRow[] itemRow = _itemTable.Select("code = " + unitObject.UnitCode);

            //DataTable colorTable = _dataSet.GetExcelTableFromStringId("ITEMQUALITY");
            //DataRow[] colorRow = colorTable.Select("code = " + _hero.unitCode);

            if (itemRow.Length > 0)
            {
                _isItem = true;

                uint bitMask = (uint)itemRow[0]["bitmask02"];
                _isQuestItem = (bitMask >> 13 & 1) == 1;

                string maxStackSize = (string)itemRow[0]["stackSize"];
                string[] splitResult = maxStackSize.Split(new char[] { ',' });
                if (splitResult.Length == 3)
                {
                    _maxStackSize = int.Parse(splitResult[1]);
                }
                if (_maxStackSize <= 0)
                {
                    _maxStackSize = 1;
                }

                _stackSize = UnitHelpFunctions.GetSimpleValue(unitObject, ItemValueNames.item_quantity.ToString());
                if (_stackSize <= 0)
                {
                    _stackSize = 1;
                }

                _itemImagePath = CreateImagePath();

                _numberOfAugmentations = UnitHelpFunctions.GetSimpleValue(unitObject, ItemValueNames.item_augmented_count.ToString());
                _numberOfUpgrades = UnitHelpFunctions.GetSimpleValue(unitObject, ItemValueNames.item_upgraded_count.ToString());

                DataTable gameGlobals = fileManager.GetDataTable("GAME_GLOBALS");
                //DataRow[] globalsRow = gameGlobals.Select("name = " + "max_item_upgrades");
                DataRow[] globalsRow = gameGlobals.Select("Index = " + 16);
                _maxNumberOfUpgrades = (int)globalsRow[0]["intValue"];

                //globalsRow = gameGlobals.Select("name = " + "max_item_augmentations");
                globalsRow = gameGlobals.Select("Index = " + 17);
                _maxNumberOfAffixes = (int)globalsRow[0]["intValue"];
                UnitObjectStats.Stat affixes = UnitHelpFunctions.GetComplexValue(unitObject, ItemValueNames.applied_affix.ToString());
                if (affixes != null)
                {
                    _numberOfAffixes = affixes.Values.Count;
                }

                int numberOfInherentAffixes = _numberOfAffixes - _numberOfAugmentations;
                _numberOfAugmentationsLeft = _maxNumberOfAffixes - numberOfInherentAffixes;

                if (_numberOfAugmentationsLeft < 0)
                {
                    _numberOfAugmentationsLeft = 0;
                }

                _maxNumberOfAugmentations = _numberOfAugmentations + _numberOfAugmentationsLeft;
                if (_maxNumberOfAugmentations > _maxNumberOfAffixes)
                {
                    _maxNumberOfAugmentations = _maxNumberOfAffixes;
                }
            }

            _items = new List<CharacterItems>();

            foreach (UnitObject item in unitObject.Items)
            {
                CharacterItems wrapper = new CharacterItems(item, fileManager);
                _items.Add(wrapper);
            }
        }

        private string CreateImagePath()
        {
            DataRow[] itemsRows = _itemTable.Select(String.Format("code = '{0}'", UnitObject.UnitCode));
            if (itemsRows.Length == 0)
            {
                return null;
            }
            int unitType = (int)itemsRows[0]["unitType"];
            string folder = (string)itemsRows[0]["folder"] + @"\icons";
            string name = (string)itemsRows[0]["name"];
            //string unitType = (string)itemsRows[0]["unitType_string"];

            string itemPath = Path.Combine(folder, name);

            return itemPath;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name
        {
            get
            {
                return UnitObject.Name;
            }
        }

        public string GetItemImagePath(bool male)
        {
            string path = _itemImagePath;

            if (_itemImagePath.StartsWith("armor"))
            {
                if (male)
                {
                    path += "_m";
                }
                else
                {
                    path += "_f";
                }
            }

            //if (path == null)
            //{
            //    return path; 
            //}
            //we don't know if we want to load dds or png yet
            return path;// += ".dds";
        }

        public bool IsItem
        {
            get { return _isItem; }
            set { _isItem = value; }
        }

        /// <summary>
        /// Do NOT use this entry for item adding/removing!
        /// </summary>
        public List<CharacterItems> WrappedItems
        {
            get
            {
                return _items;
            }
        }

        public List<UnitObject> Items
        {
            get
            {
                return UnitObject.Items;
            }

            set
            {
                UnitObject.Items = value;

            }
        }

        public int StackSize
        {
            get
            {
                return _stackSize;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                if (value > MaxStackSize)
                {
                    value = MaxStackSize;
                }

                _stackSize = value;
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.item_quantity, value);
            }
        }

        public int MaxStackSize
        {
            get
            {
                return _maxStackSize;
            }
        }

        public bool IsConsumable
        {
            get
            {
                return _isConsumable;
            }
        }

        /// <summary>
        /// Number of uses of the Nanoforge up till now
        /// </summary>
        public int NumberOfUpgrades
        {
            get { return _numberOfUpgrades; }
        }

        /// <summary>
        /// Number of uses of the Nanoforge left
        /// </summary>
        public int MaxNumberOfUpgrades
        {
            get { return _maxNumberOfUpgrades; }
        }

        /// <summary>
        /// Maximum number of Augmentrix usages given the inherent affixes
        /// </summary>
        public int MaxNumberOfAugmentations
        {
            get { return _maxNumberOfAugmentations; }
        }

        /// <summary>
        /// Number of Augmentrix usages up till now
        /// </summary>
        public int NumberOfAugmentations
        {
            get
            {
                return _numberOfAugmentations;
            }
        }

        /// <summary>
        /// Number of Augmentrix usages left
        /// </summary>
        public int NumberOfAugmentationsLeft
        {
            get { return _numberOfAugmentationsLeft; }
        }

        /// <summary>
        /// Number of already present affixes 
        /// </summary>
        public int NumberOfAffixes
        {
            get
            {
                return _numberOfAffixes;
            }
        }

        /// <summary>
        /// Maximum number of affixes
        /// </summary>
        public int MaxNumberOfAffixes
        {
            get { return _maxNumberOfAffixes; }
            set { _maxNumberOfAffixes = value; }
        }

        public InventoryTypes InventoryType
        {
            get
            {
                return (InventoryTypes)Enum.Parse(typeof(InventoryTypes), UnitObject.InventoryLocationIndex.ToString());
            }
            set
            {
                UnitObject.InventoryLocationIndex = (int)value;
            }
        }

        public Point InventoryPosition
        {
            get
            {
                return new Point(UnitObject.InventoryPositionX, UnitObject.InventoryPositionY);
            }
            set
            {
                UnitObject.InventoryPositionX = value.X;
                UnitObject.InventoryPositionY = value.Y;
            }
        }

        public Size InventorySize
        {
            get
            {
                int width = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.inventory_width);
                int height = UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.inventory_height);

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
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.inventory_width, value.Width);
                UnitHelpFunctions.SetSimpleValue(UnitObject, (int)ItemValueNames.inventory_height, value.Height);
            }
        }

        public ItemQuality Quality
        {
            get
            {
                return (ItemQuality)UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.item_quality);
            }
        }

        public Color QualityColor
        {
            get
            {
                return _qualityColor;
            }
        }

        //public Image ItemImage
        //{
        //    get
        //    {
        //        return _itemImage;
        //    }
        //}

        public bool IsQuestItem
        {
            get
            {
                return _isQuestItem;
            }
        }

        public List<UnitObject> GetItemsOfQuality(ItemQuality quality)
        {
            List<UnitObject> tmp = new List<UnitObject>();

            foreach (UnitObject item in UnitObject.Items)
            {
                if (UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.item_quality) == (int)quality)
                {
                    tmp.Add(item);
                }
            }
            return tmp;
        }

        public List<UnitObject> GetItemsOfInventoryType(InventoryTypes type)
        {
            List<UnitObject> tmp = new List<UnitObject>();

            foreach (UnitObject item in UnitObject.Items)
            {
                if (item.InventoryLocationIndex == (int)type)
                {
                    tmp.Add(item);
                }
            }

            return tmp;
        }

        public void AddItem(UnitObject item)
        {
            UnitObject.Items.Add(item);
            // todo: rewrite _items.Add(new CharacterItems(item, _dataSet));
        }

        public void RemoveItem(UnitObject item)
        {
            UnitObject.Items.Remove(item);
            CharacterItems tmpItem = _items.Find(tmp => tmp.UnitObject == item);
            _items.Remove(tmpItem);
        }

        public void AddItem(CharacterItems item)
        {
            _items.Add(item);
            UnitObject.Items.Add(item.UnitObject);
        }

        public void RemoveItem(CharacterItems item)
        {
            UnitObject.Items.Remove(item.UnitObject);
            _items.Remove(item);
        }

        public int PlayTime
        {
            get
            {
                return UnitHelpFunctions.GetSimpleValue(UnitObject, (int)ItemValueNames.played_time_in_seconds);
            }
            //set
            //{
            //    UnitHelpFunctions.SetSimpleValue(_hero, (int)ItemValueNames.played_time_in_seconds, value);
            //}
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

    public enum Gender
    {
        Male = 0,
        Female
    }
}
