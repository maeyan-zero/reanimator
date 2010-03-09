using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Reanimator.HeroEditorFunctions;
using System.Data;
using System.Windows.Forms;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public class StatComponents
    {
        Bitmap _completeStatsPanel;
        Graphics _g;

        DataTable _skillTable;
        Panel _panel;

        public StatComponents()
        {
            _panel = new Panel();
        }

        public Panel CreatePanel(ref DataTable skillTable)
        {
            _skillTable = skillTable;

            //select = "skillTab = '" + generalSkillTableId + "'";
            //DataRow[] generalSkills = _skillTable.Select(select);

            //Bitmap connectorLns = CreateConnectorLines(skills);
            //Bitmap generalSkl = CreateGeneralSkills(generalSkills);
            //Bitmap classSkl = CreateClassSkills(skills);

            //_g.DrawImage(connectorLns, new Point());
            //_g.DrawImage(generalSkl, new Point());
            //_g.DrawImage(classSkl, new Point());

            _panel.BackgroundImage = _completeStatsPanel;
            _panel.BackgroundImageLayout = ImageLayout.Stretch;
            _panel.Size = _completeStatsPanel.Size;

            return _panel;
        }
    }
}
