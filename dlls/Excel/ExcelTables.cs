using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace Reanimator.Excel
{
    public class ExcelTables : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ExcelTableTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stringId;
            public Int16 id;
        }

        List<ExcelTableTable> excelTables;

        class TableIndexManager
        {
            class TableIndexHelper
            {
                public string stringId;
                public string fileName;
                public ExcelTable excelTable;
                public Type type;

                public TableIndexHelper(string id, string name, ExcelTable table, Type t)
                {
                    stringId = id;
                    fileName = name;
                    excelTable = table;
                    type = t;
                }
            }

            List<TableIndexHelper> tables;

            public TableIndexManager()
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
                if (tableIndex.fileName == null)
                {
                    return id;
                }

                return tableIndex.fileName;
            }

            public void CreateTable(string id, byte[] buffer)
            {
                TableIndexHelper tableIndex = GetTableIndex(id);
                if (tableIndex != null)
                {
                    tableIndex.excelTable = (ExcelTable)Activator.CreateInstance(tableIndex.type, buffer);
                }
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
                    if (tableIndex.stringId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        return tableIndex;
                    }
                }

                return null;
            }
        }

        TableIndexManager tables;

        public ExcelTables(byte[] data)
            : base(data)
        {
            tables = new TableIndexManager();
            tables.AddTable("AFFIXES", null, typeof(Excel.Affixes));
            tables.AddTable("ITEMQUALITY", null, typeof(Excel.ItemQuality));
            tables.AddTable("ITEMS", null, typeof(Excel.Items));
            tables.AddTable("ITEM_LEVELS", null, typeof(Excel.ItemLevels));
            tables.AddTable("STATES", null, typeof(Excel.States));
            tables.AddTable("STATS", null, typeof(Excel.Stats));
            tables.AddTable("TREASURE", null, typeof(Excel.Treasure));
        }

        public override object GetTableArray()
        {
            return excelTables.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            excelTables = ReadTables<ExcelTableTable>(data, ref offset, Count);
        }

        public string GetTableStringId(int index)
        {
            return excelTables[index].stringId;
        }

        public ExcelTable GetTable(string stringId)
        {
            return tables.GetTable(stringId);
        }

        public bool LoadTables(string folder, Label label)
        {
            for (int i = 0; i < Count; i++)
            {
                string stringId = GetTableStringId(i);
                string fileName = tables.GetReplacement(stringId);

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
                        continue;
                    }
                }

                byte[] buffer = FileTools.StreamToByteArray(cookedFile);
                try
                {
                    tables.CreateTable(stringId, buffer);
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
