using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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

    public abstract partial class BasicItemTransferForm : Form
    {
        protected const InventoryTypes INVENTORYTYPE = InventoryTypes.Cube;
        protected const int INVENTORYWIDTH = 6;
        protected const int INVENTORYHEIGHT = 6;
        protected int ITEMUNITSIZE = 40;

        protected string _characterFolder;

        protected string _characterPath1;
        protected string _characterPath2;
        protected Unit _characterUnit1;
        protected Unit _characterUnit2;

        //Unit _selectedItemCharacter1;
        protected ItemPanel _characterItemPanel1;
        protected CharacterStatus _characterStatus1;

        //Unit _selectedItemCharacter2;
        protected ItemPanel _characterItemPanel2;
        protected CharacterStatus _characterStatus2;

        protected ItemPanel _eventSender;

        public BasicItemTransferForm()
        {
            InitializeComponent();

            this.Text += " - Location: " + INVENTORYTYPE.ToString();

            _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";

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

        protected abstract void b_loadCharacter1_Click(object sender, EventArgs e);

        protected abstract void b_loadCharacter2_Click(object sender, EventArgs e);

        protected abstract void b_transfer_Click(object sender, EventArgs e);

        protected abstract void b_transferAll_Click(object sender, EventArgs e);

        protected abstract void b_delete_Click(object sender, EventArgs e);

        protected abstract void b_undoTransfer_Click(object sender, EventArgs e);

        protected abstract void b_save_Click(object sender, EventArgs e);

        protected void SetCharacterStatus(CharacterStatus originalCharacterStatus, CharacterStatus newCharacterStatus, Panel panel, Label label)
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

        protected void _characterItemPanel_NewItemSelected_Event(ItemPanel sender, InventoryItem item)
        {
            _eventSender = sender;

            l_selectedItem.Text = item.Item.Name;

            if (item.Quantity > 1)
            {
                l_selectedItem.Text += " (x" + item.Quantity.ToString() + ")";
            }

            l_selectedItem.Tag = item;
        }

        protected string[] LoadCharacterNames()
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

        protected void InitInventory(Unit unit, ItemPanel itemPanel)
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

        protected void EmergencyAbort()
        {
            SetCharacterStatus(_characterStatus1, CharacterStatus.Error, p_status1, l_status1);
            SetCharacterStatus(_characterStatus2, CharacterStatus.Error, p_status2, l_status2);
            MessageBox.Show("The trade window will now close to ensure that your savegames and items will not be corrupted!", "Error while transfering your items!");
            this.Close();
        }

        protected void EnableComboBoxes(bool enable1, bool enable2)
        {
            EnableComboBox1(enable1);
            EnableComboBox2(enable2);
        }

        protected void EnableComboBox1(bool enable)
        {
            cb_selectCharacter1.Enabled = enable;
            b_loadCharacter1.Enabled = enable;
        }

        protected void EnableComboBox2(bool enable)
        {
            cb_selectCharacter2.Enabled = enable;
            b_loadCharacter2.Enabled = enable;
        }

        protected void EnableButtons(bool enable)
        {
            b_transfer.Enabled = enable;
            b_transferAll.Enabled = enable;
            b_delete.Enabled = enable;
            b_save.Enabled = enable;
            b_undoTransfer.Enabled = enable;
        }

        private void b_tradeFrom2To1_Click(object sender, EventArgs e)
        {
            AddEntryIfNotPresent(_characterUnit2, ItemValueNames.gold);
        }

        private void AddEntryIfNotPresent(Unit unit, ItemValueNames itemValueNames)
        {
            if (UnitHelpFunctions.GetSimpleValue(unit, itemValueNames.ToString()) == 0)
            {
                List<Unit.StatBlock.Stat> stat = new List<Unit.StatBlock.Stat>();
                stat.AddRange(unit.statBlock.stats);
            }
        }
    }
}
