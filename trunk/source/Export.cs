using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reanimator
{
    class Export
    {
        static public string CSV(DataGridView datagridview, bool[] selected)
        {
            StringWriter csv = new StringWriter();

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
                            csv.Write(",");
                        }
                    }
                }
                csv.Write(Environment.NewLine);
            }

            return csv.ToString();
        }
    }
}
