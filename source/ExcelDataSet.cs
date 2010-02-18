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

namespace Reanimator
{
    public class ExcelDataSet
    {
        DataSet xlsDataSet;
        Hashtable xlsDataTables;
        static String xlsFilePath = @"cache\dataSet.dat";

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
            //xlsDataSet = new DataSet("xlsDataSet");
           // xlsDataSet.RemotingFormat = SerializationFormat.Binary;
            this.LoadDataSet();
           // if (this.LoadSchema())
         //   {
         ///       this.SaveSchema();
         //   }
            xlsDataSet.RemotingFormat = SerializationFormat.Binary;
            xlsDataTables = new Hashtable();
        }

        private bool LoadSchema()
        {
            if (File.Exists(xlsFilePath))
            {
                try
                {
                    xlsDataSet.ReadXmlSchema(xlsFilePath);
                    return false;
                }
                catch (System.Xml.XmlException e)
                {
                    MessageBox.Show("Failed to correctly parse table schema!\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to load table schema!\n\n" + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                Directory.CreateDirectory(@"cache\");
            }

            return true;
        }

        private void SaveSchema()
        {
            xlsDataSet.WriteXmlSchema(xlsFilePath);
        }


        private void LoadDataSet()
        {
            if (File.Exists(xlsFilePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(xlsFilePath, FileMode.Open, FileAccess.Read);
                xlsDataSet = bf.Deserialize(fs) as DataSet;
            }
            else
            {
                xlsDataSet = new DataSet("xlsDataSet");
            }
        }

        public void SaveDataSet()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(xlsFilePath, FileMode.Create, FileAccess.ReadWrite);
            bf.Serialize(fs, xlsDataSet);
            fs.Close();
        }

        public void LoadTables(ProgressForm progress, Object var)
        {
            ExcelTable excelTable = var as ExcelTable;
            if (excelTable == null)
            {
                return;
            }


            // load string tables if applicable
            if (excelTable.Strings.Count > 0)
            {
                String stringsTableName = excelTable.StringId + "_STRINGS";
                if (!xlsDataTables.ContainsKey(stringsTableName))
                {
                    String xmlStringsFilePath = @"cache\" + stringsTableName + ".dat";
                    DataTable stringsDataTable = xlsDataSet.Tables[stringsTableName];

                    if (File.Exists(xmlStringsFilePath))
                    {
                        if (progress != null)
                        {
                            progress.SetLoadingText("Loading strings table xml file...");
                        }
                        /*

                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(xmlStringsFilePath, FileMode.Open, FileAccess.Read);
                        DataTable dataTable = bf.Deserialize(fs) as DataTable;
                        xlsDataSet.Tables[stringsTableName].Load(dataTable.CreateDataReader(), LoadOption.OverwriteChanges);
                        */


                        //xlsDataSet.ReadXml(xmlStringsFilePath, XmlReadMode.IgnoreSchema);
                        stringsDataTable = xlsDataSet.Tables[stringsTableName];
                    }
                    else
                    {
                        if (progress != null)
                        {
                            progress.SetLoadingText("First time generation of strings table data..." + " (Table: " + excelTable.StringId + ")");
                        }
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

                        /*
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(xmlStringsFilePath, FileMode.Create, FileAccess.ReadWrite);
                        stringsDataTable.RemotingFormat = SerializationFormat.Binary;
                        bf.Serialize(fs, stringsDataTable);
                        xlsDataSet.Tables[stringsTableName].Load(stringsDataTable.CreateDataReader(), LoadOption.OverwriteChanges);
                        */


                        //stringsDataTable.WriteXml(xmlStringsFilePath, XmlWriteMode.IgnoreSchema);
                    }

                    this.xlsDataTables.Add(stringsTableName, stringsDataTable);
                }
            }


            // load in main data table
            String tableName = excelTable.StringId;
            if (!xlsDataTables.ContainsKey(tableName))
            {
                String xmlFilePath = @"cache\" + tableName + ".dat";
                DataTable dataTable = xlsDataSet.Tables[tableName];

                if (File.Exists(xmlFilePath))
                {
                    if (progress != null)
                    {
                        progress.SetLoadingText("Loading table xml file...");
                    }

                    /*
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read);
                    DataTable dataTable2 = bf.Deserialize(fs) as DataTable;
                    if (xlsDataSet.Tables.Contains(tableName))
                    {
                        xlsDataSet.Tables[tableName].Load(dataTable2.CreateDataReader(), LoadOption.OverwriteChanges);
                    }
                    else
                    {
                        xlsDataSet.Tables.Add(dataTable2);
                    }*/


                    //xlsDataSet.ReadXml(xmlFilePath, XmlReadMode.IgnoreSchema);
                    dataTable = xlsDataSet.Tables[tableName];
                }
                else
                {
                    if (progress != null)
                    {
                        progress.SetLoadingText("First time generation of table data..." + " (Table: " + excelTable.StringId + ")");
                    }

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

                    if (progress != null)
                    {
                        progress.ConfigBar(0, array.Length, 1);
                    }

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

                    if (progress != null)
                    {
                        progress.SetLoadingText("Saving table xml file...");
                    }

                    /*

                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(xmlFilePath, FileMode.Create, FileAccess.ReadWrite);
                    dataTable.RemotingFormat = SerializationFormat.Binary;
                    bf.Serialize(fs, dataTable);
                    xlsDataSet.Tables[tableName].Load(dataTable.CreateDataReader(), LoadOption.OverwriteChanges);
                    
                    */


                    //dataTable.WriteXml(xmlFilePath, XmlWriteMode.IgnoreSchema);
                }

                this.xlsDataTables.Add(tableName, dataTable);

                //this.SaveDataSet();
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

          //  this.SaveSchema();
        }

        public DataSet GetDataSet()
        {
            return xlsDataSet;
        }
    }
}
