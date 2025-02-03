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

        public List<ItemDataSO> itemInventory;

        [HideInInspector] public List<ItemSlot> itemSlots;
        public List<EquipmentStatData> currentEquipment = new List<EquipmentStatData>();

        public Action OnEquipmentChanged;

        private void OnEnable()
        {
            currentEquipment = new List<EquipmentStatData>();
        }

        public void InvokeEquipChange()
        {
            OnEquipmentChanged?.Invoke();
        }
    }
}
