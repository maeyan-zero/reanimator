using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Forms;

namespace Reanimator.Controls
{
    public partial class TablesLoaded : UserControl
    {
        private readonly FileManager _fileManager;
        private TableEditorForm _excelTableForm;

        public TablesLoaded(FileManager fileManager, TableEditorForm excelTableForm)
        {
            InitializeComponent();

            _fileManager = fileManager;
            _excelTableForm = excelTableForm;

            if (_fileManager == null || _fileManager.DataFiles.Count == 0) return;
            loadedTables_ListBox.DataSource = new BindingSource(_fileManager.DataFiles, null);
            loadedTables_ListBox.DoubleClick += new EventHandler(_ListBox1_MouseDoubleClick);
            loadedTables_ListBox.Format += new ListControlConvertEventHandler(_LoadedTables_ListBox_Format);
        }

        private void _ListBox1_MouseDoubleClick(object sender, EventArgs e)
        {
            KeyValuePair<String, DataFile> item =
                    (KeyValuePair<String, DataFile>)loadedTables_ListBox.SelectedItem;

            if (_excelTableForm == null || _excelTableForm.IsDisposed)
            {
                _excelTableForm = new TableEditorForm(_fileManager) { };
            }

            bool isOpen = _excelTableForm.IsTabOpen(item.Key);
            if (isOpen)
            {
                _excelTableForm.FocusTabPage(item.Key);
            }
            else
            {
                _excelTableForm.CreateTab(item.Key);
                _excelTableForm.FocusTabPage(item.Key);
            }

            _excelTableForm.Show();
        }


        private void _LoadedTables_ListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            KeyValuePair<String, DataFile> item = (KeyValuePair<String, DataFile>)e.ListItem;
            e.Value = item.Key;
        }
    }
}
