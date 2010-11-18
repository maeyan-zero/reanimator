using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public partial class CharacterValuesControl : UserControl
    {
        AttributesControl _attributes;
        FactionStandingsControl _factions;
        BasicStatisticsControl _statistics;
        DefenseControl _defense;

        public AttributesControl CAttributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public BasicStatisticsControl CStatistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        public DefenseControl CDefense
        {
            get { return _defense; }
            set { _defense = value; }
        }
        
        public FactionStandingsControl CFactions
        {
            get { return _factions; }
            set { _factions = value; }
        }

        public CharacterValuesControl()
        {
            InitializeComponent();

            _attributes = new AttributesControl();
            _statistics = new BasicStatisticsControl();
            _defense = new DefenseControl();
            _factions = new FactionStandingsControl();

            _attributes.SizeChanged += new EventHandler(stats_SizeChanged);
            _statistics.SizeChanged += new EventHandler(stats_SizeChanged);
            _defense.SizeChanged += new EventHandler(stats_SizeChanged);
            _factions.SizeChanged += new EventHandler(stats_SizeChanged);

            ResizeAndSetControlLocation();

            this.Controls.Add(_attributes);
            this.Controls.Add(_statistics);
            this.Controls.Add(_defense);
            this.Controls.Add(_factions);
        }

        void stats_SizeChanged(object sender, EventArgs e)
        {
            ResizeAndSetControlLocation();
        }

        private void ResizeAndSetControlLocation()
        {
            this.SuspendLayout();

            //p_attributePanel.ScrollControlIntoView(_stats1);
            this.AutoScrollPosition = new Point();

            _attributes.Location = new Point(0, 0);
            _statistics.Location = new Point(0, _attributes.Size.Height + 4);
            _defense.Location = new Point(0, _attributes.Size.Height + _statistics.Size.Height + 8);
            _factions.Location = new Point(0, _attributes.Size.Height + _statistics.Size.Height + _defense.Size.Height + 12);

            this.ResumeLayout();
        }
    }
}
