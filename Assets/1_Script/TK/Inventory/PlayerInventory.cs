using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "PlayerInventory", menuName = "SO/PlayerInventory")]
    public class PlayerInventory : ScriptableObject
    {
        public int currentMoney; //������ �� �𸣰���;;
        public int currentInventoryCapacity;
        public int maxInventoryCapacity;
        
        public List<ItemDataSO> itemInventory;

        [HideInInspector] public List<ItemSlot> itemSlots;
        public List<EquipmentData> currentEquipment = new List<EquipmentData>();
        public List<EquipmentChannelSO> currentEquipmentEffects = new List<EquipmentChannelSO>();

        private void OnEnable()
        {
            currentInventoryCapacity = itemInventory.Capacity;
            maxInventoryCapacity = itemSlots.Count - 4; // -4�� ��񽽷� ������
            currentEquipment = new List<EquipmentData>();
        }
    }
}
