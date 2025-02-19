using System;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class EquipmentSlot : ItemSlot
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (inventoryManager.SelectedItem == null) 
                return;
            
            if (inventoryManager.SelectedItem.IsEquipment() == false) 
                return;
            
            if (inventoryManager.Inventory.currentEquipment.Contains(inventoryManager.SelectedItem.equipmentData))
            {
                Debug.Log("이미 장착중인 아이템" + inventoryManager.SelectedItem.itemName);
                PopupUI popup = PopupManager.Instance.GetPopupUI(PopupType.Text);
                TextPopup textPopup = (TextPopup)popup;
                textPopup.SetText("You already equip this equipment!");
                PopupManager.Instance.DelayPopup(PopupType.Text, 2f, () =>
                {
                    PopupManager.Instance.PopDown(PopupType.Text);
                });
                return; //중복 장착 방지
            }
            
            if (_itemDataSO != null)
                return;
            
            base.OnPointerEnter(eventData);
            
            inventoryManager.Inventory.currentEquipment.Add(_itemDataSO.equipmentData);
            inventoryManager.Inventory.itemInventory.Remove(_itemDataSO);
            
            inventoryManager.UpdateEquipInfoUI();
            
            BaseEquipment baseEquip = _itemDataSO.itemObject as BaseEquipment;
            baseEquip?.OnEquipment();
            
            inventoryManager.UpdateAllSlots();
            inventoryManager.DeselectItem();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_itemDataSO != null)
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (inventoryManager.Inventory.currentEquipment.Contains(_itemDataSO.equipmentData))
                    {
                        BaseEquipment baseEquip = _itemDataSO.itemObject as BaseEquipment;
                        baseEquip?.OffEquipment();
                        
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
            if (inventoryManager.SelectedItem == null) 
                return;
            
            if(inventoryManager.SelectedItem.IsEquipment() == false) 
                return;
            
            if(!inventoryManager.IsDragging) 
                return;
            
            if (inventoryManager.isSlotChanged == false)
                return;
            
            if (inventoryManager.Inventory.currentEquipment.Contains(inventoryManager.SelectedItem.equipmentData))
            {
                Debug.Log("이미 장착중인 아이템" + inventoryManager.SelectedItem.itemName);
                return; //중복 장착 방지
            }
            
            BaseEquipment baseEquip = _itemDataSO.itemObject as BaseEquipment;
            baseEquip?.OffEquipment();
            
            inventoryManager.isSlotChanged = false;
            inventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.equipmentData);
            inventoryManager.Inventory.itemInventory.Add(_itemDataSO);
            SetItemData(null);
            
            inventoryManager.UpdateEquipInfoUI();
        }


        public override void OnPointerUp(PointerEventData eventData) {}
    }
}
