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
            this.Text = unit.Name;

            treeViewSkills.Nodes.Clear();
            CharacterSkills skills = unit.CharacterWrapper.CharacterSkills;
            CharacterValues values = unit.CharacterWrapper.CharacterValues;
            CharacterInventory inventory = unit.CharacterWrapper.CharacterInventory;

            AddCharacterStats(values);

            AddGeneralSkills(skills);

            AddCharacterSkills(skills);

            AddCharacterItems(inventory);
        }

        private void AddCharacterItems(CharacterInventory inventory)
        {
            TreeNode inventoryNode = new TreeNode("Complete inventory");

            //CharacterInventoryType type = inventory.GetInventoryById((int)InventoryTypes.Inventory);
            foreach (CharacterInventoryType inventoryType in inventory.InventoryType)
            {
                InventoryTypesComplete tmp = (InventoryTypesComplete)inventoryType.InventoryType;
                TreeNode inventoryTypeNode = new TreeNode(tmp.ToString());
                inventoryTypeNode.ForeColor = Color.Red;

                foreach (CharacterItems item in inventoryType.Items)
                {
                    TreeNode node = CreateItemNode(item);
                    inventoryTypeNode.Nodes.Add(node);
                }

                inventoryNode.Nodes.Add(inventoryTypeNode);
            }

            treeViewSkills.Nodes.Add(inventoryNode);
        }

        private TreeNode CreateItemNode(CharacterItems item)
        {
            TreeNode node = new TreeNode(item.Name);

            node.Nodes.Add("Quest item: " + item.IsQuestItem);
            node.Nodes.Add("Consumable: " + item.IsConsumable);
            node.Nodes.Add("Affixes: " + item.NumberOfAffixes + "/" + item.MaxNumberOfAffixes);
            node.Nodes.Add("Augmentations: " + item.NumberOfAugmentations + "/" + item.MaxNumberOfAugmentations);
            node.Nodes.Add("Upgrades: " + item.NumberOfUpgrades + "/" + item.MaxNumberOfUpgrades);
            node.Nodes.Add("Quality: " + item.Quality);
            node.Nodes.Add("Quality Color: " + item.QualityColor.R + "," + item.QualityColor.G + "," + item.QualityColor.B);
            node.Nodes.Add("Stack size: " + item.StackSize + "/" + item.MaxStackSize);
            node.Nodes.Add("Has mods: " + item.WrappedItems.Count);

            TreeNode inventory = new TreeNode("Inventory");

            inventory.Nodes.Add("Type: " + item.InventoryType);
            inventory.Nodes.Add("Position: " + item.InventoryPosition.X + "," + item.InventoryPosition.Y);
            inventory.Nodes.Add("Size: " + item.InventorySize.Width + "," + item.InventorySize.Height);

            node.Nodes.Add(inventory);

            return node;
        }

        private void AddCharacterStats(CharacterValues values)
        {
            TreeNode characterValues = new TreeNode("Character stats");

            TreeNode accuracy = new TreeNode("Accuracy: " + values.Accuracy);
            accuracy.ForeColor = Color.Red;
            TreeNode strength = new TreeNode("Strength: " + values.Strength);
            strength.ForeColor = Color.Red;
            TreeNode stamina = new TreeNode("Stamina: " + values.Stamina);
            stamina.ForeColor = Color.Red;
            TreeNode willpower = new TreeNode("Willpower: " + values.Willpower);
            willpower.ForeColor = Color.Red;
            TreeNode availableAttributePoints = new TreeNode("Available attribute points: " + values.AttributePoints);
            availableAttributePoints.ForeColor = Color.Red;
            TreeNode availableSkillPoints = new TreeNode("Available skill points: " + values.SkillPoints);
            availableSkillPoints.ForeColor = Color.Red;
            TreeNode experiencePoints = new TreeNode("Experience points: " + values.Experience);
            experiencePoints.ForeColor = Color.Red;
            TreeNode achievementPoints = new TreeNode("Achievement points: " + values.AchievementPointsCur);
            achievementPoints.ForeColor = Color.Red;

            characterValues.Nodes.Add(accuracy);
            characterValues.Nodes.Add(strength);
            characterValues.Nodes.Add(stamina);
            characterValues.Nodes.Add(willpower);
            characterValues.Nodes.Add(availableAttributePoints);
            characterValues.Nodes.Add(availableSkillPoints);
            characterValues.Nodes.Add(experiencePoints);
            characterValues.Nodes.Add(achievementPoints);

            treeViewSkills.Nodes.Add(characterValues);
        }

        private void AddCharacterSkills(CharacterSkills skills)
        {
            int counter = 0;

            foreach (SkillTab skillTab in skills.SkillTabs)
            {
                counter++;
                string name = "Perks (should not be displayed in Singleplayer)";
                switch (counter)
                {
                    case 1:
                        name = "Skill Page 1";
                        break;
                    case 2:
                        name = "Skill Page 2";
                        break;
                    case 3:
                        name = "Skill Page 3";
                        break;
                }

                TreeNode node = new TreeNode(name);

                foreach (Skill skill in skillTab.Skills)
                {
                    TreeNode subNode = new TreeNode(skill.Name);
                    subNode.ForeColor = Color.Red;
                    subNode.Nodes.Add("ID: " + skill.SkillID);
                    subNode.Nodes.Add("Learned: " + skill.Learned);
                    subNode.Nodes.Add("Level: " + skill.CurrentLevel + "/" + skill.MaxLevel);
                    subNode.Nodes.Add("Description: " + skill.Description);
                    subNode.Nodes.Add("Position: " + skill.Position.X + "," + skill.Position.Y);
                    subNode.Nodes.Add("Icon name: " + skill.IconName);
                    subNode.Nodes.Add("Required skill (ID): " + (skill.RequiredSkills.Length > 0 ? skill.RequiredSkills[0].ToString() : "none"));
                    subNode.Nodes.Add("Required skill (level): " + (skill.LevelsOfRequiredSkills.Length > 0 ? skill.LevelsOfRequiredSkills[0].ToString() : "none"));

                    node.Nodes.Add(subNode);
                }

                treeViewSkills.Nodes.Add(node);
            }
        }

        private void AddGeneralSkills(CharacterSkills skills)
        {
            TreeNode general = new TreeNode("General skills (bugged for Ressurection -> where do all those extra skills come from?)");

            foreach (Skill skill in skills.GeneralSkills.Skills)
            {
                TreeNode subNode = new TreeNode(skill.Name);
                subNode.ForeColor = Color.Red;
                subNode.Nodes.Add("ID: " + skill.SkillID);
                subNode.Nodes.Add("Learned: " + skill.Learned);
                subNode.Nodes.Add("Level: " + skill.CurrentLevel + "/" + skill.MaxLevel);
                subNode.Nodes.Add("Description: " + skill.Description);
                subNode.Nodes.Add("Position: " + skill.Position.X + "," + skill.Position.Y);
                subNode.Nodes.Add("Icon name: " + skill.IconName);
                subNode.Nodes.Add("Required skill (ID): " + (skill.RequiredSkills.Length > 0 ? skill.RequiredSkills[0].ToString() : "none"));
                subNode.Nodes.Add("Required skill (level): " + (skill.LevelsOfRequiredSkills.Length > 0 ? skill.LevelsOfRequiredSkills[0].ToString() : "none"));
                general.Nodes.Add(subNode);
            }

            treeViewSkills.Nodes.Add(general);
        }

        public void AddCharacter(UnitWrapper2 wrapper)
        {
            savegameOverviewControl1.AddCharacter(wrapper);
        }
    }
}
