using System;
using System.Drawing;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class TablesLoaded : Form
    {
        private readonly TableDataSet _tableDataSet;

        public TablesLoaded(TableDataSet xlsDataSet)
        {
            InitializeComponent();
            _tableDataSet = xlsDataSet;
            loadedTables_ListBox.Sorted = true;
        }

        public void AddItem(Object o)
        {
            loadedTables_ListBox.Items.Add(o);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataFile dataFile = loadedTables_ListBox.SelectedItem as DataFile;
            if (dataFile == null) return;

            ExcelTableForm etf = new ExcelTableForm(dataFile, _tableDataSet)
            {
                Text = "Table: " + dataFile,
                MdiParent = MdiParent
            };
            etf.Show();
        }

        private void TablesLoaded_LocationChanged(object sender, EventArgs e)
        {
            Size parentSize = MdiParent.ClientSize;
            Point location = Location;
            const int distance = 10;

            if (location.X <= distance)
            {
                Location = new Point(0, Location.Y);
            }
            else if (parentSize.Width - Width - location.X - 4 <= distance)
            {
                Location = new Point(parentSize.Width - Width - 4, Location.Y);
            }
            if (location.Y <= distance)
            {
                Location = new Point(Location.X, 0);
            }
            else if (parentSize.Height - Height - location.Y - 76 <= distance)
            {
                Location = new Point(Location.X, parentSize.Height - Height - 76);
            }
        }
    }
}
