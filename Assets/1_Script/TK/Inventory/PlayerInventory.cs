using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "PlayerInventory", menuName = "SO/PlayerInventory")]
    public class PlayerInventory : ScriptableObject
    {
        public int Currency; //뭐라할 지 모르겠음;;
        public int currentInventoryCapacity;
        public int maxInventoryCapacity;
        
        public List<ItemDataSO> itemInventory;

        [HideInInspector] public List<ItemSlot> itemSlots;
        public List<EquipmentData> currentEquipment = new List<EquipmentData>();
        public List<EquipmentChannelSO> currentEquipmentEffects = new List<EquipmentChannelSO>();
        
        private void OnEnable()
        {
            #if UNITY_EDITOR

            Currency = 9999; //테스트 용도
            
            #endif
            
            currentInventoryCapacity = itemInventory.Capacity;
            maxInventoryCapacity = itemSlots.Count - 4; // -4는 장비슬롯 때문에
            currentEquipment = new List<EquipmentData>();
        }
    }
}
