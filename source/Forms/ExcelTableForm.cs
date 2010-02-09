using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableIndexDataSource
        {
            public int Unknowns1 { get; set; }
            public int Unknowns2 { get; set; }
            public int Unknowns3 { get; set; }
            public int Unknowns4 { get; set; }
        };

        private ExcelTable excelTable;
        private bool doStrings;

        public String GetExcelTableName()
        {
            return excelTable.StringId;
        }

        public ExcelTableForm(ExcelTable table)
        {
            InitializeComponent();
            excelTable = table;
            doStrings = false;
            if (MessageBox.Show("Do you wish to convert applicable string offsets to strings?\nWarning: This may increase generation time!", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                doStrings = true;
            }
            ProgressForm progress = new ProgressForm();
            progress.Shown += new EventHandler(Progress_Shown);
            progress.ShowDialog(this);
            this.Hide();
            this.Show();
        }

        private void Progress_Shown(object sender, EventArgs e)
        {
            ProgressForm progress = sender as ProgressForm;

            // file id
            this.textBox1.Text = "0x" + excelTable.StructureId.ToString("X");

            // do strings - better than before, but works.
            // no longer has as much details - though I don't think necessary anymore.
            foreach (String s in excelTable.Strings.Values)
            {
                this.listBox1.Items.Add(s);
            }

            // main table data
            progress.SetLoadingText("Generating table data...");
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Add("Index", "Index");
            object[] array = (object[])excelTable.GetTableArray();
            Type type = array[0].GetType();
            foreach (MemberInfo memberInfo in type.GetFields())
            {
                dataGridView.Columns.Add(memberInfo.Name, memberInfo.Name);
            }

            progress.ConfigBar(0, array.Length, 1);
            foreach (Object table in array)
            {
                int row = dataGridView.Rows.Add();
                int col = 1;

                progress.SetCurrentItemText("Row " + row + " of " + array.Length);
                if (row % 2 == 0)
                {
                    progress.Refresh();
                }

                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    ExcelTables.ExcelOutputAttribute excelOutputAttribute = null;
                    if (doStrings)
                    {
                        MemberInfo memberInfo = fieldInfo as MemberInfo;
                        foreach (Attribute attribute in memberInfo.GetCustomAttributes(typeof(ExcelTables.ExcelOutputAttribute), true))
                        {
                            excelOutputAttribute = attribute as ExcelTables.ExcelOutputAttribute;
                            if (excelOutputAttribute != null)
                            {
                                break;
                            }
                        }
                    }

                    Object value = fieldInfo.GetValue(table);
                    if (excelOutputAttribute != null)
                    {
                        if (excelOutputAttribute.IsStringOffset)
                        {
                            value = excelTable.Strings[value];
                        }
                        else if (excelOutputAttribute.IsIntOffset)
                        {
                            int offset = (int)value;
                            if (offset != 0 && excelTable.DataBlock != null)
                            {
                                DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                                String[] fields = excelOutputAttribute.FieldNames;
                                int i = 0;
                                foreach (String s in fields)
                                {
                                    int intValue = BitConverter.ToInt32(excelTable.DataBlock, offset + i * sizeof(Int32));
                                    i++;

                                    cell.Items.Add(s + " = " + intValue);
                                }

                                cell.Value = cell.Items[excelOutputAttribute.DefaultIndex];

                                dataGridView[col, row] = cell;
                                value = null;
                            }
                        }
                    }

                    if (value != null)
                    {
                        dataGridView[col, row].Value = value;
                    }
                    col++;
                }
            }

            // generate the table index data source - //TODO is there a better way?
            List<TableIndexDataSource> tdsList = new List<TableIndexDataSource>();
            int[][] intArrays = { excelTable.TableIndicies, excelTable.Unknowns1, excelTable.Unknowns2, excelTable.Unknowns3, excelTable.Unknowns4 };
            for (int i = 0; i < intArrays.Length; i++)
            {
                if (intArrays[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < intArrays[i].Length; j++)
                {
                    TableIndexDataSource tds;

                    if (tdsList.Count <= j)
                    {
                        tdsList.Add(new TableIndexDataSource());
                    }

                    tds = tdsList[j];
                    switch (i)
                    {
                        case 0:
                            dataGridView[i, j].Value = intArrays[i][j];
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

            if (intArrays[4] == null)
            {
                dataGridView2.Columns.RemoveAt(3);
            }
            if (intArrays[3] == null)
            {
                dataGridView2.Columns.RemoveAt(2);
            }
            if (intArrays[2] == null)
            {
                dataGridView2.Columns.RemoveAt(1);
            }
            if (intArrays[1] == null)
            {
                dataGridView2.Columns.RemoveAt(0);
            }


            progress.Dispose();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection selectedCells = dataGridView.SelectedCells;
            dataGridView2.ClearSelection();
            foreach (DataGridViewCell cell in selectedCells)
            {
                try
                {
                    dataGridView2.Rows[cell.RowIndex].Selected = true;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
