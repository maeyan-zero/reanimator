using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Reanimator.Forms.HeroEditorFunctions;
using Config = Revival.Common.Config;
using ExceptionLogger = Revival.Common.ExceptionLogger;
using Hellgate;

namespace Reanimator.Forms.ItemTransfer
{
    public partial class ItemPanel : UserControl
    {
        public delegate void NewItemSelected(ItemPanel sender, InventoryItem item);
        public event NewItemSelected NewItemSelected_Event;

        FileManager fileManager;
        PreviewManager _manager;
        bool _displayItemIcons;
        int _itemUnitSize = 40;
        bool _isMale;

        public bool IsMale
        {
            get { return _isMale; }
            set { _isMale = value; }
        }

        public int ItemUnitSize
        {
            get { return _itemUnitSize; }
            set { _itemUnitSize = value; }
        }

        public ItemPanel(bool displayItemIcons, PreviewManager iconPreview, FileManager fileManager)
        {
            InitializeComponent();
            _displayItemIcons = displayItemIcons;
            _manager = iconPreview;
            this.fileManager = fileManager;
        }

        private void RegisterItemEvents(InventoryItem item)
        {
            item.Click += new EventHandler(item_Click);
        }

        private void UnregisterItemEvents(InventoryItem item)
        {
            item.Click -= new EventHandler(item_Click);
        }

        public bool AddItem(InventoryItem item, bool isOnInit)
        {
            if (isOnInit)
            {
                RegisterItemEvents(item);
                item.ButtonUnitSize = _itemUnitSize;

                if (_displayItemIcons)// && _manager.ImagesAvailable)
                {
                    Image img = _manager.GetImage(new Size(item.Size.Width / _itemUnitSize, item.Size.Height / _itemUnitSize), item.Item.GetItemImagePath(_isMale));
                    if (img != null)
                    {
                        item.BackgroundImage = img;
                    }
                }

                CreateToolTip(item);
                this.Controls.Add(item);

                return true;
            }
            else
            {
                bool canTransfer = IsRoomAvailable(item);

                if (canTransfer)
                {
                    RegisterItemEvents(item);
                    item.ButtonUnitSize = _itemUnitSize;
                    this.Controls.Add(item);

                    return true;
                }
            }
            return false;
        }

        private void CreateToolTip(InventoryItem item)
        {
            CharacterItems tmpItem = item.Item;
            string questString = String.Empty;

            if(tmpItem.IsQuestItem)
            {
                questString = "[Questitem]" + Environment.NewLine;
            }

            string toolTip = questString +
                             tmpItem.Name + Environment.NewLine +
                             "Item quantity: " + tmpItem.StackSize + "/" + tmpItem.MaxStackSize + Environment.NewLine +
                             "Item quality: " + tmpItem.Quality.ToString() + Environment.NewLine + Environment.NewLine +
                             "Upgrades: " + tmpItem.NumberOfUpgrades + "/" + tmpItem.MaxNumberOfUpgrades + Environment.NewLine +
                             "Augments: " + tmpItem.NumberOfAugmentations + "/" + tmpItem.MaxNumberOfAugmentations + Environment.NewLine +
                             "Affixes: " + tmpItem.NumberOfAffixes + Environment.NewLine +
                             "Age: " + tmpItem.PlayTime / 60 + " ingame minutes";
            toolTip1.SetToolTip(item, toolTip);
            
        }

        public void RemoveItem(InventoryItem item)
        {
            UnregisterItemEvents(item);
            this.Controls.Remove(item);
        }

        private bool OutOfScreen(InventoryItem item)
        {
            int width = this.Size.Width / _itemUnitSize;
            int height = this.Size.Height / _itemUnitSize;

            width -= (item.UnitSize.Width - 1);
            height -= (item.UnitSize.Height - 1);
            // if the item dimensions range beyond the inventory bounds
            if (item.Position.X < width && item.Position.Y < height)
            {
                return false;
            }

            return true;
        }

