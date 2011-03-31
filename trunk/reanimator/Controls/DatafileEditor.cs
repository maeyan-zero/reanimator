using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Hellgate;
using OutputAttribute = Hellgate.ExcelFile.OutputAttribute;
using Revival.Common;
using Reanimator.Forms;
using System.Collections;
using System.IO;
using System.Reflection;

namespace Reanimator.Controls
{
    public partial class DatafileEditor : UserControl, IMdiChildBase
    {
        private FileManager _fileManager { get; set; }
        private DataFile _dataFile { get; set; }
        private DataTable _dataTable { get; set; }
        private Hashtable _specialControls;
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

        /// <summary>
        /// The operations called from this method can take a long time to run, launch new thread.
        /// </summary>
        /// <param name="progressForm"></param>
        /// <param name="var"></param>
        public void InitThreadedComponents(ProgressForm progressForm, Object var)
        {
            _CreateDataTable(); // intensive call
            _CreateTableView();
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

            _tableData_DataGridView.DoubleBuffered(true);
            _tableData_DataGridView.EnableHeadersVisualStyles = false;
            _tableData_DataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            _tableData_DataGridView.DataSource = _fileManager.XlsDataSet;
            _tableData_DataGridView.DataMember = null;

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
        }

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
                    CheckBox cb = new CheckBox
                    {
                        Parent = _rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName,
                        CheckAlign = ContentAlignment.MiddleLeft
                    };

                    cb.CheckedChanged += _RowView_CheckBox_ItemCheck;
                    _specialControls.Add(dc.ColumnName, cb);
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsBitmask) &&
                        (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsBitmask])
                {
                    CheckedListBox clb = new CheckedListBox
                    {
                        Parent = _rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        MultiColumn = false,
                        Name = dc.ColumnName
                    };

                    clb.ItemCheck += _RowView_CheckedListBox_ItemCheck;
                    _specialControls.Add(dc.ColumnName, clb);


                    ExcelFile.OutputAttribute attribute = ExcelFile.GetExcelAttribute(_dataFile.DataType.GetField(dc.ColumnName));

                    // Populate the checklist by either a enum or (less commonly) table indices
                    if (!String.IsNullOrEmpty(attribute.TableStringId))
                    {
                        // TODO: should perhaps use object delegator here, it might be faster
                        // so far its only its runtime is only 1n so doesnt really matter.
                        //
                        DataTable dataTable = _fileManager.GetDataTable(attribute.TableStringId);
                        foreach (DataRow row in dataTable.Rows)
                            clb.Items.Add(row[2], false);
                    }
                    else
                    {
                        Type cellType = dc.DataType;
                        foreach (Enum type in Enum.GetValues(cellType))
                            clb.Items.Add(type, false);
                    }
                }
                else if (dc.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsEnum) &&
                        (bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsEnum])
                {
                    ComboBox cb = new ComboBox
                    {
                        Parent = _rows_LayoutPanel,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName
                    };
                    cb.DataBindings.Add("SelectedIndex", _dataTable, dc.ColumnName);
                    cb.SelectedIndexChanged += new EventHandler(_RowView_ComboList_ItemChange);

                    Type cellType = dc.DataType;
                    foreach (Enum type in Enum.GetValues(cellType))
                        cb.Items.Add(type);
                }
                else
                {
                    TextBox tb = new TextBox
                    {
                        Text = String.Empty,
                        Parent = _rows_LayoutPanel,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        Name = dc.ColumnName
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
                bool parseSuccess = _dataFile.ParseCSV(buffer, _fileManager);
                if (parseSuccess == true)
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

            Clipboard.SetDataObject(_tableData_DataGridView.GetClipboardContent());
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

            _selectedIndexChange = true;

            _rows_LayoutPanel.SuspendLayout();
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
        private void _UpdateCheckedListBox(CheckedListBox clb, DataRow dr, DataColumn dc)
        {
            uint value = (uint)dr[dc];
            for (int i = 0; i < clb.Items.Count; i++)
            {
                clb.SetItemChecked(i, ((1 << i) & value) > 0 ? true : false);
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

            ComboBox cb = (ComboBox)sender;
            DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
            dr[cb.Name] = cb.SelectedIndex;
        }

        private void _RowView_CheckBox_ItemCheck(object sender, EventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckBox cb = (CheckBox)sender;
            DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
            dr[cb.Name] = (cb.CheckState == CheckState.Checked ? 1 : 0);
        }

        private void _RowView_CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_selectedIndexChange) return;

            CheckedListBox clb = (CheckedListBox)sender;
            DataRow dr = _dataTable.Rows[_tableData_DataGridView.CurrentRow.Index];
            uint value = (uint)dr[clb.Name];
            value ^= (uint)(1 << (e.Index));
            dr[clb.Name] = value;
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
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._tableData_DataGridView = new System.Windows.Forms.DataGridView();
            this._rows_LayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tableData_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this.button1);
            this._splitContainer.Panel1.Controls.Add(this._tableData_DataGridView);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._rows_LayoutPanel);
            this._splitContainer.Size = new System.Drawing.Size(869, 640);
            this._splitContainer.SplitterDistance = 543;
            this._splitContainer.TabIndex = 0;
            // 
            // _tableData_DataGridView
            // 
            this._tableData_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._tableData_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableData_DataGridView.Location = new System.Drawing.Point(0, 0);
            this._tableData_DataGridView.Name = "_tableData_DataGridView";
            this._tableData_DataGridView.Size = new System.Drawing.Size(543, 640);
            this._tableData_DataGridView.TabIndex = 0;
            this._tableData_DataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._tableData_DataGridView_CellDoubleClick);
            this._tableData_DataGridView.SelectionChanged += new System.EventHandler(this._tableData_DataGridView_SelectionChanged);
            this._tableData_DataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this._tableData_DataGridView_KeyUp);
            // 
            // _rows_LayoutPanel
            // 
            this._rows_LayoutPanel.AutoScroll = true;
            this._rows_LayoutPanel.ColumnCount = 2;
            this._rows_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._rows_LayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._rows_LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rows_LayoutPanel.Location = new System.Drawing.Point(0, 0);
            this._rows_LayoutPanel.Name = "_rows_LayoutPanel";
            this._rows_LayoutPanel.RowCount = 1;
            this._rows_LayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._rows_LayoutPanel.Size = new System.Drawing.Size(322, 640);
            this._rows_LayoutPanel.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(533, 298);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 54);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this._toggleRowViewButton_Click);
            // 
            // DatafileEditor
            // 
            this.Controls.Add(this._splitContainer);
            this.Name = "DatafileEditor";
            this.Size = new System.Drawing.Size(869, 640);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tableData_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
