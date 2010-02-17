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
using System.Collections;

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
      //  private bool doStrings;

        public String GetExcelTableName()
        {
            return excelTable.StringId;
        }

        public ExcelTableForm(ExcelTable table)
        {
            InitializeComponent();
            dataGridView.DataSource = xlsDataSet;
           // dataGridView.SuspendLayout();
            excelTable = table;
          //  doStrings = false;
          //  if (MessageBox.Show("Do you wish to convert applicable string offsets to strings?\nWarning: This may increase generation time!", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          // {
          //      doStrings = true;
          //  }
            ProgressForm progress = new ProgressForm();
            progress.Shown += new EventHandler(Progress_Shown);
            progress.ShowDialog(this);
            this.Hide();
            this.Show();
         //   dataGridView.ResumeLayout();
        }

        static DataSet xlsDataSet = new DataSet("xlsDataSet");

        private void Progress_Shown(object sender, EventArgs e)
        {
            ProgressForm progress = sender as ProgressForm;

            // file id
            this.textBox1.Text = "0x" + excelTable.StructureId.ToString("X");



            // not sure if we're going to store as a single xml or multiple yet...
            bool readFromDataSet = false;
            if (xlsDataSet.Tables.Count == 0 && File.Exists("dataSet.xml"))
            {
                xlsDataSet.ReadXml("dataSet.xml");
                readFromDataSet = true;
            }




            DataTable stringsDataTable = null;
            listBox1.DataSource = xlsDataSet;
            String stringTableName = excelTable.StringId + "_STRINGS";
            String xmlStringsFilePath = @"xml\" + stringTableName + ".xml";
            if (xlsDataSet.Tables.Contains(stringTableName))
            {
                listBox1.DisplayMember = stringTableName + ".string";
                listBox1.ValueMember = stringTableName + ".offset";
            }
            else
            {
                if (File.Exists(xmlStringsFilePath))
                {
                    xlsDataSet.ReadXml(xmlStringsFilePath);
                    listBox1.DataSource = xlsDataSet;
                    listBox1.DisplayMember = stringTableName + ".string";
                    listBox1.ValueMember = stringTableName + ".offset";
                }
                else
                {
                    stringsDataTable = xlsDataSet.Tables.Add(stringTableName);
                    DataColumn offsetColumn = stringsDataTable.Columns.Add("offset");
                    offsetColumn.Unique = true;
                    stringsDataTable.PrimaryKey = new DataColumn[] { offsetColumn };
                    stringsDataTable.Columns.Add("string");

                    // do strings - better than before, but works.
                    // no longer has as much details - though I don't think necessary anymore.
                    foreach (DictionaryEntry entry in excelTable.Strings)
                    {
                        stringsDataTable.Rows.Add(entry.Key, entry.Value);
                        //this.listBox1.Items.Add(s);
                    }

                    listBox1.DataSource = xlsDataSet;
                    listBox1.DisplayMember = stringTableName + ".string";
                    listBox1.ValueMember = stringTableName + ".offset";
                }
            }





            DataTable dataTable = null;
            String xmlFilePath = @"xml\" + excelTable.StringId + ".xml";
            if (xlsDataSet.Tables.Contains(excelTable.StringId))
            {
                dataGridView.DataMember = excelTable.StringId;
            }
            else
            {
                if (File.Exists(xmlFilePath))
                {
                    xlsDataSet.ReadXml(xmlFilePath);
                    dataGridView.DataMember = excelTable.StringId;
                }
                else
                {
                    dataTable = xlsDataSet.Tables.Add(excelTable.StringId);

                    // main table data
                    progress.SetLoadingText("Generating table data...");
                    DataColumn indexColumn = dataTable.Columns.Add("Index");
                    indexColumn.AutoIncrement = true;
                    indexColumn.Unique = true;
                    dataTable.PrimaryKey = new DataColumn[] { indexColumn };

                    object[] array = (object[])excelTable.GetTableArray();
                    Type type = array[0].GetType();
                    foreach (MemberInfo memberInfo in type.GetFields())
                    {
                        DataColumn dataColumn = dataTable.Columns.Add(memberInfo.Name);

                        ExcelTables.ExcelOutputAttribute excelOutputAttribute = null;
                        foreach (Attribute attribute in memberInfo.GetCustomAttributes(typeof(ExcelTables.ExcelOutputAttribute), true))
                        {
                            excelOutputAttribute = attribute as ExcelTables.ExcelOutputAttribute;
                            if (excelOutputAttribute != null)
                            {
                                break;
                            }
                        }

                        if (excelOutputAttribute != null)
                        {
                            if (excelOutputAttribute.IsStringOffset)
                            {
                                DataColumn dc = dataTable.Columns.Add(dataColumn.ColumnName + "_string");
                                dataColumn.ExtendedProperties.Add("IsStringOffset", true);
                            }
                        }
                    }



                    progress.ConfigBar(0, array.Length, 1);
                    int row = 0;
                    foreach (Object table in array)
                    {
                        int col = 1;

                        progress.SetCurrentItemText("Row " + row + " of " + array.Length);
                        if (row % 2 == 0)
                        {
                            progress.Refresh();
                        }


                        DataRow dataRow = dataTable.Rows.Add();


                        foreach (FieldInfo fieldInfo in type.GetFields())
                        {
                            ExcelTables.ExcelOutputAttribute excelOutputAttribute = null;

                            MemberInfo memberInfo = fieldInfo as MemberInfo;
                            foreach (Attribute attribute in memberInfo.GetCustomAttributes(typeof(ExcelTables.ExcelOutputAttribute), true))
                            {
                                excelOutputAttribute = attribute as ExcelTables.ExcelOutputAttribute;
                                if (excelOutputAttribute != null)
                                {
                                    break;
                                }
                            }

                            Object value = fieldInfo.GetValue(table);
                            if (excelOutputAttribute != null && false)
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

                                        // dataGridView[col, row] = cell;
                                        value = null;
                                    }
                                }
                            }

                            if (value != null)
                            {
                                dataRow[col] = value;
                            }
                            col++;

                            if (excelOutputAttribute != null)
                            {
                                if (excelOutputAttribute.IsStringOffset)
                                {
                                    col++;
                                }
                            }
                        }

                        row++;
                    }

                    dataGridView.DataMember = excelTable.StringId;
                }
            }


            if (!readFromDataSet)
            {
                DataTable dtStrings = xlsDataSet.Tables[excelTable.StringId + "_STRINGS"];
                DataTable dtData = xlsDataSet.Tables[excelTable.StringId];
                DataColumn dcParent = dtStrings.Columns["offset"];
                foreach (DataColumn dc in dtData.Columns)
                {
                    if (dc.ExtendedProperties.ContainsKey("IsStringOffset"))
                    {
                        DataColumn dcChild = dc;
                        String relationName = excelTable.StringId + dcChild.ColumnName + "StringOffset";
                        DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                        xlsDataSet.Relations.Add(relation);

                        DataColumn dcString = dtData.Columns[dcChild.ColumnName + "_string"];
                        dcString.Expression = "Parent(" + relationName + ").string";
                    }
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
                            // should we still use the "official" one?
                            // or leave as autogenerated - has anyone ever seen it NOT be ascending from 0?
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


            xlsDataSet.WriteXml("dataSet.xml", XmlWriteMode.WriteSchema);
            dataGridView.AutoResizeColumns();
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

        private void button1_Click(object sender, EventArgs e)
        {
        //    DataTable dataTable = xlsDataSet.Tables[0];
          //  DataRow[] dataRows = dataTable.Select("name = 'goggles'");
        }
    }
}
