using Sirenix.OdinInspector;
using UnityEngine;

namespace GameDevKit.InventorySystem
{
    public class ItemDefinition 
    {
        [LabelText("UIIcon")]
        public Sprite UISprite;

        [LabelText("是否可堆叠")]
        public bool IsStackable;
    }
}