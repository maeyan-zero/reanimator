using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class MonsterQuality : WikiScript
    {
        public MonsterQuality(FileManager manager) : base(manager, "monster_quality")
        {
        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            var script = new SQLTableScript("id", "",
                                            "id INT",
                                            "name TEXT",
                                            "rarity INT",
                                            "money_chance DOUBLE",
                                            "money_amount DOUBLE",
                                            "treasure_level_boost INT",
                                            "health_multi INT",
                                            "props TEXT",
                                            "affix_count INT",
                                            "affix_type1 TEXT",
                                            "affix_type2 TEXT",
                                            "affix_type3 TEXT",
                                            "affix_prob1 INT",
                                            "affix_prob2 INT",
                                            "affix_prob3 INT",
                                            "experience_multi INT");

            var data = Manager.GetDataTable("MONSTER_QUALITY");

            // ReSharper disable TooWideLocalVariableScope
            string id, name, rarity, moneyChance, moneyAmount, treasureBoost, healthMulti, props,
                affixCount, affixType1, affixType2, affixType3, affixProb1, affixProb2, affixProb3, experience;
            // ReSharper restore TooWideLocalVariableScope
            
            foreach (DataRow row in data.Rows)
            {
                if ((int) row["Index"] == 6) break; // skip irrelevent

                id = row["Index"].ToString();

                name = GetFormattedString(row["quality"].ToString());
                //name = "{{" + name + "|" + name + "}}"; // didn't work in the parser - dunno why
                name = GetSqlString(name);

                rarity = row["rarity"].ToString();
                moneyChance = row["MoneyChanceMultiplier"].ToString();
                moneyAmount = row["MoneyAmountMultiplier"].ToString();
                treasureBoost = row["TreasureLevelBoost"].ToString();
                healthMulti = row["HealthMultiplier"].ToString();

                props = row["prop1"].ToString();
                props = props.Replace("\n", "<br/>").Replace("SetStat673('", "").Replace(", ", " ").Replace(");", "").Replace("'", "");
                props = GetSqlString(props);

                affixCount = row["AffixCount"].ToString();
                affixType1 = GetSqlString(row["AffixType1_string"].ToString());
                affixType2 = GetSqlString(row["AffixType2_string"].ToString());
                affixType3 = GetSqlString(row["AffixType3_string"].ToString());
                affixProb1 = row["AffixProbability1"].ToString().Replace(";", "");
                affixProb2 = row["AffixProbability2"].ToString().Replace(";", "");
                affixProb3 = row["AffixProbability3"].ToString().Replace(";", "");
                experience = row["experienceMultiplier"].ToString();

                if (string.IsNullOrEmpty(affixProb1)) affixProb1 = "0";
                if (string.IsNullOrEmpty(affixProb2)) affixProb2 = "0";
                if (string.IsNullOrEmpty(affixProb3)) affixProb3 = "0";

                script.AddRow(id, name, rarity, moneyChance, moneyAmount, treasureBoost, healthMulti, props,
                    affixCount, affixType1, affixType2, affixType3, affixProb1, affixProb2, affixProb3, experience);
            }

            return script.GetFullScript();
        }
    }
}
