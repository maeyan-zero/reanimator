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
    public partial class ExcelTableForm : Form, IMdiChildBase
    {
        readonly FileManager _fileManager;
        private TablesLoaded _tablesLoaded;

        public ExcelTableForm(FileManager fileManager)
        {
            _fileManager = fileManager;
            InitializeComponent();
            _CreateTablesList();
        }

        private void _CreateTablesList()
        {
            _tablesLoaded = new TablesLoaded(_fileManager, this) { Dock = DockStyle.Fill };
            splitContainer2.Panel1.Controls.Add(_tablesLoaded);
        }

        public void ToggleTablesLoadedView()
        {
             splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
        }

        /// <summary>
        /// Checks if the excel table is already open.
        /// </summary>
        /// <param name="id">string id associated with the datatable</param>
        /// <returns>true if control is open</returns>
        public bool IsTabOpen(string id)
        {
            return (from TabPage tab in tabControl.TabPages
                    where tab.Text == id
                    select tab).Any();
        }

        /// <summary>
        /// Focuses the given tab from the selection.
        /// </summary>
        /// <param name="id">string id associate with the datatable</param>
        public void FocusTabPage(string id)
        {
            tabControl.SelectTab(id);
        }

        /// <summary>
        /// Creates a new data table inside a tabbed control.
        /// </summary>
        /// <param name="id">string id associated with the datafile</param>
        public void CreateTab(string id)
        {
            DataFile dataFile = _fileManager.GetDataFile(id);
            DatafileEditor editor = new DatafileEditor(dataFile, _fileManager) { Dock = DockStyle.Fill };
            TabPage tabPage = new TabPage(id) { Parent = tabControl, Name = id };

            ProgressForm progress = new ProgressForm(editor.InitThreadedComponents, null);
            progress.SetStyle(ProgressBarStyle.Marquee);
            progress.SetLoadingText("Generating DataTable...");
            progress.SetCurrentItemText(String.Empty);
            progress.ShowDialog(this);

            tabControl.SuspendLayout();
            tabPage.Controls.Add(editor);
            tabControl.ResumeLayout();
        }

        public void SaveButton()
        {
            ((DatafileEditor)tabControl.SelectedTab.Controls[0]).SaveButton();
        }

        public void Import()
        {
            ((DatafileEditor)tabControl.SelectedTab.Controls[0]).Import();
        }

        public void Export()
        {
            ((DatafileEditor)tabControl.SelectedTab.Controls[0]).Export();
        }

        public void CloseTab()
        {
            tabControl.TabPages.Remove(tabControl.SelectedTab);
        }

        private void closeTabButton_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null) return;
            tabControl.TabPages.Remove(tabControl.SelectedTab);
        }
    }


}