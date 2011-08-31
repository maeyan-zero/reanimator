using System;
using System.Data;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class Achievements : WikiScript
    {
        public Achievements(FileManager manager) : base(manager,"achievements")
        {
        }

        public override string ExportArticle()
        {
            var builder = new StringBuilder();
            var achievements = Manager.GetDataTable("ACHIEVEMENTS");
            var skills = Manager.GetDataTable("SKILLS");

            WikiTable table = new WikiTable(false, string.Empty, string.Empty,
                "Name",
                "Description",
                "Reward Skill",
                "Reward Title"
                );

            string name, desc, skill, title;
            foreach (DataRow row in achievements.Rows)
            {
                name = row["nameString_string"].ToString();

                desc = row["descripFormatString_string"].ToString();
                desc = desc.Replace("[completenum]", String.Format("{0:#,0}", row["completeNumber"]));
                desc = desc.Replace("[param1]", String.Format("{0:#,0}", (int)row["param1"] / 20));

                if ((int)row["rewardSkill"] != -1)
                {
                    var s = (int)row["rewardSkill"];
                    skill = skills.Rows[s]["effectString_string"].ToString();
                    var var = skills.Rows[s]["skillVar1"].ToString();
                    var icon = skills.Rows[s]["largeIcon"].ToString();
                    icon = GetImage(icon + ".png", 40);// "[[File:" + icon + ".png|40px]] ";
                    var = var.Replace(";", "");
                    skill = skill.Replace("[string2]", var);
                    skill = icon + " " + skill;
                }
                else
                {
                    skill = string.Empty;
                }

                title = row["rewardTitle_string"].ToString();
                title = title.Replace("[PLAYERNAME]", "");

                table.AddRow(name, desc, skill, title);
            }

            return table.GetTableSyntax();
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", string.Empty,
                "id INT NOT NULL",
                "name TEXT NOT NULL",
                "description TEXT NOT NULL",
                "skill_file TEXT",
                "skill_text TEXT",
                "title TEXT"
                );

            var achievements = Manager.GetDataTable("ACHIEVEMENTS");
            var skills = Manager.GetDataTable("SKILLS");

            object id;
            string name, desc, skillFile, skillText, title;
            foreach (DataRow row in achievements.Rows)
            {
                id = row["code"];
                name = "\"" + row["nameString_string"] + "\"";

                desc = row["descripFormatString_string"].ToString();
                desc = desc.Replace("[completenum]", String.Format("{0:#,0}", row["completeNumber"]));
                desc = desc.Replace("[param1]", String.Format("{0:#,0}", (int)row["param1"]));// / 20));
                desc = "\"" + desc + "\"";

                if ((int)row["rewardSkill"] != -1)
                {
                    var s = (int)row["rewardSkill"];
                    var skill = skills.Rows[s]["effectString_string"].ToString();
                    var var = skills.Rows[s]["skillVar1"].ToString();
                    var = var.Replace(";", "");
                    skill = skill.Replace("[string2]", var);

                    var icon = skills.Rows[s]["largeIcon"].ToString();

                    skillFile = GetSqlEncapsulatedString(GetImage(icon + ".png", 40));
                    skillText = "\"" + skill + "\"";
                }
                else
                {
                    skillFile = skillText = string.Empty;
                }

                title = row["rewardTitle_string"].ToString();
                title = title.Replace("[PLAYERNAME]", "").Trim();
                title = "\"" + title + "\"";

                table.AddRow(id.ToString(), name, desc, skillFile, skillText, title);
            }

            return table.GetFullScript();
        }
    }
}
