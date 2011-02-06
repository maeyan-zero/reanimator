using System;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class WayPointControl : UserControl
    {
        UnitObject.StatBlock.Stat.Values wayPointValues;
        int bits;

        public WayPointControl()
        {
            InitializeComponent();

            bits = 0;
        }

        public WayPointControl(UnitObject.StatBlock.Stat.Values values) : this()
        {
            wayPointValues = values;
        }

        private void InitializeWayPointList()
        {
            int value = wayPointValues.StatValue;

            clb_wayPoints.SuspendLayout();
            clb_wayPoints.BeginUpdate();
            clb_wayPoints.Items.Add("Holborn Station", ((value & 1) != 0));
            clb_wayPoints.Items.Add("Covent Garden Station", ((value & 2) != 0));
            clb_wayPoints.Items.Add("Charing Cross Station", ((value & 4) != 0));
            clb_wayPoints.Items.Add("Green Park Station", ((value & 8) != 0));
            clb_wayPoints.Items.Add("Oxford Circus Station", ((value & 16) != 0));
            clb_wayPoints.Items.Add("Temple Station", ((value & 32) != 0));
            clb_wayPoints.Items.Add("Templar Base", ((value & 64) != 0));
            clb_wayPoints.Items.Add("Monument Station", ((value & 128) != 0));
            clb_wayPoints.Items.Add("Liverpool Street Station", ((value & 256) != 0));
            clb_wayPoints.Items.Add("(Finsbury Square)", ((value & 512) != 0));
            clb_wayPoints.Items.Add("St. Paul's Station", ((value & 1024) != 0));
            clb_wayPoints.EndUpdate();
            clb_wayPoints.ResumeLayout();
        }

        private void clb_wayPoints_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bits = 0;

            foreach (int index in clb_wayPoints.CheckedIndices)
            {
                bits += (int)Math.Pow(2, index);
            }

            if (e.NewValue == CheckState.Checked)
            {
                bits += (int)Math.Pow(2, e.Index);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                bits -= (int)Math.Pow(2, e.Index);
            }

            wayPointValues.StatValue = bits;
        }

        private void WayPointControl_Load(object sender, EventArgs e)
        {
            InitializeWayPointList();
        }
    }
}
