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
            loadedTables_ListBox.Sorted = true;
        }

        public void AddItem(Object o)
        {
            loadedTables_ListBox.Items.Add(o);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ExcelTable excelTable = loadedTables_ListBox.SelectedItem as ExcelTable;
            if (excelTable != null)
            {
                ExcelTableForm etf = new ExcelTableForm(excelTable, excelDataSet);
                etf.Text = "Excel Table: " + excelTable;
                etf.MdiParent = this.MdiParent;
                etf.Show();
                return;
            }


            // TODO
            // Make a single table form!
            StringsFile stringsFile = loadedTables_ListBox.SelectedItem as StringsFile;
            if (stringsFile != null)
            {
                TableForm indexExplorer = new TableForm(stringsFile);
                StringsFile.StringBlock[] stringBlocks = stringsFile.GetFileTable();
                indexExplorer.dataGridView.DataSource = stringBlocks;
                indexExplorer.Text += ": " + stringsFile;
                indexExplorer.MdiParent = this.MdiParent;
                indexExplorer.Show();
            }
        }
    }
}
