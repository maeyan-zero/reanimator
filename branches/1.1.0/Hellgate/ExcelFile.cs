using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using Revival;
using Hellgate.Excel;

namespace Hellgate
{
    public partial class ExcelFile
    {
        public String StringID { get; private set; }
        public UInt32 StructureID { get; private set; }
        public Type StructureType { get; private set; }
        public Boolean IntegrityCheck { get; private set; }   

        private Hashtable Strings { get; set; }
        private Object[] Rows { get; set; }
        private Byte[][] AI { get; set; }
        private String[] SecondaryStrings { get; set; }
        private Byte[] Mysh { get; set; }
        private Hashtable IntPtrData { get; set; }
        private Byte[][] Signature { get; set; }

        /// <summary>
        /// Constructs an ExcelFile object from the Byte[] parameter.
        /// </summary>
        /// <param name="buffer">Byte[] of a ExcelFile type.</param>
        public ExcelFile(Byte[] buffer)
        {
            IntegrityCheck = Read(buffer);
        }

        /// <summary>
        /// Parses a Byte[] array as a ExcelFile type.
        /// </summary>
        /// <param name="buffer">Byte[] of a ExcelFile.</param>
        /// <returns>Returns true if no errors were encounted.</returns>
        private Boolean Read(Byte[] buffer)
        {
            Int32 offset = 0;

            //
            // File Header
            //
            if (!(CheckToken(Token.Cxeh, buffer, ref offset))) return false;

            ExcelHeader Header = Tools.ByteArrayToStructure<ExcelHeader>(buffer, ref offset);
            
            if (!(DataTypes.ContainsKey(Header.StructureID))) return false;
            
            StructureID = Header.StructureID;
            StructureType = (Type)DataTypes[StructureID];
            // Lazy stringID search. Should use a lamba expression here
            foreach (KeyValuePair<String, UInt32> dataTable in DataTables)
            {
                if (dataTable.Value == StructureID)
                {
                    StringID = dataTable.Key;
                    break;
                }
            }


            //
            // Strings
            //
            if (!(CheckToken(Token.Cxeh, buffer, ref offset))) return false;

            Int32 stringsBytesCount = Tools.ByteArrayToInt32(buffer, ref offset);
            Byte[] stringBytes = new Byte[stringsBytesCount];
            if (stringsBytesCount > 0)
            {
                stringBytes = Tools.ByteArrayToArray<Byte>(buffer, ref offset, stringsBytesCount);
            }



            //
            // Table Rows
            //
            if (!(CheckToken(Token.Cxeh, buffer, ref offset))) return false;
            Int32 rowCount = Tools.ByteArrayToInt32(buffer, ref offset);
            Rows = new Object[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                Rows[i] = Tools.ByteArrayToStructure(buffer, StructureType, ref offset);
            }


            //
            // Indice block
            //
            if (StructureID != 0x887988C4) // item types
            {
                if (!(CheckToken(Token.Cxeh, buffer, ref offset))) return false;
                Tools.ByteArrayToInt32Array(buffer, ref offset, rowCount);
            }
            else
            {

            }


            //
            // Secondary strings
            //
            if (!(CheckToken(Token.Cxeh, buffer, ref offset)))
            {
                offset -= sizeof(UInt32);
                Int32 stringCount = Tools.ByteArrayToInt32(buffer, ref offset);
                SecondaryStrings = new String[stringCount];
                for (int i = 0; i < stringCount; i++)
                {
                    Int32 charCount = Tools.ByteArrayToInt32(buffer, ref offset);
                    String str = Tools.ByteArrayToStringASCII(buffer, offset);
                    offset += charCount;
                    SecondaryStrings[i] = str;
                }
            }
            else
            {
                offset -= 4;
            }


            //
            // Sort Indices
            //
            Int32[][] Sort = new Int32[4][];
            for (int i = 0; i < Sort.Length; i++)
            {
                if (!(CheckToken(Token.Cxeh, buffer, ref offset))) return false;

                int count = Tools.ByteArrayToInt32(buffer, ref offset);
                Sort[i] = Tools.ByteArrayToInt32Array(buffer, ref offset, count);
            }


            //
            // Rcsh, Tysh, Mysh, Dneh
            //
            if (!(CheckToken(Token.Cxeh, buffer, ref offset))) return false;

            if (CheckToken(Token.Rcsh, buffer, ref offset))
            {
                if (!(CheckToken(0x04, buffer, ref offset))) return false;
                if (!(CheckToken(Token.Tysh, buffer, ref offset))) return false;
                if (!(CheckToken(0x02, buffer, ref offset))) return false;
                if (CheckToken(Token.Mysh, buffer, ref offset))
                {
                    //ParseMyshTables(buffer, ref offset);
                }
                else
                {
                    offset -= sizeof(UInt32);
                }
                if (!(CheckToken(Token.Dneh, buffer, ref offset))) return false;
                if (!(CheckToken(0x00, buffer, ref offset))) return false;
            }


            //
            // IntPtrData
            //
            if (StructureID != 0x1F9DDC98) //unittypes
            {
                Byte[] intPtrBytes;
                if (CheckToken(Token.Cxeh, buffer, ref offset))
                {
                    int byteCount = Tools.ByteArrayToInt32(buffer, ref offset);
                    if (byteCount != 0)
                    {
                        intPtrBytes = Tools.ByteArrayToArray<Byte>(buffer, ref offset, byteCount);
                        // todo: parse hashtable
                    }
                }
            }

            //
            // Signature
            //
            if (CheckToken(Token.Cxeh, buffer, ref offset))
            {
                int byteCount = Tools.ByteArrayToInt32(buffer, ref offset);
                int blockCount = Tools.ByteArrayToInt32(buffer, ref offset);
                if (byteCount != 0) // states & unittypes only
                {
                    byteCount = byteCount << 2;
                    Signature = new Byte[rowCount][];
                    for (int i = 0; i < blockCount; i++)
                    {
                        Signature[i] = Tools.ByteArrayToArray<Byte>(buffer, ref offset, byteCount);
                    }
                }
            }

            return (offset == buffer.Length);
        }

