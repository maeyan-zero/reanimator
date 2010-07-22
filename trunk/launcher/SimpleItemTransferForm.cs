using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Reanimator.Forms.ItemTransfer;
using Reanimator;
using Reanimator.Forms;
using System.IO;
using StringsFile = Reanimator.StringsFile;

namespace launcher.Revival
{
    public partial class SimpleItemTransferForm : BasicItemTransferForm
    {
        private TableDataSet _tableDataset;
        private UnitHelpFunctionsSimple _itemHelpFunctions;

        /// <summary>
        /// Use this constructor when starting the item transfer window from within the launcher (no additional item infos)
        /// </summary>
        public SimpleItemTransferForm()
        {
            const string fileName = "strings_items";
            StringsFile itemNameLookupFile = LoadStringFile(fileName);

            _tableDataset = new TableDataSet();
            _tableDataset.LoadTable(null, itemNameLookupFile);

            _itemHelpFunctions = new UnitHelpFunctionsSimple(_tableDataset);
        }

        private StringsFile LoadStringFile(string fileName)
        {
            MessageBox.Show("Warning - FIXME\n\nProbably doesn't work\n\nprivate StringsFile LoadStringFile(string fileName)\nin SimpleItemTransferForm.cs");

            String baseDataDir = Config.DataDirsRoot + @"\data\excel\strings\english\";
            const string fileExtension = ".xls.uni.cooked";

            String path = baseDataDir + fileName + fileExtension;

            if (!File.Exists(path))
            {
                path = path.Replace("data", "data_common");
                if (!File.Exists(path))
                {
                    return null;
                }
            }

            try
            {
                byte[] data = File.ReadAllBytes(path);

                StringsFile stringsFile = new StringsFile(fileName, typeof(StringsFile))
                {
                    FilePath = path
                };
                stringsFile.ParseData(data);

                return !stringsFile.IsGood ? null : stringsFile;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LoadStringFile");
                return null;
            }
        }

        protected override void b_loadCharacter1_Click(object sender, EventArgs e)
        {
            _characterPath1 = _characterFolder + @"\" + cb_selectCharacter1.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                _characterUnit1 = UnitHelpFunctions.OpenCharacterFile(_characterPath1);

                if (_characterUnit1 != null && _characterUnit1.IsGood)
                {
                    _itemHelpFunctions.LoadSimpleCharacterValues(_characterUnit1);

                    gb_characterName1.Text = cb_selectCharacter1.SelectedItem.ToString();
                    int level = UnitHelpFunctions.GetSimpleValue(_characterUnit1, (int)ItemValueNames.level) - 8;
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

        protected override void b_loadCharacter2_Click(object sender, EventArgs e)
        {
            _characterPath2 = _characterFolder + @"\" + cb_selectCharacter2.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                _characterUnit2 = UnitHelpFunctions.OpenCharacterFile(_characterPath2);

                if (_characterUnit2 != null && _characterUnit2.IsGood)
                {
                    _itemHelpFunctions.LoadSimpleCharacterValues(_characterUnit2);

                    gb_characterName2.Text = cb_selectCharacter2.SelectedItem.ToString();
                    int level = UnitHelpFunctions.GetSimpleValue(_characterUnit2, (int)ItemValueNames.level) - 8;
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

        protected override void b_save_Click(object sender, EventArgs e)
        {
            if (_characterUnit1 != null && _characterUnit2 != null)
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
