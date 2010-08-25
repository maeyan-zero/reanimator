using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Reanimator.Forms.ItemTransfer
{
    public partial class ItemPanel : UserControl
    {
        public delegate void NewItemSelected(ItemPanel sender, InventoryItem item);
        public event NewItemSelected NewItemSelected_Event;

        PreviewManager _manager;
        bool _displayItemIcons;
        int _itemUnitSize = 40;

        public int ItemUnitSize
        {
            get { return _itemUnitSize; }
            set { _itemUnitSize = value; }
        }

        //public InventoryItem _previewItem;

        public ItemPanel(bool displayItemIcons)
        {
            InitializeComponent();
            //_previewItem = null;
            _displayItemIcons = displayItemIcons;
            //this.Controls.Add(_previewItem);
            _manager = new PreviewManager();
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
            //_previewItem = null;

            if (isOnInit)
            {
                RegisterItemEvents(item);
                item.ButtonUnitSize = _itemUnitSize;

                if (_displayItemIcons)// && _manager.ImagesAvailable)
                {
                    Image img = _manager.GetImage(new Size(item.Size.Width / _itemUnitSize, item.Size.Height / _itemUnitSize), item.ImagePath);
                    if (img != null)
                    {
                        item.BackgroundImage = img;
                    }
                }

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
        Unit _item;
        int _quantity;
        bool _displayNamesAndQuantity;
        int _unitType;
        string _imagePath;

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        /// <summary>
        /// The item that is bound to this button
        /// </summary>
        public Unit Item
        {
            get { return _item; }
            set
            {
                _item = value;
                InitButton(_displayNamesAndQuantity);
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
            get { return new Point(_item.inventoryPositionX, _item.inventoryPositionY); }
            set
            {
                _item.inventoryPositionX = value.X; _item.inventoryPositionY = value.Y;
                this.Location = new Point(_item.inventoryPositionX * _size, _item.inventoryPositionY * _size);
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

        public InventoryItem()
            : base()
        {
        }

        public InventoryItem(Unit item, int unitType)//, string baseItem)
            : base()
        {
            _item = item;
            _unitType = unitType;

            this.BackgroundImageLayout = ImageLayout.Stretch;
            Position = new Point(_item.inventoryPositionX, _item.inventoryPositionY);
        }

        public InventoryItem(Unit item, string imagePath)//, string baseItem)
            : base()
        {
            _item = item;
            _imagePath = imagePath;

            this.BackgroundImageLayout = ImageLayout.Stretch;
            Position = new Point(_item.inventoryPositionX, _item.inventoryPositionY);
        }

        public void InitButton(bool displayNameAndQuantity)
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 4;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Text = string.Empty;
            _displayNamesAndQuantity = displayNameAndQuantity;

            if (_displayNamesAndQuantity)
            {
                _quantity = UnitHelpFunctions.GetSimpleValue(_item, ItemValueNames.item_quantity.ToString());

                if (_quantity > 0)
                {
                    this.Text += _quantity.ToString() + "x ";
                }

                this.Text += _item.Name;
            }

            SetButtonSize();
            SetRarityColor();
        }

        private void SetButtonSize()
        {
            int width = UnitHelpFunctions.GetSimpleValue(_item, ItemValueNames.inventory_width.ToString());
            int height = UnitHelpFunctions.GetSimpleValue(_item, ItemValueNames.inventory_height.ToString());

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
            int quality = UnitHelpFunctions.GetSimpleValue(_item, ItemValueNames.item_quality.ToString());

            Color color = Color.White;

            if (quality == (int)ItemQuality.Mutant || quality == (int)ItemQuality.MutantMod)
            {
                color = Color.Purple;
            }
            else if (quality == (int)ItemQuality.Normal || quality == (int)ItemQuality.NormalMod)
            {
                color = Color.White;
            }
            else if (quality == (int)ItemQuality.Unique || quality == (int)ItemQuality.UniqueMod)
            {
                color = Color.Gold;
            }
            else if (quality == (int)ItemQuality.Rare || quality == (int)ItemQuality.RareMod)
            {
                color = Color.Blue;
            }
            else if (quality == (int)ItemQuality.Uncommon)
            {
                color = Color.Green;
            }
            else if (quality == (int)ItemQuality.Legendary || quality == (int)ItemQuality.LegendaryMod)
            {
                color = Color.Orange;
            }

            this.FlatAppearance.BorderColor = color;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    _item = null;

        //    base.Dispose(disposing);
        //}
    }

    //public class PreviewManager
    //{
    //    List<NameToImage> _images;
    //    const string _configPath = "imageMapping.xml";

    //    public PreviewManager()
    //    {
    //        if (File.Exists(_configPath))
    //        {
    //            _images = XmlUtilities<List<NameToImage>>.Deserialize(_configPath);
    //        }
    //    }

    //    public Image GetImage(string itemName)
    //    {
    //        NameToImage name = _images.Find(tmp => tmp.ItemName.Contains(itemName));

    //        if (name != null)
    //        {
    //            return name.GetImage();
    //        }

    //        return null;
    //    }

    //    public Image GetImage(int unitType)
    //    {
    //        NameToImage name = _images.Find(tmp => tmp.UnitType == unitType);

    //        if (name != null)
    //        {
    //            return name.GetImage();
    //        }

    //        return null;
    //    }

    //    public bool ImagesAvailable
    //    {
    //        get
    //        {
    //            return _images != null;
    //        }
    //    }

    //    //public void Dispose()
    //    //{
    //    //    if (_images != null)
    //    //    {
    //    //        foreach (NameToImage image in _images)
    //    //        {
    //    //            image.Dispose();
    //    //        }
    //    //    }
    //    //}
    //}


    //public class NameToImage
    //{
    //    int _unitType;
    //    string _itemName;
    //    string _imagePath;
    //    Image _image;

    //    [XmlElement("ItemName")]
    //    public string ItemName
    //    {
    //        get { return _itemName; }
    //        set { _itemName = value; }
    //    }

    //    [XmlElement("ImagePath")]
    //    public string ImagePath
    //    {
    //        get { return _imagePath; }
    //        set { _imagePath = value; }
    //    }

    //    [XmlElement("UnitType")]
    //    public int UnitType
    //    {
    //        get { return _unitType; }
    //        set { _unitType = value; }
    //    }

    //    public Image GetImage()
    //    {
    //        if (_image == null)
    //        {
    //            _image = Bitmap.FromFile(_imagePath);
    //        }

    //        return _image;
    //    }

    //    //public void Dispose()
    //    //{
    //    //    _image.Dispose();
    //    //}
    //}

    public class PreviewManager
    {
        List<ImageHolder> _imageHolder;
        string _basePath = @"E:\Flagship Studios\Hellgate London\Data\mp_hellgate_1.10.180.3416_1.0.86.4580\data\units\items";

        public PreviewManager()
        {
            _imageHolder = new List<ImageHolder>();
        }

        public Image GetImage(int unitType)
        {
            ImageHolder image = _imageHolder.Find(tmp => tmp.UnitType == unitType);

            if (image != null)
            {
                return image.Image;
            }

            image = new ImageHolder(new Size(8, 8));
            if(image.Load("items", unitType))
            {
                _imageHolder.Add(image);
                return image.Image;
            }
            return null;
        }

        public Image GetImage(Size size, string imagePath)
        {
            ImageHolder image = _imageHolder.Find(tmp => tmp.ImagePath == imagePath);

            if (image != null)
            {
                return image.Image;
            }

            image = new ImageHolder(size);
            if (image.Load(_basePath, imagePath))
            {
                _imageHolder.Add(image);
                return image.Image;
            }
            return null;
        }
    }

    public class ImageHolder
    {
        Size _size;
        string _imagePath;
        int _unitType;
        Image _image;

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

        public Image Image
        {
          get { return _image; }
          set { _image = value; }
        }

        public ImageHolder(Size size)
        {
            _size = size;
        }

        public bool Load(string folder, int unitType)
        {
            _unitType = unitType;
            string filePath = Path.Combine(folder, _unitType + ".png");

            if (File.Exists(filePath))
            {
                _image = Image.FromFile(filePath);
                return true;
            }

            return false;
        }

        public bool Load(string folder, string imagePath)
        {
            _imagePath = imagePath;
            string filePath = Path.Combine(folder, imagePath) + ".dds";

            if (File.Exists(filePath))
            {
                Bitmap bmp = DevIL.DevIL.LoadBitmap(filePath);
                if (_size.Height != _size.Width)
                {
                    _image = CropImage(bmp, _size);
                }
                else
                {
                    _image = bmp;
                }
                return true;
            }

            return false;
        }

        private Image CropImage(Bitmap bmp, Size size)
        {
            int sizeX = size.Width * 68;
            int sizeY = size.Height * 68;

            int posX = (256 - sizeX) / 2;
            int posY = (256 - sizeY) / 2;

            Rectangle rect = new Rectangle(posX, posY, sizeX, sizeY);
            Bitmap bmpCrop = bmp.Clone(rect, bmp.PixelFormat);
            return (Image)bmpCrop;
        }
    }

}