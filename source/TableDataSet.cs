using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Reanimator.Forms;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Globalization;

namespace Reanimator
{
    public class TableDataSet : IDisposable
    {
        private const String TableVersion = "1.1.5";
        private const String RelationsVersion = "1.0.0";
        private const String TableVersionKey = "TableVersion";
        private const String RelationsVersionKey = "RelationsVersion";

        private DataSet _xlsDataSet;

        public TableFiles TableFiles { get; set; }
        public bool RegenerateRelations { get; private set; }

        public TableDataSet()
        {
            RegenerateRelations = true;
            LoadDataSet();
            _xlsDataSet.RemotingFormat = SerializationFormat.Binary;
        }

        private void LoadDataSet()
        {
            Directory.CreateDirectory(Config.CacheFilePath.Substring(0, Config.CacheFilePath.LastIndexOf(@"\") + 1));

            if (!File.Exists(Config.CacheFilePath))
            {
                _xlsDataSet = new DataSet("xlsDataSet") { Locale = new CultureInfo("en-us", true) };
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(Config.CacheFilePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter bf = new BinaryFormatter { TypeFormat = FormatterTypeStyle.XsdString };
                    _xlsDataSet = bf.Deserialize(fs) as DataSet;
                    if (_xlsDataSet != null)
                    {
                        bool isTableUpToDate = false;
                        String loadedCurrentVersion = _xlsDataSet.ExtendedProperties[TableVersionKey] as String;
                        String loadedRelationsVersion =
                            _xlsDataSet.ExtendedProperties[RelationsVersionKey] as String;

                        if (loadedCurrentVersion != null)
                        {
                            if (loadedCurrentVersion.CompareTo(TableVersion) == 0)
                            {
                                isTableUpToDate = true;
                            }
                        }

                        if (loadedRelationsVersion != null)
                        {
                            if (loadedRelationsVersion.CompareTo(RelationsVersion) == 0)
                            {
                                RegenerateRelations = false;
                            }
                            else
                            {
                                ClearRelations();
                            }
                        }
                        else
                        {
                            ClearRelations();
                        }

                        if (isTableUpToDate) return;

                        MessageBox.Show(
                            "The loaded dataset cache is out of date and must be regnerated before it can be used again!",
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        _xlsDataSet = new DataSet("xlsDataSet") { Locale = new CultureInfo("en-us", true) };
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load table cache!\n\n" + e, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public void SaveDataSet()
        {
            using (FileStream fs = new FileStream(Config.CacheFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter { TypeFormat = FormatterTypeStyle.XsdString };
                if (_xlsDataSet.ExtendedProperties.Contains(TableVersionKey))
                {
                    _xlsDataSet.ExtendedProperties[TableVersionKey] = TableVersion;
                }
                else
                {
                    _xlsDataSet.ExtendedProperties.Add(TableVersionKey, TableVersion);
                }
                if (_xlsDataSet.ExtendedProperties.Contains(RelationsVersionKey))
                {
                    _xlsDataSet.ExtendedProperties[RelationsVersionKey] = RelationsVersion;
                }
                else
                {
                    _xlsDataSet.ExtendedProperties.Add(RelationsVersionKey, RelationsVersion);
                }
                bf.Serialize(fs, _xlsDataSet);
                fs.Close();
            }
        }

        public void ClearDataSet()
        {
            _xlsDataSet.Clear();
            _xlsDataSet.Relations.Clear();
            _xlsDataSet.Tables.Clear();
        }

        public void LoadTable(ProgressForm progress, Object var)
        {
            DataFile dataFile = var as DataFile;
            if (dataFile == null) return;

            if (dataFile.IsStringsFile)
            {
                LoadStringsTable(progress, dataFile as StringsFile);
            }
            else if (dataFile.IsExcelFile)
            {
                LoadExcelTable(progress, dataFile as ExcelFile);
            }
        }

        private void LoadExcelTable(ProgressForm progress, ExcelFile dataFile)
        {
            // load in main data table
            String mainTableName = dataFile.StringId;
            DataTable mainDataTable = _xlsDataSet.Tables[mainTableName];
            if (mainDataTable != null) return;

            if (progress != null)
            {
                progress.SetLoadingText("Cache generation of table data... " + mainTableName);
            }

            mainDataTable = _xlsDataSet.Tables.Add(dataFile.StringId);
            List<Object> array = dataFile.Rows;
            Type type = array[0].GetType();
            List<ExcelFile.ExcelOutputAttribute> outputAttributes =
                new List<ExcelFile.ExcelOutputAttribute>(type.GetFields().Length + 1);

            #region generate_columns

            DataColumn indexColumn = mainDataTable.Columns.Add("Index");
            indexColumn.AutoIncrement = true;
            indexColumn.Unique = true;
            mainDataTable.PrimaryKey = new[] { indexColumn };
            outputAttributes.Add(null);

            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                ExcelFile.ExcelOutputAttribute excelOutputAttribute = ExcelFile.GetExcelOutputAttribute(fieldInfo);

                if (excelOutputAttribute == null)
                {
                    outputAttributes.Add(null);
                    mainDataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
                    continue;
                }

                outputAttributes.Add(excelOutputAttribute);


                if (excelOutputAttribute.IsStringOffset)
                {
                    DataColumn dataColumn = mainDataTable.Columns.Add(fieldInfo.Name, typeof(String));
                    dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsStringOffset, true);
                    dataColumn.DefaultValue = String.Empty;
                    if (excelOutputAttribute.SortId > 0)
                    {
                        dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.SortId, excelOutputAttribute.SortId);
                    }
                }
                else if (excelOutputAttribute.IsIntOffset)
                {
                    DataColumn dataColumn = mainDataTable.Columns.Add(fieldInfo.Name, typeof(String));
                    dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsIntOffset, true);
                    dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IntOffsetOrder, excelOutputAttribute.IntOffsetOrder);
                    dataColumn.DefaultValue = String.Empty;
                }
                else
                {
                    DataColumn dataColumn = mainDataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);

                    if (excelOutputAttribute.SortId > 0)
                    {
                        dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.SortId, excelOutputAttribute.SortId);
                    }

                    if (excelOutputAttribute.IsStringIndex)
                    {
                        dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsStringIndex, true);
                        outputAttributes.Add(null);
                        DataColumn dataColumnString = mainDataTable.Columns.Add(fieldInfo.Name + "_string",
                                                                                typeof(String));
                        dataColumnString.DefaultValue = String.Empty;
                    }
                    else if (excelOutputAttribute.IsStringId)
                    {
                        dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsStringId, true);
                    }
                    else if (excelOutputAttribute.IsBitmask)
                    {
                        dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsBitmask, true);
                    }
                    else if (excelOutputAttribute.IsBool)
                    {
                        dataColumn.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsBool, true);
                    }
                }
            }

