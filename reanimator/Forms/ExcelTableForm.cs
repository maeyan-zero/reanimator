using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Hellgate;
using Revival.Common;
using Reanimator.Controls;

namespace Reanimator.Forms
{
    public partial class ExcelTableForm : Form
    {
        readonly FileManager _fileManager;
        private TablesLoaded _tablesLoaded;

        public ExcelTableForm(FileManager fileManager)
        {
            _fileManager = fileManager;
            InitializeComponent();
            _CreateTablesList();
            //ProgressForm progress = new ProgressForm(_LoadTable, _dataFile);
            //progress.SetStyle(ProgressBarStyle.Marquee);
            //progress.SetLoadingText("Generating DataTable...");
            //progress.SetCurrentItemText(String.Empty);
            //progress.ShowDialog(this);
        }

        private void _CreateTablesList()
        {
            _tablesLoaded = new TablesLoaded(_fileManager, this) { Dock = DockStyle.Fill };
            splitContainer1.Panel1.Controls.Add(_tablesLoaded);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl.TabPages.Remove(tabControl.SelectedTab);
        }

        /// <summary>
        /// Checks if the excel table is already open.
        /// </summary>
        /// <param name="id">string id associated with the datatable</param>
        /// <returns>true if control is open</returns>
        public bool IsTabOpen(string id)
        {
            return tabControl.TabPages.ContainsKey(id);
        }

        /// <summary>
        /// Focuses the given tab from the selection.
        /// </summary>
        /// <param name="id">string id associate with the datatable</param>
        public void FocusTabPage(string id)
        {
            tabControl.SelectedTab = tabControl.TabPages[id];
        }

        /// <summary>
        /// Creates a new data table inside a tabbed control.
        /// </summary>
        /// <param name="id">string id associated with the datafile</param>
        public void CreateTab(string id)
        {
            DataFile dataFile = _fileManager.GetDataFile(id);
            DatafileEditor editor = new DatafileEditor(dataFile, _fileManager) { Dock = DockStyle.Fill };
            TabPage tabPage = new TabPage(id) { Parent = tabControl };

            tabControl.SuspendLayout();
            tabPage.Controls.Add(editor);
            tabControl.ResumeLayout();
        }
    }


}