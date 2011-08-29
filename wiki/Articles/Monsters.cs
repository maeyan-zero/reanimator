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
                "image TEXT",
                "name TEXT",
                "base TEXT",
                "unit TEXT",
                "quality TEXT",
                "hp_min INT",
                "hp_max INT",
                "experience INT",
                "armor INT",
                "shields INT",
                "damage TEXT",
                "damage_min INT",
                "damage_max INT",
                "critical_pct INT",
                "critical_mult INT",
                "anger_range INT",
                "interrupt_atk DOUBLE",
                "interrupt_def DOUBLE",
                "ai_def DOUBLE",
                "stealth_def DOUBLE",
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

            // todo
            // anger range
            // interrupt defence
            // ai change defence
            // stealth defence

            var monsters = Manager.GetDataTable("MONSTERS");

            string id, code, image, name, baseType, quality, hpMin, hpMax, xp, armor, shields, 
                physAtk, physDef, fireAtk, fireDef, elecAtk, elecDef, specAtk, specDef, poisAtk, poisDef, 
                treasure, treasureChamp, treasureFirst, unitType, damage, damageMin, damageMax,
                angerRange, interruptAttack, interruptDefence, aiChangeDefence, stealthDefence,
                criticalPercent, criticalMultiplier;

            foreach (DataRow row in monsters.Rows)
            {
                id = row["Index"].ToString();
                code = GetSqlEncapsulatedString(((int)row["code"]).ToString("X"));
                image = GetImage(row["name"] + ".jpg", 230);
                image = GetSqlEncapsulatedString(image);
                name = GetSqlEncapsulatedString(row["String_string"] as string ?? string.Empty);

                baseType = ((int)row["baseRow"] != -1 ? monsters.Rows[(int)row["baseRow"]]["String_string"] as string : "") ?? string.Empty;
                baseType = GetWikiArticleLink(baseType);
                baseType = GetSqlEncapsulatedString(baseType);

                unitType = GetUnitType((int) row["unitType"]);
                unitType = GetSqlEncapsulatedString(unitType);

                quality = GetQualityType((int) row["monsterQuality"]);
                quality = GetSqlEncapsulatedString(quality);

                hpMin = row["hpMin"].ToString();
                hpMax = row["hpMax"].ToString();
                xp = row["experience"].ToString();
                armor = row["armor"].ToString();
                shields = row["shields"].ToString();

                damage = GetDamageType((int) row["dmgType"]);
                damage = GetSqlEncapsulatedString(damage);
                damageMin = row["minBaseDmg"].ToString().Replace(";", "");
                damageMax = row["maxBaseDmg"].ToString().Replace(";", "");
                criticalPercent = row["criticalPct"].ToString();
                criticalMultiplier = row["criticalMult"].ToString();

                angerRange = row["angerRange"].ToString();
                interruptAttack = ((int) row["interruptAttackPct"] == 0) ? "0" : ((Convert.ToDouble((int)row["interruptAttackPct"])) / 10).ToString();
                interruptDefence = ((int)row["interruptDefensePct"] == 0) ? "0" : ((Convert.ToDouble((int)row["interruptDefensePct"])) / 10).ToString();
                stealthDefence = ((int)row["stealthDefensePct"] == 0) ? "0" : ((Convert.ToDouble((int)row["stealthDefensePct"])) / 10).ToString();
                aiChangeDefence = ((int)row["aiChangeDefense"] == 0) ? "0" : ((Convert.ToDouble((int)row["aiChangeDefense"])) / 10).ToString();

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

                table.AddRow(id, code, image, name, baseType, unitType, quality, hpMin, hpMax, xp, armor, shields,
                    damage, damageMin, damageMax, criticalPercent, criticalMultiplier,
                    angerRange, interruptAttack, interruptDefence, stealthDefence, aiChangeDefence,
                    physAtk, physDef, fireAtk, fireDef, elecAtk, elecDef, specAtk, specDef, poisAtk, poisDef,
                    treasure, treasureChamp, treasureFirst);
            }

            return table.GetFullScript();
        }


        private string GetQualityType(int p)
        {
            switch (p)
            {
                case 4:
                    return "Unique";
                case 5:
                    return "Named";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the derived type returning either Beast, Demon, Spectral or Necro.
        /// </summary>
        /// <param name="unittype">unittype from the monster class</param>
        /// <returns>The unit type as a string.</returns>
        internal string GetUnitType(int unittype)
        {
            if (unittype == -1)
                return string.Empty;

            var unittypes = Manager.GetDataTable("UNITTYPES");
            var unit = unittypes.Rows[unittype];

            if (unittype == 566 || (int)unit["isA0"] == 566 || (int)unit["isA1"] == 566 || (int)unit["isA2"] == 566)
                return "Necro Lord";
            if (unittype == 567 || (int)unit["isA0"] == 567 || (int)unit["isA1"] == 567 || (int)unit["isA2"] == 567)
                return "Beast Lord";
            if (unittype == 568 || (int)unit["isA0"] == 568 || (int)unit["isA1"] == 568 || (int)unit["isA2"] == 568)
                return "Spectral Lord";
            if (unittype == 569 || (int)unit["isA0"] == 569 || (int)unit["isA1"] == 569 || (int)unit["isA2"] == 569)
                return "Demon Lord";

            if (unittype == 10 || (int)unit["isA0"] == 10 || (int)unit["isA1"] == 10 || (int)unit["isA2"] == 10)
                return "Necro";
            if (unittype == 11 || (int)unit["isA0"] == 11 || (int)unit["isA1"] == 11 || (int)unit["isA2"] == 11)
                return "Beast";
            if (unittype == 12 || (int)unit["isA0"] == 12 || (int)unit["isA1"] == 12 || (int)unit["isA2"] == 12)
                return "Spectral";
            if (unittype == 13 || (int)unit["isA0"] == 13 || (int)unit["isA1"] == 13 || (int)unit["isA2"] == 13)
                return "Demon";

            return string.Empty;
        }

        internal string GetDamageType(int dmgtype)
        {
            switch (dmgtype)
            {
                case 1:
                    return "Physical";
                case 2:
                    return "Fire";
                case 3:
                    return "Electricity";
                case 4:
                    return "Spectral";
                case 5:
                    return "Toxic";
                default:
                    return String.Empty;
            }
        }
    }
}
