using System;
using System.Drawing;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Forms.HeroEditorFunctions;
using System.IO;
using Config = Revival.Common.Config;

namespace Reanimator.Forms
{
    public partial class HeroEditor2 : Form
    {
        UnitObject _hero;
        // todo: rewrite TableDataSet _dataSet;
        string _filePath;
        UnitHelpFunctions _itemFunctions;
        UnitWrapper _wrapper;

        //// todo: rewrite public HeroEditor2(String filePath, TableDataSet tableDataSet)
        //{
        //    _dataSet = tableDataSet;
        //    _filePath = filePath;

        //    _hero = UnitHelpFunctions.OpenCharacterFile(_dataSet, _filePath);

        //    if (_hero.IsGood)
        //    {
        //        _itemFunctions = new UnitHelpFunctions(_dataSet);
        //        _itemFunctions.LoadCharacterValues(_hero);

        //        _wrapper = new UnitWrapper(tableDataSet, _hero);

        //        InitializeComponent();
        //    }
        //}

        private void HeroEditor2_Load(object sender, EventArgs e)
        {
            string path = Path.Combine(Config.HglDir, @"data\uix\xml\");

            AtlasImageLoader loader = new AtlasImageLoader();
            loader.LoadAtlas(path + "skilltree_atlas.xml");
            loader.LoadAtlas(path + "skillpanel_atlas.xml");

            InitSkillTree(loader);
            InitSkillPanel(loader);
        }

        private void InitSkillPanel(AtlasImageLoader loader)
        {
            Panel panel = new Panel();
            panel.BackColor = Color.Transparent;
            panel.BackgroundImageLayout = ImageLayout.Stretch;
            Bitmap img = loader.GetImage("skill panel");
            this.BackgroundImage = img;
            this.ClientSize = img.Size;

            panel.Scale(new SizeF(0.7f, 0.7f));
            this.Controls.Add(panel);
        }

        private void InitSkillTree(AtlasImageLoader loader)
        { 
            if (_wrapper.Skills.SkillTabs.Count == 0) return;

            foreach (SkillTab skillTab in _wrapper.Skills.SkillTabs)
            {
                Panel panel = CreateSkillTab(skillTab, loader);
                panel.Scale(new SizeF(0.7f, 0.7f));
                this.Controls.Add(panel);
            }
        }

        private Panel CreateSkillTab(SkillTab skillTab, AtlasImageLoader loader)
        {
            Panel panel = new Panel();
            Point distanceToNeighbor = new Point(140, 140);
            //panel.AutoScroll = true;
            //panel.BackColor = Color.Red;

            foreach (Skill skill in skillTab.Skills)
            {
                SingleSkillControl control = new SingleSkillControl(skill, loader, distanceToNeighbor);
                panel.Controls.Add(control);
            }

            panel.Size = new Size(distanceToNeighbor.X * 8 + 102, distanceToNeighbor.Y * 6 + 96);

            return panel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UnitObjectStats.Stat skillLevel = _hero.Stats.Stats[18];
            UnitHelpFunctions.SaveCharacterFile(_hero, _filePath);
        }
    }
}
