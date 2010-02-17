using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.IO;

namespace Reanimator.Forms
{
    public partial class ExcelTablesLoaded : Form
    {
        public ExcelTablesLoaded()
        {
            InitializeComponent();
        }

        public ListBox GetTablesListBox()
        {
            return listBox1;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExcelTable excelTable = listBox1.SelectedItem as ExcelTable;

            ExcelTableForm etf = new ExcelTableForm(excelTable);
            etf.Text = "Excel Table: " + excelTable;
            etf.MdiParent = this.MdiParent;
            etf.Show();
        }

        private void b_clearCache_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(@"cache\");

            foreach (string file in files)
            {
                File.Delete(file);
            }

            MessageBox.Show("Cache cleared!", "Info");
        }

        private void b_cacheAll_Click(object sender, EventArgs e)
        {
            for(int counter = 0; counter < listBox1.Items.Count; counter++)
            {
                listBox1.SelectedIndex++;

                ExcelTable excelTable = (ExcelTable)listBox1.Items[counter];

                ExcelTableForm etf = new ExcelTableForm(excelTable);
                etf.Text = "Excel Table: " + excelTable;
                etf.MdiParent = this.MdiParent;
                etf.Hide();
                etf.Close();
            }

            MessageBox.Show(listBox1.Items.Count + " files cached!", "Info");
        }

        public void LoadingComplete()
        {
            label2.Text = listBox1.Items.Count.ToString();
        }
    }
}
