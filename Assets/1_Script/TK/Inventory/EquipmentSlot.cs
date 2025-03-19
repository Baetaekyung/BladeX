using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public class EquipmentSlot : ItemSlot
    {
        [SerializeField] private EquipmentSlotType slotType;
        [SerializeField] private Sprite infoIcon;
        
        public EquipmentSlotType GetSlotType => slotType;
        public Sprite            GetInfoIcon => infoIcon;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1.05f, 0.2f);

            if (_itemDataSO == null)
                return;
            
            _inventoryManager.UpdateInfoUI(_itemDataSO);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1f, 0.2f);
            
            if (_itemDataSO == null)
                return;
            
            _inventoryManager.UpdateInfoUI(null);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_itemDataSO == null)
                return;
            
            if (eventData.button != PointerEventData.InputButton.Right)
                return;
            
            if (InventoryManager.Inventory.currentEquipment.Contains(_itemDataSO.equipmentData))
            {
                BaseEquipment baseEquip = _itemDataSO.itemObject as BaseEquipment;
                baseEquip?.OffEquipment();

                InventoryManager.EquipmentDatas.Remove(_itemDataSO);
                InventoryManager.Inventory.currentEquipment.Remove(_itemDataSO.equipmentData);
                        
                _inventoryManager.AddItemToEmptySlot(_itemDataSO);
                _itemDataSO = null;
                _inventoryManager.UpdateAllSlots();
            }
        }

        public override void SetItemData(ItemDataSO newItemData)
        {
            _itemDataSO = newItemData;
            _inventoryManager.UpdateAllSlots();
        }
    }
}
