using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms.ItemQualityCalculatorForm
{
    public partial class ItemQualityCalculator : Form
    {
        FileManager _fileManager;
        DataTable _itemQuality;
        List<string> _qualities;

        public ItemQualityCalculator(FileManager fileManager)
        {
            InitializeComponent();

            _fileManager = fileManager;

            _itemQuality = _fileManager.GetDataTable("ITEM_QUALITY");

            _qualities = new List<string>();

            foreach(DataRow row in _itemQuality.Rows)
            {
                string quality = (string)row[2];

                clb_qualities.Items.Add(quality);

                _qualities.Add(quality);
            }
        }

        private void clb_qualities_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int counter = 0;
            foreach(int value in clb_qualities.CheckedIndices)
            {
                counter += (int)Math.Pow(2, value);
            }

            if (e.NewValue == CheckState.Checked)
            {
                counter += (int)Math.Pow(2, e.Index);
            }
            else
            {
                counter -= (int)Math.Pow(2, e.Index);
            }

            l_quality.Text = counter.ToString();
        }
    }
}
