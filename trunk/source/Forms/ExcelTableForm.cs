using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.Runtime.InteropServices;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableDataSource
        {
            public int Index { get; set; }
            public int Unknowns1 { get; set; }
            public int Unknowns2 { get; set; }
            public int Unknowns3 { get; set; }
            public int Unknowns4 { get; set; }
        };

        public ExcelTableForm(ExcelTable excelTable)
        {
            InitializeComponent();

            // file id
            this.textBox1.Text = "0x" + excelTable.FileId.ToString("X");

            // do strings - inefficient, but works.
            if (excelTable.Strings != null)
            {
                String s = String.Empty;
                int initialOffset = 0x24; // in excel file
                for (int i = 0, j = 0, currentOffset = -1; i < excelTable.Strings.Length; i++)
                {
                    byte b = excelTable.Strings[i];

                    if (b != 0)
                    {
                        s += (char)b;

                        if (currentOffset == -1)
                        {
                            currentOffset = i;
                        }
                    }
                    else
                    {
                        currentOffset += initialOffset;
                        this.listBox1.Items.Add(j + ": " + s + " (0x" + currentOffset.ToString("X") + " - " + currentOffset + ")");
                        s = String.Empty;
                        j++;
                        currentOffset = -1;
                    }
                }
            }

            // main table data
            dataGridView1.DataSource = excelTable.GetTableArray();

            // generate the table index data source - //TODO is there a better way?
            List<TableDataSource> tdsList = new List<TableDataSource>();
            int[][] intArrays = { excelTable.TableIndicies, excelTable.Unknowns1, excelTable.Unknowns2, excelTable.Unknowns3, excelTable.Unknowns4 };
            for (int i = 0; i < intArrays.Length; i++)
            {
                if (intArrays[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < intArrays[i].Length; j++)
                {
                    TableDataSource tds;

                    if (tdsList.Count <= j)
                    {
                        tdsList.Add(new TableDataSource());
                    }

                    tds = tdsList[j];
                    switch (i)
                    {
                        case 0:
                            tds.Index = intArrays[i][j];
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
                    }
                }
            }

            dataGridView2.DataSource = tdsList.ToArray();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection selectedCells = dataGridView1.SelectedCells;
            dataGridView2.ClearSelection();
            foreach (DataGridViewCell cell in selectedCells)
            {
                dataGridView2.Rows[cell.RowIndex].Selected = true;
            }
        }
    }
}
