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
        static public string CSV(DataGridView datagridview)
        {
            string strValue = string.Empty;

            for (int i = 0; i < datagridview.Rows.Count - 1; i++)
            {
                for (int j = 0; j < datagridview.Rows[i].Cells.Count; j++)
                {

                    if (!string.IsNullOrEmpty(datagridview[j, i].Value.ToString()))
                    {
                        if (j > 0)
                        {
                            strValue = strValue + "," + datagridview[j, i].Value.ToString();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(strValue))
                            {
                                strValue = datagridview[j, i].Value.ToString();
                            }
                            else
                            {
                                strValue = strValue + Environment.NewLine + datagridview[j, i].Value.ToString();
                            }
                        }
                    }
                }
            }

            return strValue;
        }
    }
}
