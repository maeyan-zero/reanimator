using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.Diagnostics;

namespace Reanimator.Forms
{
    public partial class TableSearch : Form
    {
        //to parse all just pick any value above 163 (the number of ExcelTables)
        const int PARSE_THIS_MANY_TABLES_JUST_FOR_FASTER_TESTING = 200;

        TableDataSet _dataSet;
        ExcelTables _excelTables;
        List<DataTable> _tables;
        int _index;

        public TableSearch(ref TableDataSet dataSet, ref ExcelTables excelTables )
        {
            InitializeComponent();

            _dataSet = dataSet;
            _excelTables = excelTables;
            _tables = new List<DataTable>();


            foreach (DataTable table in _dataSet.XlsDataSet.Tables)
            {
                clb_tablesToSearch.Items.Add(table.ToString(), true);
            }

            //foreach(ExcelTable table in excelTables.GetLoadedTables())
            //{
            //    clb_tablesToSearch.Items.Add(table.ToString(), true);

            //    //ToolStripMenuItem item = new ToolStripMenuItem(table.ToString());
            //    //item.CheckOnClick = true;
            //    //item.Click += new EventHandler(item_Click);
            //    //ttsddb_tablesToSearch.DropDownItems.Add(item);
            //}
        }

        ////keep that damn menu OPEN after an item was selected...
        //void item_Click(object sender, EventArgs e)
        //{
        //    ttsddb_tablesToSearch.ShowDropDown();
        //}

        private void StartSearch(string searchString)
        {
            _tables.Clear();
            dgv_foundTables.DataSource = null;

            int tableCounter = 0;
            int columnCounter = 0;

            UpdatePosition();

            foreach (DataTable table in _dataSet.XlsDataSet.Tables)
            {
                //ToolStripItemCollection col = ttsddb_tablesToSearch.DropDownItems;

                //ToolStripMenuItem item = (ToolStripMenuItem)(ttsddb_tablesToSearch.DropDownItems["{" + table.TableName + "}"]);

                if (clb_tablesToSearch.CheckedItems.Contains(table.TableName))
                {
                    columnCounter = 0;

                    if (tableCounter > PARSE_THIS_MANY_TABLES_JUST_FOR_FASTER_TESTING)
                    {
                        break;
                    }

                    DataView view = new DataView(table);

                    bool add = false;
                    DataTable tmp = new DataTable(table.TableName);
                    foreach (DataColumn column in table.Columns)
                    {
                        view.RowFilter = string.Format("Convert({0}, 'System.String') LIKE '{1}'", column.ColumnName, searchString);
                        view.Sort = column.ColumnName;

                        this.Text = string.Format("Searching Tables - {0} ({1}/{2}) - Column {3}/{4}", table.TableName, tableCounter + 1, clb_tablesToSearch.CheckedIndices.Count/*_dataSet.XlsDataSet.Tables.Count*/, columnCounter + 1, table.Columns.Count);
                        columnCounter++;

                        if (view.Count > 0)
                        {
                            tmp.Merge(view.ToTable());
                            add = true;
                        }
                    }

                    tableCounter++;

                    if (add)
                    {
                        _tables.Add(tmp);
                    }

                    _index = 0;

                    if (_tables.Count > 0)
                    {
                        dgv_foundTables.SuspendLayout();

                        dgv_foundTables.DataSource = _tables[0];
                        l_tableName.Text = _tables[0].TableName;

                        dgv_foundTables.ResumeLayout();

                        dgv_foundTables.Update();

                        tsl_results.Text = _tables.Count + " matches found";
                    }
                    else
                    {
                        tsl_results.Text = "No matches found";
                    }

                    UpdatePosition();
                }
            }
        }

        private void b_prev_Click(object sender, EventArgs e)
        {
            if (_index < 1)
            {
            }
            else
            {
                dgv_foundTables.SuspendLayout();

                _index--;
                dgv_foundTables.DataSource = _tables[_index];
                l_tableName.Text = _tables[_index].TableName;

                dgv_foundTables.ResumeLayout();

                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            if (_tables.Count > 1)
            {
                l_position.Text = string.Format("{0}/{1}", _index + 1, _tables.Count);
            }
            else
            {
                l_position.Text = "0/0";
            }
        }

        private void b_next_Click(object sender, EventArgs e)
        {
            if (_index < _tables.Count - 1)
            {
                dgv_foundTables.SuspendLayout();

                _index++;
                dgv_foundTables.DataSource = _tables[_index];
                l_tableName.Text = _tables[_index].TableName;

                dgv_foundTables.ResumeLayout();

                UpdatePosition();
            }
        }

        Stopwatch _stopWatch = new Stopwatch();
        private void tsb_search_Click(object sender, EventArgs e)
        {
            string searchString = tst_searchString.Text;

            _stopWatch.Reset();
            _stopWatch.Start();

            StartSearch(searchString);

            _stopWatch.Stop();
            MessageBox.Show(string.Format("{0}:{1}.{2}", _stopWatch.Elapsed.Minutes, _stopWatch.Elapsed.Seconds, _stopWatch.Elapsed.Milliseconds));
        }

        private void b_checkAll_Click(object sender, EventArgs e)
        {
            CheckItems(CheckState.Checked);
        }

        private void b_uncheckAll_Click(object sender, EventArgs e)
        {
            CheckItems(CheckState.Unchecked);
        }

        private void CheckItems(CheckState state)
        {
            clb_tablesToSearch.SuspendLayout();

            for (int counter = 0; counter < clb_tablesToSearch.Items.Count; counter++)
            {
                clb_tablesToSearch.SetItemCheckState(counter, state);
            }

            clb_tablesToSearch.ResumeLayout(); ;
        }
    }
}
