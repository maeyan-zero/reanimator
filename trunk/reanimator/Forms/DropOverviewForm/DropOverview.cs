using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;
using System.Diagnostics;

namespace Reanimator.Forms.DropOverviewForm
{
    public partial class DropOverview : Form
    {
        const int NAMECOLUMNINDEX = 2;
        FileManager _fileManager;
        //TableDataSet _dataSet;
        //FileExplorer _explorer;
        DataTable _treasureTable;
        DataTable _itemsTable;
        DataTable _unitTypesTable;
        DataTable _itemQualityTable;

        public DropOverview(FileManager fileManager)
        {
            InitializeComponent();

            _fileManager = fileManager;

            _treasureTable = _fileManager.GetDataTable("TREASURE");
            _itemsTable = _fileManager.GetDataTable("ITEMS");
            _unitTypesTable = _fileManager.GetDataTable("UNITTYPES");
            _itemQualityTable = _fileManager.GetDataTable("ITEM_QUALITY");

            for (int counter = 0; counter < _treasureTable.Rows.Count; counter++)
            {
                DataRow row = _treasureTable.Rows[counter];
                string name = (string)row[NAMECOLUMNINDEX];

                if(name != "")
                {
                    cb_dropTables.Items.Add(name);
                    cb_dropTables.SelectedIndex = 0;
                }
            }
        }

        private void cb_dropTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataRow row = GetRowFromName(_treasureTable, cb_dropTables.Text);
                TreasureTableEntry entry;

                if (row != null)
                {
                    TreeNode node = new TreeNode(cb_dropTables.Text);
                    entry = ParseRow(row, 1.0f, node);
                    tv_drops.Nodes.Clear();
                    tv_drops.Nodes.Add(node);
                }

                if (cb_expandTree.Checked)
                {
                    tv_drops.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SelectedIndexChanged");
            }
        }

