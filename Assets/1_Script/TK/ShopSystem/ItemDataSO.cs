using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [Serializable] [CreateAssetMenu(fileName = "ItemData_", menuName = "SO/Item/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite    itemImage;
        public ItemSlot  ItemSlot { get; set; }
        
        public string     itemName;
        [TextArea(4, 5)]
        public string     description;
        public ItemType   itemType;
        public ItemObject itemObject;
        
        [Space]
        [Header("장비일 때 필요한 변수들")]
        public EquipmentData equipmentData; //장비일 때만 넣어주기
        
        public bool IsEquipment() => itemType == ItemType.EQUIPMENT;
    }
}
