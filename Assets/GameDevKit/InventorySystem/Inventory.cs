using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameDevKit.InventorySystem
{
    public class Inventory
    {
        [SerializeField]
        private int _size = 10;

        [SerializeField] 
        private List<InventorySlot> _slots;

        public bool IsFull()
        {
            return _slots.Count(e => e.HasItem) >= _size;
        }

        public bool CanAcceptItem(ItemStack itemStack)
        {
            var slot = FindSlot(itemStack.Item, true);
            return !IsFull() || slot != null;
        }
        private InventorySlot FindSlot(ItemDefinition item,bool isStackable = false)
        {
            return _slots.FirstOrDefault(slot => slot.HasItem && item.IsStackable || !isStackable);
        }

        public ItemStack AddItem(ItemStack itemStack)
        {
            var relevantSlot = FindSlot(itemStack.Item, true);
            if (IsFull()&&relevantSlot==null)
            {
                throw new InventoryException(InventoryOperation.Add, "Inventory is Full");
            }

            if (relevantSlot!=null)
            {
                relevantSlot.NumberOfStack += itemStack.NumberOfItems;
            }
            //不可堆叠
            else
            {
                relevantSlot = _slots.First(slot => !slot.HasItem);
                relevantSlot.State = itemStack;
            }

            return relevantSlot.State;
        }
    }
}