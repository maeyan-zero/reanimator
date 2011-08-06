using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class Achievements : WikiScript
    {
        public Achievements(FileManager manager) : base(manager)
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
                    var = var.Replace(";", "");
                    skill = skill.Replace("[string2]", var);
                    builder.AppendLine("|" + skill);
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

        public override string ExportSchema()
        {
            throw new NotImplementedException();
        }

        public override string ExportTable()
        {
            throw new NotImplementedException();
        }
    }
}
