using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;

namespace Reanimator.Forms
{
    public partial class TableSearch : Form
    {
        //to parse all just pick any value above 168
        const int PARSE_THIS_MANY_TABLES_JUST_FOR_FASTER_TESTING = 200;

        TableDataSet _dataSet;
        ExcelTables _excelTables;
        List<DataTable> _tables;
        int _index;

        public TableSearch(ref TableDataSet dataSet, ref ExcelTables excelTables )
        {
            _dataSet = dataSet;
            _excelTables = excelTables;
            _tables = new List<DataTable>();

            InitializeComponent();
        }

        private void b_search_Click(object sender, EventArgs e)
        {
            _tables.Clear();
            string value = tb_searchString.Text;
            int counter = 0;

            foreach (DataTable table in _dataSet.XlsDataSet.Tables)
            {
                if (counter > PARSE_THIS_MANY_TABLES_JUST_FOR_FASTER_TESTING)
                {
                    break;
                }

                DataView view = new DataView(table);

                this.Text =  string.Format("Search Tables - {0} ({1}/{2})", table.TableName, counter, _dataSet.XlsDataSet.Tables.Count);
                counter++;

                bool add = false;
                DataTable tmp = new DataTable(table.TableName);
                foreach(DataColumn column in table.Columns)
                {
                    view.RowFilter = string.Format("Convert({0}, 'System.String') LIKE '*{1}*'", column.ColumnName, value);
                    view.Sort = column.ColumnName;


                    if (view.Count > 0)
                    {
                        tmp.Merge(view.ToTable());
                        add = true;
                    }
                }

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

                    l_results.Text = _tables.Count + " matches found";
                }
                else
                {
                    l_results.Text = "No matches found";
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

                dgv_foundTables.Update();
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

                dgv_foundTables.Update();
            }
        }
    }
}
