using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class EquipmentSlot : ItemSlot
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (inventoryManager.SelectedItem == null) return;
            if (inventoryManager.SelectedItem.IsEquipment() == false) return;
            
            base.OnPointerEnter(eventData);
            inventoryManager.Inventory.OnEquipmentChanged += HandleStatChange;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (inventoryManager.SelectedItem == null) return;
            if(inventoryManager.SelectedItem.IsEquipment() == false) return;

            inventoryManager.Inventory.OnEquipmentChanged -= HandleStatChange;
            
            base.OnPointerExit(eventData);
        }

        private void HandleStatChange()
        {
            if (_itemDataSO.IsEquipment() == false) return;
            
            inventoryManager.Inventory.currentEquipment.Add(_itemDataSO.statData);
            
            if (inventoryManager.PlayerStat == null)
            {
                if (_itemDataSO.statData == null) return;
                
                //foreach (var statType in _itemDataSO.statData.statAddValues.Keys)
                //{
                //    inventoryManager.PlayerStat.AddModifier(
                //        statType,
                //        _itemDataSO.statData.itemId,
                //        _itemDataSO.statData.statAddValues[statType]);
                //}
            }
        }
    }
}
