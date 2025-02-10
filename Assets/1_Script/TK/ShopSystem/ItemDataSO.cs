using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [Serializable] [CreateAssetMenu(fileName = "ItemData_", menuName = "SO/Item/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite itemImage;
        public string itemName;
        
        [TextArea(4, 5)] public string description;
        public ItemType itemType;
        
        private ItemSlot _itemSlot;
        public ItemSlot GetItemSlot => _itemSlot;

        [FormerlySerializedAs("data")]
        [FormerlySerializedAs("statData")]
        [Header("장비일 때 필요한 변수들")]
        [Space]
        public EquipmentData equipmentData; //장비일 때만 넣어주기
        public BaseEquipment equipmentObject;
        
        public bool IsEquipment() => itemType == ItemType.EQUIPMENT;
    }
}
