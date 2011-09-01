using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hellgate;
using System.Data;

namespace MediaWiki.Articles
{
    class Recipes:WikiScript
    {
        public Recipes(FileManager manager)
            : base(manager, "recipes")
        {

        }

        public override string ExportArticle()
        {
            throw new NotImplementedException();            
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(6) NOT NULL",
                "ingredients TEXT NOT NULL",
                "result TEXT NOT NULL"
            );

            var recipes = Manager.GetDataTable("RECIPES");
            var treasure = Manager.GetDataTable("TREASURE");
            var items = Manager.GetDataTable("ITEMS");

            string id, code, ingredients, result;
            string ingredient;
            int itemIndex, quantity, treasureIndex;
            string[] skipCodes={"6742","6842","6942","4443","4943","5844"};

            foreach (DataRow row in recipes.Rows)
            {
                //skip non-cube recipes
                if ((int)row["cubeRecipe"] != 1) continue;

                code = ((int)row["code"]).ToString("X");
                //skip egg/mythic/key recipes for now
                if (skipCodes.Contains(code))
                    continue;

                ingredient = string.Empty;
                ingredients = string.Empty;
                result = string.Empty;

                id = row["Index"].ToString();
                code = GetSqlString(code);

                for (int i = 1; i < 7; i++)
                {
                    itemIndex = (int)row[string.Format("ingredient{0}ItemClass", i)];
                    if (itemIndex < 0) continue;  //skip blank ingredients
                    ingredient = items.Rows[itemIndex]["String_string"].ToString();

                    quantity = (int)row[string.Format("ingredient{0}MinQuantity", i)]; //pretty sure we just need one, since there doesn't seem to be any ranges
                    if (quantity > 1)
                        ingredient += " (" + quantity + ")";
                    ingredients += ingredient + "<br />";                    
                }

                treasureIndex = (int)row["treasureResult1"];
                itemIndex = int.Parse(treasure.Rows[treasureIndex]["item1"].ToString().Split(',')[1]);  //should be just one result
                result = items.Rows[itemIndex]["String_string"].ToString();

                table.AddRow(id, code, GetSqlString(ingredients), GetSqlString(result));
            }

            return table.GetFullScript();
        }
    }
}
