using System;
using System.Collections.Generic;
using System.Drawing;
using Hellgate;

namespace Reanimator.Forms.ItemTransfer
{
    public class InventoryHandler
    {
        public enum TradeInventoryTypes
        {
            Cube,
            Stash,
            Inventory
        };

        const int maxInventoryWidth = 6;
        const int maxInventoryHeight = 256;

        int cubeHeight = 6;

        public int CubeHeight
        {
            get { return cubeHeight; }
            set { cubeHeight = value; }
        }
        int stashHeight = 100;

        public int StashHeight
        {
            get { return stashHeight; }
            set { stashHeight = value; }
        }
        int inventoryHeight = 24;

        public int InventoryHeight
        {
            get { return inventoryHeight; }
            set { inventoryHeight = value; }
        }

        bool[][,] _charInventory;

        public InventoryHandler()
        {
        }

        public void Initialize(List<UnitObject> items)
        {
            InitArray();

            InitInventories(items);
        }

        private void InitArray()
        {
            _charInventory = new bool[Enum.GetNames(typeof(TradeInventoryTypes)).Length][,];

            bool[,] cube = new bool[maxInventoryWidth, cubeHeight];
            bool[,] stash = new bool[maxInventoryWidth, stashHeight];
            bool[,] inventory = new bool[maxInventoryWidth, inventoryHeight];

            _charInventory[(int)TradeInventoryTypes.Cube] = cube;
            _charInventory[(int)TradeInventoryTypes.Stash] = stash;
            _charInventory[(int)TradeInventoryTypes.Inventory] = inventory;

            foreach (bool[,] array in _charInventory)
            {
                for(int counterY = 0; counterY < array.GetLength(1); counterY++)
                {
                    for (int counterX = 0; counterX < array.GetLength(0); counterX++)
                    {
                        array[counterX, counterY] = true;
                    }
                }
            }
        }

        private void InitInventories(List<UnitObject> items)
        {
            foreach (UnitObject item in items)
            {
                AddItem(item);

                //if (item.inventoryType == (int)InventoryTypes.Cube)
                //{
                //}
                //else if (item.inventoryType == (int)InventoryTypes.Inventory)
                //{
                //}
                //else if (item.inventoryType == (int)InventoryTypes.Stash)
                //{
                //}
                //else //equipped
                //{
                //}
            }
        }

        public void AddItem(UnitObject item)
        {
            // get inventory type that the item uses
            TradeInventoryTypes type = TradeInventoryTypes.Inventory;

            if (item.inventoryType == (int)InventoryTypes.Cube)
            {
                type = TradeInventoryTypes.Cube;
            }
            else if (item.inventoryType == (int)InventoryTypes.Stash)
            {
                type = TradeInventoryTypes.Stash;
            }
            else if (item.inventoryType == (int)InventoryTypes.Inventory)
            {
                type = TradeInventoryTypes.Inventory;
            }
            else
            {
                return;
            }

            AddOrRemoveItem(type, item, false);
        }

        public void RemoveItem(UnitObject item)
        {
            // get inventory type that the item uses
            TradeInventoryTypes type = TradeInventoryTypes.Inventory;

            if (item.inventoryType == (int)InventoryTypes.Cube)
            {
                type = TradeInventoryTypes.Cube;
            }
            else if (item.inventoryType == (int)InventoryTypes.Stash)
            {
                type = TradeInventoryTypes.Stash;
            }
            else if (item.inventoryType == (int)InventoryTypes.Inventory)
            {
                type = TradeInventoryTypes.Inventory;
            }
            else
            {
                return;
            }

            AddOrRemoveItem(type, item, true);
        }

        private void AddOrRemoveItem(TradeInventoryTypes type, UnitObject item, bool removeItem)
        {
            // get corresponding inventory
            bool[,] inventory = _charInventory[(int)type];

            int x = item.inventoryPositionX;
            int y = item.inventoryPositionY;

            int width = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.inventory_width.ToString());
            int height = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.inventory_height.ToString());

            if (width <= 0)
            {
                width = 1;
            }
            if (height <= 0)
            {
                height = 1;
            }

            AllocateInventorySpace(inventory, x, y, width, height, removeItem);
        }

        private void AllocateInventorySpace(bool[,] inventory, int x, int y, int width, int height, bool freeThisSpace)
        {
            if (CheckIfSpaceIsAvailable(inventory, x, y, width, height))
            {
                for (int counterX = 0; counterX < width; counterX++)
                {
                    for (int counterY = 0; counterY < height; counterY++)
                    {
                        inventory[x + counterX, y + counterY] = freeThisSpace;
                    }
                }
            }
        }

        private bool CheckIfSpaceIsAvailable(bool[,] inventory, int x, int y, int width, int height)
        {
            for (int counterX = 0; counterX < width; counterX++)
            {
                for (int counterY = 0; counterY < height; counterY++)
                {
                    bool available = inventory[x + counterX, y + counterY];

                    if (!available)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override string ToString()
        {
            string inv = string.Empty;

            for (int counter = 0; counter < _charInventory.Length; counter++)
            {
                inv += ((TradeInventoryTypes)Enum.ToObject(typeof(TradeInventoryTypes), counter)).ToString() + "\n";

                for (int counterY = 0; counterY < _charInventory[counter].GetLength(1); counterY++)
                {
                    for (int counterX = 0; counterX < _charInventory[counter].GetLength(0); counterX++)
                    {
                        bool isFree =  _charInventory[counter][counterX, counterY];

                        if (isFree)
                        {
                            inv += ".";
                        }
                        else
                        {
                            inv += "x";
                        }
                    }

                    inv += "\n";
                }

                inv += "\n\n";
            }

            return inv;
        }
    }

    public class ItemInfo
    {
        UnitObject _item;
        Size _itemSize;

        public Point Location
        {
            get { return GetLocation(); }
            set { SetLocation(value); }
        }

        public Size ItemSize
        {
            get { return _itemSize; }
        }

        public UnitObject Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public ItemInfo(UnitObject item)
        {
            _item = item;

            int width = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.inventory_width.ToString());
            int height = UnitHelpFunctions.GetSimpleValue(item, ItemValueNames.inventory_height.ToString());

            _itemSize.Width = width;
            _itemSize.Height = height;
        }

        private void SetLocation(Point point)
        {
            _item.inventoryPositionX = point.X;
            _item.inventoryPositionY = point.Y;
        }

        private Point GetLocation()
        {
            int x = _item.inventoryPositionX;
            int y = _item.inventoryPositionY;

            return new Point(x, y);
        }
    }
}
