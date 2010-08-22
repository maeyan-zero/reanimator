using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace Reanimator.Forms.ItemTransfer
{
    public abstract partial class BasicItemTransferForm : Form
    {
        protected bool _enableItemPreview = false;
        protected bool _enablePalladiumTrading = false;
        protected bool _backupCharacters = false;
        protected bool _displayItemIcons = false;

        protected InventoryTypes INVENTORYTYPE = InventoryTypes.Cube;
        protected const int INVENTORYWIDTH = 6;
        protected int INVENTORYHEIGHT = 6;
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
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "trading.xml")))
            {
                ItemTradingOptions options = XmlUtilities<ItemTradingOptions>.Deserialize("trading.xml");
                INVENTORYTYPE = options.InventoryToUse;
                INVENTORYHEIGHT = options.InventoryHeight;
                _enableItemPreview = options.EnableItemPreview;
                _enablePalladiumTrading = options.EnablePalladiumTrading;
                _backupCharacters = options.BackupCharacters;
                _displayItemIcons = options.DisplayItemIcons;
            }
            //ItemTradingOptions options2 = new ItemTradingOptions();
            //options2.EnableItemPreview = true;
            //options2.EnablePalladiumTrading = true;
            //options2.BackupCharacters = true;
            //options2.InventoryHeight = 6;
            //options2.InventoryToUse = InventoryTypes.Inventory;
            //XmlUtilities<ItemTradingOptions>.Serialize(options2, "trading2.xml");

            InitializeComponent();

            this.Text += " - Location: " + INVENTORYTYPE.ToString();

            _characterFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games\\Hellgate\\Save\\Singleplayer");

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

            //moved to complex form
            //string[] characters = LoadCharacterNames();
            //cb_selectCharacter1.DataSource = characters;
            //cb_selectCharacter2.DataSource = characters.Clone();

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
                label.Text = "Error loading the character";
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
            else if (newCharacterStatus == CharacterStatus.AlreadyLoaded)
            {
                panel.BackColor = Color.Red;
                label.Text = "Character already loaded";
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

        protected void EnableTradingControls(bool enable)
        {
            b_transfer.Enabled = enable;
            b_transferAll.Enabled = enable;
            b_delete.Enabled = enable;
            b_save.Enabled = enable;
            b_undoTransfer.Enabled = enable;

            if (_enablePalladiumTrading)
            {
                EnablePalladiumControls(enable);
            }
        }

        protected void EnablePalladiumControls(bool enable)
        {
            nud_palladium.Enabled = enable;
            b_tradeFrom1To2.Enabled = enable;
            b_tradeFrom2To1.Enabled = enable;
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

        /// <summary>
        /// Displays a warning message when starting up Reanimator
        /// </summary>
        /// <param name="caption">The caption of the warning window. If set to null or string.Empty, a default caption will be dispalyed</param>
        /// <param name="message">The message of the warning window. If set to null or string.Empty, a default message will be dispalyed</param>
        public void DisplayWarningMessage(string caption, string message)
        {
            string captionText = "Warning!";
            string messageText = "Using the Item Trading function may corrupt your character savegames!" + Environment.NewLine +
                                 "Make sure to unequip all weapons and shields from the three weapon slots (F1 - F3) before continuing and create backups of all characters you're going to trade with!" + Environment.NewLine + Environment.NewLine +
                                 "If you notice some strange behavior when starting the game with a character you traded with (e.g. weapon slots not displaying the weapons you place in them) IMMEDIATELY quit the game and DO NOT load any other characters!";

            if (caption != null && caption != string.Empty)
            {
                captionText = caption;
            }

            if (caption != null && caption != string.Empty)
            {
                captionText = caption;
            }

            MessageBox.Show(messageText, captionText, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void cb_selectCharacter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_enableItemPreview)
            {
                b_loadCharacter1_Click(sender, e);
            }
        }

        private void cb_selectCharacter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_enableItemPreview)
            {
                b_loadCharacter2_Click(sender, e);
            }
        }
    }

        public enum CharacterStatus
    {
        NotLoaded,
        Loaded,
        Modified,
        Saved,
        Error,
        AlreadyLoaded
    }

    [XmlRoot("ItemTradingOptions")]
    public class ItemTradingOptions
    {
        bool _enableItemPreview;
        bool _enablePalladiumTrading;
        InventoryTypes _inventoryToUse;
        int _inventoryHeight;
        bool _backupCharacters;
        bool _displayItemIcons;

        [XmlElement("EnableItemPreview")]
        public bool EnableItemPreview
        {
            get { return _enableItemPreview; }
            set { _enableItemPreview = value; }
        }

        [XmlElement("EnablePalladiumTrading")]
        public bool EnablePalladiumTrading
        {
            get { return _enablePalladiumTrading; }
            set { _enablePalladiumTrading = value; }
        }

        [XmlElement("InventoryToUse")]
        public InventoryTypes InventoryToUse
        {
            get { return _inventoryToUse; }
            set { _inventoryToUse = value; }
        }

        [XmlElement("InventoryHeight")]
        public int InventoryHeight
        {
            get { return _inventoryHeight; }
            set { _inventoryHeight = value; }
        }

        [XmlElement("BackupCharacters")]
        public bool BackupCharacters
        {
          get { return _backupCharacters; }
          set { _backupCharacters = value; }
        }

        [XmlElement("DisplayItemIcons")]
        public bool DisplayItemIcons
        {
            get { return _displayItemIcons; }
            set { _displayItemIcons = value; }
        }
    }
}
