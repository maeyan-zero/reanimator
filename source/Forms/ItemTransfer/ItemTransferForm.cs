using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Reanimator.Excel;

namespace Reanimator.Forms.ItemTransfer
{
    public enum CharacterStatus
    {
        NotLoaded,
        Loaded,
        Modified,
        Saved,
        Error
    }

    public partial class ItemTransferForm : Form
    {
        const InventoryTypes INVENTORYTYPE = InventoryTypes.Cube;
        const int INVENTORYWIDTH = 6;
        const int INVENTORYHEIGHT = 6;
        const int ITEMUNITSIZE = 40;

        string _characterFolder;

        ExcelTables _excelTables;
        UnitHelpFunctions _itemHelpFunctions;

        string _characterPath1;
        string _characterPath2;
        Unit _characterUnit1;
        Unit _characterUnit2;

        //Unit _selectedItemCharacter1;
        ItemPanel _characterItemPanel1;
        CharacterStatus _characterStatus1;

        //Unit _selectedItemCharacter2;
        ItemPanel _characterItemPanel2;
        CharacterStatus _characterStatus2;

        ItemPanel _eventSender;

        public ItemTransferForm(ref TableDataSet dataSet, ref ExcelTables excelTables)
        {
            InitializeComponent();

            this.Text += " - Location: " + INVENTORYTYPE.ToString();

            _itemHelpFunctions = new UnitHelpFunctions(ref dataSet, ref excelTables);

            _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

            _excelTables = excelTables;

            string[] characters = LoadCharacterNames();

            cb_selectCharacter1.DataSource = characters;
            cb_selectCharacter2.DataSource = characters.Clone();

            _characterItemPanel1 = new ItemPanel();
            _characterItemPanel2 = new ItemPanel();

            _characterItemPanel1.NewItemSelected_Event += new ItemPanel.NewItemSelected(_characterItemPanel_NewItemSelected_Event);
            //_characterItemPanel1.ItemDoubleClicked_Event += new ItemPanel.ItemDoubleClicked(_characterItemPanel_ItemDoubleClicked_Event);
            _characterItemPanel2.NewItemSelected_Event += new ItemPanel.NewItemSelected(_characterItemPanel_NewItemSelected_Event);
            //_characterItemPanel2.ItemDoubleClicked_Event += new ItemPanel.ItemDoubleClicked(_characterItemPanel_ItemDoubleClicked_Event);

            _characterItemPanel1.ItemUnitSize = ITEMUNITSIZE;
            _characterItemPanel2.ItemUnitSize = ITEMUNITSIZE;

            _characterItemPanel1.Size = new Size(INVENTORYWIDTH * ITEMUNITSIZE, INVENTORYHEIGHT * ITEMUNITSIZE);
            _characterItemPanel2.Size = new Size(INVENTORYWIDTH * ITEMUNITSIZE, INVENTORYHEIGHT * ITEMUNITSIZE);

            _characterItemPanel1.Location = new Point(16, 18);
            _characterItemPanel2.Location = new Point(16, 18);

            SetCharacterStatus(_characterStatus1, CharacterStatus.NotLoaded, p_status1, l_status1);
            SetCharacterStatus(_characterStatus2, CharacterStatus.NotLoaded, p_status2, l_status2);

            // use inventory panels, as a normal groupBox doesn't provide the option "AutoScroll"
            p_inventory1.Controls.Add(_characterItemPanel1);
            p_inventory2.Controls.Add(_characterItemPanel2);

            EnableComboBoxes(true, true);
        }

        private void SetCharacterStatus(CharacterStatus originalCharacterStatus, CharacterStatus newCharacterStatus, Panel panel, Label label)
        {
            originalCharacterStatus = newCharacterStatus;

            if (newCharacterStatus == CharacterStatus.Error)
            {
                panel.BackColor = Color.Red;
                label.Text = "An error occured";
            }
            else if (newCharacterStatus == CharacterStatus.Loaded)
            {
                panel.BackColor = Color.Green;
                label.Text = "Character loaded";
            }
            else if (newCharacterStatus == CharacterStatus.Modified)
            {
                panel.BackColor = Color.Orange;
                label.Text = "Character was modified";
            }
            else if (newCharacterStatus == CharacterStatus.NotLoaded)
            {
                panel.BackColor = Color.Silver;
                label.Text = "No character loaded";
            }
            else if (newCharacterStatus == CharacterStatus.Saved)
            {
                panel.BackColor = Color.Lime;
                label.Text = "Character saved";
            }
        }

