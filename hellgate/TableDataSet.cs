using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Globalization;
using Revival.Common;
using OutputAttribute = Hellgate.ExcelFile.OutputAttribute;
using ColumnKeys = Hellgate.ExcelFile.ColumnTypeKeys;

namespace Hellgate
{
    public partial class FileManager
    {
        public void ClearDataSet()
        {
            XlsDataSet.Clear();
            XlsDataSet.Relations.Clear();
            XlsDataSet.Tables.Clear();
        }

        public DataTable LoadTable(DataFile dataFile, bool relations)
        {
            if ((XlsDataSet == null))
            {
                XlsDataSet = new DataSet("xlsDataSet")
                {
                    Locale = new CultureInfo("en-us", true),
                    RemotingFormat = SerializationFormat.Binary
                };
            }

            DataTable dataTable = null;

            if ((dataFile.IsStringsFile))
                dataTable = LoadStringsTable(dataFile as StringsFile);

            if ((dataFile.IsExcelFile))
                dataTable = LoadExcelTable(dataFile as ExcelFile, relations);

            return dataTable;
        }

        private DataTable LoadExcelTable(ExcelFile excelFile, bool relations)
        {
            String tableName = excelFile.StringID;
            DataTable dataTable = XlsDataSet.Tables[tableName];
            if (!(dataTable == null)) return dataTable;

            dataTable = XlsDataSet.Tables.Add(tableName);
            dataTable.TableName = tableName;

            Type dataType = excelFile.Rows[0].GetType();
            List<OutputAttribute> outputAttributes = new List<OutputAttribute>();

            #region Generate Columns
            DataColumn indexColumn = dataTable.Columns.Add("Index");
            indexColumn.AutoIncrement = true;
            indexColumn.Unique = true;
            dataTable.PrimaryKey = new[] { indexColumn };
            outputAttributes.Add(null);

            foreach (FieldInfo fieldInfo in dataType.GetFields())
            {
                OutputAttribute excelAttribute = ExcelFile.GetExcelOutputAttribute(fieldInfo);

                if ((excelAttribute == null))
                {
                    outputAttributes.Add(null);
                    dataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
                    continue;
                }

                outputAttributes.Add(excelAttribute);

                DataColumn dataColumn = dataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
                
                // Define DataColumn DataType, overrides default above
                if ((excelAttribute.IsStringOffset))
                {
                    dataColumn.DataType = typeof(String);
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsStringOffset, true);
                    dataColumn.DefaultValue = String.Empty;
                }

                if ((excelAttribute.IsIntOffset))
                {
                    dataColumn.DataType = typeof(String);
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsIntOffset, true);
                    dataColumn.DefaultValue = String.Empty;
                }

                if ((excelAttribute.IsSecondaryString))
                {
                    dataColumn.DataType = typeof(String);
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsSecondaryString, true);
                    dataColumn.DefaultValue = String.Empty;
                }

