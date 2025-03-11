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
        
        public int Currency { get; set; }
        
        private void OnEnable()
        {
#if UNITY_EDITOR
            
            Currency = 9999; //�׽�Ʈ �뵵
            
#endif
            
            currentInventoryCapacity = itemInventory.Capacity;
            maxInventoryCapacity = itemSlots.Count - 4; // -4�� ��񽽷� ������
            currentEquipment = new List<EquipmentData>();
        }
    }
}
