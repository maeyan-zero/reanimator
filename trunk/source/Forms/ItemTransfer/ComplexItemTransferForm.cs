using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Reanimator.Forms.ItemTransfer
{
    public class ComplexItemTransferForm : BasicItemTransferForm
    {
        TableFiles _tableFiles;
        UnitHelpFunctions _itemHelpFunctions;

        /// <summary>
        /// Use this constructor when starting the item transfer window from within Reanimator (additional item infos)
        /// </summary>
        /// <param name="dataSet">The dataset to use</param>
        /// <param name="tableFiles">The exceltables to use</param>
        public ComplexItemTransferForm(TableDataSet dataSet, TableFiles tableFiles)
        {
            _itemHelpFunctions = new UnitHelpFunctions(dataSet, tableFiles);

            _tableFiles = tableFiles;

            //preload the tables
            dataSet.GetExcelTableFromStringId("ITEMS");
            dataSet.GetExcelTableFromStringId("AFFIXES");

            //adding the character names triggers the cb_selectCharacter1_SelectedIndexChanged event which will load the first character
            string[] characters = LoadCharacterNames();
            cb_selectCharacter1.DataSource = characters;
            cb_selectCharacter2.DataSource = characters.Clone();

            EnableTradingControls(false);
        }

        protected override void b_loadCharacter1_Click(object sender, EventArgs e)
        {
            _characterPath1 = _characterFolder + @"\" + cb_selectCharacter1.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                _characterUnit1 = UnitHelpFunctions.OpenCharacterFile(_tableFiles, _characterPath1);

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
                    _characterPath1 = null;
                    _characterUnit1 = null;
                    _characterItemPanel1.Controls.Clear();
                    SetCharacterStatus(_characterStatus1, CharacterStatus.Error, p_status1, l_status1);
                    //MessageBox.Show("Error while parsing the character file!");
                }
            }
            else
            {
                _characterPath1 = null;
                _characterUnit1 = null;
                _characterItemPanel1.Controls.Clear();
                SetCharacterStatus(_characterStatus1, CharacterStatus.AlreadyLoaded, p_status1, l_status1);
                SameCharacterSelected();
            }

            CheckAndSetButtonStatus();
        }

        protected override void b_loadCharacter2_Click(object sender, EventArgs e)
        {
            _characterPath2 = _characterFolder + @"\" + cb_selectCharacter2.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                _characterUnit2 = UnitHelpFunctions.OpenCharacterFile(_tableFiles, _characterPath2);

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
                    _characterPath2 = null;
                    _characterUnit2 = null;
                    _characterItemPanel2.Controls.Clear();
                    SetCharacterStatus(_characterStatus2, CharacterStatus.Error, p_status2, l_status2);
                    //MessageBox.Show("Error while parsing the character file!");
                }
            }
            else
            {
                _characterPath2 = null;
                _characterUnit2 = null;
                _characterItemPanel2.Controls.Clear();
                SetCharacterStatus(_characterStatus2, CharacterStatus.AlreadyLoaded, p_status2, l_status2);
                SameCharacterSelected();
            }

            CheckAndSetButtonStatus();
        }

        private void SameCharacterSelected()
        {
            if (!_enableItemPreview)
            {
                MessageBox.Show("You cannot load the same character for trading!");
            }
            else
            {
                EnableTradingControls(false);
            }
        }

        private void CreateBackup(string characterPath)
        {
            File.Copy(characterPath, characterPath + ".ItemTradingBackup", true);
        }

        private void CheckAndSetButtonStatus()
        {
            if (_characterUnit1 != null && _characterUnit2 != null && _characterUnit1 != _characterUnit2)
            {
                EnableTradingControls(true);
            }
            else
            {
                EnableTradingControls(false);
            }
        }

        protected override void b_save_Click(object sender, EventArgs e)
        {
            CreateBackup(_characterPath1);
            CreateBackup(_characterPath2);
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

        protected override void b_transfer_Click(object sender, EventArgs e)
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

        protected override void b_transferAll_Click(object sender, EventArgs e)
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

        protected override void b_delete_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryItem item = (InventoryItem)l_selectedItem.Tag;

                _eventSender.RemoveItem(item);

                if (_characterUnit1 != null && item != null)
                {
                    _characterUnit1.Items.Remove(item.Item);
                }
                if (_characterUnit2 != null && item != null)
                {
                    _characterUnit2.Items.Remove(item.Item);
                }

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

        protected override void b_undoTransfer_Click(object sender, EventArgs e)
        {
            b_loadCharacter1_Click(null, null);
            b_loadCharacter2_Click(null, null);

            EnableComboBoxes(true, true);
        }
    }
}
