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
    public partial class ExcelTableForm : ThreadedFormBase
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

        public String GetExcelTableName()
        {
            return excelTable.StringId;
        }

        public ExcelTableForm(ExcelTable table)
        {
            InitializeComponent();
            excelTable = table;

            dataGridView.DoubleBuffered(true);
            dataGridView.EnableHeadersVisualStyles = false;
            //dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            //dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.SkyBlue;
            dataGridView.DataSource = xlsDataSet;
            listBox1.DataSource = xlsDataSet;
            
            ProgressForm progress = new ProgressForm(LoadTables, null);
            progress.ShowDialog(this);
            this.Hide();
            this.Show();
        }

        static DataSet xlsDataSet = new DataSet("xlsDataSet");

        private void LoadTables(ProgressForm progress, Object var)
        {
            // file id
            this.textBox1.Text = "0x" + excelTable.StructureId.ToString("X");


            // load our data set design
            String dataSetSchemaString = @"cache\dataSet.xsd";
            if (File.Exists(dataSetSchemaString))
            {
                xlsDataSet.ReadXmlSchema(dataSetSchemaString);
            }
            else
            {
                Directory.CreateDirectory(@"cache\");
            }


            // load string tables if applicable
            if (excelTable.Strings.Count > 0)
            {
                String stringsTableName = excelTable.StringId + "_STRINGS";
                String xmlStringsFilePath = @"cache\" + stringsTableName + ".xml";

                DataTable stringsDataTable = xlsDataSet.Tables[stringsTableName];
                bool loadInTable = false;
                if (stringsDataTable == null)
                {
                    loadInTable = true;
                }
                else
                {
                    if (stringsDataTable.Rows.Count == 0)
                    {
                        loadInTable = true;
                    }
                }

                if (loadInTable)
                {
                    if (File.Exists(xmlStringsFilePath))
                    {
                        progress.SetLoadingText("Loading strings table xml file...");
                        xlsDataSet.ReadXml(xmlStringsFilePath, XmlReadMode.IgnoreSchema);
                    }
                    else
                    {
                        progress.SetLoadingText("First time generation of strings table data...");
                        if (stringsDataTable == null)
                        {
                            stringsDataTable = xlsDataSet.Tables.Add(stringsTableName);
                        }

                        DataColumn offsetColumn = stringsDataTable.Columns["offset"];
                        if (offsetColumn == null)
                        {
                            offsetColumn = stringsDataTable.Columns.Add("offset");
                            offsetColumn.Unique = true;
                            stringsDataTable.PrimaryKey = new DataColumn[] { offsetColumn };
                        }

                        if (!stringsDataTable.Columns.Contains("string"))
                        {
                            stringsDataTable.Columns.Add("string");
                        }

                        foreach (DictionaryEntry entry in excelTable.Strings)
                        {
                            stringsDataTable.Rows.Add(entry.Key, entry.Value);
                        }

                        stringsDataTable.WriteXml(xmlStringsFilePath, XmlWriteMode.IgnoreSchema);
                    }
                }

                listBox1.DisplayMember = stringsTableName + ".string";
                listBox1.ValueMember = stringsTableName + ".offset";
            }
            else
            {
                // should we hid/remove list box when no strings?
                // TODO
                listBox1.DataSource = null;
            }


            // load in main data table
            String tableName = excelTable.StringId;
            String xmlFilePath = @"cache\" + tableName + ".xml";

            DataTable dataTable = xlsDataSet.Tables[tableName];
            bool loadInDataTable = false;
            if (dataTable == null)
            {
                loadInDataTable = true;
            }
            else
            {
                if (dataTable.Rows.Count == 0)
                {
                    loadInDataTable = true;
                }
            }

            if (loadInDataTable)
            {
                if (File.Exists(xmlFilePath))
                {
                    progress.SetLoadingText("Loading table xml file...");
                    xlsDataSet.ReadXml(xmlFilePath, XmlReadMode.IgnoreSchema);
                }
                else
                {
                    progress.SetLoadingText("First time generation of table data...");

                    if (dataTable == null)
                    {
                        dataTable = xlsDataSet.Tables.Add(excelTable.StringId);
                    }
                    object[] array = (object[])excelTable.GetTableArray();
                    Type type = array[0].GetType();

                    #region generate_columns
                    DataColumn indexColumn = dataTable.Columns.Add("index");
                    indexColumn.AutoIncrement = true;
                    indexColumn.Unique = true;
                    dataTable.PrimaryKey = new DataColumn[] { indexColumn };

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
                    #endregion

                    progress.ConfigBar(0, array.Length, 1);

                    #region generate_rows
                    int row = 0;

                    foreach (Object table in array)
                    {
                        progress.SetCurrentItemText("Row " + row + " of " + array.Length);

                        DataRow dataRow = dataTable.Rows.Add();
                        int col = 1;

                        foreach (FieldInfo fieldInfo in type.GetFields())
                        {
                            Object value = fieldInfo.GetValue(table);
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

                            // to fix up / reimplement
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
                    #endregion

                    #region generate_relations
                    DataTable dtStrings = xlsDataSet.Tables[excelTable.StringId + "_STRINGS"];
                    DataTable dtData = xlsDataSet.Tables[excelTable.StringId];

                    if (dtData.ChildRelations.Count == 0 && dtData.ParentRelations.Count == 0 && dtStrings != null)
                    {
                        DataColumn dcParent = dtStrings.Columns["offset"];

                        foreach (DataColumn dc in dtData.Columns)
                        {
                            if (dc.ExtendedProperties.ContainsKey("IsStringOffset"))
                            {
                                DataColumn dcChild = dc;
                                String relationName = excelTable.StringId + dcChild.ColumnName + "StringOffset";
                                DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                                xlsDataSet.Relations.Add(relation);

                                dcChild.AllowDBNull = false;
                                dcChild.DefaultValue = String.Empty;
                                DataColumn dcString = dtData.Columns[dcChild.ColumnName + "_string"];
                                dcString.Expression = "Parent(" + relationName + ").string";

                            }
                        }
                    }
                    #endregion

                    progress.SetLoadingText("Saving table xml file...");
                    dataTable.WriteXml(xmlFilePath, XmlWriteMode.IgnoreSchema);
                }
            }

            dataGridView.DataMember = excelTable.StringId;

            // generate the table index data source
            // TODO is there a better way?
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

            xlsDataSet.WriteXmlSchema(dataSetSchemaString);
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
            DataTable affixTable = xlsDataSet.Tables["AFFIXES"];
            EnumerableRowCollection<DataRow> query = from affix in affixTable.AsEnumerable()
                                                     where affix.Field<string>("affix").CompareTo("-1") != 0
                                                     orderby affix.Field<string>("affix_string")
                                                     select affix;

            DataView view = query.AsDataView();


            /*   EnumerableRowCollection<DataRow> query2 = from affix in view.GetEnumerator()
                                                        where affix.Field<string>("affix_string").StartsWith("Pet")
                                                        orderby affix.Field<string>("affix_string")
                                                        select affix;

               view = query2.AsDataView();*/
            dataGridView.DataSource = view;
            dataGridView.DataMember = null;


            //DataTable dataTable = xlsDataSet.Tables[0];
            //   DataRow[] dataRows = dataTable.Select("name = 'goggles'");
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
