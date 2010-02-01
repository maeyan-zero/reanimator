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
using System.Reflection;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TableIndexDataSource
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
                        this.listBox1.Items.Add(j + ": " + s + " (0x" + currentOffset.ToString("X") + " - " + currentOffset + ")");
                        s = String.Empty;
                        j++;
                        currentOffset = -1;
                    }
                }
            }
            
            // main table data
            dataGridView1.AutoGenerateColumns = false;
            object[] array = (object[])excelTable.GetTableArray();
            Type type = array[0].GetType();
            foreach (MemberInfo memberInfo in type.GetFields())
            {
                dataGridView1.Columns.Add(memberInfo.Name, memberInfo.Name);
            }
            for (int i = 0; i < array.Length; i++)
            {
                int row = dataGridView1.Rows.Add();
                int col = 0;

                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    dataGridView1[col, row].Value = fieldInfo.GetValue(array[i]);
                    col++;
                }
            }
            
            // generate the table index data source - //TODO is there a better way?
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
                    TableIndexDataSource tds;

                    if (tdsList.Count <= j)
                    {
                        tdsList.Add(new TableIndexDataSource());
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
                try
                {
                    dataGridView2.Rows[cell.RowIndex].Selected = true;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
