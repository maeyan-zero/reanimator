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
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms.ItemTransfer
{
    public partial class ItemTransferForm : Form
    {
        TableDataSet _dataSet;
        UnitHelpFunctions _itemHelpFunctions;
        DataTable _items;
        bool _isMale = false;
        PreviewManager _previewManager;

        string _characterPath1;
        string _characterPath2;

        //enables some additional functions when debugging
        bool _debug = false;
#if DEBUG
        //_debug = true;
#endif
        //specifies wether a character is directly loaded as soon as he gets selected via the combobox
        bool _enableItemPreview = true;
        //specifies wether palladium trading is enabled
        //bool _enablePalladiumTrading = true;
        //specifies wether a characterbackup should be created before saving  a character
        bool _backupCharacters = true;
        //specifies wether item icons are loaded to display the item
        bool _displayItemIcons = true;
        //specifies wether the item name and quantity is displayed
        bool _displayNamesAndQuantity = true;

        InventoryTypes INVENTORYTYPE = InventoryTypes.Cube;
        const int INVENTORYWIDTH = 6;
        int INVENTORYHEIGHT = 6;
        int ITEMUNITSIZE = 40;

        string _characterFolder;

        UnitWrapper _characterUnit1;
        UnitWrapper _characterUnit2;

        //Unit _selectedItemCharacter1;
        ItemPanel _characterItemPanel1;
        CharacterStatus _characterStatus1;

        //Unit _selectedItemCharacter2;
        ItemPanel _characterItemPanel2;
        CharacterStatus _characterStatus2;

        ItemPanel _eventSender;

        public ItemTransferForm(TableDataSet dataSet)
        {
            InitializeComponent();

            _dataSet = dataSet;
            _itemHelpFunctions = new UnitHelpFunctions(_dataSet);

            //preload the tables
            _items = _dataSet.GetExcelTableFromStringId("ITEMS");
            _dataSet.GetExcelTableFromStringId("AFFIXES");

            _previewManager = new PreviewManager();

            //if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "trading.xml")))
            //{
            //    ItemTradingOptions options = XmlUtilities<ItemTradingOptions>.Deserialize("trading.xml");
            //    INVENTORYTYPE = options.InventoryToUse;
            //    INVENTORYHEIGHT = options.InventoryHeight;
            //    _enableItemPreview = options.EnableItemPreview;
            //    _enablePalladiumTrading = options.EnablePalladiumTrading;
            //    _backupCharacters = options.BackupCharacters;
            //    _displayItemIcons = options.DisplayItemIcons;
            //    _displayNamesAndQuantity = options.DisplayNamesAndQuantity;
            //    _debug = options.Debug;
            //}

            //ItemTradingOptions options2 = new ItemTradingOptions();
            //options2.EnableItemPreview = true;
            //options2.EnablePalladiumTrading = true;
            //options2.BackupCharacters = true;
            //options2.InventoryHeight = 6;
            //options2.InventoryToUse = InventoryTypes.Inventory;
            //XmlUtilities<ItemTradingOptions>.Serialize(options2, "trading2.xml");

            this.Text += " - Location: " + INVENTORYTYPE.ToString();

            _characterItemPanel1 = new ItemPanel(_displayItemIcons, _previewManager);
            _characterItemPanel2 = new ItemPanel(_displayItemIcons, _previewManager);

            _characterItemPanel1.NewItemSelected_Event += new ItemPanel.NewItemSelected(_characterItemPanel_NewItemSelected_Event);
            _characterItemPanel2.NewItemSelected_Event += new ItemPanel.NewItemSelected(_characterItemPanel_NewItemSelected_Event);

            _characterItemPanel1.ItemUnitSize = ITEMUNITSIZE;
            _characterItemPanel2.ItemUnitSize = ITEMUNITSIZE;

            SetPanelSize();

            _characterItemPanel1.Location = new Point(16, 18);
            _characterItemPanel2.Location = new Point(16, 18);

            SetCharacterStatus(_characterStatus1, CharacterStatus.NotLoaded, p_status1, l_status1);
            SetCharacterStatus(_characterStatus2, CharacterStatus.NotLoaded, p_status2, l_status2);

            // use inventory panels, as a normal groupBox doesn't provide the option "AutoScroll"
            p_inventory1.Controls.Add(_characterItemPanel1);
            p_inventory2.Controls.Add(_characterItemPanel2);

            EnableComboBoxes(true, true);

            if (_debug)
            {
                InitDebugControls();
            }
            ts_debugControl.Enabled = _debug;
            ts_debugControl.Visible = _debug;
            cb_isMale.Visible = _debug;

            EnableTradingControls(false);

            _characterFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games\\Hellgate\\Save\\Singleplayer");

            //adding the character names triggers the cb_selectCharacter1_SelectedIndexChanged event which will load the first character
            string[] characters = LoadCharacterNames();
            cb_selectCharacter1.DataSource = characters;
            cb_selectCharacter2.DataSource = characters.Clone();
        }

        private void SetPanelSize()
        {
            _characterItemPanel1.Size = new Size(INVENTORYWIDTH * ITEMUNITSIZE, INVENTORYHEIGHT * ITEMUNITSIZE);
            _characterItemPanel2.Size = new Size(INVENTORYWIDTH * ITEMUNITSIZE, INVENTORYHEIGHT * ITEMUNITSIZE);
        }

        private void SetCharacterStatus(CharacterStatus originalCharacterStatus, CharacterStatus newCharacterStatus, Panel panel, Label label)
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

        private void _characterItemPanel_NewItemSelected_Event(ItemPanel sender, InventoryItem item)
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

            for (int counter = 0; counter < characters.Length; counter++)
            {
                string charName = characters[counter].Replace(_characterFolder + @"\", string.Empty);
                charName = charName.Replace(".hg1", string.Empty);

                characters[counter] = charName;
            }

            return characters;
        }

        private void EmergencyAbort()
        {
            SetCharacterStatus(_characterStatus1, CharacterStatus.Error, p_status1, l_status1);
            SetCharacterStatus(_characterStatus2, CharacterStatus.Error, p_status2, l_status2);
            MessageBox.Show("The trade window will now close to ensure that your savegames and items will not be corrupted!", "Error while transfering your items!");
            this.Close();
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

        private void EnableTradingControls(bool enable)
        {
            b_transfer.Enabled = enable;
            b_transferAll.Enabled = enable;
            b_delete.Enabled = enable;
            b_save.Enabled = enable;
            b_undoTransfer.Enabled = enable;

            EnablePalladiumControls(enable);
        }

        private void EnablePalladiumControls(bool enable)
        {
            nud_palladium.Enabled = enable;
            b_tradeFrom1To2.Enabled = enable;
            b_tradeFrom2To1.Enabled = enable;
        }

        private void b_tradeFrom1To2_Click(object sender, EventArgs e)
        {
            TradePalladium(_characterUnit1, _characterUnit2);
        }

        private void b_tradeFrom2To1_Click(object sender, EventArgs e)
        {
            TradePalladium(_characterUnit2, _characterUnit1);
        }

        private void TradePalladium(UnitWrapper sender, UnitWrapper receiver)
        {
            if (CharactersLoaded())
            {
                int palladiumToTrade = (int)nud_palladium.Value;

                //check if the sending character has enough palladium to trade
                if (palladiumToTrade > sender.Values.Palladium)
                {
                    palladiumToTrade = sender.Values.Palladium;
                }
                //calculate the maximum amount of palladium the receiving character can store
                int maximumTradableAmount = receiver.Values.MaxPalladium - receiver.Values.Palladium;

                //calculate the real amount of palladium for trading
                if (maximumTradableAmount < palladiumToTrade)
                {
                    palladiumToTrade = maximumTradableAmount;
                }

                receiver.Values.Palladium += palladiumToTrade;
                sender.Values.Palladium -= palladiumToTrade;

                l_palladium1.Text = _characterUnit1.Values.Palladium.ToString();
                l_palladium2.Text = _characterUnit2.Values.Palladium.ToString();

                RequiresUserVerification();
            }
        }

        private bool CharactersLoaded()
        {
            return (_characterUnit1 != null && _characterUnit2 != null);
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

        public bool IsMale
        {
            get { return _isMale; }
            set { _isMale = value; }
        }

        private void InitDebugControls()
        {
            tscb_area.Text = INVENTORYTYPE.ToString();
            if (_isMale)
            {
                tscb_gender.Text = "male";
            }
            else
            {
                tscb_gender.Text = "female";
            }
            tstb_height.Text = INVENTORYHEIGHT.ToString();
            tscb_displayNames.Text = _displayNamesAndQuantity.ToString();
            tscb_itemIcons.Text = _displayItemIcons.ToString();
        }

        private void b_loadCharacter1_Click(object sender, EventArgs e)
        {
            l_selectedItem.ResetText();
            l_selectedItem.Tag = null;
            _characterPath1 = _characterFolder + @"\" + cb_selectCharacter1.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                Unit character = UnitHelpFunctions.OpenCharacterFile(_dataSet, _characterPath1);

                if (character != null && character.IsGood)
                {
                    _itemHelpFunctions.LoadCharacterValues(character);
                    _characterUnit1 = new UnitWrapper(_dataSet, character);

                    //if (WeaponSlotsPopulated(_characterUnit1))
                    //{
                    //    MessageBox.Show("There still seem to be weapons left in your weapon slots." + Environment.NewLine +
                    //    "Please unequipp all weapons before trading items or continue at your own risk!", "Warning!");
                    //}

                    gb_characterName1.Text = cb_selectCharacter1.SelectedItem.ToString();
                    int level = _characterUnit1.Values.Level;
                    gb_characterName1.Text += " (Level " + level.ToString() + " " + _characterUnit1.ClassName + ")";
                    l_palladium1.Text = _characterUnit1.Values.Palladium.ToString();

                    SetCharacterStatus(_characterStatus1, CharacterStatus.Loaded, p_status1, l_status1);

                    InitInventory(_characterUnit1, _characterItemPanel1);
                }
                else
                {
                    _characterPath1 = null;
                    _characterUnit1 = null;
                    _characterItemPanel1.Controls.Clear();
                    l_palladium1.Text = "-";
                    SetCharacterStatus(_characterStatus1, CharacterStatus.Error, p_status1, l_status1);
                    //MessageBox.Show("Error while parsing the character file!");
                }
            }
            else
            {
                _characterPath1 = null;
                _characterUnit1 = null;
                _characterItemPanel1.Controls.Clear();
                l_palladium1.Text = "-";
                SetCharacterStatus(_characterStatus1, CharacterStatus.AlreadyLoaded, p_status1, l_status1);
                SameCharacterSelected();
            }

            CheckAndSetButtonStatus();
        }

        private void InitInventory(UnitWrapper unit, ItemPanel itemPanel)
        {
            itemPanel.Controls.Clear();

            try
            {
                foreach (Unit item in unit.Items.Items)
                {
                    if (item.inventoryType == (int)INVENTORYTYPE)
                    {
//Unit.StatBlock.Stat atat = item.Stats.GetStatById((int)ItemValueNames.applied_affix);

                        DataRow[] itemsRows = _items.Select(String.Format("code = '{0}'", item.unitCode));
                        if (itemsRows.Length == 0)
                        {

                        }
                        int unitType = (int)itemsRows[0]["unitType"];
                        string folder = (string)itemsRows[0]["folder"] + @"\icons";
                        string name = (string)itemsRows[0]["name"];
                        //string unitType = (string)itemsRows[0]["unitType_string"];

                        string itemPath = Path.Combine(folder, name);

                        if(itemPath.StartsWith("armor"))
                        {
                            if (_isMale)
                            {
                                itemPath += "_m";
                            }
                            else
                            {
                                itemPath += "_f";
                            }
                        }

                        InventoryItem iItem = new InventoryItem(item, itemPath);
                        iItem.InitButton(_displayNamesAndQuantity);
                        itemPanel.AddItem(iItem, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "InitInventory: " + unit.Name);
            }
        }

        private bool WeaponSlotsPopulated(UnitWrapper unit)
        {
            if (unit.Items.Items.Find(tmp => tmp.inventoryType == (int)InventoryTypes.CurrentWeaponSet) == null)
            {
                return false;
            }
            return true;
        }

        private void b_loadCharacter2_Click(object sender, EventArgs e)
        {
            l_selectedItem.ResetText();
            l_selectedItem.Tag = null;
            _characterPath2 = _characterFolder + @"\" + cb_selectCharacter2.SelectedItem + ".hg1";

            if (_characterPath1 != _characterPath2)
            {
                Unit character = UnitHelpFunctions.OpenCharacterFile(_dataSet, _characterPath2);

//XmlUtilities<Unit>.Serialize(character, @"F:\before.xml");

                if (character != null && character.IsGood)
                {
                    _itemHelpFunctions.LoadCharacterValues(character);
                    _characterUnit2 = new UnitWrapper(_dataSet, character);

                    //if(WeaponSlotsPopulated(_characterUnit2))
                    //{
                    //    MessageBox.Show("There still seem to be weapons left in your weapon slots." + Environment.NewLine +
                    //    "Please unequipp all weapons before trading items or continue at your own risk!", "Warning!");
                    //}

                    gb_characterName2.Text = cb_selectCharacter2.SelectedItem.ToString();
                    int level = _characterUnit2.Values.Level;

                    gb_characterName2.Text += " (Level " + level.ToString() + " " + _characterUnit2.ClassName + ")";
                    l_palladium2.Text = _characterUnit2.Values.Palladium.ToString();

                    SetCharacterStatus(_characterStatus2, CharacterStatus.Loaded, p_status2, l_status2);

                    InitInventory(_characterUnit2, _characterItemPanel2);
                }
                else
                {
                    _characterPath2 = null;
                    _characterUnit2 = null;
                    _characterItemPanel2.Controls.Clear();
                    l_palladium2.Text = "-";
                    SetCharacterStatus(_characterStatus2, CharacterStatus.Error, p_status2, l_status2);
                    //MessageBox.Show("Error while parsing the character file!");
                }
            }
            else
            {
                _characterPath2 = null;
                _characterUnit2 = null;
                _characterItemPanel2.Controls.Clear();
                l_palladium2.Text = "-";
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

        private void b_save_Click(object sender, EventArgs e)
        {
            if (_backupCharacters)
            {
                CreateBackup(_characterPath1);
                CreateBackup(_characterPath2);
            }
            if(_characterUnit1 != null && _characterUnit2 != null)
            {
                if (MessageBox.Show("Are you sure you want to save these changes?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
//XmlUtilities<Unit>.Serialize(_characterUnit2.HeroUnit, @"F:\after.xml");
                    UnitHelpFunctions.SaveCharacterFile(_characterUnit1.HeroUnit, _characterPath1);
                    UnitHelpFunctions.SaveCharacterFile(_characterUnit2.HeroUnit, _characterPath2);

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
                                _characterUnit2.Items.AddItem(item.Item);
                                _characterUnit1.Items.RemoveItem(item.Item);
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
                                _characterUnit1.Items.AddItem(item.Item);
                                _characterUnit2.Items.RemoveItem(item.Item);
                                _characterItemPanel2.RemoveItem(item);
                            }
                            else
                            {
                                MessageBox.Show("There is not enough free space!");
                            }
                            l_selectedItem.ResetText();
                            l_selectedItem.Tag = null;
                        }

                        RequiresUserVerification();
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

        private void RequiresUserVerification()
        {
            EnableComboBoxes(false, false);
            SetCharacterStatus(_characterStatus1, CharacterStatus.Modified, p_status1, l_status1);
            SetCharacterStatus(_characterStatus2, CharacterStatus.Modified, p_status2, l_status2);
        }

        private void b_transferAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (_characterUnit1 != null && _characterUnit2 != null)
                {
                    l_selectedItem.ResetText();
                    l_selectedItem.Tag = null;

                    List<Unit> tmpItem = new List<Unit>();


                    for (int counter = 0; counter < _characterUnit1.Items.Items.Count; counter++)
                    {
                        if (_characterUnit1.Items.Items[counter].inventoryType == (int)INVENTORYTYPE)
                        {
                            tmpItem.Add(_characterUnit1.Items.Items[counter]);
                            _characterUnit1.Items.Items.RemoveAt(counter);

                            counter--;
                        }
                    }

                    for (int counter = 0; counter < _characterUnit2.Items.Items.Count; counter++)
                    {
                        if (_characterUnit2.Items.Items[counter].inventoryType == (int)INVENTORYTYPE)
                        {
                            _characterUnit1.Items.Items.Add(_characterUnit2.Items.Items[counter]);
                            _characterUnit2.Items.Items.RemoveAt(counter);

                            counter--;
                        }
                    }

                    _characterUnit2.Items.Items.AddRange(tmpItem.ToArray());

                    InitInventory(_characterUnit1, _characterItemPanel1);
                    InitInventory(_characterUnit2, _characterItemPanel2);

                    RequiresUserVerification();
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

                if (_characterUnit1 != null && item != null)
                {
                    _characterUnit1.Items.Items.Remove(item.Item);
                }
                if (_characterUnit2 != null && item != null)
                {
                    _characterUnit2.Items.Items.Remove(item.Item);
                }

                l_selectedItem.ResetText();
                l_selectedItem.Tag = null;

                RequiresUserVerification();
            }
            catch (Exception)
            {
                EmergencyAbort();
            }
        }

        private void b_undoTransfer_Click(object sender, EventArgs e)
        {
            b_loadCharacter1_Click(null, null);
            b_loadCharacter2_Click(null, null);

            EnableComboBoxes(true, true);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    _characterItemPanel1.Dispose();
        //    _characterItemPanel2.Dispose();

        //    base.Dispose(disposing);
        //}

        private void useMaleArmor_CheckedChanged(object sender, EventArgs e)
        {
            _isMale = cb_isMale.Checked;
        }
        
        private void tscb_area_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tscb_area.SelectedItem.ToString() == "Cube")
            {
                INVENTORYTYPE = InventoryTypes.Cube;
            }
            else if (tscb_area.SelectedItem.ToString() == "Inventory")
            {
                INVENTORYTYPE = InventoryTypes.Inventory;
            }
            else if (tscb_area.SelectedItem.ToString() == "Stash")
            {
                INVENTORYTYPE = InventoryTypes.Stash;
            }
        }

        private void tstb_height_TextChanged(object sender, EventArgs e)
        {
            int value;
            bool valid = int.TryParse(tstb_height.Text, out value);

            if (valid)
            {
                INVENTORYHEIGHT = value;
                SetPanelSize();
            }
        }

        private void tscb_gender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tscb_gender.SelectedItem.ToString() == "male")
            {
                _isMale = true;
            }
            else
            {
                _isMale = false;
            }
        }

        private void tscb_displayNames_Click(object sender, EventArgs e)
        {
            if (tscb_displayNames.SelectedItem.ToString() == "true")
            {
                _displayNamesAndQuantity = true;
            }
            else
            {
                _displayNamesAndQuantity = false;
            }
        }

        private void tscb_itemIcons_Click(object sender, EventArgs e)
        {
            if (tscb_itemIcons.SelectedItem.ToString() == "true")
            {
                _displayItemIcons = true;
            }
            else
            {
                _displayItemIcons = false;
            }
        }

        public void Dispose()
        {
            _previewManager.Dispose();
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
        bool _displayNamesAndQuantity;
        bool _debug;

        [XmlElement("Debug")]
        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }

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

        [XmlElement("DisplayNamesAndQuantity")]
        public bool DisplayNamesAndQuantity
        {
            get { return _displayNamesAndQuantity; }
            set { _displayNamesAndQuantity = value; }
        }
    }
}