        //void _characterItemPanel_ItemDoubleClicked_Event(ItemPanel sender, InventoryItem item)
        //{
        //    _characterItemPanel_NewItemSelected_Event(sender, item);

        //    b_transfer_Click(sender, null);

        //}

        void _characterItemPanel_NewItemSelected_Event(ItemPanel sender, InventoryItem item)
        {
            _eventSender = sender;

            l_selectedItem.Text = item.Item.Name;

            if (item.Quantity > 1)
            {
                l_selectedItem.Text += " (x" + item.Quantity.ToString() + ")";
            }

            l_selectedItem.Tag = item;
        }

        private string[] LoadCharacterNames()
        {
            string[] characters = Directory.GetFiles(_characterFolder, "*.hg1");

            for(int counter = 0; counter < characters.Length; counter++)
            {
                string charName = characters[counter].Replace(_characterFolder + @"\", string.Empty);
                charName = charName.Replace(".hg1", string.Empty);

                characters[counter] = charName;
            }

            return characters;
        }

        private void b_loadCharacter1_Click(object sender, EventArgs e)
        {
            _characterPath1 = _characterFolder + @"\" + cb_selectCharacter1.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                _characterUnit1 = UnitHelpFunctions.OpenCharacterFile(ref _excelTables, _characterPath1);

                if (_characterUnit1 != null && _characterUnit1.IsGood)
                {
                    _itemHelpFunctions.LoadCharacterValues(_characterUnit1);

                    gb_characterName1.Text = cb_selectCharacter1.SelectedItem.ToString();
                    int level = UnitHelpFunctions.GetSimpleValue(_characterUnit1, ItemValueNames.level.ToString()) - 8;
                    gb_characterName1.Text += " (Level " + level.ToString() + ")";

                    SetCharacterStatus(_characterStatus1, CharacterStatus.Loaded, p_status1, l_status1);

                    InitInventory(_characterUnit1, _characterItemPanel1);
                }
                else
                {
                    MessageBox.Show("Error while parsing the character file!");
                }
            }
            else
            {
                MessageBox.Show("You cannot load the same character for trading!");
            }
        }

        private void b_loadCharacter2_Click(object sender, EventArgs e)
        {
            _characterPath2 = _characterFolder + @"\" + cb_selectCharacter2.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                _characterUnit2 = UnitHelpFunctions.OpenCharacterFile(ref _excelTables, _characterPath2);

                if (_characterUnit2 != null && _characterUnit2.IsGood)
                {
                    _itemHelpFunctions.LoadCharacterValues(_characterUnit2);

                    gb_characterName2.Text = cb_selectCharacter2.SelectedItem.ToString();
                    int level = UnitHelpFunctions.GetSimpleValue(_characterUnit2, ItemValueNames.level.ToString()) - 8;
                    gb_characterName2.Text += " (Level " + level.ToString() + ")";

                    SetCharacterStatus(_characterStatus2, CharacterStatus.Loaded, p_status2, l_status2);

                    InitInventory(_characterUnit2, _characterItemPanel2);
                }
                else
                {
                    MessageBox.Show("Error while parsing the character file!");
                }
            }
            else
            {
                MessageBox.Show("You cannot load the same character for trading!");
            }
        }

