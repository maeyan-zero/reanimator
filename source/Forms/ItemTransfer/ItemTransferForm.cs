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
        string _characterFolder;

        ExcelTables _excelTables;
        UnitHelpFunctions _itemHelpFunctions;

        string _characterPath1;
        string _characterPath2;
        Unit _characterUnit1;
        Unit _characterUnit2;

        Unit _selectedItemCharacter1;
        ListBox _listBoxcharacter1;

        Unit _selectedItemCharacter2;
        ListBox _listBoxcharacter2;

        public ItemTransferForm(ref TableDataSet dataSet, ref ExcelTables excelTables)
        {
            InitializeComponent();

            _itemHelpFunctions = new UnitHelpFunctions(ref dataSet, ref excelTables);

            _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

            _excelTables = excelTables;

            _listBoxcharacter1 = lb_characterEquipment1;
            _listBoxcharacter2 = lb_characterEquipment2;

            string[] characters = LoadCharacterNames();

            cb_selectCharacter1.DataSource = characters;
            cb_selectCharacter2.DataSource = characters.Clone();
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
                gb_characterName1.Text = cb_selectCharacter1.SelectedItem.ToString();
                _characterUnit1 = UnitHelpFunctions.OpenCharacterFile(ref _excelTables, _characterPath1);
                _itemHelpFunctions.LoadCharacterValues(_characterUnit1);
                //_itemHelpFunctions.PopulateItems(ref _characterUnit1);

                InitInventory(_characterUnit1, lb_characterEquipment1, lb_characterInventory1, lb_characterStash1, lb_characterCube1);
            }
        }

        private void b_loadCharacter2_Click(object sender, EventArgs e)
        {
            _characterPath2 = _characterFolder + @"\" + cb_selectCharacter2.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                gb_characterName2.Text = cb_selectCharacter2.SelectedItem.ToString();
                _characterUnit2 = UnitHelpFunctions.OpenCharacterFile(ref _excelTables, _characterPath2);
                _itemHelpFunctions.LoadCharacterValues(_characterUnit2);
                //_itemHelpFunctions.PopulateItems(ref _characterUnit2);

                InitInventory(_characterUnit2, lb_characterEquipment2, lb_characterInventory2, lb_characterStash2, lb_characterCube2);
            }
        }

        private void InitInventory(Unit unit, ListBox equipped, ListBox inventory, ListBox stash, ListBox cube)
        {
            try
            {
                foreach (Unit item in unit.Items)
                {
                    if (item.inventoryType == (int)InventoryTypes.Inventory)
                    {
                        inventory.Items.Add(item);
                    }
                    else if (item.inventoryType == (int)InventoryTypes.Stash)
                    {
                        stash.Items.Add(item);
                    }
                    else if (item.inventoryType == (int)InventoryTypes.Cube)
                    {
                        cube.Items.Add(item);
                    }
                    else if (item.inventoryType == (int)InventoryTypes.QuestRewards)
                    {
                        //do nothing, as trading quest rewards would be meaningless
                    }
                    else
                    {
                        equipped.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitInventory");
            }
        }

        private void b_transferToRight_Click(object sender, EventArgs e)
        {
            if (_selectedItemCharacter1 != null)
            {
                _listBoxcharacter2.Items.Add(_selectedItemCharacter1);
                _listBoxcharacter1.Items.Remove(_selectedItemCharacter1);

                //modify item location
                //modify item position
                //modify item ids
            }
        }

        private void b_transferToLeft_Click(object sender, EventArgs e)
        {
            if (_selectedItemCharacter2 != null)
            {
                _listBoxcharacter1.Items.Add(_selectedItemCharacter2);
                _listBoxcharacter2.Items.Remove(_selectedItemCharacter2);
            }
        }

        private void b_delete_Click(object sender, EventArgs e)
        {
            //needed?
        }

        private void lb_character1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedItemCharacter1 = (Unit)_listBoxcharacter1.SelectedItem;
        }

        private void lb_character2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedItemCharacter2 = (Unit)_listBoxcharacter2.SelectedItem;
        }

        private void tc_character1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listBoxcharacter1 = (ListBox)tc_character1.SelectedTab.Controls[0];
        }

        private void tc_character2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listBoxcharacter2 = (ListBox)tc_character2.SelectedTab.Controls[0];
        }
    }
}