            #endregion

            const int updateRate = 50;
            if (progress != null)
            {
                progress.ConfigBar(0, array.Count / updateRate, 1);
            }

            #region generate_rows

            int row = 1;
            object[] baseRow = new object[outputAttributes.Count];

            foreach (Object table in array)
            {
                if (progress != null && row % updateRate == 0)
                {
                    progress.SetCurrentItemText("Row " + row + " of " + array.Count);
                }

                int col = 1;

                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    Object value = fieldInfo.GetValue(table);
                    ExcelFile.ExcelOutputAttribute excelOutputAttribute = outputAttributes[col];

                    if (excelOutputAttribute == null)
                    {
                        baseRow[col++] = value;
                        continue;
                    }

                    if (excelOutputAttribute.IsStringOffset)
                    {
                        int valueInt = (int)value;

                        baseRow[col++] = dataFile.Strings[valueInt];
                    }
                    else if (excelOutputAttribute.IsStringIndex)
                    {
                        int valueInt = (int)value;
                        String stringValue = valueInt == -1 ? String.Empty : dataFile.SecondaryStrings[valueInt];

                        baseRow[col++] = value;
                        baseRow[col++] = stringValue;
                    }
                    else if (excelOutputAttribute.IsIntOffset)
                    {
                        int valueInt = (int)value;

                        if (valueInt > 0)
                        {
                            baseRow[col++] = dataFile.ParseIntOffset(valueInt);
                        }
                        else
                        {
                            baseRow[col++] = 0;
                        }
                    }
                    else
                    {
                        baseRow[col++] = value;
                    }
                }

