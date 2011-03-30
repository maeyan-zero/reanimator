using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms
{
    public partial class SavegameOverviewForm : Form
    {
        public SavegameOverviewForm()
        {
            InitializeComponent();
            savegameOverviewControl1.EntrySelectedEvent += new EntrySelected(savegameOverviewControl1_EntrySelectedEvent);
        }

        void savegameOverviewControl1_EntrySelectedEvent(object sender, EventArgs e)
        {
            SavegameOverviewEntry entry = (SavegameOverviewEntry)sender;
            UnitWrapper2 unit = (UnitWrapper2)entry.Tag;
            textBox1.Text = unit.Name;

            treeViewSkills.Nodes.Clear();
            CharacterSkills skills = unit.CharacterWrapper.CharacterSkills;

            int counter = 0;

            TreeNode general = new TreeNode("General skills (bugged for Ressurection)");

            foreach (Skill skill in skills.GeneralSkills.Skills)
            {
                string text = string.Format("ID: {0}, Learned: {1}, Level: {2}/{3}, DescriptionID: {4}", skill.Name, skill.Learned, skill.CurrentLevel, skill.MaxLevel, skill.Description);

                general.Nodes.Add(text);
            }

            treeViewSkills.Nodes.Add(general);

            foreach (SkillTab skillTab in skills.SkillTabs)
            {
                counter++;
                string name = "Perks (should not be displayed in Singleplayer)";
                switch(counter)
                {
                    case 1:
                        name = "Skill Page 1 (bugged for Singleplayer)";
                        break;
                    case 2:
                        name = "Skill Page 2 (bugged for Singleplayer)";
                        break;
                    case 3:
                        name = "Skill Page 3 (bugged for Singleplayer)";
                        break;
                }                

                TreeNode node = new TreeNode(name);

                foreach (Skill skill in skillTab.Skills)
                {
                    string text = string.Format("ID: {0}, Learned: {1}, Level: {2}/{3}, DescriptionID: {4}", skill.Name, skill.Learned, skill.CurrentLevel, skill.MaxLevel, skill.Description);

                    node.Nodes.Add(text);
                }

                treeViewSkills.Nodes.Add(node);
            }
        }

        public void AddCharacter(UnitWrapper2 wrapper)
        {
            savegameOverviewControl1.AddCharacter(wrapper);
        }
    }
}
