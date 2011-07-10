using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Controls;

namespace Reanimator.Forms
{
    public partial class TableEditorForm : Form, IMdiChildBase
    {
        private readonly FileManager _fileManager;
        private TablesLoaded _tablesLoaded;

        /// <summary>
        /// The form constructor.
        /// </summary>
        /// <param name="fileManager">Requires initialized Hellgate.FileManager</param>
        public TableEditorForm(FileManager fileManager)
        {
            _fileManager = fileManager;
            _InitializeComponent();
            _CreateTablesList();
        }

        /// <summary>
        /// Creates the list of tables and opens the table in a new tab when double clicked.
        /// </summary>
        private void _CreateTablesList()
        {
            _tablesLoaded = new TablesLoaded(_fileManager, this) { Dock = DockStyle.Fill };
            _splitContainer2.Panel1.Controls.Add(_tablesLoaded);
        }

        /// <summary>
        /// Checks if the excel table is already open.
        /// </summary>
        /// <param name="id">string id associated with the datatable</param>
        /// <returns>true if control is open</returns>
        public bool IsTabOpen(string id)
        {
            return (from TabPage tab in _tabControl.TabPages
                    where tab.Name == id
                    select tab).Any();
        }

        /// <summary>
        /// Focuses the given tab from the selection.
        /// </summary>
        /// <param name="id">string id associate with the datatable</param>
        public void FocusTabPage(string id)
        {
            _tabControl.SelectTab(id);
        }

        /// <summary>
        /// Creates a new data table inside a tabbed control.
        /// </summary>
        /// <param name="id">string id associated with the datafile</param>
        public void CreateTab(string id)
        {
            DataFile dataFile = _fileManager.GetDataFile(id);
            DatafileEditor editor = new DatafileEditor(dataFile, _fileManager) { Dock = DockStyle.Fill };
            TabPage tabPage = new TabPage(id) { Parent = _tabControl, Name = id };

            ProgressForm progress = new ProgressForm(editor.InitThreadedComponents, null);
            progress.SetStyle(ProgressBarStyle.Marquee);
            progress.SetLoadingText("Generating DataTable...");
            progress.SetCurrentItemText(String.Empty);
            progress.ShowDialog(this);

            _tabControl.SuspendLayout();
            tabPage.Controls.Add(editor);
            editor.AddedToTabPage = true;
            _tabControl.ResumeLayout();
        }

        /// <summary>
        /// Prompts the user for a save location.
        /// </summary>
        public void Save()
        {
            if (_tabControl.SelectedTab == null) return;
            ((DatafileEditor)_tabControl.SelectedTab.Controls[0]).Save();
        }

        /// <summary>
        /// Saves the current document in serialized format.
        /// </summary>
        public void SaveAs()
        {
            if (_tabControl.SelectedTab == null) return;
            ((DatafileEditor)_tabControl.SelectedTab.Controls[0]).SaveAs();
        }

        /// <summary>
        /// Imports a text document into this table.
        /// </summary>
        public void Import()
        {
            if (_tabControl.SelectedTab == null) return;
            ((DatafileEditor)_tabControl.SelectedTab.Controls[0]).Import();
        }

        /// <summary>
        /// Exports the table as a text document.
        /// </summary>
        public void Export()
        {
            if (_tabControl.SelectedTab == null) return;
            ((DatafileEditor)_tabControl.SelectedTab.Controls[0]).Export();
        }

        /// <summary>
        /// Closes the current tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _closeTabButton_Click(object sender, EventArgs e)
        {
            if (_tabControl.SelectedTab == null) return;
            _tabControl.TabPages.Remove(_tabControl.SelectedTab);
        }

        /// <summary>
        /// Hides or Shows the list of tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _tableViewToggleButton_Click(object sender, EventArgs e)
        {
            _splitContainer1.Panel1Collapsed = !_splitContainer1.Panel1Collapsed;
        }

        /// <summary>
        /// Reloads the current tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _reloadTableButton_Click(object sender, EventArgs e)
        {
            // TODO: this doesn't work
            if (_tabControl.SelectedTab == null) return;
            string id = _tabControl.SelectedTab.Name;
            _tabControl.SuspendLayout();
            _tabControl.TabPages.Remove(_tabControl.SelectedTab);
            this.CreateTab(id);
            this.FocusTabPage(id);
            _tabControl.ResumeLayout();
        }

    }
}