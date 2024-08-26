using System;
using UnityEngine;

namespace GameDevKit.InventorySystem
{
    [Serializable]
    public class ItemStack
    {
        private ItemDefinition _item;

        [SerializeField]
        private int _itemNum;

        public bool IsStackable => _item!=null&&_item.IsStackable;

        public ItemDefinition Item => _item;
        public int NumberOfItems
        {
            get => _itemNum;
            set
            {
                value = value < 0 ? 0 : value;
                _itemNum =  _item.IsStackable ? value : 1;
            }
        }

        public ItemStack(ItemDefinition item, int itemNum)
        {
            _item = item;
            NumberOfItems = itemNum;
        }

        public ItemStack()
        {
        }
    }
}