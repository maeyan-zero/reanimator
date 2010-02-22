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
using System.Runtime.Serialization.Formatters;
using System.Globalization;

namespace Reanimator
{
    public class ExcelDataSet : IDisposable
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
            if (File.Exists(Config.CacheFilePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(Config.CacheFilePath, FileMode.Open, FileAccess.Read))
                    {

                        BinaryFormatter bf = new BinaryFormatter();
                        bf.TypeFormat = FormatterTypeStyle.XsdString;
                        xlsDataSet = bf.Deserialize(fs) as DataSet;
                    }

                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to load table cache!\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Directory.CreateDirectory(Config.CacheFilePath.Substring(0, Config.CacheFilePath.LastIndexOf(@"\") + 1));
            xlsDataSet = new DataSet("xlsDataSet");
            xlsDataSet.Locale = new CultureInfo("en-us", true);
        }

        public void SaveDataSet()
        {
            using (FileStream fs = new FileStream(Config.CacheFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.TypeFormat = FormatterTypeStyle.XsdString;
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

        public void LoadTable(ProgressForm progress, Object var)
        {
            ExcelTable excelTable = var as ExcelTable;
            if (excelTable == null)
            {
                return;
            }



            /*
            // load string tables if applicable
            
            String stringsTableName = mainTableName + "_STRINGS";
            DataTable stringsDataTable = xlsDataSet.Tables[stringsTableName];
            if (excelTable.Strings.Count > 0)
            {
                if (!xlsDataSet.Tables.Contains(stringsTableName))
                {
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
            */

            // load in main data table
            String mainTableName = excelTable.StringId;
            DataTable mainDataTable = xlsDataSet.Tables[mainTableName];
            if (!xlsDataSet.Tables.Contains(mainTableName))
            {
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

                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    ExcelTables.ExcelOutputAttribute excelOutputAttribute = null;

                    foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(ExcelTables.ExcelOutputAttribute), true))
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

                        if (excelOutputAttribute.IsStringOffset == true)
                        {
                            DataColumn dataColumn = mainDataTable.Columns.Add(fieldInfo.Name, typeof(String));
                            dataColumn.ExtendedProperties.Add("IsStringOffset", true);
                            continue;
                        }
                    }
                    else
                    {
                        outputAttributes.Add(null);
                    }

                    mainDataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
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

                        if (excelOutputAttribute != null)
                        {
                            if (excelOutputAttribute.IsStringOffset == true)
                            {
                                value = excelTable.Strings[value];
                            }
                        }

                        baseRow[col] = value;
                        col++;

                        if (excelOutputAttribute != null)
                        {
                            if (excelOutputAttribute.IsStringOffset)
                            {
                                //col++;
                            }
                        }
                    }

                    mainDataTable.Rows.Add(baseRow);
                    row++;
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
                            // dataGridView[i, j].Value = intArrays[i][j];
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

        public void ClearRelations()
        {
            xlsDataSet.Relations.Clear();
        }

        public void GenerateRelations(ExcelTable excelTable)
        {
            String isRelExtKey = "IsRelationGenerated";
            object[] array = (object[])excelTable.GetTableArray();
            Type type = array[0].GetType();
            int col = 0;

            String mainTableName = excelTable.StringId;
            //String stringsTableName = mainTableName + "_STRINGS";
            DataTable mainDataTable = xlsDataSet.Tables[mainTableName];
            //DataTable stringsDataTable = xlsDataSet.Tables[stringsTableName];

            // remove all extra generated columns on this table
            for (col = 0; col < mainDataTable.Columns.Count; col++)
            {
                DataColumn dc = mainDataTable.Columns[col];

                if (dc.ExtendedProperties.Contains(isRelExtKey))
                {
                    if ((bool)dc.ExtendedProperties[isRelExtKey] == true)
                    {
                        mainDataTable.Columns.Remove(dc);
                    }
                }
            }

            col = 1;
            // regenerate relations
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                ExcelTables.ExcelOutputAttribute excelOutputAttribute = null;

                foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(ExcelTables.ExcelOutputAttribute), true))
                {
                    excelOutputAttribute = attribute as ExcelTables.ExcelOutputAttribute;
                    if (excelOutputAttribute != null)
                    {
                        break;
                    }
                }

                if (excelOutputAttribute != null)
                {
                    DataColumn dcChild = mainDataTable.Columns[col];

                    /*if (excelOutputAttribute.IsStringOffset && stringsDataTable != null)
                    {
                        DataColumn dcParent = stringsDataTable.Columns["offset"];
                        
                        String relationName = excelTable.StringId + dcChild.ColumnName + "StringOffset";
                        DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                        xlsDataSet.Relations.Add(relation);

                        DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String), "Parent(" + relationName + ").string");
                        dcString.SetOrdinal(col+1);
                        dcString.ExtendedProperties.Add("IsStringOffset", true);
                        col++;
                    }
                    else*/ if (excelOutputAttribute.IsStringId)
                    {
                        DataTable dtStrings = xlsDataSet.Tables[excelOutputAttribute.StringTable];
                        if (dtStrings != null)
                        {
                            DataColumn dcParent = dtStrings.Columns["ReferenceId"];
                            
                            String relationName = excelTable.StringId + dcChild.ColumnName + "StringId";
                            DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                            xlsDataSet.Relations.Add(relation);

                            DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String), "Parent(" + relationName + ").String");
                            dcString.SetOrdinal(col + 1);
                            dcString.ExtendedProperties.Add("IsRelationGenerated", true);
                            col++;
                        }
                    }
                }

                col++;
            }
        }

        public DataSet XlsDataSet
        {
            get { return xlsDataSet; }
        }

        public int LoadedTableCount
        {
            get { return xlsDataSet.Tables.Count; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (xlsDataSet != null)
            {
                xlsDataSet.Dispose();
            }
        }

        #endregion
    }
}