                mainDataTable.Rows.Add(baseRow);
                row++;
            }

            #endregion
        }

        private void LoadStringsTable(ProgressForm progress, StringsFile stringsFile)
        {
            // if already done, can't do again
            if (_xlsDataSet.Tables.Contains(stringsFile.StringId)) return;


            // update progress
            if (progress != null)
            {
                progress.SetCurrentItemText(stringsFile.StringId);
            }


            // generate datatable
            DataTable dt = _xlsDataSet.Tables.Add(stringsFile.StringId);
            foreach (StringsFile.StringBlock stringsBlock in stringsFile.StringsTable)
            {
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("ReferenceId", stringsBlock.ReferenceId.GetType());
                    dt.Columns.Add("Unknown1", stringsBlock.Unknown.GetType());
                    dt.Columns.Add("StringId", stringsBlock.StringId.GetType());
                    dt.Columns.Add("Unknown2", stringsBlock.Reserved.GetType());
                    dt.Columns.Add("String", stringsBlock.String.GetType());
                    dt.Columns.Add("Attribute1", stringsBlock.Attribute1.GetType());
                    dt.Columns.Add("Attribute2", stringsBlock.Attribute2.GetType());
                    dt.Columns.Add("Attribute3", stringsBlock.Attribute3.GetType());
                    dt.Columns.Add("Attribute4", stringsBlock.Attribute3.GetType());
                }

                dt.Rows.Add(stringsBlock.ReferenceId, stringsBlock.Unknown, stringsBlock.StringId, stringsBlock.Reserved,
                    stringsBlock.String, stringsBlock.Attribute1, stringsBlock.Attribute2, stringsBlock.Attribute3, stringsBlock.Attribute4);
            }
        }

        public void ClearRelations()
        {
            _xlsDataSet.Relations.Clear();
        }

        public void GenerateRelations(DataFile dataFile)
        {
            // todo: temp fix
            ExcelFile excelTable = dataFile as ExcelFile;
            if (excelTable == null) return;

            List<Object> array = dataFile.Rows;
            Type type = array[0].GetType();
            int col;

            String mainTableName = excelTable.StringId;
            DataTable mainDataTable = _xlsDataSet.Tables[mainTableName];

            // remove all extra generated columns on this table
            for (col = 0; col < mainDataTable.Columns.Count; col++)
            {
                DataColumn dc = mainDataTable.Columns[col];

                if (!dc.ExtendedProperties.Contains(ExcelFile.ColumnTypeKeys.IsRelationGenerated)) continue;

                if ((bool)dc.ExtendedProperties[ExcelFile.ColumnTypeKeys.IsRelationGenerated])
                {
                    mainDataTable.Columns.Remove(dc);
                }
            }

            col = 1;
            // regenerate relations
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                ExcelFile.ExcelOutputAttribute excelOutputAttribute = null;

                foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(ExcelFile.ExcelOutputAttribute), true))
                {
                    excelOutputAttribute = attribute as ExcelFile.ExcelOutputAttribute;
                    if (excelOutputAttribute != null)
                    {
                        break;
                    }
                }

                if (excelOutputAttribute != null)
                {
                    DataColumn dcChild = mainDataTable.Columns[col];

                    if (excelOutputAttribute.IsStringId)
                    {
                        DataTable dtStrings = _xlsDataSet.Tables[excelOutputAttribute.Table];
                        if (dtStrings != null)
                        {
                            DataColumn dcParent = dtStrings.Columns["ReferenceId"];

                            String relationName = String.Format("{0}_{1}_{2}", excelTable.StringId, dcChild.ColumnName, ExcelFile.ColumnTypeKeys.IsStringId);
                            // parent and child are swapped here for StringId relations
                            DataRelation relation = new DataRelation(relationName, dcChild, dcParent, false);
                            _xlsDataSet.Relations.Add(relation);

                            DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String));
                            dcString.SetOrdinal(col + 1);
                            dcString.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsRelationGenerated, true);
                            // need to use MIN (and thus Child) for cases of strings with same reference id (e.g. Items; SINGULAR and PLURAL, etc)
                            dcString.Expression = "MIN(Child(" + relationName + ").String)";
                            col++;
                        }
                    }
                    else if (excelOutputAttribute.TableId != "")
                    {
                        DataTable dt = GetExcelTable(excelOutputAttribute.TableId);
                        if (dt == null && excelOutputAttribute.Table != null)
                        {
                            dt = _xlsDataSet.Tables[excelOutputAttribute.Table];
                        }

                        if (dt != null)
                        {
                            DataColumn dcParent = dt.Columns["Index"];

                            String relationName = excelTable.StringId + dcChild.ColumnName + ExcelFile.ColumnTypeKeys.IsTableIndex;
                            DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                            _xlsDataSet.Relations.Add(relation);

                            DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String), String.Format("Parent({0}).{1}", relationName, excelOutputAttribute.Column));
                            dcString.SetOrdinal(col + 1);
                            dcString.ExtendedProperties.Add(ExcelFile.ColumnTypeKeys.IsRelationGenerated, true);
                            col++;
                        }
                    }
                }

                col++;
            }
        }

        public DataTable GetExcelTable(int code)
        {
            DataTable excelTables = _xlsDataSet.Tables["EXCELTABLES"];
            if (excelTables == null) return null;

            DataRow[] rows = excelTables.Select(String.Format("code = '{0}'", code));
            return rows.Length == 0 ? null : _xlsDataSet.Tables[rows[0][1].ToString()];
        }

        public DataTable GetExcelTable(string code)
        {
            DataTable excelTables = _xlsDataSet.Tables["EXCELTABLES"];
            if (excelTables == null) return null;

            DataRow[] rows = excelTables.Select(code);
            return rows.Length == 0 ? null : _xlsDataSet.Tables[rows[0][1].ToString()];
        }

        public DataSet XlsDataSet
        {
            get { return _xlsDataSet; }
        }

        public int LoadedTableCount
        {
            get { return _xlsDataSet.Tables.Count; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_xlsDataSet != null)
            {
                _xlsDataSet.Dispose();
            }
        }

        #endregion
    }
}
