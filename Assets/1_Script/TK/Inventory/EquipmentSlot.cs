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
            
            inventoryManager.Inventory.currentEquipment.Add(_itemDataSO.equipmentData);
            inventoryManager.Inventory.itemInventory.Remove(_itemDataSO);
            
            _itemDataSO.equipmentObject.OnEquipment();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_itemDataSO != null)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (inventoryManager.Inventory.currentEquipment.Contains(_itemDataSO.equipmentData))
                    {
                        _itemDataSO.equipmentObject.OffEquipment();
                        
                        inventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.equipmentData);
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
            
            _itemDataSO.equipmentObject.OffEquipment();

            inventoryManager.isSlotChanged = false;
            inventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.equipmentData);
            inventoryManager.Inventory.itemInventory.Add(_itemDataSO);
            SetItemData(null);
            
            inventoryManager.UpdateEquipInfoUI();
        }


        public override void OnPointerUp(PointerEventData eventData) {}
    }
}
