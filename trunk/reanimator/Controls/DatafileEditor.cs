using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Hellgate;
using Hellgate.Excel;
using Hellgate.Excel.TestCentre;
using Revival.Common;
using Reanimator.Forms;
using OutputAttribute = Hellgate.ExcelFile.OutputAttribute;

namespace Reanimator.Controls
{
    public partial class DatafileEditor : UserControl, IMdiChildBase
    {
        public bool AddedToTabPage { get; set; }

        private readonly FileManager _fileManager;
        private readonly DataFile _dataFile;
        private readonly Hashtable _specialControls;

        private DataTable _dataTable;
        private bool _selectedIndexChange;

        /// <summary>
        /// Creates a DatafileEditor component, designed for editing DataFiles.
        /// </summary>
        /// <param name="dataFile">The dataFile in context.</param>
        /// <param name="fileManager">FileManager dependency.</param>
        public DatafileEditor(DataFile dataFile, FileManager fileManager)
        {
            _dataFile = dataFile;
            _fileManager = fileManager;
            _selectedIndexChange = false;
            _specialControls = new Hashtable();

            InitializeComponent();
        }

        private String _dataMember;
        public void DisconnectFromDataSet()
        {
            _dataMember = _tableData_DataGridView.DataMember;

            _tableData_DataGridView.DataMember = null;
            _tableData_DataGridView.DataSource = null;
        }

        public void ReconnectToDataSet()
        {
            _tableData_DataGridView.DataSource = _fileManager.XlsDataSet;
            _tableData_DataGridView.DataMember = _dataMember;
        }

        /// <summary>
        /// The operations called from this method can take a long time to run, launch new thread.
        /// </summary>
        /// <param name="progressForm"></param>
        /// <param name="var"></param>
        public void InitThreadedComponents(ProgressForm progressForm, Object var)
        {
            _CreateDataTable();
            _CreateTableView();

            // don't create row view for UnitData type until specifically asked to due to large number of columns
            //if (_dataFile.StringId == "ITEMS" || _dataFile.StringId == "MONSTERS" ||
            //    _dataFile.StringId == "PLAYERS" || _dataFile.StringId == "OBJECTS" ||
            //    _dataFile.StringId == "MISSILES") return;

            _CreateRowView();
        }

        /// <summary>
        /// Creates the DataTable which serves as a datasource.
        /// </summary>
        private void _CreateDataTable()
        {
            _dataTable = _fileManager.LoadTable(_dataFile, true);
            //_dataTable.RowChanged += (sender, e) => { _dataChanged = true; };
            // Im sure there is a boolean on the dataTable that tells if the table has been modified.
        }

        /// <summary>
        /// Defines the table view section of the form.
        /// </summary>
        private void _CreateTableView()
        {
            _tableData_DataGridView.SuspendLayout();

            _tableData_DataGridView.DoubleBuffered(true); // performance enhancement
            _tableData_DataGridView.EnableHeadersVisualStyles = false; // performance enhancement
            _tableData_DataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue; // make it look pretty
            _tableData_DataGridView.AutoGenerateColumns = false; // it's quicker to generate them manually
            _tableData_DataGridView.DataSource = _fileManager.XlsDataSet;
            _tableData_DataGridView.DataMember = null;

            // just some debug stuff - left for easy access the next time
            //_tableData_DataGridView.DataSourceChanged += new EventHandler(_tableData_DataGridView_DataSourceChanged);
            //_tableData_DataGridView.DataMemberChanged += new EventHandler(_tableData_DataGridView_DataMemberChanged);
            //_tableData_DataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(_tableData_DataGridView_DataBindingComplete);
            //_tableData_DataGridView.ColumnAdded += new DataGridViewColumnEventHandler(_tableData_DataGridView_ColumnAdded);
            //_tableData_DataGridView.ColumnRemoved += new DataGridViewColumnEventHandler(_tableData_DataGridView_ColumnRemoved);
            //_tableData_DataGridView.Columns.CollectionChanged += new CollectionChangeEventHandler(Columns_CollectionChanged);

            // manually populate columns - using AutoGenerateColumn causes massive lag issues when called in the wrong order
            // and this happens to be slightly faster than auto generated columns anyways, so might as well use it
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
            _tableData_DataGridView.DataMember = _dataFile.IsStringsFile ? FileManager.StringsTableName : _dataFile.StringId;

            // set the code column to display as hex
            DataGridViewColumn codeColumn = _tableData_DataGridView.Columns["code"];
            if (codeColumn != null) codeColumn.DefaultCellStyle.Format = "X04";

            _tableData_DataGridView.ResumeLayout();
        }

