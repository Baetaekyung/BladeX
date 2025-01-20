using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "ItemData_", menuName = "SO/Item/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite itemImage;
        public string itemName;
        
        [TextArea(4, 5)] public string description;
        public ItemType itemType;
        
        private ItemSlot _itemSlot;
        public ItemSlot GetItemSlot => _itemSlot;

        [Space]
        public EquipmentStatData statData; //장비일 때만 넣어주기
        
        public void SetSlot(ItemSlot slot) => _itemSlot = slot;
        public bool IsEquipment() => itemType == ItemType.EQUIPMENT;
    }
}
