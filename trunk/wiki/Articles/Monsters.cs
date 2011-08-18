using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hellgate;

namespace MediaWiki.Articles
{
    class Monsters : WikiScript
    {
        public Monsters(FileManager manager) : base(manager,"monsters")
        {
        }

        public override string ExportArticle()
        {
            //not sure what we're doing here since we don't want everything at once
            throw new NotImplementedException();
        }

        public override string ExportTableInsertScript()
        {
            SQLTableScript table = new SQLTableScript("id", "code",
                "id INT NOT NULL",
                "code VARCHAR(4) NOT NULL",
                "name TEXT",
                "base TEXT",
                "quality TEXT",
                "hp_min INT",
                "hp_max INT",
                "experience INT",
                "armor INT",
                "shields INT",
                "phys_atk INT",
                "phys_def INT",
                "fire_atk INT",
                "fire_def INT",
                "elec_atk INT",
                "elec_def INT",
                "spec_atk INT",
                "spec_def INT",
                "pois_atk INT",
                "pois_def INT",
                "treasure INT",
                "treasure_champion INT",
                "treasure_first INT"
                );

            var monsters = Manager.GetDataTable("MONSTERS");

            string id, code, name, baseType, quality, hpMin, hpMax, xp, armor, shields, 
                physAtk, physDef, fireAtk, fireDef, elecAtk, elecDef, specAtk, specDef, poisAtk, poisDef, 
                treasure, treasureChamp, treasureFirst;

            foreach (DataRow row in monsters.Rows)
            {
                id = row["Index"].ToString();
                code = GetSqlEncapsulatedString(((int)row["code"]).ToString("X"));
                name = GetSqlEncapsulatedString(row["String_string"] as string ?? string.Empty);

                baseType = ((int)row["baseRow"] != -1 ? monsters.Rows[(int)row["baseRow"]]["String_string"] as string : "") ?? string.Empty;
                baseType = GetWikiArticleLink(baseType);
                baseType = GetSqlEncapsulatedString(baseType);

                quality = (int)row["monsterQuality"] != -1 ? (string)row["monsterQuality_string"] : string.Empty;
                quality = GetWikiArticleLink(quality);
                quality = GetSqlEncapsulatedString(quality);

                hpMin = row["hpMin"].ToString();
                hpMax = row["hpMax"].ToString();
                xp = row["experience"].ToString();
                armor = row["armor"].ToString();
                shields = row["shields"].ToString();

                physAtk = row["sfxPhysicalAbilityPct"].ToString();
                physDef = row["sfxPhysicalDefensePct"].ToString();
                fireAtk = row["sfxFireAbilityPct"].ToString();
                fireDef = row["sfxFireDefensePct"].ToString();
                elecAtk = row["sfxElectricAbilityPct"].ToString();
                elecDef = row["sfxElectricDefensePct"].ToString();
                specAtk = row["sfxSpectralAbilityPct"].ToString();
                specDef = row["sfxSpectralDefensePct"].ToString();
                poisAtk = row["sfxToxicAbilityPct"].ToString();
                poisDef = row["sfxToxicDefensePct"].ToString();

                treasure = row["treasure"].ToString();
                treasureChamp = row["championTreasure"].ToString();
                treasureFirst = row["firstTimeTreasure"].ToString();

                table.AddRow(id, code, name, baseType, quality, hpMin, hpMax, xp, armor, shields,
                    physAtk, physDef, fireAtk, fireDef, elecAtk, elecDef, specAtk, specDef, poisAtk, poisDef,
                    treasure, treasureChamp, treasureFirst);
            }

            return table.GetFullScript();
        }
    }
}