        private DataRow GetRowFromName(DataTable table, string rowName)
        {
            try
            {
                for (int counter = 0; counter < table.Rows.Count; counter++)
                {
                    DataRow row = table.Rows[counter];
                    string name = (string)row[NAMECOLUMNINDEX];

                    if (name == rowName)
                    {
                        return row;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GetRowFromName");
                return null;
            }
        }

        private TreasureTableEntry ParseRow(DataRow row, float dropChance, TreeNode node)
        {
            //the index of the first item entry 13 for SP, 19 for MP
            int tablePosition = 13;

            if ((_fileManager.ClientVersion & FileManager.ClientVersionFlags.Resurrection) == FileManager.ClientVersionFlags.Resurrection)
            {
                tablePosition = 19;
            }

            float noDrop = 0;
            bool picks = false;
            string drops = "";

            try
            {
                bool pickTypes = (int)row["pickType"] != 0;
                string pickString = (string)row["picks"];
                picks = !pickString.StartsWith("0");

                noDrop = (float)row["noDrop"];
                node.Tag = noDrop;
                drops = string.Format("{0:0.00}", 100 - noDrop) + "%";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ParseRow(1)");
            }
            try
            {
                TreasureTableEntry tableEntry = new TreasureTableEntry();

 
                int numberOfEntries = CountItemEntries(row, picks, tablePosition);

                float generalDropChance = dropChance * (1.0f - noDrop / 100) * (1.0f / numberOfEntries);

                for (int counter = 1; counter <= 8; counter++)
                {
                    TreasureTableItemEntry entry = new TreasureTableItemEntry();

                    string entryString = (string)row[tablePosition];
                    Debug.Assert(entryString != null);

                    string[] entries = entryString.Split(',');
                    entry.tableToUse = int.Parse(entries[0]);
                    tablePosition++;
                    entry.entryId = int.Parse(entries[1]);
                    entry.quantity = (int)row[tablePosition];
                    tablePosition++;

                    entry.dropChance = generalDropChance;

                    if (entry.tableToUse == 0)
                    {
                        break;
                    }

                    DataRow referencedRow = null;
                    if (entry.tableToUse == -1 && entry.entryId > -1)
                    {
                        //DataRow refRow = _treasureTable.Rows[entry.entryId];
                        //entry.name = (string)refRow[1];
                        //referencedRow = GetRowFromName(_treasureTable, entry.name);
                    }
                    else if (entry.tableToUse == 1 && entry.entryId > -1)
                    {
                        DataRow refRow = _itemsTable.Rows[entry.entryId];
                        entry.name = (string)refRow[NAMECOLUMNINDEX];
                        entry.tableType = "Items";
                        referencedRow = GetRowFromName(_itemsTable, entry.name);
                    }
                    else if (entry.tableToUse == 2 && entry.entryId > -1)
                    {
                        DataRow refRow = _unitTypesTable.Rows[entry.entryId];
                        entry.name = (string)refRow[NAMECOLUMNINDEX];
                        entry.tableType = "UnitTypes";
                        referencedRow = GetRowFromName(_unitTypesTable, entry.name);
                    }
                    else if (entry.tableToUse == 3 && entry.entryId > -1)
                    {
                        DataRow refRow = _treasureTable.Rows[entry.entryId];
                        entry.name = (string)refRow[NAMECOLUMNINDEX];
                        entry.tableType = "Treasure";
                        referencedRow = GetRowFromName(_treasureTable, entry.name);
                    }
                    else if (entry.tableToUse == 4 && entry.entryId > -1)
                    {
                        DataRow refRow = _itemQualityTable.Rows[entry.entryId];
                        string name = (string)refRow[NAMECOLUMNINDEX];
                        entry.name = string.Format("All except \"{0}\"", name);
                        entry.tableType = "ItemQuality";
                        referencedRow = GetRowFromName(_itemQualityTable, name);
                    }
                    else if (entry.tableToUse == 5 && entry.entryId > -1)
                    {
                        //DataRow refRow = _treasureTable.Rows[entry.entryId];
                        //entry.name = (string)refRow[1];
                        //referencedRow = GetRowFromName(_treasureTable, entry.name);
                    }

                    if (referencedRow != null)
                    {
                        string chance = string.Format("{0:0.00}", entry.dropChance * 100) + "%";

                        if (entry.name == string.Empty)
                        {
                            MessageBox.Show(string.Format("The referenced entry ({0}[{1}]) doesn't have a name! Please check if the reference points to a valid item.", entry.tableType, entry.entryId), "Warning!");
                        }

                        TreeNode tNode = new TreeNode(entry.tableType + "[" + entry.entryId + "] (" + drops + "): " + entry.name + " - " + chance);

                        if (entry.tableToUse == 3)
                        {
                            entry.treasureTableEntry = ParseRow(referencedRow, entry.dropChance, tNode);
                        }

                        //if only one item will be picked, color the name of the entry red
                        if (picks == true)
                        {
                            tNode.ForeColor = Color.Red;
                        }
                        else
                        {
                            tNode.ForeColor = Color.Black;
                        }

                        node.Nodes.Add(tNode);
                    }

                    if (entry.dropChance * 100 <= 0.05)
                    {
                        MessageBox.Show("Warning! The drop rate of the item " + entry.name + " is pretty low (0.05%)! (" + entry.dropChance * 100 + "%)");
                    }

                    if (entry.tableToUse != 0)
                    {
                        tableEntry.itemEntries.Add(entry);
                    }
                }

                return tableEntry;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ParseRow(2)");
                return null;
            }
        }

        private int CountItemEntries(DataRow row, bool picks, int tablePosition)
        {
            try
            {
                int numberOfEntries = 0;

                if (picks)
                {
                    //iterate over all 8 item entries
                    for (int counter = 1; counter <= 8; counter++)
                    {
                        //check if the entry is actually used (first number != 0)
                        string entryString = (string)row[tablePosition + (counter - 1) * 2];
                        Debug.Assert(entryString != null);
                        bool used = !entryString.StartsWith("0");

                        if (used)
                        {
                            numberOfEntries++;
                        }
                    }

                    if (numberOfEntries == 0)
                    {
                        MessageBox.Show("Entry doesn't contain any items!", "Warning!");
                    }
                }
                else
                {
                    numberOfEntries = 1;
                }
                return numberOfEntries;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CountItemEntries");
                return 0;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_expandTree.Checked)
            {
                tv_drops.ExpandAll();
            }
            else
            {
                tv_drops.CollapseAll();
            }
        }
    }


    public class TreasureTableEntry
    {
        public List<TreasureTableItemEntry> itemEntries;

        public TreasureTableEntry()
        {
            itemEntries = new List<TreasureTableItemEntry>();
        }
    }

    public class TreasureTableItemEntry
    {
        public string tableType;
        public int tableToUse;
        public int entryId;
        public int quantity;
        public float dropChance;
        public string name;
        public TreasureTableEntry treasureTableEntry;

        public override string ToString()
        {
            return name + " - " + dropChance + "%";
        }
    }
}
