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

            builder.AppendLine("{| class=\"wikitable\"");
            builder.AppendLine("!Name");
            builder.AppendLine("!Description");
            builder.AppendLine("!Reward Skill");
            builder.AppendLine("!Reward Title");

            foreach (DataRow row in achievements.Rows)
            {
                builder.AppendLine("|-");
                builder.AppendLine("|" + row["nameString_string"]);
                var descrip = row["descripFormatString_string"].ToString();
                descrip = descrip.Replace("[completenum]", String.Format("{0:#,0}", row["completeNumber"]));
                descrip = descrip.Replace("[param1]", String.Format("{0:#,0}", (int) row["param1"] / 20));
                builder.AppendLine("|" + descrip);
                if ((int) row["rewardSkill"] != -1)
                {
                    var s = (int) row["rewardSkill"];
                    var skill = skills.Rows[s]["effectString_string"].ToString();
                    var var = skills.Rows[s]["skillVar1"].ToString();
                    var icon = skills.Rows[s]["largeIcon"].ToString();
                    icon = GetImage(icon + ".png", 40);// "[[File:" + icon + ".png|40px]] ";
                    var = var.Replace(";", "");
                    skill = skill.Replace("[string2]", var);
                    builder.AppendLine("|" + icon + skill);
                }
                else
                {
                    builder.AppendLine("|");
                }
                var title = row["rewardTitle_string"].ToString();
                title = title.Replace("[PLAYERNAME]", "");
                builder.AppendLine("|" + title);
            }

            builder.AppendLine("|}");
            return builder.ToString();
        }

        public override string ExportTableInsertScript()
        {
            TableScript = new SQLTableScript("id",
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
                desc = desc.Replace("[param1]", String.Format("{0:#,0}", (int)row["param1"] / 20));
                desc = "\"" + desc + "\"";

                if ((int)row["rewardSkill"] != -1)
                {
                    var s = (int)row["rewardSkill"];
                    var skill = skills.Rows[s]["effectString_string"].ToString();
                    var var = skills.Rows[s]["skillVar1"].ToString();
                    var = var.Replace(";", "");
                    skill = skill.Replace("[string2]", var);

                    var icon = skills.Rows[s]["largeIcon"].ToString();

                    skillFile =  "\""+icon + ".png\"";
                    skillText = "\"" + skill + "\"";
                }
                else
                {
                    skillFile = skillText = "''";
                }

                title = row["rewardTitle_string"].ToString();
                title = title.Replace("[PLAYERNAME]", "").Trim();
                title = "\"" + title + "\"";

                TableScript.AddRow(id.ToString(), name, desc, skillFile, skillText, title);
            }

            return TableScript.GetFullScript();
        }

        public override string ExportSchema()
        {
            var schema = "DROP TABLE IF EXISTS " + FullTableName + ";\n" +
            "CREATE TABLE " + FullTableName + @" (
             id INT NOT NULL,
             name TEXT NOT NULL,
             description TEXT NOT NULL,
             skill_file TEXT,
             skill_text TEXT,
             title TEXT,
             PRIMARY KEY(id)
             );";
            return schema;
        }

        public override string ExportTable()
        {
            var achievements = Manager.GetDataTable("ACHIEVEMENTS");
            var skills = Manager.GetDataTable("SKILLS");

            var builder = new StringBuilder();
            builder.AppendLine("INSERT INTO " + FullTableName);
            builder.AppendLine("VALUES");

            foreach (DataRow row in achievements.Rows)
            {
                builder.Append("(");
                builder.Append(row["code"] + ", ");
                builder.Append("\""+row["nameString_string"] + "\", ");

                var descrip = row["descripFormatString_string"].ToString();
                descrip = descrip.Replace("[completenum]", String.Format("{0:#,0}", row["completeNumber"]));
                descrip = descrip.Replace("[param1]", String.Format("{0:#,0}", (int)row["param1"] / 20));
                builder.Append("\"" + descrip + "\", ");

                if ((int)row["rewardSkill"] != -1)
                {
                    var s = (int)row["rewardSkill"];
                    var skill = skills.Rows[s]["effectString_string"].ToString();
                    var var = skills.Rows[s]["skillVar1"].ToString();
                    var = var.Replace(";", "");
                    skill = skill.Replace("[string2]", var);

                    var icon = skills.Rows[s]["largeIcon"].ToString();
                    icon += ".png";

                    builder.Append("\"" + icon + "\", ");
                    builder.Append("\"" + skill + "\", ");
                }
                else
                {
                    builder.Append("'', ");
                    builder.Append("'', ");
                }

                var title = row["rewardTitle_string"].ToString();
                title = title.Replace("[PLAYERNAME]", "").Trim();

                builder.Append("\"" + title + "\"");
                builder.AppendLine("),");
            }
            builder.Remove(builder.Length - 3, 3);
            builder.AppendLine(";");
            return builder.ToString();
        }
    }
}
