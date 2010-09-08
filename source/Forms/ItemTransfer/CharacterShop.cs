using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Reanimator.Forms.ItemTransfer
{
    public partial class CharacterShop : Form
    {
        private const InventoryTypes InventoryType = InventoryTypes.Cube;
        private const int InventoryWidth = 6;
        private const int InventoryHeight = 6;
        private const int ItemUnitSize = 40;

        private readonly string _characterFolder;

        private TableFiles _tableFiles;
        private TableDataSet _dataSet;

        private string _characterPath;
        private Unit _characterUnit;

        private readonly UnitHelpFunctions _itemHelpFunctions;

        private InventoryItem _selectedItem;
        private readonly ItemPanel _characterItemPanel;
        private CharacterStatus CharacterStatus = 0;

        public CharacterShop(TableDataSet dataSet, TableFiles tableFiles)
        {
            InitializeComponent();

            this.Text += " - Location: " + InventoryType;

            _itemHelpFunctions = new UnitHelpFunctions(dataSet);

            _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

            _tableFiles = tableFiles;
            _dataSet = dataSet;

            string[] characters = LoadCharacterNames();

            cb_selectCharacter.DataSource = characters;

            _characterItemPanel = new ItemPanel(false);
            _characterItemPanel.NewItemSelected_Event += _characterItemPanel_NewItemSelected_Event;
            _characterItemPanel.ItemUnitSize = ItemUnitSize;
            _characterItemPanel.Size = new Size(InventoryWidth * ItemUnitSize, InventoryHeight * ItemUnitSize);
            _characterItemPanel.Location = new Point(16, 18);

            SetCharacterStatus(CharacterStatus, CharacterStatus.NotLoaded, p_status, l_status);

            // use inventory panels, as a normal groupBox doesn't provide the option "AutoScroll"
            p_inventory.Controls.Add(_characterItemPanel);
        }

        void _characterItemPanel_NewItemSelected_Event(ItemPanel sender, InventoryItem item)
        {
            _selectedItem = item;
            l_selectedItem.Text = item.Item.Name;

            if (item.Quantity > 1)
            {
                l_selectedItem.Text += " (x" + item.Quantity + ")";
            }

            l_selectedItem.Tag = item;
        }

        private string[] LoadCharacterNames()
        {
            string[] characters = Directory.GetFiles(_characterFolder, "*.hg1");

            for (int counter = 0; counter < characters.Length; counter++)
            {
                string charName = characters[counter].Replace(_characterFolder + @"\", string.Empty);
                charName = charName.Replace(".hg1", string.Empty);

                characters[counter] = charName;
            }

            return characters;
        }

        private void b_loadCharacter_Click(object sender, EventArgs e)
        {
            _characterPath = _characterFolder + @"\" + cb_selectCharacter.SelectedItem + ".hg1";

            //_characterUnit = UnitHelpFunctions.OpenCharacterFile(_tableFiles, _characterPath);

            if (_characterUnit != null && _characterUnit.IsGood)
            {
                _itemHelpFunctions.LoadCharacterValues(_characterUnit);

                gb_characterName.Text = cb_selectCharacter.SelectedItem.ToString();
                int level = UnitHelpFunctions.GetSimpleValue(_characterUnit, ItemValueNames.level.ToString()) - 8;
                gb_characterName.Text += " (Level " + level.ToString() + ")";

                SetCharacterStatus(CharacterStatus, CharacterStatus.Loaded, p_status, l_status);

                InitInventory(_characterUnit, _characterItemPanel);
            }
            else
            {
                MessageBox.Show("Error while parsing the character file!", "b_loadCharacter_Click");
            }
        }

        private void InitInventory(Unit unit, ItemPanel itemPanel)
        {
            itemPanel.Controls.Clear();

            try
            {
                foreach (Unit item in unit.Items)
                {
                    if (item.inventoryType == (int)InventoryType)
                    {
                        InventoryItem iItem = new InventoryItem(item, 0);
                        iItem.InitButton(false);
                        itemPanel.AddItem(iItem, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitInventory: " + unit.Name);
            }
        }

        private void b_save_Click(object sender, EventArgs e)
        {
            if (_characterUnit != null)
            {
                if (MessageBox.Show("Are you sure you want to save these changes?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UnitHelpFunctions.SaveCharacterFile(_characterUnit, _characterPath);

                    MessageBox.Show("Saving successful!");
                }

                SetCharacterStatus(CharacterStatus, CharacterStatus.Saved, p_status, l_status);
            }
        }

        private void EmergencyAbort()
        {
            SetCharacterStatus(CharacterStatus, CharacterStatus.Error, p_status, l_status);
            MessageBox.Show("The trade window will now close to ensure that your savegames and items will not be corrupted!", "Error while transfering your items!");
            this.Close();
        }

        private void SetCharacterStatus(CharacterStatus originalCharacterStatus, CharacterStatus newCharacterStatus, Panel panel, Label label)
        {
            originalCharacterStatus = newCharacterStatus;

            switch (newCharacterStatus)
            {
                case CharacterStatus.Error:
                    panel.BackColor = Color.Red;
                    label.Text = "An error occured";
                    break;
                case CharacterStatus.Loaded:
                    panel.BackColor = Color.Green;
                    label.Text = "Character loaded";
                    break;
                case CharacterStatus.Modified:
                    panel.BackColor = Color.Orange;
                    label.Text = "Character was modified";
                    break;
                case CharacterStatus.NotLoaded:
                    panel.BackColor = Color.Silver;
                    label.Text = "No character loaded";
                    break;
                case CharacterStatus.Saved:
                    panel.BackColor = Color.Lime;
                    label.Text = "Character saved";
                    break;
            }
        }

        private void b_addAffix_Click(object sender, EventArgs e)
        {
            if (_characterUnit == null || _selectedItem == null) return;

            Unit.StatBlock.Stat value = UnitHelpFunctions.GetComplexValue(_selectedItem.Item, ItemValueNames.applied_affix.ToString());
            if (value != null)
            {
                MessageBox.Show(value.ToString());
            }
        }
    }
}
