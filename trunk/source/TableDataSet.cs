using System;
using System.Collections.Generic;
using System.Data;
using Reanimator.Forms;
using System.IO;
using Reanimator.Excel;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Globalization;

namespace Reanimator
{
    public class TableDataSet : IDisposable
    {
        private const String TableVersion = "1.0.2";
        private const String RelationsVersion = "1.0.0";
        private const String TableVersionKey = "TableVersion";
        private const String RelationsVersionKey = "RelationsVersion";

        DataSet _xlsDataSet;
        readonly Hashtable _xlsDataTables;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableIndexDataSource
        {
            public int Unknowns1 { get; set; }
            public int Unknowns2 { get; set; }
            public int Unknowns3 { get; set; }
            public int Unknowns4 { get; set; }
        };

        public ExcelTables ExcelTables { get; set; }
        public StringsTables StringsTables { get; set; }
        public bool RegenerateRelations { get; private set; }

        public TableDataSet()
        {
            RegenerateRelations = true;
            LoadDataSet();
            _xlsDataSet.RemotingFormat = SerializationFormat.Binary;
            _xlsDataTables = new Hashtable();
        }

        private void LoadDataSet()
        {
            if (File.Exists(Config.CacheFilePath))
            {
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
                            String loadedRelationsVersion = _xlsDataSet.ExtendedProperties[RelationsVersionKey] as String;

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

                            MessageBox.Show("The loaded dataset cache is out of date and must be regnerated before it can be used again!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to load table cache!\n\n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Directory.CreateDirectory(Config.CacheFilePath.Substring(0, Config.CacheFilePath.LastIndexOf(@"\") + 1));
            _xlsDataSet = new DataSet("xlsDataSet") { Locale = new CultureInfo("en-us", true) };
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
            StringsFile stringsFile = var as StringsFile;
            if (stringsFile != null)
            {
                LoadStringsTable(progress, stringsFile);
            }

            ExcelTable excelTable = var as ExcelTable;
            if (excelTable != null)
            {
                LoadExcelTable(progress, excelTable);
            }

        }

        private void LoadExcelTable(ProgressForm progress, ExcelTable excelTable)
        {
            // load in main data table
            String mainTableName = excelTable.StringId;
            DataTable mainDataTable = _xlsDataSet.Tables[mainTableName];
            if (mainDataTable == null)
            {
                if (progress != null)
                {
                    progress.SetLoadingText("Cache generation of table data... " + mainTableName);
                }

                mainDataTable = _xlsDataSet.Tables.Add(excelTable.StringId);
                object[] array = (object[]) excelTable.GetTableArray();
                Type type = array[0].GetType();
                List<ExcelTable.ExcelOutputAttribute> outputAttributes =
                    new List<ExcelTable.ExcelOutputAttribute>(type.GetFields().Length + 1);

                #region generate_columns

                DataColumn indexColumn = mainDataTable.Columns.Add("Index");
                indexColumn.AutoIncrement = true;
                indexColumn.Unique = true;
                mainDataTable.PrimaryKey = new[] {indexColumn};
                outputAttributes.Add(null);

                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    ExcelTable.ExcelOutputAttribute excelOutputAttribute = null;

                    foreach (
                        Attribute attribute in
                            fieldInfo.GetCustomAttributes(typeof (ExcelTable.ExcelOutputAttribute), true))
                    {
                        excelOutputAttribute = attribute as ExcelTable.ExcelOutputAttribute;
                        if (excelOutputAttribute != null)
                        {
                            break;
                        }
                    }

                    if (excelOutputAttribute != null)
                    {
                        outputAttributes.Add(excelOutputAttribute);

                        if (excelOutputAttribute.IsStringOffset)
                        {
                            DataColumn dataColumn = mainDataTable.Columns.Add(fieldInfo.Name, typeof (String));
                            dataColumn.ExtendedProperties.Add(ExcelTable.ColumnTypeKeys.IsStringOffset, true);
                            dataColumn.DefaultValue = String.Empty;
                            continue;
                        }
                        if (excelOutputAttribute.IsStringIndex)
                        {
                            DataColumn dataColumn = mainDataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
                            dataColumn.ExtendedProperties.Add(ExcelTable.ColumnTypeKeys.IsStringIndex, true);
                            outputAttributes.Add(null);
                            DataColumn dataColumnString = mainDataTable.Columns.Add(fieldInfo.Name + "_string",
                                                                                    typeof (String));
                            dataColumnString.DefaultValue = String.Empty;
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
                    if (progress != null)
                    {
                        progress.SetCurrentItemText("Row " + row + " of " + array.Length);
                    }
                    int col = 1;

                    foreach (FieldInfo fieldInfo in type.GetFields())
                    {
                        Object value = fieldInfo.GetValue(table);
                        ExcelTable.ExcelOutputAttribute excelOutputAttribute = outputAttributes[col];

                        if (excelOutputAttribute != null)
                        {
                            if (excelOutputAttribute.IsStringOffset)
                            {
                                baseRow[col] = excelTable.Strings[value];
                                col++;
                            }
                            else if (excelOutputAttribute.IsStringIndex)
                            {
                                int valueInt = (int) value;
                                String stringValue = valueInt == -1
                                                         ? String.Empty
                                                         : excelTable.SecondaryStrings[valueInt];
                                baseRow[col] = value;
                                col++;
                                baseRow[col] = stringValue;
                                col++;
                            }
                            else
                            {
                                baseRow[col] = value;
                                col++;
                            }
                        }
                        else
                        {
                            baseRow[col] = value;
                            col++;
                        }
                    }

                    mainDataTable.Rows.Add(baseRow);
                    row++;
                }

                #endregion

                _xlsDataTables.Add(mainTableName, mainDataTable);
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
                    if (tdsList.Count <= j)
                    {
                        tdsList.Add(new TableIndexDataSource());
                    }

                    TableIndexDataSource tds = tdsList[j];
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

        private void LoadStringsTable(ProgressForm progress, StringsFile stringsFile)
        {
            progress.SetCurrentItemText(stringsFile.Name);

            if (_xlsDataSet.Tables.Contains(stringsFile.Name))
            {
                return;
            }

            DataTable dt = _xlsDataSet.Tables.Add(stringsFile.Name);
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

        public void GenerateRelations(ExcelTable excelTable)
        {
            object[] array = (object[])excelTable.GetTableArray();
            Type type = array[0].GetType();
            int col;

            String mainTableName = excelTable.StringId;
            DataTable mainDataTable = _xlsDataSet.Tables[mainTableName];

            // remove all extra generated columns on this table
            for (col = 0; col < mainDataTable.Columns.Count; col++)
            {
                DataColumn dc = mainDataTable.Columns[col];

                if (!dc.ExtendedProperties.Contains(ExcelTable.ColumnTypeKeys.IsRelationGenerated)) continue;

                if ((bool)dc.ExtendedProperties[ExcelTable.ColumnTypeKeys.IsRelationGenerated])
                {
                    mainDataTable.Columns.Remove(dc);
                }
            }

            col = 1;
            // regenerate relations
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                ExcelTable.ExcelOutputAttribute excelOutputAttribute = null;

                foreach (Attribute attribute in fieldInfo.GetCustomAttributes(typeof(ExcelTable.ExcelOutputAttribute), true))
                {
                    excelOutputAttribute = attribute as ExcelTable.ExcelOutputAttribute;
                    if (excelOutputAttribute != null)
                    {
                        break;
                    }
                }

                if (excelOutputAttribute != null)
                {
                    DataColumn dcChild = mainDataTable.Columns[col];
                    dcChild.ExtendedProperties.Clear();

                    if (excelOutputAttribute.IsStringId)
                    {
                        DataTable dtStrings = _xlsDataSet.Tables[excelOutputAttribute.Table];
                        if (dtStrings != null)
                        {
                            DataColumn dcParent = dtStrings.Columns["ReferenceId"];

                            String relationName = String.Format("{0}_{1}_{2}", excelTable.StringId, dcChild.ColumnName, ExcelTable.ColumnTypeKeys.IsStringId);
                            // parent and child are swapped here for StringId relations
                            DataRelation relation = new DataRelation(relationName, dcChild, dcParent, false);
                            _xlsDataSet.Relations.Add(relation);

                            DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String));
                            dcString.SetOrdinal(col + 1);
                            dcString.ExtendedProperties.Add(ExcelTable.ColumnTypeKeys.IsRelationGenerated, true);
                            // need to use MIN (and thus Child) for cases of strings with same reference id (e.g. Items; SINGULAR and PLURAL, etc)
                            dcString.Expression = "MIN(Child(" + relationName + ").String)";
                            dcChild.ExtendedProperties.Add(ExcelTable.ColumnTypeKeys.IsStringId, true);
                            col++;
                        }
                    }
                    else if (excelOutputAttribute.IsTableIndex)
                    {
                        DataTable dt = GetExcelTable(excelOutputAttribute.TableId);
                        if (dt == null && excelOutputAttribute.Table != null)
                        {
                            dt = _xlsDataSet.Tables[excelOutputAttribute.Table];
                        }

                        if (dt != null)
                        {
                            DataColumn dcParent = dt.Columns["Index"];

                            String relationName = excelTable.StringId + dcChild.ColumnName + ExcelTable.ColumnTypeKeys.IsTableIndex;
                            DataRelation relation = new DataRelation(relationName, dcParent, dcChild, false);
                            _xlsDataSet.Relations.Add(relation);

                            DataColumn dcString = mainDataTable.Columns.Add(dcChild.ColumnName + "_string", typeof(String), String.Format("Parent({0}).{1}", relationName, excelOutputAttribute.Column));
                            dcString.SetOrdinal(col + 1);
                            dcString.ExtendedProperties.Add(ExcelTable.ColumnTypeKeys.IsRelationGenerated, true);
                            dcChild.ExtendedProperties.Add(ExcelTable.ColumnTypeKeys.IsTableIndex, true);
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
