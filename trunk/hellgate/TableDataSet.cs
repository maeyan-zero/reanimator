using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Revival.Common;
using OutputAttribute = Hellgate.ExcelFile.OutputAttribute;
using ColumnKeys = Hellgate.ExcelFile.ColumnTypeKeys;

namespace Hellgate
{
    public partial class FileManager
    {
        public const String StringsTableName = "StringsTable";

        public DataTable LoadTable(DataFile dataFile, bool doRelations, bool force = false)
        {
            DataTable dataTable = null;
            if (dataFile.IsStringsFile)
            {
                dataTable = _LoadStringsTable();
            }
            else if (dataFile.IsExcelFile)
            {
                dataTable = _LoadExcelTable(dataFile as ExcelFile, doRelations, force);
            }

            return dataTable;
        }

        private DataTable _LoadExcelTable(ExcelFile excelFile, bool doRelations, bool force)
        {
            String tableName = excelFile.StringId;
            DataTable dataTable = XlsDataSet.Tables[tableName];
            if (dataTable != null && force == false) return dataTable;
            if (dataTable != null && force == true)
            {
                XlsDataSet.Relations.Clear();
                XlsDataSet.Tables.Remove(tableName);
            }
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

            FieldInfo[] fieldInfos = dataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                OutputAttribute excelAttribute = ExcelFile.GetExcelAttribute(fieldInfo);

                // The only private field we add is the TableHeader
                if (fieldInfo.IsPrivate)
                {
                    if (fieldInfo.FieldType != typeof(ExcelFile.RowHeader)) continue;

                    outputAttributes.Add(null);
                    dataTable.Columns.Add(fieldInfo.Name, typeof(String));
                    continue;
                }


                Type fieldType = fieldInfo.FieldType;
                bool isArray = false;
                bool isEnum = false;
                if (fieldInfo.FieldType.BaseType == typeof(Array))
                {
                    fieldType = typeof(String);
                    isArray = true;
                }
                else if (fieldInfo.FieldType.BaseType == typeof(Enum) && excelAttribute == null)
                {
                    fieldType = fieldInfo.FieldType;
                    isEnum = true;
                }

                DataColumn dataColumn = dataTable.Columns.Add(fieldInfo.Name, fieldType);
                if (isArray)
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsArray, true);
                }
                else if (isEnum)
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsEnum, true);
                }

                if (excelAttribute == null)
                {
                    outputAttributes.Add(null);
                    continue;
                }

                outputAttributes.Add(excelAttribute);

                if (excelAttribute.IsStringOffset)
                {
                    dataColumn.DataType = typeof(String);
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsStringOffset, true);
                    dataColumn.DefaultValue = String.Empty;
                }

                if (excelAttribute.IsScript)
                {
                    dataColumn.DataType = typeof(String);
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsScript, true);
                    dataColumn.DefaultValue = String.Empty;
                }

                if (excelAttribute.IsSecondaryString)
                {
                    dataColumn.DataType = typeof(String);
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsSecondaryString, true);
                    dataColumn.DefaultValue = String.Empty;
                }

                if (excelAttribute.IsStringIndex)
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsStringIndex, true);

                    // Add new column for the string
                    DataColumn dataColumnString = dataTable.Columns.Add(fieldInfo.Name + "_string", typeof(String));
                    dataColumnString.DefaultValue = String.Empty;
                    outputAttributes.Add(null);
                    dataColumnString.ExtendedProperties.Add(ColumnKeys.IsRelationGenerated, true);
                }

                if (excelAttribute.IsTableIndex)
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsTableIndex, true);

                    // Add new column for the string
                    DataColumn dataColumnString = dataTable.Columns.Add(fieldInfo.Name + "_string", typeof(String));
                    dataColumnString.DefaultValue = String.Empty;
                    outputAttributes.Add(null);
                    dataColumnString.ExtendedProperties.Add(ColumnKeys.IsRelationGenerated, true);
                }

                if (excelAttribute.IsBitmask)
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsBitmask, true);
                }

                if (excelAttribute.IsBool)
                {
                    dataColumn.ExtendedProperties.Add(ColumnKeys.IsBool, true);
                }
            }

            if (excelFile.Attributes.HasStats) // items, missiles, monsters, objects, players
            {
                DataColumn extendedDataColumn = dataTable.Columns.Add("Stats");
                extendedDataColumn.DataType = typeof(String);
                extendedDataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsStats, true);
                outputAttributes.Add(null);
            }
            #endregion

            #region Generate Rows
            int row = 1;
            object[] baseRow = new object[outputAttributes.Count];
            ObjectDelegator objectDelegator = new ObjectDelegator(fieldInfos);
            foreach (Object tableRow in excelFile.Rows)
            {
                int col = 1;
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    Object value = objectDelegator[fieldInfo.Name](tableRow);

                    if (fieldInfo.IsPrivate)
                    {
                        if (fieldInfo.FieldType != typeof(ExcelFile.RowHeader)) continue;
                        baseRow[col++] = FileTools.ObjectToStringGeneric(value, ",");
                        continue;
                    }

                    OutputAttribute excelOutputAttribute = outputAttributes[col];

                    if (excelOutputAttribute == null)
                    {
                        if (value.GetType().BaseType == typeof(Array))
                        {
                            value = ((Array)value).ToString(",");
                        }

                        baseRow[col++] = value;
                        continue;
                    }

                    if (excelOutputAttribute.IsStringOffset)
                    {
                        int valueInt = (int)value;
                        baseRow[col++] = (valueInt != -1) ? excelFile.ReadStringTable(valueInt) : String.Empty;
                        continue;
                    }

                    if (excelOutputAttribute.IsSecondaryString)
                    {
                        int valueInt = (int)value;
                        baseRow[col++] = (valueInt != -1) ? excelFile.ReadSecondaryStringTable(valueInt) : String.Empty;
                        continue;
                    }

                    if (excelOutputAttribute.IsScript)
                    {
                        int valueInt = (int)value;
                        if (valueInt == 0)
                        {
                            baseRow[col++] = String.Empty;
                            continue;
                        }

                        ExcelScript excelScript = new ExcelScript(this);
                        String script;

                        try
                        {
                            if (ExcelScript.DebugEnabled)
                            {
                                int[] scriptCode = excelFile.ReadScriptTable(valueInt);
                                script = scriptCode != null ? FileTools.ArrayToStringGeneric(scriptCode, ",") : String.Empty;

                                script = excelScript.Decompile(excelFile.ScriptBuffer, valueInt, script, excelFile.StringId, row, col, fieldInfo.Name);
                            }
                            else
                            {
                                script = excelScript.Decompile(excelFile.ScriptBuffer, valueInt);
                            }
                        }
                        catch (Exception)
                        {
                            int[] scriptCode = excelFile.ReadScriptTable(valueInt);
                            script = scriptCode != null ? FileTools.ArrayToStringGeneric(scriptCode, ",") : "ScriptError";
                        }

                        baseRow[col++] = script;
                        continue;
                    }

                    if (excelOutputAttribute.IsTableIndex || excelOutputAttribute.IsStringIndex)
                    {
                        if (value.GetType().BaseType == typeof(Array))
                        {
                            value = ((Array)value).ToString(",");
                        }
                        else
                        {
                            value = (int)value;
                        }

                        baseRow[col++] = value;
                        col++; // for _strings relational column
                        continue;
                    }

                    // Else its something else, ie bitmask, bool, table/string index
                    baseRow[col++] = value;
                }

                // stats, only a component of the UnitData row type
                if (excelFile.Attributes.HasStats)
                {
                    baseRow[col++] = FileTools.ArrayToStringGeneric(excelFile.ReadStats(row - 1), ",");
                }

                dataTable.Rows.Add(baseRow);
                row++;
            }

            #endregion

            // Generate Relationships as required
            if (doRelations) GenerateRelations(excelFile);

            return dataTable;
        }

        private DataTable _LoadStringsTable()
        {
            ObjectDelegator objectDelegator;
            if (!DataFileDelegators.TryGetValue("Strings_Strings", out objectDelegator)) return null;

            DataTable dataTable = XlsDataSet.Tables[StringsTableName];
            if (dataTable != null) return dataTable;

            dataTable = XlsDataSet.Tables.Add(StringsTableName);
            dataTable.TableName = StringsTableName;

            Type dataType = typeof(StringsFile.StringBlock);
            FieldInfo[] fieldInfos = dataType.GetFields();

            // generate columns
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                dataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
            }

            // we want all strings within a single table
            foreach (DataFile dataFile in DataFiles.Values)
            {
                if (!dataFile.IsStringsFile) continue;
                StringsFile stringsFile = (StringsFile)dataFile;

                // generate rows
                foreach (StringsFile.StringBlock tableRow in stringsFile.Rows)
                {
                    DataRow dataRow = dataTable.NewRow();

                    int col = 0;
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        dataRow[col++] = objectDelegator[fieldInfo.Name](tableRow);
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private DataTable LoadRelatedTable(String stringId)
        {
            if ((String.IsNullOrEmpty(stringId))) return null;

            DataTable dataTable = XlsDataSet.Tables[stringId];
            if (dataTable != null) return dataTable;

            DataFile dataFile = GetDataFile(stringId);
            return (dataFile != null) ? LoadTable(dataFile, true) : null;
        }

        private void GenerateRelations(ExcelFile excelFile)
        {
            if ((excelFile == null)) return;

            Type type = excelFile.Attributes.RowType;
            int col;

            String mainTableName = excelFile.StringId;
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
            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {

                if (fieldInfo.IsPrivate)
                {
                    if (fieldInfo.FieldType == typeof(ExcelFile.RowHeader))
                    {
                        col++;
                    }
                    continue;
                }

                OutputAttribute excelOutputAttribute =
                    ExcelFile.GetExcelAttribute(fieldInfo);

                if ((excelOutputAttribute == null))
                {
                    col++;
                    continue;
                }

                DataColumn dcChild = mainDataTable.Columns[col];

                if ((excelOutputAttribute.IsStringIndex))
                {
                    DataTable dtStrings = XlsDataSet.Tables[StringsTableName] ?? _LoadStringsTable();

                    if (dtStrings != null)
                    {
                        DataColumn dcParent = dtStrings.Columns["ReferenceId"];

                        String relationName = String.Format("{0}_{1}_{2}", excelFile.StringId, dcChild.ColumnName, ColumnKeys.IsStringIndex);
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

                if (excelOutputAttribute.IsTableIndex)
                {
                    String tableStringId = excelOutputAttribute.TableStringId;

                    DataTable dt = XlsDataSet.Tables[tableStringId] ?? LoadRelatedTable(tableStringId);

                    if (dt != null)
                    {
                        DataColumn dcParent = dt.Columns["Index"];
                        String relatedColumn = dt.Columns[2].ColumnName;

                        if (dcChild.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsArray) && (bool)dcChild.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsArray])
                        {
                            col++;
                            continue;
                        }

                        String relationName = excelFile.StringId + dcChild.ColumnName + ExcelFile.ColumnTypeKeys.IsTableIndex;
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
        /// Gets an excel DataTable from the DataSet.
        /// If it has not been generated, it will be generated and stored.
        /// </summary>
        /// <param name="stringId">The excel table string name (see EXCELTABLE "StringId" column).</param>
        /// <returns>The excel as a DataTable or null if not valid StringId.</returns>
        public DataTable GetDataTable(String stringId)
        {
            if ((String.IsNullOrEmpty(stringId))) return null;
            return XlsDataSet.Tables[stringId] ?? LoadRelatedTable(stringId);
        }
    }
}