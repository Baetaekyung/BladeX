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

        [HideInInspector] 
        public List<ItemSlot>      itemSlots        = new();
        public List<EquipmentData> currentEquipment = new();
        
        public int Coin { get; set; }
        public event Action OnCoinChanged;
        
        
        private PlayerInventory Initialize()
        {
            PlayerInventory inventory = Instantiate(this);

            inventory.itemSlots = itemSlots;

            #region Deep copy

            List<ItemDataSO> tempInventory = new List<ItemDataSO>();

            foreach(var itemData in itemInventory)
            {
                tempInventory.Add(itemData);
            }

            #endregion
            inventory.itemInventory = tempInventory;            
            inventory.Coin = 0;
            
            inventory.currentInventoryCapacity = 0;
            inventory.maxInventoryCapacity = itemSlots.Count - 5; // -5는 장비슬롯 때문에
            inventory.currentEquipment = new List<EquipmentData>();

            return inventory;
        }

        public PlayerInventory Clone() => Initialize();

        public void AddCoin(int _amount)
        {
            Coin += _amount;
            OnCoinChanged.Invoke();
        }
    }
}
