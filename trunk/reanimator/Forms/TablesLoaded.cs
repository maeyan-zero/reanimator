using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class TablesLoaded : Form
    {
        private readonly FileManager _fileManager;

        public TablesLoaded(FileManager fileManager)
        {
            InitializeComponent();

            _fileManager = fileManager;
            loadedTables_ListBox.DataSource = new BindingSource(_fileManager.DataFiles, null);
        }

        private void _ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            KeyValuePair<String, DataFile> item = (KeyValuePair<String, DataFile>) loadedTables_ListBox.SelectedItem;

            ExcelTableForm etf = new ExcelTableForm(item.Value, _fileManager)
            {
                Text = item.Key,
                MdiParent = MdiParent
            };
            etf.Show();
        }


        private void _LoadedTables_ListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            KeyValuePair<String, DataFile> item = (KeyValuePair<String, DataFile>)e.ListItem;
            e.Value = item.Key;
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
