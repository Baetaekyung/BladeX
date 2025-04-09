using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public class EquipmentSlot : ItemSlot
    {
        [SerializeField] private EquipmentSlotType slotType;
        [SerializeField] private Sprite            infoIcon;
        
        public EquipmentSlotType GetSlotType => slotType;
        public Sprite            GetInfoIcon => infoIcon;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if(transform != null)
            {
                transform.DOKill();
                transform.DOScale(1.05f, 0.2f);
            }

            if (!_itemDataSO)
                return;
            
            InvenManager.UpdateItemInformationUI(_itemDataSO);
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
            
            InvenManager.UpdateItemInformationUI(null);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(transform != null)
                transform.DOKill();

            if (!_itemDataSO)
                return;
            
            if (eventData.button != PointerEventData.InputButton.Right)
                return;
            
            if (InventoryManager.Inventory.currentEquipment.Contains(_itemDataSO.equipmentData))
                OffEquipment();
        }

        private void OffEquipment()
        {
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
            _itemDataSO = newItemData;

            InvenManager.UpdateAllSlots();
        }
    }
}
