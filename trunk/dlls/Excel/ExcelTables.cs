using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace Reanimator.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct ExcelTables_Table
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] unknown;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szStringId;
        public Int16 id;
    }

    public class ExcelTables : ExcelTable
    {
        List<ExcelTables_Table> tables;
        Stats stats;
        public Stats Stats
        {
            get { return stats; }
        }

        States states;
        public States States
        {
            get { return states; }
        }

        Items items;
        public Items Items
        {
            get { return items; }
        }

        ItemLevels itemLevels;
        public ItemLevels ItemLevels
        {
            get { return itemLevels; }
        }

        public ExcelTables(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return null;
        }

        protected override void ParseTables(byte[] data)
        {
            tables = ReadTables<ExcelTables_Table>(data, ref offset, Count);
        }

        public string GetTableStringId(int index)
        {
            return tables[index].szStringId;
        }

        public bool LoadTables(string szFolder, Label label)
        {
            for (int i = 0; i < Count; i++)
            {
                string szStringId = GetTableStringId(i);
                string szFileName = szFolder + "\\" + szStringId + ".txt.cooked";
                FileStream cookedFile;

                string currentItem = szStringId.ToLower() + ".txt.cooked";
                label.Text = currentItem;

                try
                {
                    cookedFile = new FileStream(szFileName, FileMode.Open);
                }
                catch (Exception)
                {
                    try
                    {
                        szFileName = szFileName.Replace("_common", "");
                        cookedFile = new FileStream(szFileName, FileMode.Open);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                byte[] buffer = FileTools.StreamToByteArray(cookedFile);
                try
                {
                    if (szStringId.Equals("STATS", StringComparison.OrdinalIgnoreCase))
                    {
                        stats = new Stats(buffer);
                    }
                    else if (szStringId.Equals("STATES", StringComparison.OrdinalIgnoreCase))
                    {
                        states = new States(buffer);
                    }
                    else if (szStringId.Equals("ITEMS", StringComparison.OrdinalIgnoreCase))
                    {
                        items = new Items(buffer);
                    }
                    else if (szStringId.Equals("ITEM_LEVELS", StringComparison.OrdinalIgnoreCase))
                    {
                        itemLevels = new ItemLevels(buffer);
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