        private void InitInventory(Unit unit, ItemPanel itemPanel)
        {
            itemPanel.Controls.Clear();

            try
            {
                foreach (Unit item in unit.Items)
                {
                    if (item.inventoryType == (int)INVENTORYTYPE)
                    {
                        InventoryItem iItem = new InventoryItem(item);
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
            if(_characterUnit1 != null && _characterUnit2 != null)
            {
                if (MessageBox.Show("Are you sure you want to save these changes?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UnitHelpFunctions.SaveCharacterFile(_characterUnit1, _characterPath1);
                    UnitHelpFunctions.SaveCharacterFile(_characterUnit2, _characterPath2);

                    MessageBox.Show("Saving successful!");
                }

                EnableComboBoxes(true, true);
                SetCharacterStatus(_characterStatus1, CharacterStatus.Saved, p_status1, l_status1);
                SetCharacterStatus(_characterStatus2, CharacterStatus.Saved, p_status2, l_status2);
            }
        }

        private void b_transfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (_characterUnit1 != null && _characterUnit2 != null)
                {
                    if (l_selectedItem.Tag != null)
                    {
                        InventoryItem item = (InventoryItem)l_selectedItem.Tag;

                        if (_eventSender == _characterItemPanel1)
                        {
                            if (_characterItemPanel2.AddItem(item, false))
                            {
                                _characterUnit2.Items.Add(item.Item);
                                _characterUnit1.Items.Remove(item.Item);
                                _characterItemPanel1.RemoveItem(item);
                            }
                            else
                            {
                                MessageBox.Show("There is not enough free space!");
                            }
                            l_selectedItem.ResetText();
                            l_selectedItem.Tag = null;
                        }
                        else
                        {
                            if (_characterItemPanel1.AddItem(item, false))
                            {
                                _characterUnit1.Items.Add(item.Item);
                                _characterUnit2.Items.Remove(item.Item);
                                _characterItemPanel2.RemoveItem(item);
                            }
                            else
                            {
                                MessageBox.Show("There is not enough free space!");
                            }
                            l_selectedItem.ResetText();
                            l_selectedItem.Tag = null;
                        }

                        EnableComboBoxes(false, false);
                        SetCharacterStatus(_characterStatus1, CharacterStatus.Modified, p_status1, l_status1);
                        SetCharacterStatus(_characterStatus2, CharacterStatus.Modified, p_status2, l_status2);
                    }
                }
                else
                {
                    MessageBox.Show("You have to load two characters to transfere items!");
                }
            }
            catch (Exception)
            {
                EmergencyAbort();
            }
        }

        private void b_transferAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (_characterUnit1 != null && _characterUnit2 != null)
                {
                    List<Unit> tmpItem = new List<Unit>();

                    for (int counter = 0; counter < _characterUnit1.Items.Count; counter++)
                    {
                        if (_characterUnit1.Items[counter].inventoryType == (int)INVENTORYTYPE)
                        {
                            tmpItem.Add(_characterUnit1.Items[counter]);
                            _characterUnit1.Items.RemoveAt(counter);

                            counter--;
                        }
                    }

                    for (int counter = 0; counter < _characterUnit2.Items.Count; counter++)
                    {
                        if (_characterUnit2.Items[counter].inventoryType == (int)INVENTORYTYPE)
                        {
                            _characterUnit1.Items.Add(_characterUnit2.Items[counter]);
                            _characterUnit2.Items.RemoveAt(counter);

                            counter--;
                        }
                    }

                    _characterUnit2.Items.AddRange(tmpItem.ToArray());

                    InitInventory(_characterUnit1, _characterItemPanel1);
                    InitInventory(_characterUnit2, _characterItemPanel2);

                    EnableComboBoxes(false, false);
                    SetCharacterStatus(_characterStatus1, CharacterStatus.Modified, p_status1, l_status1);
                    SetCharacterStatus(_characterStatus2, CharacterStatus.Modified, p_status2, l_status2);
                }
            }
            catch (Exception)
            {
                EmergencyAbort();
            }
        }

        private void b_delete_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryItem item = (InventoryItem)l_selectedItem.Tag;

                _eventSender.RemoveItem(item);

                l_selectedItem.ResetText();
                l_selectedItem.Tag = null;

                EnableComboBoxes(false, false);
                SetCharacterStatus(_characterStatus1, CharacterStatus.Modified, p_status1, l_status1);
                SetCharacterStatus(_characterStatus2, CharacterStatus.Modified, p_status2, l_status2);
            }
            catch (Exception)
            {
                EmergencyAbort();
            }
        }

        private void EmergencyAbort()
        {
            SetCharacterStatus(_characterStatus1, CharacterStatus.Error, p_status1, l_status1);
            SetCharacterStatus(_characterStatus2, CharacterStatus.Error, p_status2, l_status2);
            MessageBox.Show("The trade window will now close to ensure that your savegames and items will not be corrupted!", "Error while transfering your items!");
            this.Close();
        }

        private void b_undoTransfer_Click(object sender, EventArgs e)
        {
            b_loadCharacter1_Click(null, null);
            b_loadCharacter2_Click(null, null);

            EnableComboBoxes(true, true);
        }

        private void EnableComboBoxes(bool enable1, bool enable2)
        {
            EnableComboBox1(enable1);
            EnableComboBox2(enable2);
        }

        private void EnableComboBox1(bool enable)
        {
            cb_selectCharacter1.Enabled = enable;
            b_loadCharacter1.Enabled = enable;
        }

        private void EnableComboBox2(bool enable)
        {
            cb_selectCharacter2.Enabled = enable;
            b_loadCharacter2.Enabled = enable;
        }
    }
}
