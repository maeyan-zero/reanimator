using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

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

        readonly TableDataSet _tableDataSet;
        readonly Hashtable _specialControls;
        private DataTable _dataTable;
        private bool _dataChanged;
        private bool _selectedIndexChange;
        private DataView _dataView;

        private readonly DataFile _dataFile;
        private readonly ExcelFile _excelFile;

        public ExcelTableForm(DataFile dataFile, TableDataSet tableDataSet)
        {
            _dataFile = dataFile;
            _excelFile = _dataFile as ExcelFile;
            _tableDataSet = tableDataSet;
            _specialControls = new Hashtable();
            _dataChanged = false;
            _selectedIndexChange = false;

            Init();

            ProgressForm progress = new ProgressForm(LoadTable, _dataFile);
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

        private void UseDataView()
        {
            String temp = tableData_DataGridView.DataMember;
            DataTable dataTable = _tableDataSet.XlsDataSet.Tables[temp];
            _dataView = dataTable.DefaultView;
            tableData_DataGridView.DataMember = null;
            tableData_DataGridView.DataSource = _dataView;
        }

        private void Init()
        {
            InitializeComponent();

            tableData_DataGridView.DoubleBuffered(true);
            tableData_DataGridView.EnableHeadersVisualStyles = false;
            tableData_DataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            tableData_DataGridView.DataSource = _tableDataSet.XlsDataSet;
            tableData_DataGridView.DataMember = null;
        }

        private void LoadTable(ProgressForm progress, Object var)
        {
            DataFile dataFile = var as DataFile;
            if (dataFile == null) return;


            // table tab
            tableData_DataGridView.SuspendLayout();
            _dataTable = _tableDataSet.LoadTable(progress, dataFile);
            if (_dataTable == null)
            {
                MessageBox.Show("Failed to load DataTable!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // need to manually populate columns due to fillweight = 100 by default (overflow crap; 655 * 100 > max int)
            if (_dataTable.Columns.Count > 655)
            {
                tableData_DataGridView.AutoGenerateColumns = false;
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

                tableData_DataGridView.Columns.AddRange(columns);
                tableData_DataGridView.DataMember = _dataFile.StringId;
            }
            else
            {
                tableData_DataGridView.DataMember = _dataFile.StringId;
            }
            tableData_DataGridView.ResumeLayout();


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

                    cb.CheckedChanged += cb_ItemCheck;
                    _specialControls.Add(dc.ColumnName, cb);

                    column++;
                    continue;
                }

                new Label { Text = dc.ColumnName, Parent = rows_LayoutPanel, AutoSize = true, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft};

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

                    clb.ItemCheck += clb_ItemCheck;
                    _specialControls.Add(dc.ColumnName, clb);

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

                    if ((dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsRelationGenerated) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsRelationGenerated]) ||
                        (column == 0))
                    {
                        tb.ReadOnly = true;
                        if (relationTextBox != null)
                            relationTextBox.TextChanged += (sender, e) =>
                                    tb.ResetText();
                    }

                    if ((dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsStringIndex) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsStringIndex]) ||
                        (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsStringId) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsStringId]))
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


            if (_excelFile == null)
            {
                tabControl1.TabPages.RemoveByKey("stringsPage");
                tabControl1.TabPages.RemoveByKey("indexArraysPage");
                return;
            }

            // strings tab
            if (_excelFile.SecondaryStrings != null && _excelFile.SecondaryStrings.Count > 0)
            {
                strings_ListBox.SuspendLayout();
                strings_ListBox.DataSource = _excelFile.SecondaryStrings;
                strings_ListBox.ResumeLayout();
            }
            else
            {
                tabControl1.TabPages.RemoveByKey("stringsPage");
            }

            // table sort index tab
            // todo: make me nicer
            List<TableIndexDataSource> tdsList = new List<TableIndexDataSource>();
            int[][] intArrays = { _excelFile.TableIndicies, _excelFile.SortIndex1, _excelFile.SortIndex2, _excelFile.SortIndex3, _excelFile.SortIndex4 };
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

            if (tdsList.Count == 0)
            {
                tabControl1.TabPages.RemoveByKey("indexArraysPage");
            }

            indexArrays_DataGridView.SuspendLayout();
            indexArrays_DataGridView.DataSource = tdsList.ToArray();
            indexArrays_DataGridView.ResumeLayout();
        }

        void DoIntOffsetType(DataColumn dc)
        {
            //Type type = dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IntOffsetType] as Type;
            //if (type == null) return;

            //if (type == typeof(ExcelFile.SingleInt32))
            //{
            //    NumericUpDown nud = new NumericUpDown
            //                            {
            //                                Parent = rows_LayoutPanel,
            //                                AutoSize = true,
            //                                Dock = DockStyle.Fill
            //                            };

            //    nud.ValueChanged += nud_ValueChanged;
            //    _specialControls.Add(dc.ColumnName, nud);
            //}
        }

        void nud_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            NumericUpDown nud = sender as NumericUpDown;
            if (nud == null) return;

           // DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
           // dr[nud.Name] = (cb.CheckState == CheckState.Checked ? 1 : 0);
        }

        void cb_ItemCheck(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckBox cb = sender as CheckBox;
            if (cb == null) return;

            DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
            dr[cb.Name] = (cb.CheckState == CheckState.Checked ? 1 : 0);
        }

        void clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckedListBox clb = sender as CheckedListBox;
            if (clb == null) return;

            DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
            uint value = (uint) dr[clb.Name];
            value ^= (uint) (1 << (e.Index));
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
                if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBool) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBool])
                {
                    CheckBox cb = _specialControls[dc.ColumnName] as CheckBox;
                    if (cb == null) continue;

                    DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
                    cb.Checked = ((int)dr[dc]) == 1 ? true : false;
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox clb = _specialControls[dc.ColumnName] as CheckedListBox;
                    if (clb == null) continue;

                    DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
                    UpdateCheckedListBox(clb, dr, dc);
                }
            }

            rows_LayoutPanel.ResumeLayout();

            _selectedIndexChange = false;
        }

        public void SaveButton()
        {
            DataTable table = ((DataSet)tableData_DataGridView.DataSource).Tables[tableData_DataGridView.DataMember];
            if (table == null) return;

            String saveType = _dataFile.IsExcelFile ? "Cooked Excel Files" : "Cooked String Files";
            String saveExtension = _dataFile.IsExcelFile ? "txt.cooked" : "xls.uni.cooked";
            String saveInitialPath = Path.Combine(Config.HglDir, _dataFile.FilePath);

            String savePath = FileTools.SaveFileDiag(saveExtension, saveType, _dataFile.FileName, saveInitialPath);
            if (String.IsNullOrEmpty(savePath)) return;

            byte[] data = _dataFile.GenerateFile(table);
            if (FileTools.WriteFile(savePath, data))
            {
                MessageBox.Show("File saved Successfully!", "Completed", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void regenTable_Click(object sender, EventArgs e)
        {
            // make sure we're trying to rebuild an excel table, and that it's actually in the dataset
            if (_excelFile == null) return;
            if (!_tableDataSet.XlsDataSet.Tables.Contains(_dataFile.StringId)) return;

            // remove from view or die, lol
            tableData_DataGridView.DataMember = null;
            tableData_DataGridView.DataSource = null;
            strings_ListBox.DataSource = null;

            // remove and reload
            DataTable dt = _tableDataSet.XlsDataSet.Tables[_dataFile.StringId];
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

            _tableDataSet.XlsDataSet.Tables.Remove(_dataFile.StringId);
            _tableDataSet.LoadTable(null, _dataFile);

            // display updated table
            MessageBox.Show("Table regenerated!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // todo: when adding new columns the window will need to be close/reopened to show the changes
            // the dataGridView is storing its own little cache or something - 
            tableData_DataGridView.Refresh();
            tableData_DataGridView.DataSource = _tableDataSet.XlsDataSet;
            tableData_DataGridView.DataMember = _dataFile.StringId;
            strings_ListBox.DataSource = _excelFile.SecondaryStrings;
            _dataChanged = true;

            MessageBox.Show(
                "Attention: Currently a bug exists such that you must close this form and re-open it to see any changes for the regeneration.\nDoing so will ask if you wish to apply your changes to the cache data.\n\nAlso of note is the you can't edit any cells until you close the window - FIX ME.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int tableView = 0;
            if (tabControl1.SelectedIndex != tableView) return;

            DataGridViewSelectedRowCollection dataRows = tableData_DataGridView.SelectedRows;
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
                DataGridViewSelectedRowCollection dataRows = tableData_DataGridView.SelectedRows;
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

        private void ImportButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Config.ScriptDir
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] buffer = null;
                try
                {
                    buffer = File.ReadAllBytes(fileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("There was a problem opening the file. Make sure the file isn't locked by another program (like Excel)");
                    return;
                }
                _dataTable.Clear();
                _dataTable.Columns[0].AutoIncrementStep = -1;
                _dataTable.Columns[0].AutoIncrementSeed = -1;
                _dataTable.Columns[0].AutoIncrementStep = 1;
                _dataTable.Columns[0].AutoIncrementSeed = 0;


                if (CSVtoDataTable(buffer, _dataTable))
                {
                    MessageBox.Show("Import okay!");
                }
                else
                {
                    MessageBox.Show("Error importing this table. Make sure the table has the correct number of columns and the correct data. If you have a string where a number is expected, the import will not work.");
                }
            }
        }

        /// <summary>
        /// Reads the source array in the column sequence of the destination datatable.
        /// </summary>
        /// <param name="source">The array of the CSV data.</param>
        /// <param name="destination">The datatable it is being loaded into.</param>
        /// <returns></returns>
        public static bool CSVtoDataTable(byte[] source, DataTable destination)
        {
            if (source == null) return false;
            if (destination == null) return false;
            if (source.Length == 0) return false;

            int offset = 0;
            int length = source.Length;
            bool ignoreFirstRow = true;
            byte deliminter = (byte)'\t';
            DataRow dataRow;
            Type dataType;
            int column = 0;

            if (ignoreFirstRow)
            {
                while (offset < length)
                {
                    if (source[offset++] == (byte)0x0A)
                    {
                        break;
                    }
                }
            }

            dataRow = destination.NewRow();

            while (!(offset >= length))
            {
                foreach (DataColumn dataColumn in destination.Columns)
                {
                    if (dataColumn.ColumnName == "Index")
                    {
                        column++;
                        continue;
                    }
                    if (dataColumn.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsRelationGenerated))
                    {
                        column++;
                        continue;
                    }

                    if (offset == length) break;

                    dataType = dataColumn.DataType;
                    byte[] newStringBuffer = FileTools.GetDelimintedByteArray(source, ref offset, deliminter);
                    string newString = newStringBuffer == null ? String.Empty : System.Text.Encoding.ASCII.GetString(newStringBuffer);

                    try
                    {
                        if (dataType.BaseType == typeof(Enum))
                        {
                            dataType = typeof(UInt32);
                        }

                        if (dataType == typeof(String)) //hellgate thing only
                        {
                            newString = newString.Replace("\\n", "\n");
                            newString = newString.Replace("\"", "");
                            dataRow[column] = newString;
                        }
                        else if (dataType == typeof(Int32))
                        {
                            dataRow[column] = Int32.Parse(newString);
                        }
                        else if (dataType == typeof(UInt32))
                        {
                            dataRow[column] = UInt32.Parse(newString);
                        }
                        else if (dataType == typeof(Single))
                        {
                            dataRow[column] = Single.Parse(newString);
                        }
                        else if (dataType == typeof(byte))
                        {
                            dataRow[column] = Byte.Parse(newString);
                        }
                        else if (dataType == typeof(Int64))
                        {
                            dataRow[column] = Int64.Parse(newString);
                        }
                        else if (dataType == typeof(UInt64))
                        {
                            dataRow[column] = UInt64.Parse(newString);
                        }
                        else if (dataType == typeof(short))
                        {
                            dataRow[column] = short.Parse(newString);
                        }
                    }
                    catch
                    {
                        // bad data type
                    }
                    column++;
                }

                column = 0;
                destination.Rows.Add(dataRow);
                dataRow = destination.NewRow();
            }

            return true;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                // ensure we're trying to export from a valid form
                ExcelTableForm excelTable = this;
                if (excelTable == null) return;

                // what columns do we want to export?
                CSVSelection select = new CSVSelection(_dataTable);
                if (select.ShowDialog(this) != DialogResult.OK) return;

                // compiles the CSV string
                string strValue = Export.CSV(_dataTable, select.selected, '\t');

                // prompts the user to choose where to save the file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    InitialDirectory = Config.ScriptDir,
                    FileName = _dataFile.FileName
                };
                if (saveFileDialog.ShowDialog(this) != DialogResult.OK) return;

                // done
                File.WriteAllText(saveFileDialog.FileName, strValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export of this form not supported at this time or unknown error!\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ExceptionLogger.LogException(ex, "_ExportCSVToolStripMenuItem_Click", false);
            }
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this Object dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}