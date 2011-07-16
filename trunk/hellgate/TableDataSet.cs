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
            if (dataTable != null && !force) return dataTable;
            if (dataTable != null)
            {
                XlsDataSet.Relations.Clear();
                XlsDataSet.Tables.Remove(tableName);
            }
            dataTable = XlsDataSet.Tables.Add(tableName);
            dataTable.TableName = tableName;

            Type dataType = excelFile.Attributes.RowType;
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
                        int scriptOffset = (int)value;
                        if (scriptOffset == 0)
                        {
                            baseRow[col++] = String.Empty;
                            continue;
                        }

                        String script;
                        if (scriptOffset == 9649 && excelFile.StringId == "SKILLS") // todo: not sure what's with this script...
                        {
                            /* Compiled Bytes:
                             * 26,30,700,6,26,1,399,358,669,616562688,711,26,62,3,17,669,641728512,26,8,711,26,62,3,17,358,669,322961408,26,5,700,6,26,1,399,358,388,0
                             * 
                             * Ending Stack (FIFO):
                             * SetStat669('sfx_attack_pct', 'all', 30 * ($sklvl - 1))
                             * SetStat669('sfx_duration_pct', 'all', get_skill_level(@unit, 'Shield_Mastery'))
                             * SetStat669('damage_percent_skill', 8 * get_skill_level(@unit, 'Shield_Mastery'))
                             * SetStat669('damage_percent_skill', 8 * get_skill_level(@unit, 'Shield_Mastery')) + 5 * ($sklvl - 1)
                             * 
                             * The last SetStat has strange overhang - decompiling wrong?
                             * Or is it "supposed" to be there?
                             * i.e. It's actually decompiling correctly, but because I've assumed such scripts to be wrong (as the end +5... segment is useless) we get the Stack exception
                             */
                            int[] scriptCode = excelFile.ReadScriptTable(scriptOffset);
                            script = scriptCode != null ? FileTools.ArrayToStringGeneric(scriptCode, ",") : "ScriptError";
                            baseRow[col++] = script;
                            continue;
                        }

                        //if (fieldInfo.Name == "props1" && row == 45)
                        //{
                        //    int bp = 0;
                        //}

                        ExcelScript excelScript = new ExcelScript(this);

                        try
                        {
                            if (ExcelScript.DebugEnabled)
                            {
                                int[] scriptCode = excelFile.ReadScriptTable(scriptOffset);
                                script = scriptCode != null ? FileTools.ArrayToStringGeneric(scriptCode, ",") : String.Empty;

                                script = excelScript.Decompile(excelFile.ScriptBuffer, scriptOffset, script, excelFile.StringId, row, col, fieldInfo.Name);
                            }
                            else
                            {
                                script = excelScript.Decompile(excelFile.ScriptBuffer, scriptOffset);
                            }

                            //if (script.StartsWith("SetStat669('unlimited_in_merchant_inventory', 1);"))
                            //{
                            //    int bp = 0;
                            //}
                        }
                        catch (Exception)
                        {
                            int[] scriptCode = excelFile.ReadScriptTable(scriptOffset);
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
            if (doRelations) _GenerateRelations(excelFile);

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

        private DataTable _LoadRelatedTable(String stringId)
        {
            if ((String.IsNullOrEmpty(stringId))) return null;

            DataTable dataTable = XlsDataSet.Tables[stringId];
            if (dataTable != null) return dataTable;

            DataFile dataFile = GetDataFile(stringId);
            return (dataFile != null) ? LoadTable(dataFile, true) : null;
        }

        private void _GenerateRelations(ExcelFile excelFile)
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

                OutputAttribute excelOutputAttribute = ExcelFile.GetExcelAttribute(fieldInfo);

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

                    DataTable dt = XlsDataSet.Tables[tableStringId] ?? _LoadRelatedTable(tableStringId);

                    if (dt != null)
                    {
                        DataColumn dcParent = dt.Columns["Index"];
                        String relatedColumn = dt.Columns[2].ColumnName;

                        // todo
                        if (dcChild.ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsArray) && (bool)dcChild.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsArray])
                        {
                            //if (excelFile.StringId == "ITEMS")
                            //{
                            //    int bp = 0;
                            //}
                            col++;
                            continue;
                        }

                        //String relationNameOld = excelFile.StringId + dcChild.ColumnName + ExcelFile.ColumnTypeKeys.IsTableIndex;
                        String relationName = String.Format("{0}_{1}_{2}", excelFile.StringId, dcChild.ColumnName, ExcelFile.ColumnTypeKeys.IsTableIndex);
                        DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);

                        //if (relationName == "SKILLS_uiThrobsOnState_IsTableIndex")
                        //{
                        //    int bp = 0;
                        //}

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
            return XlsDataSet.Tables[stringId] ?? _LoadRelatedTable(stringId);
        }
    }
}