        /// <summary>
        /// Serializes the ExcelFile to a Byte[].
        /// </summary>
        /// <param name="dataTable">A DataTable that contains the relational data set.</param>
        /// <returns>The ExcelFile as a Byte[].</returns>
        public Byte[] Create(DataTable dataTable)
        {
            return null;
        }

        /// <summary>
        /// Generates a DataTable of the ExcelFile based on the StructureType and row data.
        /// </summary>
        /// <returns>A DataTable of the ExcelFile.</returns>
        public DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable(StringID);

            //
            // Generate the DataTable columns.
            //
            #region Generate Columns
            DataColumn indexColumn = dataTable.Columns.Add("Index");
            indexColumn.AutoIncrement = true;
            indexColumn.Unique = true;
            dataTable.PrimaryKey = new[] { indexColumn };

            foreach (FieldInfo fieldInfo in StructureType.GetFields())
            {
                String columnName = Char.ToUpper(fieldInfo.Name[0]) + fieldInfo.Name.Substring(1); //format the column name
                DataColumn dataColumn = dataTable.Columns.Add(columnName, fieldInfo.FieldType);

                #region Excel Attributes
                ExcelAttribute excelAttribute = GetExcelAttribute(fieldInfo);
                if (excelAttribute != null)
                {
                    if (excelAttribute.IsValueList)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.IsValueList, true);
                    }
                    if (excelAttribute.IsBool)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.IsBool, true);
                    }
                    if (excelAttribute.IsBitmask)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.IsBitmask, true);
                    }
                    if (excelAttribute.IsIntOffset)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.IsIntOffset, true);
                    }
                    if (excelAttribute.IsStringIndex)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.IsStringIndex, true);
                    }
                    if (excelAttribute.IsTableIndex)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.IsTableIndex, true);
                        dataColumn.ExtendedProperties.Add(ColumnKeys.TableStringID, excelAttribute.TableStringID);
                    }
                    if (excelAttribute.SortID != 0)
                    {
                        dataColumn.ExtendedProperties.Add(ColumnKeys.SortID, excelAttribute.SortID);
                        if (excelAttribute.SortDefault)
                        {
                            dataColumn.ExtendedProperties.Add(ColumnKeys.SortDefault, true);
                        }
                        if (!String.IsNullOrEmpty(excelAttribute.SortType))
                        {
                            dataColumn.ExtendedProperties.Add(ColumnKeys.SortType, excelAttribute.SortType);
                        }
                    }
                #endregion
                }
            }
            #endregion

            //
            // Generate the DataTable rows.
            //
            #region Generate Rows
            foreach (Object obj in Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                Int32 col = 1;

                foreach (FieldInfo fieldInfo in StructureType.GetFields())
                {
                    Object value = fieldInfo.GetValue(obj);
                    dataRow[col++] = value;

                    //ExcelAttribute excelAttribute = GetExcelAttribute(fieldInfo);
                    //if (excelAttribute != null)
                    //{
                    //    if (excelAttribute.IsStringIndex)
                    //    {
                    //        col++;
                    //    }
                    //    if (excelAttribute.IsTableIndex)
                    //    {
                    //        col++;
                    //    }
                    //}
                }

                dataTable.Rows.Add(dataRow);
            }
            #endregion

            return dataTable;
        }

    }
}
