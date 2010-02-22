using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reanimator
{
    static class Export
    {
        static public string CSV(DataGridView datagridview, bool[] selected, string delimiter)
        {
            StringWriter csv = new StringWriter();

            /*
            // column headers
            for (int col = 0; col < datagridview.Rows[0].Cells.Count; col++)
            {
                if (selected[col] == true)
                {
                    csv.Write(datagridview.Columns[col].Name);
                    if (col < datagridview.Rows[0].Cells.Count)
                    {
                        csv.Write("\t");
                    }
               }
            }
            csv.Write(Environment.NewLine);
            */

            // rows
            for (int row = 0; row < datagridview.Rows.Count - 1; row++)
            {
                for (int col = 0; col < datagridview.Rows[row].Cells.Count; col++)
                {
                    if (selected[col] == true)
                    {
                        if (datagridview[col, row].Value != null)
                        {
                            string stringBuffer = datagridview[col, row].Value.ToString();

                            if (stringBuffer.Contains(',') || stringBuffer.Contains('"'))
                            {
                                //stringBuffer = stringBuffer.Replace("\"", "\"\"");
                                //stringBuffer = stringBuffer.Replace(",", "\",\"");
                                stringBuffer = stringBuffer.Insert(0, "\"");
                                stringBuffer = stringBuffer.Insert(stringBuffer.Length, "\"");
                                csv.Write(stringBuffer);
                            }
                            else
                            {
                                csv.Write(datagridview[col, row].Value);
                            }
                        }
                        if (col < datagridview.Rows[row].Cells.Count)
                        {
                            if (delimiter == "Commar")
                            {
                                csv.Write(",");
                            }
                            else if (delimiter == "Tab")
                            {
                                csv.Write("\t");
                            }
                        }
                    }
                }
                csv.Write(Environment.NewLine);
            }

            return csv.ToString();
        }
    }
}