        private bool OverlapsWithOtherControls(InventoryItem item)
        {
            foreach (Control control in this.Controls)
            {
                if (item != (InventoryItem)control)
                {
                    if (item.Bounds.IntersectsWith(control.Bounds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void PreviewItem(InventoryItem item)
        {
            //_previewItem = item;
        }

        private bool IsRoomAvailable(InventoryItem item)
        {
            int width = this.Size.Width / _itemUnitSize;
            int height = this.Size.Height / _itemUnitSize;

            int itemWidth = item.UnitSize.Width;
            int itemHeight = item.UnitSize.Height;

            width -= (itemWidth - 1);
            height -= (itemHeight - 1);

            Point originalLocation = item.Position;

            for (int counterY = 0; counterY < height; counterY++)
            {
                for (int counterX = 0; counterX < width; counterX++)
                {
                    item.Position = new Point(counterX, counterY);

                    if (!OverlapsWithOtherControls(item))
                    {
                        return true;
                    }
                    else
                    {
                        item.Position = originalLocation;
                        continue;
                    }
                }
            }

            return false;
        }

        void item_Click(object sender, EventArgs e)
        {
            InventoryItem item = (InventoryItem)sender;

            NewItemSelected_Event(this, item);
            //_previewItem = item;
        }
    }


    [Serializable]
    public class InventoryItem : Button
    {
        int _size = 40;
        CharacterItems _item;
        bool _displayNamesAndQuantity;

        public CharacterItems Item
        {
            get
            {
                return _item;
            }
        }

        /// <summary>
        /// The drawing size of one unit (1x1)
        /// </summary>
        public int ButtonUnitSize
        {
            get { return _size; }
            set
            {
                _size = value;
                SetButtonSize();
            }
        }

        /// <summary>
        /// The grid position of the button
        /// </summary>
        public Point Position
        {
            get { return _item.InventoryPosition; }
            set
            {
                _item.InventoryPosition = value;
                this.Location = new Point(_item.InventoryPosition.X * _size, _item.InventoryPosition.Y * _size);
            }
        }

        /// <summary>
        /// The grid size of the item
        /// </summary>
        public Size UnitSize
        {
            get
            {
                return new Size(this.Size.Width / _size, this.Size.Height / _size);
            }
        }

        public InventoryItem()
            : base()
        {
        }

        public InventoryItem(CharacterItems item)//, string baseItem)
            : base()
        {
            _item = item;

            this.BackgroundImageLayout = ImageLayout.Stretch;
            Position = _item.InventoryPosition;
        }

        public void InitButton(bool displayNameAndQuantity)
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 2;
            this.TextAlign = ContentAlignment.BottomRight;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Text = string.Empty;
            this._displayNamesAndQuantity = displayNameAndQuantity;
            //this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            int quantity = _item.StackSize;

            if (quantity < 1)
            {
                quantity = 1;
            }

            if (_displayNamesAndQuantity)
            {
                this.Text = quantity.ToString();// Item.Name;
            }

            SetButtonSize();
            SetRarityColor();
        }

        private void SetButtonSize()
        {
            int width = _item.InventorySize.Width;
            int height = _item.InventorySize.Height;

            if (width <= 0)
            {
                width = 1;
            }
            if (height <= 0)
            {
                height = 1;
            }

            this.Size = new Size(_size * width, _size * height);
        }

        private void SetRarityColor()
        {
            ItemQuality quality = _item.Quality;

            Color color = Color.White;

            if (quality == ItemQuality.Mutant || quality == ItemQuality.MutantMod)
            {
                color = Color.Purple;
            }
            else if (quality == ItemQuality.Normal || quality == ItemQuality.NormalMod)
            {
                color = Color.White;

// add check for quest item

                //bitmask2 = 8192
                //if(false)
                //{
                //    color = Color.Red;
                //}
            }
            else if (quality == ItemQuality.Unique || quality == ItemQuality.UniqueMod)
            {
                color = Color.Gold;
            }
            else if (quality == ItemQuality.Rare || quality == ItemQuality.RareMod)
            {
                color = Color.Blue;
            }
            else if (quality == ItemQuality.Uncommon)
            {
                color = Color.Green;
            }
            else if (quality == ItemQuality.Legendary || quality == ItemQuality.LegendaryMod)
            {
                color = Color.Orange;
            }
            else if (quality == ItemQuality.DoubleEdged || quality == ItemQuality.DoubleEdgedMod)
            {
                color = Color.FromArgb(96, 255, 255);
            }
            else if (quality == ItemQuality.Mythic || quality == ItemQuality.MythicMod)
            {
                color = Color.Purple;
            }
            if (_item.IsQuestItem)
            {
                color = Color.Red;
            }

            this.FlatAppearance.BorderColor = color;
        }
    }

    public class PreviewManager
    {
        FileManager fileManager;
        List<ImageHolder> _imageHolder;
        string _basePath = Path.Combine(Config.HglDir, @"Data\mp_hellgate_1.10.180.3416_1.0.86.4580\data\units\items");

        public PreviewManager(FileManager fileManager)
        {
            _imageHolder = new List<ImageHolder>();
            this.fileManager = fileManager;
        }

        public Image GetImage(int unitType)
        {
            try
            {
                ImageHolder image = _imageHolder.Find(tmp => tmp.UnitType == unitType);

                if (image != null)
                {
                    return image.Image;
                }

                image = new ImageHolder(new Size(8, 8), fileManager);
                if (image.Load("items", unitType))
                {
                    _imageHolder.Add(image);
                    return image.Image;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return null;
            }
        }

        public Image GetImage(Size size, string imagePath)
        {
            try
            {
                ImageHolder image = _imageHolder.Find(tmp => tmp.ImagePath == imagePath);

                if (image != null)
                {
                    return image.Image;
                }

                image = new ImageHolder(size, fileManager);
                if (image.Load(_basePath, imagePath))
                {
                    _imageHolder.Add(image);
                    return image.Image;
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return null;
            }
        }

        public void Dispose()
        {
            foreach(ImageHolder holder in _imageHolder)
            {
                holder.Dispose();
            }
        }
    }

    public class ImageHolder
    {
        FileManager fileManager;
        Size _size;
        string _imagePath;
        int _unitType;
        Bitmap _image;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        public int UnitType
        {
            get { return _unitType; }
            set { _unitType = value; }
        }

        public Bitmap Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public ImageHolder(Size size, FileManager fileManager)
        {
            _size = size;
            this.fileManager = fileManager;
        }

        public bool Load(string folder, int unitType)
        {
            _unitType = unitType;
            string filePath = Path.Combine(folder, _unitType + ".png");

            if (File.Exists(filePath))
            {
                _image = new Bitmap(filePath);
                return true;
            }

            return false;
        }

        public bool Load(string folder, string imagePath)
        {
            try
            {
                _imagePath = imagePath;

                if (File.Exists(Path.Combine(folder, _imagePath) + ".dds"))
                {
                    _image = LoadImage(folder, _imagePath, ".dds");
                    if (_size.Height != _size.Width)
                    {
                        _image = CropImage(_image, _size);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return false;
            }
        }

        private Bitmap CropImage(Bitmap bmp, Size size)
        {
            try
            {
                int sizeX = size.Width * 68;
                int sizeY = size.Height * 68;

                int posX = (256 - sizeX) / 2;
                int posY = (256 - sizeY) / 2;

                Rectangle rect = new Rectangle(posX, posY, sizeX, sizeY);
                Bitmap tmp = bmp.Clone(rect, bmp.PixelFormat);
                bmp.Dispose();

                return tmp;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return null;
            }
        }

        public void Dispose()
        {
            if (_image != null)
            {
                _image.Dispose();
            }
        }

        private Bitmap LoadImage(string folder, string imagePath, string extension)
        {
            //return DevIL.DevIL.LoadBitmap(filePath);
            string path = Path.Combine(folder, imagePath) + extension;
            FreeImageAPI.FreeImageBitmap bmp = FreeImageAPI.FreeImageBitmap.FromFile(path);
            return bmp.ToBitmap();
            //string path = Path.Combine(@"data\units\items", filePath);
            //string path = @"data\units\items\misc\dye_kit\low\dye_kit_diffuse.dds";

            //if (_fileExplorer.GetFileExists(path))
            //{
            //    byte[] imageBytes = _fileExplorer.GetFileBytes(path);
            //    MemoryStream imageStream = new MemoryStream(imageBytes, false);
            //    FreeImageAPI.FreeImageBitmap bmp = FreeImageAPI.FreeImageBitmap.FromStream(imageStream);

            //    return bmp.ToBitmap();
            //}
            //return null;
        }
    }
}