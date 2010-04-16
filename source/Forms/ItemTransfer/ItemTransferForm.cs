using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reanimator.Excel;

namespace Reanimator.Forms.ItemTransfer
{
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

        //Unit _selectedItemCharacter2;
        ItemPanel _characterItemPanel2;

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

            // use inventory panels, as a normal groupBox doesn't provide the option "AutoScroll"
            p_inventory1.Controls.Add(_characterItemPanel1);
            p_inventory2.Controls.Add(_characterItemPanel2);
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

                if (_characterUnit1.IsGood)
                {
                    _itemHelpFunctions.LoadCharacterValues(_characterUnit1);

                    gb_characterName1.Text = cb_selectCharacter1.SelectedItem.ToString();
                    int level = UnitHelpFunctions.GetSimpleValue(_characterUnit1, ItemValueNames.level.ToString()) - 8;
                    gb_characterName1.Text += " (Level " + level.ToString() + ")";

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

                if (_characterUnit2.IsGood)
                {
                    _itemHelpFunctions.LoadCharacterValues(_characterUnit2);

                    gb_characterName2.Text = cb_selectCharacter2.SelectedItem.ToString();
                    int level = UnitHelpFunctions.GetSimpleValue(_characterUnit2, ItemValueNames.level.ToString()) - 8;
                    gb_characterName2.Text += " (Level " + level.ToString() + ")";

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
                MessageBox.Show(ex.Message, "InitInventory");
            }
        }

        private void b_save_Click(object sender, EventArgs e)
        {
            if(_characterUnit1 != null && _characterUnit2 != null)
            {
                UnitHelpFunctions.SaveCharacterFile(_characterUnit1, _characterPath1);
                UnitHelpFunctions.SaveCharacterFile(_characterUnit2, _characterPath2);

                MessageBox.Show("Saving successful!");
            }
        }

        private void b_transfer_Click(object sender, EventArgs e)
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
                }
            }
            else
            {
                MessageBox.Show("You have to load two characters to transfere items!");
            }
        }

        private void b_transferAll_Click(object sender, EventArgs e)
        {
            //ItemPanel buffer = new ItemPanel();

            //for (int counter = 0; counter < _characterItemPanel1.Controls.Count; counter++)
            //{
            //    buffer.Controls.Add(_characterItemPanel1.Controls[counter]);
            //    _characterItemPanel1.Controls.RemoveAt(counter);
            //}

            //for (int counter = 0; counter < _characterItemPanel2.Controls.Count; counter++)
            //{
            //    _characterItemPanel1.Controls.Add(_characterItemPanel2.Controls[counter]);
            //    _characterItemPanel2.Controls.RemoveAt(counter);
            //}

            //for (int counter = 0; counter < buffer.Controls.Count; counter++)
            //{
            //    _characterItemPanel2.Controls.Add(buffer.Controls[counter]);
            //    //buffer.Controls.RemoveAt(counter);
            //}
        }

        private void b_delete_Click(object sender, EventArgs e)
        {
            InventoryItem item = (InventoryItem)l_selectedItem.Tag;

            _eventSender.RemoveItem(item);

            l_selectedItem.ResetText();
            l_selectedItem.Tag = null;
        }
    }
}
