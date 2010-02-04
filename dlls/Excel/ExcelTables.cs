using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Reanimator.Excel
{
    public class ExcelTables : ExcelTable
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public class ExcelOutputAttribute : System.Attribute
        {
            public bool IsStringOffset { get; set; }
            public bool IsIntOffset { get; set; }
            public String[] FieldNames { get; set; }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ExcelTableTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stringId;
            public Int16 id;
        }

        class ExcelTableManagerManager
        {
            class TableIndexHelper
            {
                public string StringId { get; set; }
                public string FileName { get; set; }
                public ExcelTable excelTable;
                public Type type;

                public TableIndexHelper(string id, string name, ExcelTable table, Type t)
                {
                    StringId = id;
                    FileName = name;
                    excelTable = table;
                    type = t;
                }
            }

            List<TableIndexHelper> tables;

            public ExcelTableManagerManager()
            {
                tables = new List<TableIndexHelper>();
            }

            public void AddTable(string stringId, string fileName, Type type)
            {
                if (GetTableIndex(stringId) == null)
                {
                    tables.Add(new TableIndexHelper(stringId, fileName, null, type));
                }
            }

            public string GetReplacement(string id)
            {
                TableIndexHelper tableIndex = GetTableIndex(id);

                if (tableIndex == null)
                {
                    return id;
                }
                if (tableIndex.FileName == null)
                {
                    return id;
                }

                return tableIndex.FileName;
            }

            public ExcelTable CreateTable(string id, byte[] buffer)
            {
                TableIndexHelper tableIndex = GetTableIndex(id);
                if (tableIndex != null)
                {
                    tableIndex.excelTable = (ExcelTable)Activator.CreateInstance(tableIndex.type, buffer);
                    tableIndex.excelTable.StringId = tableIndex.StringId;
                    return tableIndex.excelTable;
                }

                return null;
            }

            public ExcelTable GetTable(string stringId)
            {
                TableIndexHelper tableIndex = GetTableIndex(stringId);
                if (tableIndex != null)
                {
                    return tableIndex.excelTable;
                }

                return null;
            }

            private TableIndexHelper GetTableIndex(string id)
            {
                foreach (TableIndexHelper tableIndex in tables)
                {
                    if (tableIndex.StringId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        return tableIndex;
                    }
                    else if (tableIndex.FileName != null)
                    {
                        if (tableIndex.FileName.Equals(id, StringComparison.OrdinalIgnoreCase))
                        {
                            return tableIndex;
                        }
                    }
                }

                return null;
            }
        }

        ExcelTableManagerManager excelTables;

        public ExcelTables(byte[] data)
            : base(data)
        {
            excelTables = new ExcelTableManagerManager();
            excelTables.AddTable("AFFIXES", null, typeof(Excel.Affixes));
            excelTables.AddTable("AICOMMONSTATE", null, typeof(Excel.AiCommonState));
            excelTables.AddTable("BOOKMARKS", null, typeof(Excel.AiCommonState));
            excelTables.AddTable("CONDITIONFUNCTIONS", null, typeof(Excel.ConditionFunctions));
            excelTables.AddTable("DAMAGETYPES", null, typeof(Excel.DamageTypes));
            excelTables.AddTable("DIALOG", null, typeof(Excel.Dialog));
            excelTables.AddTable("DIFFICULTY", null, typeof(Excel.Difficulty));
            excelTables.AddTable("FILTER_CHATFILTER", "CHATFILTER", typeof(Excel.Filter));
            excelTables.AddTable("FILTER_NAMEFILTER", "NAMEFILTER", typeof(Excel.Filter));
            excelTables.AddTable("FONTCOLOR", null, typeof(Excel.FontColor));
            excelTables.AddTable("GAME_GLOBALS", "GAMEGLOBALS", typeof(Excel.GameGlobals));
            excelTables.AddTable("GLOBAL_INDEX", "GLOBALINDEX", typeof(Excel.GlobalIndex));
            excelTables.AddTable("GLOBAL_STRING", "GLOBALSTRING", typeof(Excel.GlobalIndex));
            excelTables.AddTable("INTERACT", null, typeof(Excel.Interact));
            excelTables.AddTable("INTERACTMENU", null, typeof(Excel.InteractMenu));
            excelTables.AddTable("INVENTORY", null, typeof(Excel.Inventory));
            excelTables.AddTable("INVENTORYTYPES", null, typeof(Excel.InventoryTypes));
            excelTables.AddTable("INVLOC", null, typeof(Excel.InvLoc));
            excelTables.AddTable("ITEM_LEVELS", null, typeof(Excel.ItemLevels));
            excelTables.AddTable("ITEM_QUALITY", "ITEMQUALITY", typeof(Excel.ItemQuality));
            excelTables.AddTable("ITEMS", null, typeof(Excel.Items));
            excelTables.AddTable("LEVEL", "LEVELS", typeof(Excel.Levels));
            excelTables.AddTable("MISSILES", null, typeof(Excel.Items));
            excelTables.AddTable("MONSTERS", null, typeof(Excel.Items));
            excelTables.AddTable("OBJECTS", null, typeof(Excel.Items));
            excelTables.AddTable("PLAYERS", null, typeof(Excel.Items));
            excelTables.AddTable("PROCS", null, typeof(Excel.Procs));
            excelTables.AddTable("SKILLS", null, typeof(Excel.Skills));
            excelTables.AddTable("STATES", null, typeof(Excel.States));
            excelTables.AddTable("STATS", null, typeof(Excel.Stats));
            excelTables.AddTable("TREASURE", null, typeof(Excel.Treasure));
        }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ExcelTableTable>(data, ref offset, Count);
        }

        public string GetTableStringId(int index)
        {
            return ((ExcelTableTable)tables[index]).stringId;
        }

        public ExcelTable GetTable(string stringId)
        {
            return excelTables.GetTable(stringId);
        }

        public bool LoadTables(string folder, Label label, ListBox excelTablesLoaded)
        {
            excelTablesLoaded.Sorted = true;

            for (int i = 0; i < Count; i++)
            {
                string stringId = GetTableStringId(i);
                string fileName = excelTables.GetReplacement(stringId);

                string filePath = folder + "\\" + fileName + ".txt.cooked";
                FileStream cookedFile;

                string currentItem = fileName.ToLower() + ".txt.cooked";
                label.Text = currentItem;

                try
                {
                    cookedFile = new FileStream(filePath, FileMode.Open);
                }
                catch (Exception)
                {
                    try
                    {
                        filePath = filePath.Replace("_common", "");
                        cookedFile = new FileStream(filePath, FileMode.Open);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("file not found: " + filePath);
                        continue;
                    }
                }

                byte[] buffer = FileTools.StreamToByteArray(cookedFile);
                try
                {
                    ExcelTable excelTable = excelTables.CreateTable(stringId, buffer);
                    if (excelTable != null)
                    {
                        excelTablesLoaded.Items.Add(excelTable);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to parse cooked file " + currentItem + "\n\n" + e.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                if (cookedFile != null)
                {
                    cookedFile.Dispose();
                }
            }

            return true;
        }
    }
}
