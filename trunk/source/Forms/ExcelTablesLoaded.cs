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
        TableDataSet tableDataSet;

        public ExcelTablesLoaded(TableDataSet xlsDataSet)
        {
            InitializeComponent();
            tableDataSet = xlsDataSet;
            loadedTables_ListBox.Sorted = true;
        }

        public void AddItem(Object o)
        {
            loadedTables_ListBox.Items.Add(o);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Object table = loadedTables_ListBox.SelectedItem;
            if (table != null)
            {
                ExcelTableForm etf = new ExcelTableForm(table, tableDataSet);
                etf.Text = "Table: " + table;
                etf.MdiParent = this.MdiParent;
                etf.Show();
            }
        }
    }
}
