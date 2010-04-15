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
    public partial class TablesLoaded : Form
    {
        TableDataSet tableDataSet;

        public TablesLoaded(TableDataSet xlsDataSet)
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

        private void TablesLoaded_LocationChanged(object sender, EventArgs e)
        {
            Size parentSize = this.MdiParent.ClientSize;
            Point location = this.Location;
            int distance = 10;

            if (location.X <= distance)
            {
                this.Location = new Point(0, this.Location.Y);
            }
            else if (parentSize.Width - this.Width - location.X - 4 <= distance)
            {
                this.Location = new Point(parentSize.Width - this.Width - 4, this.Location.Y);
            }
            if (location.Y <= distance)
            {
                this.Location = new Point(this.Location.X, 0);
            }
            else if (parentSize.Height - this.Height - location.Y - 76 <= distance)
            {
                this.Location = new Point(this.Location.X, parentSize.Height - this.Height - 76);
            }
        }
    }
}
