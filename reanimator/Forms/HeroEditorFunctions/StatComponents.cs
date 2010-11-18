using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Reanimator.HeroEditorFunctions;
using System.Data;
using System.Windows.Forms;
using Reanimator;
using Reanimator.Properties;

namespace Reanimator.HeroEditorFunctions
{
    public class StatComponents
    {
        SkillPanel _skillPanel;

        Bitmap _completeStatsPanel;
        Graphics _g;

        Panel _panel;

        DataTable _charValues;
        Unit _hero;

        public StatComponents(SkillPanel skillPanel)
        {
            _skillPanel = skillPanel;

            _panel = new Panel();

            _completeStatsPanel = _skillPanel.GetImageFromInventoryPanel("character");
            _completeStatsPanel.MakeTransparent(Color.White);

            _g = Graphics.FromImage(_completeStatsPanel);
        }

        public Panel CreatePanel(Unit heroUnit, DataTable charValues)
        {
            _charValues = charValues;
            _hero = heroUnit;

            //select = "skillTab = '" + generalSkillTableId + "'";
            //DataRow[] generalSkills = _skillTable.Select(select);

            //Bitmap statusMenu = CreateStatusPanel();

            //Bitmap connectorLns = CreateConnectorLines(skills);
            //Bitmap generalSkl = CreateGeneralSkills(generalSkills);
            //Bitmap classSkl = CreateClassSkills(skills);

            //_g.DrawImage(connectorLns, new Point());
            //_g.DrawImage(generalSkl, new Point());
            //_g.DrawImage(classSkl, new Point());
            _completeStatsPanel.Save(@"F:\test.bmp");

            CreateScrollButtons();

            _panel.BackgroundImage = _completeStatsPanel;
            _panel.BackgroundImageLayout = ImageLayout.Stretch;
            _panel.Size = _completeStatsPanel.Size;

            return _panel;
        }

        private void CreateScrollButtons()
        {

        }

        private Bitmap CreateStatusPanel()
        {
            return _skillPanel.GetImageFromInventoryPanel("left panel blank");
        }
    }
}
