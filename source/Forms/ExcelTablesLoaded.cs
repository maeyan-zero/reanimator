using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;

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
    }
}
