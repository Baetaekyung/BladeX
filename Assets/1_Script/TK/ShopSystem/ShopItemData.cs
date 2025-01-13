using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "ShopItem_", menuName = "SO/Shop/ItemData")]
    public class ShopItemData : ScriptableObject
    {
        [SerializeField] private PlayerInventory inventory;
        
        public Sprite itemImage;
        public string itemName;
        [TextArea(4, 5)] public string description;
        public int price;

        public bool CanBuy()
        {
            return inventory.currentMoney >= price;
        }

        public void Buy()
        {
            if (CanBuy() == false) return;

            inventory.currentMoney -= price;
        }
    }
}
