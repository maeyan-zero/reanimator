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
    public partial class CharacterShop : Form
    {
        const InventoryTypes INVENTORYTYPE = InventoryTypes.Cube;

        string _characterFolder;

        ExcelTables _excelTables;

        string _characterPath1;
        Unit _characterUnit1;

        UnitHelpFunctions _itemHelpFunctions;

        //Unit _selectedItemCharacter1;
        ItemPanel _characterItemPanel1;
        CharacterStatus _characterStatus1;

        ItemPanel _eventSender;

        public CharacterShop()
        {
            InitializeComponent();
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
            _characterPath1 = _characterFolder + @"\" + cb_selectCharacter1.SelectedItem + ".hg1";

            _characterUnit1 = UnitHelpFunctions.OpenCharacterFile(ref _excelTables, _characterPath1);

            if (_characterUnit1.IsGood)
            {
                _itemHelpFunctions.LoadCharacterValues(_characterUnit1);

                gb_characterName1.Text = cb_selectCharacter1.SelectedItem.ToString();
                int level = UnitHelpFunctions.GetSimpleValue(_characterUnit1, ItemValueNames.level.ToString()) - 8;
                gb_characterName1.Text += " (Level " + level.ToString() + ")";

                //SetCharacterStatus(_characterStatus1, CharacterStatus.Loaded, p_status1, l_status1);

                //InitInventory(_characterUnit1, _characterItemPanel1);
            }
            else
            {
                MessageBox.Show("Error while parsing the character file!");
            }
        }
    }
}
