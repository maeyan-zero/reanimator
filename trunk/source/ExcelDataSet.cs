using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Reanimator.Forms;
using System.IO;
using Reanimator.Excel;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace Reanimator
{
    public class ExcelDataSet
    {
        DataSet xlsDataSet;
        Hashtable xlsDataTables;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableIndexDataSource
        {
            public int Unknowns1 { get; set; }
            public int Unknowns2 { get; set; }
            public int Unknowns3 { get; set; }
            public int Unknowns4 { get; set; }
        };

        public ExcelDataSet()
        {
            this.LoadDataSet();
            xlsDataSet.RemotingFormat = SerializationFormat.Binary;
            xlsDataTables = new Hashtable();
        }

        private void LoadDataSet()
        {
            if (File.Exists(Config.cacheFilePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(Config.cacheFilePath, FileMode.Open, FileAccess.Read))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        xlsDataSet = bf.Deserialize(fs) as DataSet;
                    }

                    return;
                }
                catch (Exception) { }
            }

            Directory.CreateDirectory(Config.cacheFilePath.Substring(0, Config.cacheFilePath.LastIndexOf(@"\") + 1));
            xlsDataSet = new DataSet("xlsDataSet");
        }

        public void SaveDataSet()
        {
            using (FileStream fs = new FileStream(Config.cacheFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, xlsDataSet);
                fs.Close();
            }
        }

        public void ClearDataSet()
        {
            xlsDataSet.Clear();
            xlsDataSet.Relations.Clear();
            xlsDataSet.Tables.Clear();
        }

        public void LoadTables(ProgressForm progress, Object var)
        {
            ExcelTable excelTable = var as ExcelTable;
            if (excelTable == null)
            {
                return;
            }


            // load string tables if applicable
            String mainTableName = excelTable.StringId;
            String stringsTableName = mainTableName + "_STRINGS";
            DataTable stringsDataTable = xlsDataSet.Tables[stringsTableName];
            if (excelTable.Strings.Count > 0)
            {
                if (!xlsDataSet.Tables.Contains(stringsTableName))
                {
                    String xmlStringsFilePath = @"cache\" + stringsTableName + ".dat";

                    if (progress != null)
                    {
                        progress.SetLoadingText("Cache generation of strings data... " + stringsTableName);
                    }
                    if (stringsDataTable == null)
                    {
                        stringsDataTable = xlsDataSet.Tables.Add(stringsTableName);
                    }

                    DataColumn offsetColumn = stringsDataTable.Columns["offset"];
                    if (offsetColumn == null)
                    {
                        offsetColumn = stringsDataTable.Columns.Add("offset", typeof(int));
                        offsetColumn.Unique = true;
                        stringsDataTable.PrimaryKey = new DataColumn[] { offsetColumn };
                    }

                    if (!stringsDataTable.Columns.Contains("string"))
                    {
                        stringsDataTable.Columns.Add("string", typeof(String));
                    }

                    foreach (DictionaryEntry entry in excelTable.Strings)
                    {
                        stringsDataTable.Rows.Add(entry.Key, entry.Value);
                    }

                    this.xlsDataTables.Add(stringsTableName, stringsDataTable);
                }
            }


            // load in main data table
            DataTable mainDataTable = xlsDataSet.Tables[mainTableName];
            if (!xlsDataSet.Tables.Contains(mainTableName))
            {
                String xmlFilePath = @"cache\" + mainTableName + ".dat";

                if (progress != null)
                {
                    progress.SetLoadingText("Cache generation of table data... " + mainTableName);
                }

                if (mainDataTable == null)
                {
                    mainDataTable = xlsDataSet.Tables.Add(excelTable.StringId);
                }
                object[] array = (object[])excelTable.GetTableArray();
                Type type = array[0].GetType();
                List<ExcelTables.ExcelOutputAttribute> outputAttributes = new List<ExcelTables.ExcelOutputAttribute>(type.GetFields().Length + 1);

                #region generate_columns
                DataColumn indexColumn = mainDataTable.Columns.Add("index");
                indexColumn.AutoIncrement = true;
                indexColumn.Unique = true;
                mainDataTable.PrimaryKey = new DataColumn[] { indexColumn };
                outputAttributes.Add(null);

                foreach (FieldInfo memberInfo in type.GetFields())
                {
                    DataColumn dataColumn = mainDataTable.Columns.Add(memberInfo.Name, memberInfo.FieldType);
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
                        outputAttributes.Add(excelOutputAttribute);

                        if (excelOutputAttribute.IsStringOffset && stringsDataTable != null)
                        {
                            DataColumn dcString = mainDataTable.Columns.Add(dataColumn.ColumnName + "_string", typeof(String));
                            dcString.DefaultValue = String.Empty;
                            dcString.AllowDBNull = false;

                            outputAttributes.Add(null);
                        }
                    }
                    else
                    {
                        outputAttributes.Add(null);
                    }
                }
                #endregion

                if (progress != null)
                {
                    progress.ConfigBar(0, array.Length, 1);
                }

                #region generate_rows
                int row = 1;
                object[] baseRow = new object[outputAttributes.Count];

                foreach (Object table in array)
                {
                    progress.SetCurrentItemText("Row " + row + " of " + array.Length);
                    int col = 1;

                    foreach (FieldInfo fieldInfo in type.GetFields())
                    {
                        Object value = fieldInfo.GetValue(table);
                        ExcelTables.ExcelOutputAttribute excelOutputAttribute = outputAttributes[col];

                        /*
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
                        }*/

                        if (value != null)
                        {
                            baseRow[col] = value;
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

                    mainDataTable.Rows.Add(baseRow);
                    row++;
                }
                #endregion

                #region generate_relations
                if (mainDataTable.ChildRelations.Count == 0 && mainDataTable.ParentRelations.Count == 0 && stringsDataTable != null)
                {
                    DataColumn dcParent = stringsDataTable.Columns["offset"];
                    int col = 0;

                    foreach (ExcelTables.ExcelOutputAttribute oa in outputAttributes)
                    {
                        if (oa == null)
                        {
                            col++;
                            continue;
                        }

                        if (oa.IsStringOffset)
                        {
                            DataColumn dcChild = mainDataTable.Columns[col];
                            String relationName = excelTable.StringId + dcChild.ColumnName + "StringOffset";
                            DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                            xlsDataSet.Relations.Add(relation);

                            DataColumn dcString = mainDataTable.Columns[dcChild.ColumnName + "_string"];
                            dcString.Expression = "Parent(" + relationName + ").string";
                        }

                        col++;
                    }
                }
                #endregion

                this.xlsDataTables.Add(mainTableName, mainDataTable);
            }


            // generate the table index data source
            // TODO is there a better way?
            // TODO get rid of me
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
        }

        public DataSet GetDataSet()
        {
            return xlsDataSet;
        }

        public int LoadedTableCount
        {
            get { return xlsDataSet.Tables.Count; }
        }
    }
}
