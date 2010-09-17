using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class ExcelForm : Form
    {
        public ExcelForm()
        {
            InitializeComponent();
            EnableDoubleBuffering();
        }

        public ExcelForm(DataTable dataTable)
            : this()
        {
            this.dataGridView.DataSource = dataTable.DefaultView;
            this.Text = "Excel Editor: " + dataTable.TableName;
            this.DrawRowView();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
        }

        private void DrawRowView()
        {
            DataView dataTable = (DataView)dataGridView.DataSource;

            tableLayoutPanel.SuspendLayout();
            foreach (DataColumn dataColumn in dataTable.Table.Columns)
            {
                Label emptyLabel = new Label()
                {
                    AutoSize = true,
                    Dock = dataColumn.Ordinal == dataTable.Table.Columns.Count ? DockStyle.Fill : DockStyle.Top,
                    Parent = tableLayoutPanel,
                    Name = dataColumn.ColumnName + "Label",
                    Text = dataColumn.ColumnName,
                    TextAlign = ContentAlignment.MiddleRight,
                };

                if (dataColumn.ExtendedProperties.Contains(ExcelFile.ColumnKeys.IsValueList))
                {
                    ComboBox comboBox = new ComboBox()
                    {
                        AutoSize = true,
                        Dock = ((dataColumn.Ordinal) == (dataTable.Table.Columns.Count - 1)) ? DockStyle.Fill : DockStyle.Top,
                        Parent = tableLayoutPanel,
                        Name = dataColumn.ColumnName + "ComboBox"
                    };
                    String[] valueList = Enum.GetNames(dataColumn.DataType);
                    comboBox.Items.AddRange(valueList);
                    comboBox.DataBindings.Add("SelectedIndex", dataGridView.DataSource, dataColumn.ColumnName);
                }
                else if (dataColumn.ExtendedProperties.Contains(ExcelFile.ColumnKeys.IsBool))
                {
                    CheckBox checkBox = new CheckBox()
                    {
                        Dock = DockStyle.Top,
                        
                        Parent = tableLayoutPanel,
                        Name = dataColumn.ColumnName + "CheckBox",
                    };
                    checkBox.DataBindings.Add("Checked", dataGridView.DataSource, dataColumn.ColumnName);
                }
                else if (dataColumn.ExtendedProperties.Contains(ExcelFile.ColumnKeys.IsBitmask))
                {
                    CheckedListBox checkedListBox = new CheckedListBox()
                    {
                        Dock = ((dataColumn.Ordinal) == (dataTable.Table.Columns.Count - 1)) ? DockStyle.Fill : DockStyle.None,
                        
                        Parent = tableLayoutPanel,
                        Name = dataColumn.ColumnName + "CheckedListBox"
                    };

                    foreach (String flag in Enum.GetNames(dataColumn.DataType))
                    {
                        checkedListBox.Items.Add(flag);
                    }

                    checkedListBox.DataBindings.Add("Text", dataGridView.DataSource, dataColumn.ColumnName);
                }
                else if (dataColumn.DataType == typeof(Single))
                {
                    NumericUpDown numericUpDown = new NumericUpDown()
                    {
                        DecimalPlaces = 2,
                        Parent = tableLayoutPanel,
                        Name = dataColumn.ColumnName + "Numeric",
                        Dock = ((dataColumn.Ordinal) == (dataTable.Table.Columns.Count - 1)) ? DockStyle.Fill : DockStyle.Top,
                        AutoSize = true
                    };
                    numericUpDown.DataBindings.Add("Value", dataGridView.DataSource, dataColumn.ColumnName);
                }
                else
                {
                    TextBox textBox = new TextBox()
                    {
                        AutoSize = true,
                        Dock = ((dataColumn.Ordinal) == (dataTable.Table.Columns.Count - 1)) ? DockStyle.Fill : DockStyle.Top,
                        Parent = tableLayoutPanel,
                        Name = dataColumn.ColumnName + "TextBox",
                    };
                    textBox.DataBindings.Add("Text", dataGridView.DataSource, dataColumn.ColumnName);
                }
            }
            tableLayoutPanel.ResumeLayout();
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = splitContainer.Panel2Collapsed ? false : true;
        }

        private void EnableDoubleBuffering()
        {
            PropertyInfo pi = this.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this, true, null);
        }

        private void dataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            tableLayoutPanel.SuspendLayout();
            if (dataGridView.SelectedRows.Count == 1)
            {
                BindingContext[dataGridView.DataSource].Position = dataGridView.SelectedRows[0].Index;
            }
            tableLayoutPanel.ResumeLayout();
        }

    }
}