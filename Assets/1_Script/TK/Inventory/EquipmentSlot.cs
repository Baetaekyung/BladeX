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
            if (_itemDataSO != null) return;
            
            base.OnPointerEnter(eventData);
            
            inventoryManager.Inventory.currentEquipment.Add(_itemDataSO.statData);
            inventoryManager.Inventory.itemInventory.Remove(_itemDataSO);
            
            inventoryManager.Inventory.OnEquipmentChanged += HandleStatChange;
            inventoryManager.Inventory.InvokeEquipChange();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_itemDataSO != null)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (inventoryManager.Inventory.currentEquipment.Contains(_itemDataSO.statData))
                    {
                        inventoryManager.Inventory.OnEquipmentChanged -= HandleStatChange;
                        inventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.statData);
                        inventoryManager.UpdateEquipInfoUI();
                        
                        inventoryManager.AddItemToEmptySlot(_itemDataSO);
                        SetItemData(null);
                        SetItemImage(null);
                    }
                }
            }
        }
        
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (inventoryManager.SelectedItem == null) return;
            if(inventoryManager.SelectedItem.IsEquipment() == false) return;
            if(!inventoryManager.isDragging) return;
            if (inventoryManager.isSlotChanged == false) return;
            
            inventoryManager.isSlotChanged = false;
            inventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.statData);
            inventoryManager.Inventory.itemInventory.Add(_itemDataSO);
            SetItemData(null);
            
            inventoryManager.Inventory.OnEquipmentChanged -= HandleStatChange;
            inventoryManager.UpdateEquipInfoUI();
        }

        private void HandleStatChange()
        {
            if (_itemDataSO.IsEquipment() == false) return;
            
            inventoryManager.UpdateEquipInfoUI();
            
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
        
        public override void OnPointerUp(PointerEventData eventData) {}
    }
}