                if ((excelAttribute.IsStringIndex))
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsStringIndex, true);

                    // Add new column for the string
                    DataColumn dataColumnString = dataTable.Columns.Add(fieldInfo.Name + "_string", typeof(String));
                    dataColumnString.DefaultValue = String.Empty;
                    outputAttributes.Add(null);
                    dataColumnString.ExtendedProperties.Add(ColumnKeys.IsRelationGenerated, true);
                }

                if ((excelAttribute.IsTableIndex))
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsTableIndex, true);

                    // Add new column for the string
                    DataColumn dataColumnString = dataTable.Columns.Add(fieldInfo.Name + "_string", typeof(String));
                    dataColumnString.DefaultValue = String.Empty;
                    outputAttributes.Add(null);
                    dataColumnString.ExtendedProperties.Add(ColumnKeys.IsRelationGenerated, true);
                }

                if ((excelAttribute.IsBitmask))
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsBitmask, true);
                }

                if ((excelAttribute.IsBool))
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsBool, true);
                }
            }

            if ((excelFile.ExcelMap.HasExtended)) // items, missiles, monsters, objects, players
            {
                DataColumn extendedDataColumn = dataTable.Columns.Add("ExtendedProperties");
                extendedDataColumn.DataType = typeof(String);
                extendedDataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsExtendedProps, true);
                outputAttributes.Add(null);
            }
            #endregion

            #region Generate Rows
            int row = 1;
            object[] baseRow = new object[outputAttributes.Count];

            foreach (Object tableRow in excelFile.Rows)
            {
                int col = 1;
                foreach (FieldInfo fieldInfo in dataType.GetFields())
                {
                    Object value = fieldInfo.GetValue(tableRow);
                    OutputAttribute excelOutputAttribute = outputAttributes[col];

                    if ((excelOutputAttribute == null))
                    {
                        baseRow[col++] = value;
                        continue;
                    }

                    if ((excelOutputAttribute.IsStringOffset))
                    {
                        int valueInt = (int)value;
                        baseRow[col++] = (!(valueInt == -1)) ? excelFile.ReadStringTable(valueInt) : String.Empty;
                        continue;
                    }

                    if ((excelOutputAttribute.IsSecondaryString))
                    {
                        int valueInt = (int)value;
                        baseRow[col++] = (!(valueInt == -1)) ? excelFile.ReadSecondaryStringTable(valueInt) : String.Empty;
                        continue;
                    }

                    if ((excelOutputAttribute.IsIntOffset))
                    {
                        int valueInt = (int)value;
                        baseRow[col++] = (!(valueInt == 0)) ? FileTools.ArrayToStringGeneric(excelFile.ReadIntegerTable(valueInt), ",") : String.Empty;
                        continue;
                    }

                    if ((excelOutputAttribute.IsTableIndex) || (excelOutputAttribute.IsStringIndex))
                    {
                        int valueInt = (int)value;
                        baseRow[col++] = valueInt;
                        col++;
                        continue;
                    }

                    // Else its something else, ie bitmask, bool, table/string index
                    baseRow[col++] = value;
                }

                // Extended properties, only a component of the items class
                if ((excelFile.ExcelMap.HasExtended))
                {
                    baseRow[col++] = FileTools.ArrayToStringGeneric(excelFile.ReadExtendedProperties(row - 1), ",");
                }

                dataTable.Rows.Add(baseRow);
                row++;
            }
            #endregion

            // Generate Relationships as required
            if ((relations))
                GenerateRelations(excelFile);

            return dataTable;
        }

        private DataTable LoadStringsTable(StringsFile stringsFile)
        {
            String tableName = stringsFile.StringID;
            DataTable dataTable = XlsDataSet.Tables[tableName];
            if (!(dataTable == null)) return dataTable;

            dataTable = XlsDataSet.Tables.Add(tableName);
            dataTable.TableName = tableName;

            Type dataType = typeof(StringsFile.StringBlock);
            
            // Generate Columns
            foreach (FieldInfo fieldInfo in dataType.GetFields())
            {
                dataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
            }

            // Generate Rows
            foreach (StringsFile.StringBlock tableRow in stringsFile.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                int col = 0;
                foreach (FieldInfo fieldInfo in dataType.GetFields())
                {
                    dataRow[col++] = fieldInfo.GetValue(tableRow);
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private DataTable LoadRelatedTable(String stringID)
        {
            if ((String.IsNullOrEmpty(stringID))) return null;

            DataTable dataTable = XlsDataSet.Tables[stringID];
            if (!(dataTable == null)) return dataTable;

            DataFile dataFile = GetDataFile(stringID);
            return (!(dataFile == null)) ? LoadTable(dataFile, false) : null;
        }

        private void GenerateRelations(ExcelFile excelFile)
        {
            if ((excelFile == null)) return;

            List<object> array = excelFile.Rows;
            Type type = excelFile.Rows[0].GetType();
            int col;

            String mainTableName = excelFile.StringID;
            DataTable mainDataTable = XlsDataSet.Tables[mainTableName];

            // remove all extra generated columns on this table
            for (col = 0; col < mainDataTable.Columns.Count; col++)
            {
                DataColumn dc = mainDataTable.Columns[col];

                if (!(dc.ExtendedProperties.Contains(ColumnKeys.IsRelationGenerated))) continue;
                
                mainDataTable.Columns.Remove(dc);
            }

            col = 1;
            // regenerate relations
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                OutputAttribute excelOutputAttribute =
                    ExcelFile.GetExcelOutputAttribute(fieldInfo);

                if ((excelOutputAttribute == null))
                {
                    col++;
                    continue;
                }

                DataColumn dcChild = mainDataTable.Columns[col];

                if ((excelOutputAttribute.IsStringIndex))
                {
                    DataTable dtStrings = XlsDataSet.Tables[excelOutputAttribute.TableStringID] ??
                                          LoadRelatedTable(excelOutputAttribute.TableStringID);

                    if (dtStrings != null)
                    {
                        DataColumn dcParent = dtStrings.Columns["ReferenceId"];

                        String relationName = String.Format("{0}_{1}_{2}", excelFile.StringID, dcChild.ColumnName, ColumnKeys.IsStringIndex);
                        // parent and child are swapped here for StringId relations
                        DataRelation relation = new DataRelation(relationName, dcChild, dcParent, false);
                        XlsDataSet.Relations.Add(relation);

                        DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String));
                        dcString.SetOrdinal(col + 1);
                        dcString.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsRelationGenerated, true);
                        // need to use MIN (and thus Child) for cases of strings with same reference id (e.g. Items; SINGULAR and PLURAL, etc)
                        dcString.Expression = "MIN(Child(" + relationName + ").String)";
                        col++;
                    }
                }

                if ((excelOutputAttribute.IsTableIndex))
                {
                    String tableStringId = excelOutputAttribute.TableStringID;

                    DataTable dt = XlsDataSet.Tables[tableStringId] ?? LoadRelatedTable(tableStringId);

                    if (dt != null)
                    {
                        DataColumn dcParent = dt.Columns["Index"];

                        string relatedColumn = dt.Columns[1].ColumnName;


                        String relationName = excelFile.StringID + dcChild.ColumnName + ExcelFile.ColumnTypeKeys.IsTableIndex;
                        DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                        XlsDataSet.Relations.Add(relation);

                        DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String), String.Format("Parent({0}).{1}", relationName, relatedColumn));
                        dcString.SetOrdinal(col + 1);
                        dcString.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsRelationGenerated, true);
                        col++;
                    }
                }

                col++;
            }
        }

        /// <summary>
        /// Gets an excel DataTable from the DataSet.<br />
        /// If it has not been generated, it will be generated and stored.
        /// </summary>
        /// <param name="stringId">The excel table string name (see EXCELTABLE "StringId" column).</param>
        /// <returns>The excel as a DataTable or null if not valid StringId.</returns>
        public DataTable GetDataTable(String stringID)
        {
            if ((String.IsNullOrEmpty(stringID))) return null;
            return XlsDataSet.Tables[stringID] ?? LoadRelatedTable(stringID);
        }

        public int LoadedDataTableCount
        {
            get { return (!(XlsDataSet == null)) ? XlsDataSet.Tables.Count : 0; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (XlsDataSet != null)
            {
                XlsDataSet.Dispose();
            }
        }

        #endregion
    }
}
