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
        int _size = 40;
        InventoryItem _previewItem;
        //InventoryItem _preview;

        public ItemPanel()
        {
            InitializeComponent();
            //_preview = new InventoryItem();
            //_preview.Size = new Size(10, 10);
            _previewItem = null;
            //this.Controls.Add(_preview);
            this.Controls.Add(_previewItem);

            //_previewItem = _preview;
        }

        public void AddItem(InventoryItem item)
        {
            item.Click += new EventHandler(item_Click);
            this.Controls.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            item.Click += new EventHandler(item_Click);
            this.Controls.Remove(item);
        }

        void item_Click(object sender, EventArgs e)
        {
            InventoryItem item = (InventoryItem)sender;

            if(_previewItem == item) // leave the control where it is
            {
                if (!TestForOverlappingControls(item) && !OutOfScreen(item))
                {
                    //_previewItem = _preview;
                    _previewItem = null;
                }
            }
            else // set the movable control to the selected control
            {
                _previewItem = item;
            }
        }

        private bool OutOfScreen(InventoryItem item)
        {
            int width = this.Size.Width / _size;
            int height = this.Size.Height / _size;
            // if the item dimensions range beyond the inventory bounds
            if (item.Position.X + item.UnitSize.Width - 1 < width && item.Position.Y + item.UnitSize.Height - 1 < height)
            {
                return false;
            }

            return true;
        }

        private bool TestForOverlappingControls(InventoryItem item)
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

        private void ItemPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_previewItem != null)
            {
                Point oldLoc = _previewItem.Position;

                int x = e.X / _size;
                int y = e.Y / _size;

                _previewItem.Position = new Point(x, y);

                if (OutOfScreen(_previewItem) == true)
                {
                    _previewItem.Position = oldLoc;
                }
            }
        }
    }



    [Serializable]
    public class InventoryItem : Button
    {
        int _size = 40;
        Unit _item;

        public Unit Item
        {
            get { return _item; }
            set
            {
                _item = value;
                InitButton(value);
            }
        }

        public int ButtonUnitSize
        {
            get { return _size; }
            set
            {
                _size = value;
                SetButtonSize();
            }
        }

        public Point Position
        {
            get { return new Point(_item.inventoryPositionX, _item.inventoryPositionY); }
            set
            {
                _item.inventoryPositionX = value.X; _item.inventoryPositionY = value.Y;
                this.Location = new Point(_item.inventoryPositionX * _size, _item.inventoryPositionY * _size);
            }
        }

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

            int quantity = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.item_quantity.ToString());

            if (quantity > 0)
            {
                this.Text += quantity.ToString() + "x ";
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
