using System;
using UnityEngine;

namespace GameDevKit.InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField]
        private ItemStack _state;

        private bool _active;


        private Action<ItemStack,bool> StateChange;
        
        public ItemStack State
        {
            get => _state;
            set
            {
                _state = value;
                NotifyAboutStateChange();
            }
        }

        public bool Active 
        {
            get => _active;
            set
            {
                _active = value;
                NotifyAboutStateChange();
            }
        }

        public int NumberOfStack
        {
            get => _state.NumberOfItems;
            set
            {
                _state.NumberOfItems = value;
                NotifyAboutStateChange();
            }
        }

        public bool HasItem => _state?.Item != null;
        public void Clear()
        {
            State = null;
        }
        private void NotifyAboutStateChange()
        {
            StateChange?.Invoke(_state, _active);
        }
    }
}