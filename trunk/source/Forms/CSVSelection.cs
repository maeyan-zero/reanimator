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

        public CSVSelection()
        {
            InitializeComponent();
        }

        public CSVSelection(DataGridView datagridview)
        {
            InitializeComponent();
            selected = new bool[datagridview.ColumnCount];
            for (int i = 0; i < datagridview.ColumnCount; i++)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Text = datagridview.Columns[i].Name;
                checkbox.Click += new EventHandler(checkbox_Click);
                flowLayoutPanel1.Controls.Add(checkbox);
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
    }
}
