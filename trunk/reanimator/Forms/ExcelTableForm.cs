using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Hellgate;
using Revival.Common;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form, IMdiChildBase
    {
        class TableIndexDataSource
        {
            public int SortIndex1 { get; set; }
            public int SortIndex2 { get; set; }
            public int SortIndex3 { get; set; }
            public int SortIndex4 { get; set; }
        };

        readonly FileManager _fileManager;
        readonly Hashtable _specialControls;
        private DataTable _dataTable;
        private bool _dataChanged;
        private bool _selectedIndexChange;
        private DataView _dataView;

        private readonly DataFile _dataFile;
        private readonly ExcelFile _excelFile;
        private readonly StringsFile _stringsFile;

        private bool IsExcelFile { get { return _excelFile != null; } }
        private bool isStringsFile { get { return _stringsFile != null; } }

        public ExcelTableForm(DataFile dataFile, FileManager fileManager)
        {
            _dataFile = dataFile;
            _excelFile = _dataFile as ExcelFile;
            _stringsFile = _dataFile as StringsFile;
            _fileManager = fileManager;
            _specialControls = new Hashtable();
            _dataChanged = false;
            _selectedIndexChange = false;

            _Init();

            ProgressForm progress = new ProgressForm(_LoadTable, _dataFile);
            progress.SetStyle(ProgressBarStyle.Marquee);
            progress.SetLoadingText("Generating DataTable...");
            progress.SetCurrentItemText(String.Empty);
            progress.ShowDialog(this);
            _dataTable.RowChanged += (sender, e) => { _dataChanged = true; };


            //rows_ListBox.SelectedIndex = 0;

        }

        private void _UseDataView()
        {
            String temp = _tableData_DataGridView.DataMember;
            DataTable dataTable = _fileManager.XlsDataSet.Tables[temp];
            _dataView = dataTable.DefaultView;
            _tableData_DataGridView.DataMember = null;
            _tableData_DataGridView.DataSource = _dataView;
        }

        private void _Init()
        {
            InitializeComponent();

            _tableData_DataGridView.DoubleBuffered(true);
            _tableData_DataGridView.EnableHeadersVisualStyles = false;
            _tableData_DataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            _tableData_DataGridView.DataSource = _fileManager.XlsDataSet;
            _tableData_DataGridView.DataMember = null;
        }

        private TableLayoutPanel _RowViewFactory(TableLayoutPanel rows_LayoutPanel)
        {
            // make it look pretty
            rows_LayoutPanel.CellPaint += (sender, e) => { if (e.Row % 2 == 0) e.Graphics.FillRectangle(Brushes.AliceBlue, e.CellBounds); };
            rows_LayoutPanel.SuspendLayout();
            int column = 0;
            TextBox relationTextBox = null;
            foreach (DataColumn dc in _dataTable.Columns)
            {
                if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBool) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBool])
                {
                    CheckBox cb = new CheckBox
                    {
                        Parent = rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName,
                        Text = dc.ColumnName
                    };
                    rows_LayoutPanel.SetColumnSpan(cb, 2);

                    cb.CheckedChanged += _RowView_CheckBox_ItemCheck;
                    _specialControls.Add(dc.ColumnName, cb);

                    column++;
                    continue;
                }

                new Label
                {
                    Text = dc.ColumnName,
                    Parent = rows_LayoutPanel,
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox clb = new CheckedListBox
                    {
                        Parent = rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        MultiColumn = false,
                        Name = dc.ColumnName
                    };

                    clb.ItemCheck += _RowView_CheckedListBox_ItemCheck;
                    _specialControls.Add(dc.ColumnName, clb);

                    Type cellType = dc.DataType;

                    foreach (Enum type in Enum.GetValues(cellType))
                    {
                        clb.Items.Add(type, false);
                    }
                }
                else
                {
                    TextBox tb = new TextBox
                    {
                        Text = String.Empty,
                        Parent = rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill
                    };
                    tb.DataBindings.Add("Text", _dataTable, dc.ColumnName);

                    if ((dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsRelationGenerated) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsRelationGenerated]) || column == 0)
                    {
                        tb.ReadOnly = true;
                        if (relationTextBox != null) relationTextBox.TextChanged += (sender, e) => tb.ResetText();
                    }

                    if ((dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsStringIndex) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsStringIndex]) ||
                        (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsStringOffset) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsStringOffset]))
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

            new Label
            {
                Text = String.Empty,
                Parent = rows_LayoutPanel,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            rows_LayoutPanel.ResumeLayout();
            rows_LayoutPanel.Width += 10;


            // fixes mouse scroll wheel
            // todo: this is dodgy and causes focused elements within the layoutpanel to lose focus (e.g. a text box) - rather anoying
            rows_LayoutPanel.Click += (sender, e) => rows_LayoutPanel.Focus();
            rows_LayoutPanel.MouseEnter += (sender, e) => rows_LayoutPanel.Focus();

            return rows_LayoutPanel;
        }

        private void _LoadTable(ProgressForm progress, Object var)
        {
            DataFile dataFile = var as DataFile;
            if (dataFile == null) return;
            _dataTable = _fileManager.GetDataTable(dataFile.StringId);
            //int rowCount = _dataTable.Rows.Count;
            //Object[] rows = new Object[rowCount];
            //int row = 0;
            //foreach (DataRow dr in _dataTable.Rows)
            //{
            //    rows[row++] = dr[0] + ": " + dr[2];
            //}
            //rows_ListBox.SuspendLayout();
            //rows_ListBox.Items.AddRange(rows);
            //rows_ListBox.SelectedIndexChanged += _Rows_ListBox_SelectedIndexChanged;
            //rows_ListBox.ResumeLayout();

            tableLayoutPanel1 = _RowViewFactory(tableLayoutPanel1);

            //// Table View
            _tableData_DataGridView.SuspendLayout();
            _dataTable = _fileManager.LoadTable(dataFile, true);
            if (_dataTable == null)
            {
                MessageBox.Show("Failed to load DataTable!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // need to manually populate columns due to fillweight = 100 by default (overflow crap; 655 * 100 > max int)
            if (_dataTable.Columns.Count > 655)
            {
                _tableData_DataGridView.AutoGenerateColumns = false;
                DataGridViewColumn[] columns = new DataGridViewColumn[_dataTable.Columns.Count];

                int i = 0;
                foreach (DataGridViewTextBoxColumn dataGridViewColumn in
                    from DataColumn dataColumn in _dataTable.Columns
                    select new DataGridViewTextBoxColumn
                    {
                        Name = dataColumn.ColumnName,
                        FillWeight = 1,
                        DataPropertyName = dataColumn.ColumnName
                    })
                {
                    columns[i++] = dataGridViewColumn;
                }

                _tableData_DataGridView.Columns.AddRange(columns);
                _tableData_DataGridView.DataMember = _dataFile.StringId;
            }
            else
            {
                _tableData_DataGridView.DataMember = _dataFile.IsStringsFile ? FileManager.StringsTableName : _dataFile.StringId;
            }

            DataGridViewColumn codeColumn = _tableData_DataGridView.Columns["code"];
            if (codeColumn != null) codeColumn.DefaultCellStyle.Format = "X04";

            _tableData_DataGridView.ResumeLayout();

            //// List View



            //// Debug Stuff
            // secondary strings)
            if (_excelFile == null || !ExcelFile.EnableDebug || _excelFile.IndexSortArray == null)
            {
                tabControl1.TabPages.RemoveByKey("stringsPage");
                tabControl1.TabPages.RemoveByKey("indexArraysPage");
                return;
            }

            // strings tab
            if (_excelFile.SecondaryStrings != null && _excelFile.SecondaryStrings.Count > 0)
            {
                _strings_ListBox.SuspendLayout();
                _strings_ListBox.DataSource = _excelFile.SecondaryStrings;
                _strings_ListBox.ResumeLayout();
            }
            else
            {
                tabControl1.TabPages.RemoveByKey("stringsPage");
            }

            // is messy, but all we need for debugging
            List<TableIndexDataSource> tidsList = new List<TableIndexDataSource>();
            for (int i = 0; i < _excelFile.IndexSortArray.Count; i++)
            {
                for (int j = 0; j < _excelFile.IndexSortArray[i].Count(); j++)
                {
                    TableIndexDataSource tids;
                    if (tidsList.Count <= j)
                    {
                        tids = new TableIndexDataSource();
                        tidsList.Add(tids);
                    }
                    else
                    {
                        tids = tidsList[j];
                    }

                    if (i == 0) tids.SortIndex1 = _excelFile.IndexSortArray[i][j];
                    if (i == 1) tids.SortIndex2 = _excelFile.IndexSortArray[i][j];
                    if (i == 2) tids.SortIndex3 = _excelFile.IndexSortArray[i][j];
                    if (i == 3) tids.SortIndex4 = _excelFile.IndexSortArray[i][j];
                }
            }

            if (tidsList.Count == 0)
            {
                tabControl1.TabPages.RemoveByKey("indexArraysPage");
            }

            indexArrays_DataGridView.SuspendLayout();
            indexArrays_DataGridView.DataSource = tidsList.ToArray();
            indexArrays_DataGridView.ResumeLayout();
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            NumericUpDown nud = sender as NumericUpDown;
            if (nud == null) return;

            // DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
            // dr[nud.Name] = (cb.CheckState == CheckState.Checked ? 1 : 0);
        }

        private void _RowView_CheckBox_ItemCheck(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckBox cb = (CheckBox) sender;
            DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
            dr[cb.Name] = (cb.CheckState == CheckState.Checked ? 1 : 0);
        }

        private void _RowView_CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckedListBox clb = (CheckedListBox) sender;
            DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
            uint value = (uint)dr[clb.Name];
            value ^= (uint)(1 << (e.Index));
            dr[clb.Name] = value;
        }

        private static void _UpdateCheckedListBox(CheckedListBox clb, DataRow dr, DataColumn dc)
        {
            uint value = (uint)dr[dc];
            for (int i = 0; i < clb.Items.Count; i++)
            {
                clb.SetItemChecked(i, ((1 << i) & value) > 0 ? true : false);
            }
        }

        private void _Rows_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedIndexChange = true;

            tableLayoutPanel1.SuspendLayout();
            BindingContext[_dataTable].Position = _tableData_DataGridView.CurrentRow.Index;

            foreach (DataColumn dc in _dataTable.Columns)
            {
                if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBool) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBool])
                {
                    CheckBox cb = _specialControls[dc.ColumnName] as CheckBox;
                    if (cb == null) continue;

                    DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
                    cb.Checked = ((int)dr[dc]) == 1 ? true : false;
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox clb = _specialControls[dc.ColumnName] as CheckedListBox;
                    if (clb == null) continue;

                    DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
                    _UpdateCheckedListBox(clb, dr, dc);
                }
            }

            tableLayoutPanel1.ResumeLayout();

            _selectedIndexChange = false;
        }

        public void SaveButton()
        {
            DataTable table = ((DataSet)_tableData_DataGridView.DataSource).Tables[_tableData_DataGridView.DataMember];
            if (table == null) return;

            String saveType = _dataFile.IsExcelFile ? "Cooked Excel Tables" : "Cooked String Tables";
            String saveExtension = _dataFile.IsExcelFile ? "txt.cooked" : "uni.xls.cooked";
            String saveInitialPath = Path.Combine(Config.HglDir, _dataFile.FilePath);

            String savePath = FormTools.SaveFileDialogBox(saveExtension, saveType, _dataFile.FileName, saveInitialPath);
            if (String.IsNullOrEmpty(savePath)) return;

            if (!_dataFile.ParseDataTable(table, _fileManager))
            {
                MessageBox.Show("Error: Failed to parse data table!");
                return;
            }

            byte[] data = _dataFile.ToByteArray();
            if (FormTools.WriteFileWithRetry(savePath, data))
            {
                MessageBox.Show("Table saved Successfully!", "Completed", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void _RegenTable_Click()
        {
            // make sure we're trying to rebuild an excel table, and that it's actually in the dataset
            if (_excelFile == null) return;
            if (!_fileManager.XlsDataSet.Tables.Contains(_dataFile.StringId)) return;

            // remove from view or die, lol
            _tableData_DataGridView.DataMember = null;
            _tableData_DataGridView.DataSource = null;
            _strings_ListBox.DataSource = null;

            // remove and reload
            DataTable dt = _fileManager.XlsDataSet.Tables[_dataFile.StringId];
            if (dt.ChildRelations.Count != 0)
            {
                dt.ChildRelations.Clear();
            }
            if (dt.ParentRelations.Count != 0)
            {
                //MessageBox.Show("Warning - Has Parent Relations!\nTest Me!\n\nPossible cache dataset relations issue!", "Warning", MessageBoxButtons.OK,
                //                MessageBoxIcon.Exclamation);
                dt.ParentRelations.Clear();
            }

            _fileManager.XlsDataSet.Tables.Remove(_dataFile.StringId);
            _fileManager.LoadTable(_dataFile, true);

            // display updated table
           //  MessageBox.Show("Table regenerated!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // todo: when adding new columns the window will need to be close/reopened to show the changes
            // the dataGridView is storing its own little cache or something - 
            _tableData_DataGridView.Refresh();
            _tableData_DataGridView.DataSource = _fileManager.XlsDataSet;
            _tableData_DataGridView.DataMember = _dataFile.StringId;
            _strings_ListBox.DataSource = _excelFile.SecondaryStrings;
            _dataChanged = true;

            MessageBox.Show(
                "Attention: Currently a bug exists such that you must close this form and re-open it to see any changes for the regeneration.\nDoing so will ask if you wish to apply your changes to the cache data.\n\nAlso of note is the you can't edit any cells until you close the window - FIX ME.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int tableView = 0;
            if (tabControl1.SelectedIndex != tableView) return;

            DataGridViewSelectedRowCollection dataRows = _tableData_DataGridView.SelectedRows;
            foreach (DataGridViewRow dataRow in dataRows)
            {
                DataRow copiedRow = (dataRow.DataBoundItem as DataRowView).Row;
                DataRow newRow = _dataTable.NewRow();
                newRow.ItemArray = copiedRow.ItemArray;
                newRow[0] = _dataTable.Rows.Count;
                _dataTable.Rows.Add(newRow);
            }
        }

        private void copyScriptLabel_Click(object sender, EventArgs e)
        {
            int tableView = 0;
            if (tabControl1.SelectedIndex != tableView) return;

            using (StringWriter sw = new StringWriter())
            {
                DataGridViewSelectedRowCollection dataRows = _tableData_DataGridView.SelectedRows;
                sw.Write("<file id=\"" + _dataTable.TableName + "\">\n");
                foreach (DataGridViewRow dataRow in dataRows)
                {
                    sw.Write("\t<entity id=\"" + dataRow.Index + "\">\n");
                    foreach (DataGridViewCell dataCell in dataRow.Cells)
                    {
                        if (_dataTable.Columns[dataCell.ColumnIndex].Caption.Contains("tcv4")) continue;
                        if (_dataTable.Columns[dataCell.ColumnIndex].Caption.Equals("Index")) continue;
                        if (_dataTable.Columns[dataCell.ColumnIndex].Caption.Contains("_string")) continue;
                        sw.Write("\t\t<attribute id=\"" + _dataTable.Columns[dataCell.ColumnIndex].Caption + "\">" + dataCell.Value.ToString() + "</attribute>\n");
                    }
                    sw.Write("\t</entity>\n");
                }
                sw.Write("</file>");
                Clipboard.SetText(sw.ToString());
            }
        }

        private void _ImportButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Config.LastDirectory,
                Filter = "Text Files (.txt)|*.txt|All Types (*.*)|*.*"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK) return;
            Config.LastDirectory = Path.GetDirectoryName(fileDialog.FileName);

            byte[] buffer;
            try
            {
                buffer = File.ReadAllBytes(fileDialog.FileName);
            }
            catch
            {
                MessageBox.Show("There was a problem opening the file. Make sure the file isn't locked by another program (like Excel)", "Reading Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            try
            {
                bool parseSuccess = IsExcelFile ? _excelFile.ParseCSV(buffer, _fileManager) : _stringsFile.ParseCSV(buffer);
                if (parseSuccess == true)
                {
                    _RegenTable_Click();
                }
                else
                {
                    MessageBox.Show("Error importing this table. Make sure the table has the correct number of columns and the correct data. If you have a string where a number is expected, the import will not work.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical parsing error. Are you importing the right table?\nDetails: " + ex.Message);
            }
        }

        private void _ExportButton_Click(object sender, EventArgs e)
        {
            // Parse the DataTable object
            if (_dataFile.ParseDataTable(_dataTable, _fileManager) != true)
            {
                MessageBox.Show("Error parsing dataTable. Please contact a developer.");
                // Todo log error. this should never happen
                return;
            }

            // Export it to a CSV stream
            byte[] buffer = _dataFile.ExportCSV(_fileManager);
            if (buffer == null)
            {
                MessageBox.Show("Error exporting CSV. Please contact a developer");
                // Todo log error. this should never happen
                return;
            }

            // Prompt the user where to save
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Config.LastDirectory,
                FileName = _dataFile.FileName,
                Filter = "Text Files (.txt)|*.txt|All Types (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                File.WriteAllBytes(saveFileDialog.FileName, buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorry, there was a problem saving the file.\n" + ex.Message, "Saving Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionLogger.LogException(ex, false);
                return;
            }
        }

        private void _TableData_DataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.C || !e.Control) return;

            Clipboard.SetDataObject(_tableData_DataGridView.GetClipboardContent());
        }

        private void _TableData_DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // ensure valid double click
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // ensure valid script column
            DataGridViewColumn dataGridViewColumn = _tableData_DataGridView.Columns[e.ColumnIndex];
            DataColumn dataColumn = _dataTable.Columns[dataGridViewColumn.Name];
            if (!dataColumn.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsScript) || !(bool)dataColumn.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsScript]) return;

            // todo: ensure each cell only has at most one editor for itself
            
            // create editor
            DataGridViewCell dataGridViewCell = _tableData_DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (dataGridViewCell == null) return; // shouldn't happen, but just in case

            ScriptEditor scriptEditor = new ScriptEditor(_fileManager, dataGridViewCell, "todo", dataColumn.ColumnName) { MdiParent = MdiParent };
            scriptEditor.Show();
        }

        private void _ToggleRowView(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this Object dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}