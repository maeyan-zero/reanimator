using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hellgate;
using MediaWiki.Articles;

namespace MediaWiki.Util
{
    class Treasure
    {
        public static FileManager Manager { get; set; }

        private static readonly int[] Exclude = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 354, 355, 572, 793 };

        public string Name;
        public double NoDrop;
        public PickTypes PickType;
        public Bitmask01 SpawnCondition;
        public List<Drop> Drops = new List<Drop>(8);
        public int Difficulty;
        public Treasure Parent = null;

        // Artifical
        public double NormalChance = -1;
        public double NightmareChance = -1;
        public double HellChance = -1;

        public override bool Equals(object obj)
        {
            var t2 = obj as Treasure;
            if (t2 == null) return false;

            for (var i = 0; i < 8; i++)
                if (!Drops[i].Equals(t2.Drops[i])) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Drops.Aggregate(0, (current, drop) => current + drop.Content.GetHashCode() + (int) drop.Type * drop.Value);
        }

        public class Drop
        {
            public ClassType Type = 0;
            public object Content = -1;
            public int Value = -1;

            public override bool Equals(object obj)
            {
                var d2 = obj as Drop;
                if (d2 == null) return false;
                if (Content == null && d2.Content == null) return true;
                if (Content == null) return false;
                if (Type != d2.Type) return false;
                //if (Value != d2.Value) return false;
                return (Content.Equals((d2.Content)));
            }
        }

        public Treasure(DataRow data, Treasure parent = null)
        {
            Parent = parent;
            Name = data["treasureClass"].ToString();
            NoDrop = 100 - (float) data["noDrop"];
            PickType = (PickTypes) data["pickType"];
            SpawnCondition = (Bitmask01) data["spawnCondition"];
            Difficulty = (int) data["gameModeRestriction1"];
            for (var i = 0; i < 8; i++)
                Drops.Add(GetDropClass(data, i + 1));
        }

        internal Drop GetDropClass(DataRow data, int dropid)
        {
            var drop = new Drop();
            var item = data["item" + dropid].ToString().Split(',');
            var val1 = (ClassType) Int32.Parse(item[0]);
            var val2 = Int32.Parse(item[1]);
            if (val1 == ClassType.Treasure && Exclude.Contains(val2)) return drop;
            drop.Type = val1;
            drop.Content = GetDropContent(val1, val2);
            drop.Value = (int)data["value" + dropid];
            return drop;
        }

        internal object GetDropContent(ClassType val1, int val2)
        {
            switch (val1)
            {
                case ClassType.Item:
                    var itemTable = Manager.GetDataTable("ITEMS");
                    var itemRow = itemTable.Rows[val2];
                    var itemName = (string)itemRow["String_string"];
                    itemName = WikiScript.GetWikiArticleLink(itemName);
                    return itemName;
                case ClassType.Unit:
                    var unitTable = Manager.GetDataTable("UNITTYPES");
                    var unitRow = unitTable.Rows[val2];
                    var unitName = WikiScript.GetFormattedString((string)unitRow["type"]);
                    return unitName;
                case ClassType.Treasure:
                    var data = Manager.GetDataTable("TREASURE").Rows[val2];
                    return new Treasure(data, this);
                case ClassType.Quality:
                    var qualityTable = Manager.GetDataTable("ITEM_QUALITY");
                    var qualityRow = qualityTable.Rows[val2];
                    var qualityName = (string)qualityRow["displayName_string"];
                    return qualityName;
                case ClassType.Nothing:
                    return "Nothing";
                case 0:
                case ClassType.Test1:
                case ClassType.Test2:
                    return null;
                default:
                    throw new ArgumentException("Unknown drop type: " + val1);
            }
        }

        public void Simplify()
        {
            foreach (var drop in Drops.Where(drop => drop.Type == ClassType.Treasure))
            {
                if (!Simplify((Treasure)drop.Content))
                {
                    Simplify();
                    break;
                }
            }
        }

        private bool Simplify(Treasure treasure)
        {
            Treasure last = null;
            var doSimplify = false;

            foreach (var drop in treasure.Drops)
            {
                if (drop.Type == 0) continue;
                if (!(drop.Content is Treasure)) break;
                if (last != null)
                {
                    if (drop.Content.Equals(last))
                    {
                        doSimplify = true;
                        continue;
                    }
                    if (doSimplify)
                    {
                        treasure.Drops.Remove(drop);
                        treasure.Parent.Drops.Add(drop);
                        return false;
                    }
                }

                last = (Treasure) drop.Content;
            }

            if (doSimplify)
            {
                Drop lastDrop = null;
                foreach (var drop in treasure.Drops)
                {
                    var t = drop.Content as Treasure;
                    if (t == null) break;

                    if (lastDrop != null)
                        if (!(drop.Equals(lastDrop)))
                        {
                            lastDrop = drop;
                            continue;
                        }

                    lastDrop = drop;
                    switch (t.Difficulty)
                    {
                        case 0:
                            treasure.NormalChance = t.NoDrop;
                            break;
                        case 1:
                            treasure.NightmareChance = t.NoDrop;
                            break;
                        case 2:
                            treasure.HellChance = t.NoDrop;
                            break;
                    }

                    treasure.Drops = ((Treasure) drop.Content).Drops;
                }
            }
            return true;
        }

        public enum ClassType
        {
            Item = 1,
            Unit = 2,
            Treasure = 3,
            Quality = 4,
            Test1 = 5,
            Test2 = 6,
            Nothing = 7
        }

        [FlagsAttribute]
        public enum Bitmask01 : uint
        {
            CreateForAllPlayersInLevel = (1 << 0),
            RequiredUsableByOperator = (1 << 1),
            RequiredUsableBySpawner = (1 << 2),
            SubscriberOnly = (1 << 3),
            MaxSlots = (1 << 4),
            ResultsNotRequired = (1 << 5),
            StackTreasure = (1 << 6),
            MultiplayerOnly = (1 << 7),
            SinglePlayerOnly = (1 << 8),
            BaseOnPlayerLevel = (1 << 9)
        }

        public enum PickTypes
        {
            Null = -1,
            One = 0,
            All = 1,
            ModifiersOnly = 2,
            IndPercent = 3,
            OneEliminate = 4,
            FirstValid = 5
        }
    }
}
