using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;
using System.Windows.Forms;

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

        public CharacterWrapper CharacterWrapper
        {
            get { return characterWrapper; }
            private set { characterWrapper = value; }
        }

        public UnitWrapper2(string savegamePath, FileManager fileManager)
        {
            baseCharacter = CharacterFile.LoadCharacter(savegamePath);
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
            characterWrapper = new CharacterWrapper(baseCharacter.Character, fileManager); 
        }
    }

    public class CharacterWrapper : CharacterProperty
    {
        CharacterClass characterClass;
        CharacterGameMode characterGameMode;
        CharacterValues characterValues;
        Gender gender;

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

        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public CharacterWrapper(UnitObject character, FileManager fileManager)
            : base(character, fileManager)
        {
            characterClass = GetCharacterClass(character);
            characterGameMode = new CharacterGameMode(character, fileManager);
            characterValues = new CharacterValues(character, fileManager);
            gender = characterClass.ToString().EndsWith("_Male") ? Gender.Male : Gender.Female;
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
                return UnitObject.PlayerFlags1.Contains((int)GameMode.Elite) || unitObject.PlayerFlags2.Contains((int)GameMode.Elite);
            }
            set
            {
                if (value)
                {
                    if (!UnitObject.PlayerFlags1.Contains((int)GameMode.Elite))
                    {
                        UnitObject.PlayerFlags1.Add((int)GameMode.Elite);
                    }
                    if (!UnitObject.PlayerFlags2.Contains((int)GameMode.Elite))
                    {
                        UnitObject.PlayerFlags2.Add((int)GameMode.Elite);
                    }
                }
                else
                {
                    UnitObject.PlayerFlags1.Remove((int)GameMode.Elite);
                    UnitObject.PlayerFlags2.Remove((int)GameMode.Elite);
                }
            }
        }

        public bool IsHardcore
        {
            get
            {
                return UnitObject.PlayerFlags1.Contains((int)GameMode.Hardcore) || UnitObject.PlayerFlags2.Contains((int)GameMode.Hardcore);
            }
            set
            {
                if (value)
                {
                    if (!UnitObject.PlayerFlags1.Contains((int)GameMode.Hardcore))
                    {
                        UnitObject.PlayerFlags1.Add((int)GameMode.Hardcore);
                    }
                    if (!UnitObject.PlayerFlags2.Contains((int)GameMode.Hardcore))
                    {
                        UnitObject.PlayerFlags2.Add((int)GameMode.Hardcore);
                    }
                }
                else
                {
                    UnitObject.PlayerFlags1.Remove((int)GameMode.Hardcore);
                    UnitObject.PlayerFlags2.Remove((int)GameMode.Hardcore);
                }
            }
        }

        public bool IsHardcoreDead
        {
            get
            {
                return UnitObject.PlayerFlags1.Contains((int)GameMode.HardcoreDead) || UnitObject.PlayerFlags2.Contains((int)GameMode.HardcoreDead);
            }
            set
            {
                if (value)
                {
                    if (!UnitObject.PlayerFlags1.Contains((int)GameMode.HardcoreDead))
                    {
                        UnitObject.PlayerFlags1.Add((int)GameMode.HardcoreDead);
                    }
                    if (!UnitObject.PlayerFlags2.Contains((int)GameMode.HardcoreDead))
                    {
                        UnitObject.PlayerFlags2.Add((int)GameMode.HardcoreDead);
                    }
                }
                else
                {
                    UnitObject.PlayerFlags1.Remove((int)GameMode.HardcoreDead);
                    UnitObject.PlayerFlags2.Remove((int)GameMode.HardcoreDead);
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
