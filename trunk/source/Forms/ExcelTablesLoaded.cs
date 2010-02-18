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

        public void BindListBoxDataSource(Object dataSource)
        {
            loadedTables_ListBox.DataSource = dataSource;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExcelTable excelTable = loadedTables_ListBox.SelectedValue as ExcelTable;

            if (excelTable != null)
            {
                ExcelTableForm etf = new ExcelTableForm(excelTable, excelDataSet);
                etf.Text = "Excel Table: " + excelTable;
                etf.MdiParent = this.MdiParent;
                etf.Show();
            }
        }
    }
}
