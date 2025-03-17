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
        private ItemSlot _itemSlot;
        
        public string     itemName;
        [TextArea(4, 5)]
        public string     description;
        public ItemType   itemType;
        public ItemObject itemObject;

        public bool useQuickSlot = false;

        [Space]
        [Header("����� �� �ʿ��� ������")]
        public EquipmentData equipmentData; //����� ���� �־��ֱ�
        
        public ItemSlot GetItemSlot => _itemSlot;
        public bool IsEquipment() => itemType == ItemType.EQUIPMENT;
    }
}
