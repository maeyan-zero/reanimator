using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class CSVSelection : Form
    {
        public bool[] selected;
        public bool selectAll;

        public CSVSelection()
        {
            InitializeComponent();
        }

        public CSVSelection(DataTable dataTable)
        {
            InitializeComponent();
            selected = new bool[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Text = dataTable.Columns[i].ColumnName;
                checkbox.Click += new EventHandler(checkbox_Click);
                flowLayoutPanel1.Controls.Add(checkbox);

                // dont tick related columns and index by default
                if (!(dataTable.Columns[i].ExtendedProperties.ContainsKey(ExcelFile.ColumnTypeKeys.IsRelationGenerated)))
                {
                    if (!(dataTable.Columns[i].ColumnName == "Index"))
                    {
                        checkbox.CheckState = CheckState.Checked;
                        selected[i] = checkbox.Checked;
                    }
                }
            }
        }

        private void checkbox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                CheckBox check = (CheckBox)flowLayoutPanel1.Controls[i];
                //if (check.GetType())
                {
                    selected[i] = check.Checked;
                }
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            if (selectAll == true)
            {
                selectAll = false;
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    CheckBox check = (CheckBox)flowLayoutPanel1.Controls[i];
                    {
                        check.Checked = false;
                        selected[i] = check.Checked;
                    }
                }
            }
            else
            {
                selectAll = true;
                for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                {
                    CheckBox check = (CheckBox)flowLayoutPanel1.Controls[i];
                    {
                        check.Checked = true;
                        selected[i] = check.Checked;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
