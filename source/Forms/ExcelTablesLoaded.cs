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
        ExcelDataSet excelDataSet;

        public ExcelTablesLoaded(ExcelDataSet xlsDataSet)
        {
            InitializeComponent();
            excelDataSet = xlsDataSet;
        }

        public ListBox GetTablesListBox()
        {
            return listBox1;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExcelTable excelTable = listBox1.SelectedValue as ExcelTable;

            ExcelTableForm etf = new ExcelTableForm(excelTable, excelDataSet);
            etf.Text = "Excel Table: " + excelTable;
            etf.MdiParent = this.MdiParent;
            etf.Show();
        }

        public void LoadingComplete()
        {
            label2.Text = listBox1.Items.Count.ToString();
        }
    }
}