        #region debug stuff
        //private void _tableData_DataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        //{
        //    //Debug.WriteLine("Column Added: " + e.Column.Name);
        //}

        //private void _tableData_DataGridView_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        //{
        //    Debug.WriteLine("[" + _dataFile.StringId + "] Column Removed: " + e.Column.Name);
        //}

        //private void Columns_CollectionChanged(object sender, CollectionChangeEventArgs e)
        //{
        //    //Debug.WriteLine("[" + _dataFile.StringId + "] Column Collection Changed (Action = " + e.Action + ", Element = " + e.Element + ")");
        //}

        //private void _tableData_DataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        //{
        //    int bp = 0;
        //}

        //private void _tableData_DataGridView_DataMemberChanged(object sender, EventArgs e)
        //{
        //    int bp = 0;
        //}

        //private void _tableData_DataGridView_DataSourceChanged(object sender, EventArgs e)
        //{
        //    int bp = 0;
        //}
        #endregion

        /// <summary>
        /// Defines the row view section of the form.
        /// </summary>
        private void _CreateRowView()
        {
            //_rows_LayoutPanel.CellPaint += (sender, e) => { if (e.Row % 2 == 0) e.Graphics.FillRectangle(Brushes.AliceBlue, e.CellBounds); };
            _rows_LayoutPanel.SuspendLayout();
            int column = 0;
            TextBox relationTextBox = null;

            foreach (DataColumn dc in _dataTable.Columns)
            {
                column++;

                new Label
                {
                    Text = dc.ColumnName,
                    Parent = _rows_LayoutPanel,
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };

                column++;

                if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBool) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBool])
                {
                    CheckBox checkBox = new CheckBox
                    {
                        Parent = _rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName,
                        CheckAlign = ContentAlignment.MiddleLeft
                    };

                    checkBox.CheckedChanged += _RowView_CheckBox_ItemCheck;
                    _specialControls.Add(dc.ColumnName, checkBox);
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox checkedListBox = new CheckedListBox
                    {
                        Parent = _rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        MultiColumn = false,
                        Name = dc.ColumnName
                    };

                    checkedListBox.ItemCheck += _RowView_CheckedListBox_ItemCheck;
                    _specialControls.Add(dc.ColumnName, checkedListBox);


                    OutputAttribute attribute = ExcelFile.GetExcelAttribute(_dataFile.DataType.GetField(dc.ColumnName));

                    // Populate the checklist by either a enum or (less commonly) table indices
                    if (!String.IsNullOrEmpty(attribute.TableStringId))
                    {
                        // TODO: should perhaps use object delegator here, it might be faster
                        // so far its only its runtime is only 1n so doesnt really matter.
                        //
                        DataTable dataTable = _fileManager.GetDataTable(attribute.TableStringId);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            checkedListBox.Items.Add(row[2], false);
                        }
                    }
                    else
                    {
                        Type cellType = dc.DataType;
                        foreach (Enum type in Enum.GetValues(cellType))
                        {
                            checkedListBox.Items.Add(type, false);
                        }
                    }
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsEnum) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsEnum])
                {
                    ComboBox comboBox = new ComboBox
                    {
                        Parent = _rows_LayoutPanel,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName
                    };

                    // todo: consider lookomg into overloaded FindString or FindExactString methods and check (change) the startIndex parameter
                    Binding comboBoxBinding = comboBox.DataBindings.Add("SelectedIndex", _dataTable, dc.ColumnName, true);
                    comboBoxBinding.Format += _ComboBoxFormat;
                    comboBox.SelectedIndexChanged += _RowView_ComboList_ItemChange;

                    // need order as VALUE order - Enum.GetValues provides sorted as UNSIGNED values e.g. {0, 1, 2, 3, -3, -2, -1} instead of {-3, -2, -1, 0, 1, 2, 3}
                    List<Enum> enumValues = Enum.GetValues(dc.DataType).Cast<Enum>().ToList();
                    enumValues.Sort(); // List sort works as SIGNED
                    comboBox.Items.AddRange(enumValues.ToArray());
                    comboBox.Tag = Math.Abs(Convert.ToInt32(enumValues.Min())); // we need a minimum value to get our base offset
                }
                else
                {
                    TextBox textBox = new TextBox
                    {
                        Text = String.Empty,
                        Parent = _rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName
                    };
                    textBox.DataBindings.Add("Text", _dataTable, dc.ColumnName);

                    if ((dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsRelationGenerated) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsRelationGenerated]) || column == 0)
                    {
                        textBox.ReadOnly = true;
                        if (relationTextBox != null)
                        {
                            relationTextBox.TextChanged += (sender, e) => textBox.ResetText();
                        }
                    }

                    if ((dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsStringIndex) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsStringIndex]) ||
                        (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsStringOffset) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsStringOffset]))
                    {
                        relationTextBox = textBox;
                    }
                    else
                    {
                        relationTextBox = null;
                    }
                }
            }

            new Label
            {
                Text = String.Empty,
                Parent = _rows_LayoutPanel,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            _rows_LayoutPanel.ResumeLayout();
            _rows_LayoutPanel.Width += 10;


            // fixes mouse scroll wheel
            // todo: this is dodgy and causes focused elements within the layoutpanel to lose focus (e.g. a text box) - rather anoying
            _rows_LayoutPanel.Click += (sender, e) => _rows_LayoutPanel.Focus();
            _rows_LayoutPanel.MouseEnter += (sender, e) => _rows_LayoutPanel.Focus();
        }

        /// <summary>
        /// Save the modified file over the original.
        /// </summary>
        public void Save()
        {

        }

        /// <summary>
        /// Prompts the user for a location to save the data file.
        /// </summary>
        public void SaveAs()
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

        /// <summary>
        /// Promopts the user for a file then imports it into the current table.
        /// </summary>
        public void Import()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
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
                bool parseSuccess = _dataFile.ParseCSV(buffer, _fileManager);
                if (parseSuccess)
                {
                    _RegenTable();
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

        /// <summary>
        /// Prompts the user for a locations where the current table is executed.
        /// </summary>
        public void Export()
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
            String initialDir = Directory.Exists(Config.LastDirectory) ? Config.LastDirectory : Config.HglDir;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = initialDir,
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

        private void _ComboBoxFormat(object sender, ConvertEventArgs convertEventArgs)
        {
            // we need to wait until we've added this editor control to the tab page before we format it
            // otherwise we actually modify the DataGridView value for whatever reason I couldn't be bothered figuring out
            if (!AddedToTabPage) return;

            Binding binding = (Binding)sender;
            ComboBox comboBox = (ComboBox)binding.Control;
            Debug.Assert(comboBox != null);

            //Debug.WriteLine("_ComboBoxFormat: " + comboBox.Name);
            int val = (convertEventArgs.Value is uint) ? (int)(uint)convertEventArgs.Value : (int)convertEventArgs.Value;

            convertEventArgs.Value = val + (Int32)comboBox.Tag;
        }

        /// <summary>
        /// Opens views specific to the cell context.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tableData_DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

            //ScriptEditor scriptEditor = new ScriptEditor(_fileManager, dataGridViewCell, "todo", dataColumn.ColumnName) { MdiParent = MdiParent };
            //scriptEditor.Show();
        }

        /// <summary>
        /// Adds Ctrl+C keybpard shortcut to the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tableData_DataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.C || !e.Control) return;

            Object clipboardContent = _tableData_DataGridView.GetClipboardContent();
            if (clipboardContent == null) return;

            Clipboard.SetDataObject(clipboardContent);
        }

        /// <summary>
        /// Triggers methods to update the row view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tableData_DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (_tableData_DataGridView.CurrentRow == null) return;
            if (_tableData_DataGridView.CurrentRow.IsNewRow) return;
            //if (_tableData_DataGridView.Columns[0].Name != "Index") return; // seems to happen sometimes when lots of tables are open...
            if (_tableData_DataGridView.CurrentRow.Cells[0].Value == null) return; // occurs when the grid view is disconnected from the data set

            _selectedIndexChange = true;
            _rows_LayoutPanel.SuspendLayout();

            int dataTableIndex = (Int32)_tableData_DataGridView.CurrentRow.Cells[0].Value;
            BindingContext[_dataTable].Position = dataTableIndex;

            foreach (DataColumn dc in _dataTable.Columns)
            {
                if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBool) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBool])
                {
                    CheckBox checkBox = _specialControls[dc.ColumnName] as CheckBox;
                    if (checkBox == null) continue;

                    DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
                    checkBox.Checked = (((int)dr[dc]) == 1);
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBitmask) && (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox checkedListBox = _specialControls[dc.ColumnName] as CheckedListBox;
                    if (checkedListBox == null) continue;

                    DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
                    _UpdateCheckedListBox(checkedListBox, dr, dc);
                }
            }

            _rows_LayoutPanel.ResumeLayout();

            _selectedIndexChange = false;
        }

        /// <summary>
        /// Hides and Shows the row view on the side of the griddata.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _toggleRowViewButton_Click(object sender, EventArgs e)
        {
            _splitContainer.Panel2Collapsed = !_splitContainer.Panel2Collapsed;
        }

        /// <summary>
        /// Updates the ticked components of a checkedlist box.
        /// </summary>
        /// <param name="clb">The checkbox in context.</param>
        /// <param name="dr">The row containing the integer.</param>
        /// <param name="dc">The column containing the integer.</param>
        private static void _UpdateCheckedListBox(CheckedListBox clb, DataRow dr, DataColumn dc)
        {
            uint value = (uint)dr[dc];
            for (int i = 0; i < clb.Items.Count; i++)
            {
                bool isChecked = (((1 << i) & value) > 0);
                clb.SetItemChecked(i, isChecked);
            }
        }

        private void _RegenTable()
        {
            // make sure we're trying to rebuild an excel table, and that it's actually in the dataset
            //if (_excelFile == null) return;
            if (!_fileManager.XlsDataSet.Tables.Contains(_dataFile.StringId)) return;

            // remove from view or die, lol
            _tableData_DataGridView.DataMember = null;
            _tableData_DataGridView.DataSource = null;

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

            MessageBox.Show(
                "Attention: Currently a bug exists such that you must close this form and re-open it to see any changes for the regeneration.\nDoing so will ask if you wish to apply your changes to the cache data.\n\nAlso of note is the you can't edit any cells until you close the window - FIX ME.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void _RowView_ComboList_ItemChange(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            ComboBox comboBox = (ComboBox)sender;
            DataGridViewRow currentRow = _tableData_DataGridView.CurrentRow;
            if (currentRow == null) return;
            //Debug.Assert(currentRow != null);
            return;
            DataRow dr = _dataTable.Rows[currentRow.Index];
            dr[comboBox.Name] = comboBox.SelectedIndex;
        }

        private void _RowView_CheckBox_ItemCheck(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckBox checkBox = (CheckBox)sender;
            DataGridViewRow currentRow = _tableData_DataGridView.CurrentRow;
            Debug.Assert(currentRow != null);

            DataRow dr = _dataTable.Rows[currentRow.Index];
            dr[checkBox.Name] = (checkBox.CheckState == CheckState.Checked ? 1 : 0);
        }

        private void _RowView_CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckedListBox checkedListBox = (CheckedListBox)sender;
            DataGridViewRow currentRow = _tableData_DataGridView.CurrentRow;
            Debug.Assert(currentRow != null);

            DataRow dr = _dataTable.Rows[currentRow.Index];
            uint value = (uint)dr[checkedListBox.Name];
            value ^= (uint)(1 << (e.Index));
            dr[checkedListBox.Name] = value;
        }

        private void _nud_ValueChanged(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            NumericUpDown nud = sender as NumericUpDown;
            if (nud == null) return;

            // DataRow dr = _dataTable.Rows[rows_ListBox.SelectedIndex];
            // dr[nud.Name] = (cb.CheckState == CheckState.Checked ? 1 : 0);
        }

        private void _DuplicateRows(object sender, EventArgs e)
        {
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

        private void InitializeComponent()
        {
            _splitContainer = new SplitContainer();
            _tableData_DataGridView = new DataGridView();
            _rows_LayoutPanel = new TableLayoutPanel();
            button1 = new Button();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(_tableData_DataGridView)).BeginInit();
            SuspendLayout();
            // 
            // _splitContainer
            // 
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.FixedPanel = FixedPanel.Panel2;
            _splitContainer.Location = new Point(0, 0);
            _splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            _splitContainer.Panel1.Controls.Add(button1);
            _splitContainer.Panel1.Controls.Add(_tableData_DataGridView);
            // 
            // _splitContainer.Panel2
            // 
            _splitContainer.Panel2.Controls.Add(_rows_LayoutPanel);
            _splitContainer.Size = new Size(869, 640);
            _splitContainer.SplitterDistance = 543;
            _splitContainer.TabIndex = 0;
            // 
            // _tableData_DataGridView
            // 
            _tableData_DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _tableData_DataGridView.Dock = DockStyle.Fill;
            _tableData_DataGridView.Location = new Point(0, 0);
            _tableData_DataGridView.Name = "_tableData_DataGridView";
            _tableData_DataGridView.Size = new Size(543, 640);
            _tableData_DataGridView.TabIndex = 0;
            _tableData_DataGridView.CellDoubleClick += _tableData_DataGridView_CellDoubleClick;
            _tableData_DataGridView.SelectionChanged += _tableData_DataGridView_SelectionChanged;
            _tableData_DataGridView.KeyUp += _tableData_DataGridView_KeyUp;
            // 
            // _rows_LayoutPanel
            // 
            _rows_LayoutPanel.AutoScroll = true;
            _rows_LayoutPanel.ColumnCount = 2;
            _rows_LayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _rows_LayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            _rows_LayoutPanel.Dock = DockStyle.Fill;
            _rows_LayoutPanel.Location = new Point(0, 0);
            _rows_LayoutPanel.Name = "_rows_LayoutPanel";
            _rows_LayoutPanel.RowCount = 1;
            _rows_LayoutPanel.RowStyles.Add(new RowStyle());
            _rows_LayoutPanel.Size = new Size(322, 640);
            _rows_LayoutPanel.TabIndex = 0;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Right;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Location = new Point(533, 298);
            button1.Name = "button1";
            button1.Size = new Size(10, 54);
            button1.TabIndex = 1;
            button1.UseVisualStyleBackColor = true;
            button1.Click += _toggleRowViewButton_Click;
            // 
            // DatafileEditor
            // 
            Controls.Add(_splitContainer);
            Name = "DatafileEditor";
            Size = new Size(869, 640);
            _splitContainer.Panel1.ResumeLayout(false);
            _splitContainer.Panel2.ResumeLayout(false);
            _splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(_tableData_DataGridView)).EndInit();
            ResumeLayout(false);
        }
    }
}
