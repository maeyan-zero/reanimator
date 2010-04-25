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
using System.Threading;

namespace Reanimator.Forms
{
    public partial class TableSearch : Form
    {
        Thread _searchThread;
        bool _started;
        TableDataSet _dataSet;
        ExcelTables _excelTables;
        List<DataTable> _tables;
        int _index;

        public TableSearch(ref TableDataSet dataSet, ref ExcelTables excelTables )
        {
            InitializeComponent();

            _started = false;

            _dataSet = dataSet;
            _excelTables = excelTables;
            _tables = new List<DataTable>();

            l_nOfTables.Text = _dataSet.XlsDataSet.Tables.Count.ToString();

            foreach (DataTable table in _dataSet.XlsDataSet.Tables)
            {
                clb_tablesToSearch.Items.Add(table.ToString(), true);
            }
        }

        private void StartSearch()
        {
            this.Invoke((MethodInvoker)delegate { tsb_search.Text = "Abort"; });
            _started = true;

            string searchString = tst_searchString.Text;

            _tables.Clear();
            this.Invoke((MethodInvoker)delegate { dgv_foundTables.DataSource = null; });

            int tableCounter = 0;
            int columnCounter = 0;

            this.Invoke((MethodInvoker)delegate { UpdatePosition(); });

            this.Invoke((MethodInvoker)delegate { tspb_progress.Maximum = clb_tablesToSearch.CheckedItems.Count - 1; });
            this.Invoke((MethodInvoker)delegate { tspb_progress.Value = 0; });

            foreach (DataTable table in _dataSet.XlsDataSet.Tables)
            {
                if (clb_tablesToSearch.CheckedItems.Contains(table.TableName))
                {
                    columnCounter = 0;

                    DataView view = new DataView(table);

                    bool add = false;
                    DataTable tmp = new DataTable(table.TableName);
                    foreach (DataColumn column in table.Columns)
                    {
                        view.RowFilter = string.Format("Convert({0}, 'System.String') LIKE '{1}'", column.ColumnName, searchString);
                        view.Sort = column.ColumnName;

                        string newTitle = string.Format("Searching Tables - {0} ({1}/{2}) - Column {3}/{4}", table.TableName, tableCounter + 1, clb_tablesToSearch.CheckedIndices.Count/*_dataSet.XlsDataSet.Tables.Count*/, columnCounter + 1, table.Columns.Count);
                        this.Invoke((MethodInvoker)delegate { this.Text = newTitle; });
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
                        //this.Invoke((MethodInvoker)delegate { dgv_foundTables.SuspendLayout(); });

                        if (_tables.Count == 1)
                        {
                            this.Invoke((MethodInvoker)delegate { dgv_foundTables.DataSource = _tables[0]; });
                        }

                        this.Invoke((MethodInvoker)delegate { l_tableName.Text = _tables[0].TableName; });

                        //this.Invoke((MethodInvoker)delegate { dgv_foundTables.ResumeLayout(); });

                        //this.Invoke((MethodInvoker)delegate { dgv_foundTables.Update(); });

                        this.Invoke((MethodInvoker)delegate { tsl_results.Text = _tables.Count + " matches found"; });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate { tsl_results.Text = "No matches found"; });
                    }

                    this.Invoke((MethodInvoker)delegate { UpdatePosition(); });
                    this.Invoke((MethodInvoker)delegate { tspb_progress.PerformStep(); });
                }
            }

            _started = false;
            this.Invoke((MethodInvoker)delegate { tsb_search.Text = "Start"; });
            this.Invoke((MethodInvoker)delegate { this.Text = "Searching Tables - Done"; });
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
            //_stopWatch.Reset();
            //_stopWatch.Start();

            if (clb_tablesToSearch.CheckedItems.Count == 0)
            {
                MessageBox.Show("You have to select at least one table to search in!");
                return;
            }

            if (!_started)
            {
                _searchThread = new Thread(StartSearch);
                _searchThread.Start();
            }
            else
            {
                tsb_search.Text = "Start";
                _searchThread.Abort();
                _started = false;
                this.Text = "Searching Tables - Aborted";
            }

            //_stopWatch.Stop();
            //MessageBox.Show(string.Format("{0}:{1}.{2}", _stopWatch.Elapsed.Minutes, _stopWatch.Elapsed.Seconds, _stopWatch.Elapsed.Milliseconds));
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

        private void TableSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_started)
            {
                _searchThread.Abort();
            }
        }
    }
}
