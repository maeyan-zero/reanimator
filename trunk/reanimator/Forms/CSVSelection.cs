using System;
using System.Data;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class CSVSelection : Form
    {
        public bool[] Selected;
        public bool SelectAll;

        public CSVSelection()
        {
            InitializeComponent();
        }

        public CSVSelection(DataTable dataTable)
        {
            InitializeComponent();
            Selected = new bool[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                CheckBox checkbox = new CheckBox {Text = dataTable.Columns[i].ColumnName};
                checkbox.Click += _Checkbox_Click;
                flowLayoutPanel1.Controls.Add(checkbox);

                // dont tick related columns and index by default
                if (dataTable.Columns[i].ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsRelationGenerated)) continue;
                if (dataTable.Columns[i].ColumnName == "Index") continue;

                checkbox.CheckState = CheckState.Checked;
                Selected[i] = checkbox.Checked;
            }
        }

        private void _Checkbox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                CheckBox check = (CheckBox)flowLayoutPanel1.Controls[i];
                //if (check.GetType())
                {
                    Selected[i] = check.Checked;
                }
            }
        }

        private void _ButtonSelectAll_Click(object sender, EventArgs e)
        {
            if (SelectAll)
            {
                SelectAll = false;
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    CheckBox check = (CheckBox)flowLayoutPanel1.Controls[i];
                    {
                        check.Checked = false;
                        Selected[i] = check.Checked;
                    }
                }
            }
            else
            {
                SelectAll = true;
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    CheckBox check = (CheckBox)flowLayoutPanel1.Controls[i];
                    {
                        check.Checked = true;
                        Selected[i] = check.Checked;
                    }
                }
            }
        }

        private void _Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
