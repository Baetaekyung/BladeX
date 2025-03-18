using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "PlayerInventory", menuName = "SO/PlayerInventory")]
    public class PlayerInventory : ScriptableObject
    {
        public int currentInventoryCapacity;
        public int maxInventoryCapacity;
        
        public List<ItemDataSO> itemInventory;

        [HideInInspector] public List<ItemSlot> itemSlots;
        public List<EquipmentData> currentEquipment = new List<EquipmentData>();
        public List<EquipmentChannelSO> currentEquipmentEffects = new List<EquipmentChannelSO>();
        
        public int Coin { get; set; }
        
        public PlayerInventory Clone()
        {
            var inventory = Instantiate(this);
            inventory.Initialize();
            
            return inventory;
        }

        public void Initialize()
        {
            PlayerInventory inventory = Instantiate(this);

            inventory.itemSlots = itemSlots;
            inventory.itemInventory = new List<ItemDataSO>();
            inventory.currentEquipmentEffects = new List<EquipmentChannelSO>();
            
            inventory.Coin = 0;
            
            inventory.currentInventoryCapacity = itemInventory.Count;
            inventory.maxInventoryCapacity = itemSlots.Count - 4; // -4는 장비슬롯 때문에
            inventory.currentEquipment = new List<EquipmentData>();
        }


    }
}
