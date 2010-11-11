using System;
using System.Data;

namespace Revival.Common
{
    public static class TableTools
    {
        public static DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if ((int)row[FieldNames[0]] == -1) //lazy. function cant be reused
                {
                    continue; // ignore values -1
                }

                if (!FieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    DataRow newRow = CreateRowClone(row, newTable.NewRow(), FieldNames);
                    newTable.Rows.Add(newRow);
                    SetLastValues(lastValues, row, FieldNames);
                    newRow[0] = row[0];//modified to store index, not distinct value
                }
            }

            return newTable;
        }

        public static bool FieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        public static DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private static void SetLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }
    }
}
