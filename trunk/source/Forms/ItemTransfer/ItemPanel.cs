using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms.ItemTransfer
{
    public partial class ItemPanel : UserControl
    {
        public delegate void NewItemSelected(ItemPanel sender, InventoryItem item);
        public event NewItemSelected NewItemSelected_Event;
        public delegate void ItemDoubleClicked(ItemPanel sender, InventoryItem item);
        public event ItemDoubleClicked ItemDoubleClicked_Event;

        int _itemUnitSize = 40;

        public int ItemUnitSize
        {
            get { return _itemUnitSize; }
            set { _itemUnitSize = value; }
        }

        public InventoryItem _previewItem;

        public ItemPanel()
        {
            InitializeComponent();
            _previewItem = null;
            this.Controls.Add(_previewItem);
        }

        private void RegisterItemEvents(InventoryItem item)
        {
            item.Click += new EventHandler(item_Click);
            //item.DoubleClick += new EventHandler(item_DoubleClick);

            //item.MouseDown += new MouseEventHandler(item_MouseDown);
            //item.MouseUp += new MouseEventHandler(item_MouseUp);
            //item.MouseMove += new MouseEventHandler(item_MouseMove);
        }

        private void UnregisterItemEvents(InventoryItem item)
        {
            item.Click -= new EventHandler(item_Click);
            //item.DoubleClick -= new EventHandler(item_DoubleClick);

            //item.MouseDown -= new MouseEventHandler(item_MouseDown);
            //item.MouseUp -= new MouseEventHandler(item_MouseUp);
            //item.MouseMove -= new MouseEventHandler(item_MouseMove);
        }

        public bool AddItem(InventoryItem item, bool isOnInit)
        {
            _previewItem = null;

            if (isOnInit)
            {
                RegisterItemEvents(item);
                item.ButtonUnitSize = _itemUnitSize;
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
            _previewItem = item;
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

            //if (_previewItem == item) // leave the control where it is
            //{
            //    if (!OverlapsWithOtherControls(item) && !OutOfScreen(item))
            //    {
            //        //_previewItem = _preview;
            //        _previewItem = null;
            //    }
            //}
            //else // set the movable control to the selected control
            //{
                NewItemSelected_Event(this, item);
                _previewItem = item;
            //}
        }

        //void item_DoubleClick(object sender, EventArgs e)
        //{
        //    InventoryItem item = (InventoryItem)sender;
        //    ItemDoubleClicked_Event(this, item);
        //}

        //private void item_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (_mouseIsDraggingItem)
        //    {
        //        Point oldLoc = _previewItem.Position;

        //        int x = e.X / _itemUnitSize;
        //        int y = e.Y / _itemUnitSize;

        //        _previewItem.Position = new Point(x, y);

        //        if (OutOfScreen(_previewItem) == true)
        //        {
        //            _previewItem.Position = oldLoc;
        //        }
        //    }
        //}

        //bool _mouseIsDraggingItem;
        //private void item_MouseDown(object sender, MouseEventArgs e)
        //{
        //    InventoryItem item = (InventoryItem)sender;
        //    _previewItem = item;
        //    NewItemSelected_Event(this, item);

        //    if (_previewItem != null)
        //    {
        //        _mouseIsDraggingItem = true;
        //    }
        //}

        //private void item_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (_previewItem != null)
        //    {
        //        _mouseIsDraggingItem = false;
        //    }
        //}
    }


    [Serializable]
    public class InventoryItem : Button
    {
        int _size = 40;
        Unit _item;
        int _quantity;

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
                InitButton(value);
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

        public InventoryItem()
            : base()
        {
        }

        public InventoryItem(Unit item)
            : base()
        {
            _item = item;

            InitButton(item);

            Position = new Point(_item.inventoryPositionX, _item.inventoryPositionY);
        }

        public void InitButton(Unit item)
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 4;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Text = string.Empty;

            _quantity = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.item_quantity.ToString());

            if (_quantity > 0)
            {
                this.Text += _quantity.ToString() + "x ";
            }

            this.Text += _item.Name;

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
    }
}
