using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Reanimator.Excel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form, IMdiChildBase
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableIndexDataSource
        {
            public int Unknowns1 { get; set; }
            public int Unknowns2 { get; set; }
            public int Unknowns3 { get; set; }
            public int Unknowns4 { get; set; }
        };

        readonly ExcelTable _excelTable;
        readonly StringsFile _stringsFile;
        readonly TableDataSet _tableDataSet;
        readonly Hashtable _bitmasks;
        private DataTable _dataTable;
        private bool _dataChanged;
        private bool _selectedIndexChange;
        private DataView _dataView;

        public ExcelTableForm(Object table, TableDataSet tableDataSet)
        {
            _excelTable = table as ExcelTable;
            _stringsFile = table as StringsFile;
            _tableDataSet = tableDataSet;
            _bitmasks = new Hashtable();
            _dataChanged = false;
            _selectedIndexChange = false;

            Init();

            ProgressForm progress = new ProgressForm(LoadTable, table);
            progress.ShowDialog(this);
            _dataTable.RowChanged += (sender, e) => { _dataChanged = true; };

            // make it look pretty
            rows_LayoutPanel.CellPaint += (sender, e) => { if (e.Row % 2 == 0) e.Graphics.FillRectangle(Brushes.AliceBlue, e.CellBounds); };
            rows_ListBox.SelectedIndex = 0;

            // fixes mouse scroll wheel
            // todo: this is dodgy and causes focused elements within the layoutpanel to lose focus (e.g. a text box) - rather anoying
            rows_LayoutPanel.Click += (sender, e) => rows_LayoutPanel.Focus();
            rows_LayoutPanel.MouseEnter += (sender, e) => rows_LayoutPanel.Focus();
        }

        private void ExcelTableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_dataChanged) return;

            DialogResult dr = MessageBox.Show("Table data has been changed.\nDo you wish to save changes to the cache?", "Question",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dr == DialogResult.No) return;

            if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            ProgressForm progress = new ProgressForm(SaveTable, _tableDataSet);
            progress.SetStyle(ProgressBarStyle.Marquee);
            progress.ShowDialog(this);
        }

        private static void SaveTable(ProgressForm progressBar, object o)
        {
            TableDataSet tableDataSet = o as TableDataSet;
            if (tableDataSet == null) return;

            progressBar.SetLoadingText("Please Wait...");
            progressBar.SetCurrentItemText("Saving updated cache data...");
            tableDataSet.SaveDataSet();
        }

        private void UseDataView()
        {
            String temp = dataGridView.DataMember;
            DataTable dataTable = _tableDataSet.XlsDataSet.Tables[temp];
            _dataView = dataTable.DefaultView;
            dataGridView.DataMember = null;
            dataGridView.DataSource = _dataView;
        }

        private void Init()
        {
            InitializeComponent();

            dataGridView.DoubleBuffered(true);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dataGridView.DataSource = _tableDataSet.XlsDataSet;
            dataGridView.DataMember = null;
            // dataGridView.DataSource = _dataView;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _dataView.Sort = tstb_sortCriteria.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "toolStripButton1_Click");
            }
        }

        private void LoadTable(ProgressForm progress, Object var)
        {
            // table tab
            dataGridView.SuspendLayout();
            _tableDataSet.LoadTable(progress, var);
            if (_stringsFile != null)
            {
                dataGridView.DataMember = _stringsFile.Name;
                _dataTable = _tableDataSet.XlsDataSet.Tables[_stringsFile.Name];
            }
            else if (_excelTable != null)
            {
                dataGridView.DataMember = _excelTable.StringId;
                _dataTable = _tableDataSet.XlsDataSet.Tables[_excelTable.StringId];
            }
            else
            {
                dataGridView.ResumeLayout();
                return;
            }
            dataGridView.ResumeLayout();


            // strings tab
            listBox1.SuspendLayout();
            if (_excelTable.SecondaryStrings != null)
            {
                listBox1.DataSource = _excelTable.SecondaryStrings;
            }
            listBox1.ResumeLayout();


            // table sort index tab
            dataGridView2.SuspendLayout();
            // TODO do better
            // TODO remove me once unknowns no longer unknowns
            List<TableIndexDataSource> tdsList = new List<TableIndexDataSource>();
            int[][] intArrays = { _excelTable.TableIndicies, _excelTable.SortIndex1, _excelTable.SortIndex2, _excelTable.SortIndex3, _excelTable.SortIndex4 };
            for (int i = 0; i < intArrays.Length; i++)
            {
                if (intArrays[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < intArrays[i].Length; j++)
                {
                    if (tdsList.Count <= j)
                    {
                        tdsList.Add(new TableIndexDataSource());
                    }

                    TableIndexDataSource tds = tdsList[j];
                    switch (i)
                    {
                        case 0:
                            // should we still use the "official" one?
                            // or leave as autogenerated - has anyone ever seen it NOT be ascending from 0?
                            // TODO
                            //dataGridView[i, j].Value = intArrays[i][j];
                            break;
                        case 1:
                            tds.Unknowns1 = intArrays[i][j];
                            break;
                        case 2:
                            tds.Unknowns2 = intArrays[i][j];
                            break;
                        case 3:
                            tds.Unknowns3 = intArrays[i][j];
                            break;
                        case 4:
                            tds.Unknowns4 = intArrays[i][j];
                            break;
                    }
                }
            }
            dataGridView2.DataSource = tdsList.ToArray();
            dataGridView2.ResumeLayout();


            // list view tab
            int rowCount = _dataTable.Rows.Count;
            Object[] rows = new Object[rowCount];
            int row = 0;
            foreach (DataRow dr in _dataTable.Rows)
            {
                rows[row++] = dr[0] + ": " + dr[1];
            }
            rows_ListBox.SuspendLayout();
            rows_ListBox.Items.AddRange(rows);
            rows_ListBox.SelectedIndexChanged += rows_ListBox_SelectedIndexChanged;
            rows_ListBox.ResumeLayout();

            rows_LayoutPanel.SuspendLayout();
            int column = 0;
            TextBox relationTextBox = null;
            foreach (DataColumn dc in _dataTable.Columns)
            {
                new Label { Text = dc.ColumnName, Parent = rows_LayoutPanel, AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft};

                if (dc.ExtendedProperties.ContainsKey(ExcelTable.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelTable.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox clb = new CheckedListBox
                                             {
                                                 Parent = rows_LayoutPanel,
                                                 AutoSize = true,
                                                 Dock = DockStyle.Fill,
                                                 MultiColumn = false,
                                                 Name = dc.ColumnName
                                             };

                    clb.ItemCheck += clb_ItemCheck;
                    _bitmasks.Add(dc.ColumnName, clb);

                    Type cellType = dc.DataType;
                    foreach (Enum type in Enum.GetValues(cellType))
                    {
                        clb.Items.Add(type, false);
                    }
                }
                else
                {
                    TextBox tb = new TextBox { Text = String.Empty, Parent = rows_LayoutPanel, AutoSize = true, Dock = DockStyle.Fill};
                    tb.DataBindings.Add("Text", _dataTable, dc.ColumnName);

                    if (dc.ExtendedProperties.ContainsKey(ExcelTable.ColumnTypeKeys.IsRelationGenerated) && (bool)dc.ExtendedProperties[ExcelTable.ColumnTypeKeys.IsRelationGenerated] || column == 0)
                    {
                        tb.ReadOnly = true;
                        if (relationTextBox != null)
                            relationTextBox.TextChanged +=
                                (sender, e) =>
                                    {
                                        tb.ResetText();
                                    };
                    }

                    if ((dc.ExtendedProperties.ContainsKey(ExcelTable.ColumnTypeKeys.IsStringIndex) && (bool)dc.ExtendedProperties[ExcelTable.ColumnTypeKeys.IsStringIndex]) ||
                        (dc.ExtendedProperties.ContainsKey(ExcelTable.ColumnTypeKeys.IsStringId) && (bool)dc.ExtendedProperties[ExcelTable.ColumnTypeKeys.IsStringId]))
                    {
                        relationTextBox = tb;
                    }
                    else
                    {
                        relationTextBox = null;
                    }
                }
                
                column++;
            }

            new Label {Text = String.Empty, Parent = rows_LayoutPanel, AutoSize = true, Dock = DockStyle.Fill};
            rows_LayoutPanel.ResumeLayout();
        }

        void clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckedListBox clb = sender as CheckedListBox;
            if (clb == null) return;

            DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
            uint value = (uint) dr[clb.Name];
            value ^= (uint) ((1 << (e.Index))*(e.NewValue == CheckState.Checked ? 1 : 0));
            dr[clb.Name] = value;
        }

        static void UpdateCheckedListBox(CheckedListBox clb, DataRow dr, DataColumn dc)
        {
            uint value = (uint) dr[dc];
            for (int i = 0; i < clb.Items.Count; i++)
            {
                clb.SetItemChecked(i, ((1 << i) & value) > 0 ? true : false);
            }
        }

        void rows_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedIndexChange = true;

            rows_LayoutPanel.SuspendLayout();
            BindingContext[_dataTable].Position = rows_ListBox.SelectedIndex;

            foreach (DataColumn dc in _dataTable.Columns)
            {
                if (dc.ExtendedProperties.ContainsKey(ExcelTable.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelTable.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox clb = _bitmasks[dc.ColumnName] as CheckedListBox;
                    if (clb == null) continue;

                    DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
                    UpdateCheckedListBox(clb, dr, dc);
                }
            }

            rows_LayoutPanel.ResumeLayout();

            _selectedIndexChange = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            /*DataTable affixTable = xlsDataSet.Tables["AFFIXES"];
            EnumerableRowCollection<DataRow> query = from affix in affixTable.AsEnumerable()
                                                     where affix.Field<string>("affix").CompareTo("-1") != 0
                                                     orderby affix.Field<string>("affix_string")
                                                     select affix;

            DataView view = query.AsDataView();
            */

            /*   EnumerableRowCollection<DataRow> query2 = from affix in view.GetEnumerator()
                                                        where affix.Field<string>("affix_string").StartsWith("Pet")
                                                        orderby affix.Field<string>("affix_string")
                                                        select affix;

               view = query2.AsDataView();
            dataGridView.DataSource = view;
            dataGridView.DataMember = null;*/


            //DataTable dataTable = xlsDataSet.Tables[0];
            //   DataRow[] dataRows = dataTable.Select("name = 'goggles'");
        }

        public void SaveButton()
        {
            DataTable table = ((DataSet)dataGridView.DataSource).Tables[dataGridView.DataMember];
            if (table == null) return;

            bool saveResults;

            if (_stringsFile == null)
            {
                String savePath = FileTools.SaveFileDiag("txt.cooked", "Excel Cooked", _excelTable.StringId.ToLower(), Config.DataDirsRoot);
                if (String.IsNullOrEmpty(savePath)) return;

                byte[] excelFileData = _excelTable.GenerateExcelFile((DataSet)dataGridView.DataSource);
                if (excelFileData == null || excelFileData.Length == 0)
                {
                    MessageBox.Show("Failed to generate excel table data!", "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                saveResults = FileTools.WriteFile(savePath, excelFileData);
            }
            else
            {
                String savePath = FileTools.SaveFileDiag(StringsFile.FileExtention, "Strings Cooked",
                                                         _stringsFile.Name.ToLower(),
                                                         _stringsFile.FilePath.Substring(0, _stringsFile.FilePath.LastIndexOf(@"\")));
                if (String.IsNullOrEmpty(savePath)) return;

                byte[] stringsFileData = _stringsFile.GenerateStringsFile(table);
                if (stringsFileData == null || stringsFileData.Length == 0)
                {
                    MessageBox.Show("Failed to generate string byte data!", "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                saveResults = FileTools.WriteFile(savePath, stringsFileData);
            }

            if (saveResults)
            {
                MessageBox.Show("File saved Successfully!", "Completed", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void regenTable_Click(object sender, EventArgs e)
        {
            // make sure we're trying to rebuild an excel table, and that it's actually in the dataset
            if (_excelTable == null) return;
            if (!_tableDataSet.XlsDataSet.Tables.Contains(_excelTable.StringId)) return;

            // remove from view or die, lol
            dataGridView.DataMember = null;
            dataGridView.DataSource = null;
            listBox1.DataSource = null;

            // remove and reload
            DataTable dt = _tableDataSet.XlsDataSet.Tables[_excelTable.StringId];
            if (dt.ChildRelations.Count != 0)
            {
                dt.ChildRelations.Clear();
            }
            if (dt.ParentRelations.Count != 0)
            {
                MessageBox.Show("Warning - Has Parent Relations!\nTest Me!\n\nPossible cache dataset relations issue!", "Warning", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                dt.ParentRelations.Clear();
            }

            _tableDataSet.XlsDataSet.Tables.Remove(_excelTable.StringId);
            _tableDataSet.LoadTable(null, _excelTable);
            _tableDataSet.GenerateRelations(_excelTable);

            // display updated table
            MessageBox.Show("Table regenerated!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // todo: when adding new columns the window will need to be close/reopened to show the changes
            // the dataGridView is storing its own little cache or something - 
            dataGridView.Refresh();
            dataGridView.DataSource = _tableDataSet.XlsDataSet;
            dataGridView.DataMember = _excelTable.StringId;
            listBox1.DataSource = _excelTable.SecondaryStrings;
            _dataChanged = true;

            MessageBox.Show(
                "Attention: Currently a bug exists such that you must close this form and re-open it to see any changes for the regeneration.\nDoing so will ask if you wish to apply your changes to the cache data.\n\nAlso of note is the you can't edit any cells until you close the window - FIX ME.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}