using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public class EquipmentSlot : ItemSlot
    {
        [SerializeField] private EquipmentSlotType slotType;
        [SerializeField] private Sprite            infoIcon;

        private WeaponSO _weaponData;
        public EquipmentSlotType GetSlotType => slotType;
        public Sprite            GetInfoIcon => infoIcon;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (transform != null)
            {
                transform.DOKill();
                transform.DOScale(1.05f, 0.2f);
            }

            if (!_itemDataSO)
                return;
            
            if(GetSlotType == EquipmentSlotType.WEAPON)
            {
                InvenManager.UpdateItemInformationUI(_weaponData);
            }
            else
            {
                InvenManager.UpdateItemInformationUI(_itemDataSO);
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if(transform != null)
            {
                transform.DOKill();
                transform.DOScale(1f, 0.2f);
            }
            
            if (!_itemDataSO)
                return;
            
            if(GetSlotType != EquipmentSlotType.WEAPON)
            {
                InvenManager.UpdateItemInformationUI(itemData: null);
            }
            else
            {
                InvenManager.UpdateItemInformationUI(weapon: null);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(transform != null)
                transform.DOKill();

            if (!_itemDataSO)
                return;
            
            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            if (GetSlotType == EquipmentSlotType.WEAPON)
                return;
            
            if (InventoryManager.Inventory.currentEquipment.Contains(_itemDataSO.equipmentData))
                OffEquipment();
        }

        private void OffEquipment()
        {
            if (GetSlotType == EquipmentSlotType.WEAPON)
                return;

            var baseEquip = _itemDataSO.itemObject as Equipment;
            baseEquip?.OffEquipment();

            InventoryManager.EquipmentDatas.Remove(_itemDataSO);
            InventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.equipmentData);

            InvenManager.AddItemToEmptySlot(_itemDataSO);

            _itemDataSO = null;
            InvenManager.UpdateAllSlots();
        }

        public override void SetItemData(ItemDataSO newItemData)
        {
            if (GetSlotType == EquipmentSlotType.WEAPON)
                return;

            _itemDataSO = newItemData;

            InvenManager.UpdateAllSlots();
        }

        public void SetWeaponData(WeaponSO weapon)
        {
            if (GetSlotType != EquipmentSlotType.WEAPON)
                return;

            _weaponData = weapon;

            InvenManager.UpdateAllSlots();
        }
    }
}
