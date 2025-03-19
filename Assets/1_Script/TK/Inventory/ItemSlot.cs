using System;
using DG.Tweening;
using Swift_Blade.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ItemSlot : MonoBehaviour,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region UI region

        [SerializeField] protected Image itemImage;
        [SerializeField] protected Image accentFrame;
        [SerializeField] protected Sprite emptySprite;
        [SerializeField] protected TextMeshProUGUI countText;

        #endregion
        
        protected ItemDataSO _itemDataSO;
        
        protected InventoryManager _inventoryManager => InventoryManager.Instance;
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //현재 드래그 중이면 이미 선택된 아이템이 존재한다는 뜻이므로 return
            if (_inventoryManager.IsDragging) 
                return; 
            
            //빈 슬롯인 경우에 클릭해도 의미가 없기 때문에 return
            if (IsEmptySlot())
                return;
            
            if (eventData.button == PointerEventData.InputButton.Right) //우클릭으로 사용
                TryEquipEquipment();
        }

        private void TryEquipEquipment()
        {
            //장비는 인벤토리에 1개만 있다고 가정하고 만든 것
            if (_itemDataSO.itemType == ItemType.EQUIPMENT)
            {
                if (InventoryManager.Inventory
                    .currentEquipment.Contains(_itemDataSO.equipmentData))
                {
                    TryEquipDuplicatedEquipment();
                    return;
                }
                    
                InventoryManager.EquipmentDatas.Add(_itemDataSO);
                InventoryManager.Inventory.currentEquipment.Add(_itemDataSO.equipmentData);
                _inventoryManager.GetMatchTypeEquipSlot(
                    _itemDataSO.equipmentData.slotType).SetItemData(_itemDataSO);
                InventoryManager.Inventory.itemInventory.Remove(_itemDataSO);
                    
                BaseEquipment baseEquip = _itemDataSO.itemObject as BaseEquipment;
                baseEquip?.OnEquipment();
            
                _itemDataSO = null;
                    
                _inventoryManager.UpdateAllSlots();
            }
        }

        private void TryEquipDuplicatedEquipment()
        {
            PopupUI popup = PopupManager.Instance.GetPopupUI(PopupType.Text);
            TextPopup textPopup = (TextPopup)popup;
            textPopup.SetText("이미 장착중이다.");
            PopupManager.Instance.DelayPopup(PopupType.Text, 2f, () =>
            {
                PopupManager.Instance.PopDown(PopupType.Text);
            });
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _inventoryManager.UpdateInfoUI(_itemDataSO);
            
            if (!accentFrame.gameObject.activeSelf)
                accentFrame.gameObject.SetActive(true);

            transform.DOKill();
            transform.DOScale(1.06f, 0.5f);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _inventoryManager.UpdateInfoUI(null);
            
            if (accentFrame.gameObject.activeSelf)
                accentFrame.gameObject.SetActive(false);
            
            transform.DOScale(1f, 0.5f);
        }

        public void SetItemImage(Sprite sprite)
        {
            if (sprite == null)
            {
                itemImage.sprite = emptySprite;
                itemImage.color = Color.black;
                return;
            }

            itemImage.color = Color.white;
            itemImage.sprite = sprite;

            if (this is not EquipmentSlot)
            {
                if (_itemDataSO.itemType == ItemType.EQUIPMENT)
                {
                    countText.text = string.Empty;
                    return;
                }
                
                int count = _inventoryManager.GetItemCount(_itemDataSO);
                if (count == -1)
                {
                    SetItemImage(null);
                    countText.text = string.Empty;
                }
            }
        }
        
        public bool IsEmptySlot() => _itemDataSO == null;

        public ItemDataSO GetSlotItemData()
        {
            if (_itemDataSO == null)
                return null;
            
            return _itemDataSO;
        }

        public virtual void SetItemData(ItemDataSO newItemData)
        {
            _itemDataSO = newItemData;
            
            int count = _inventoryManager.GetItemCount(_itemDataSO);

            if (count == -1)
            {
                if (_itemDataSO.itemType == ItemType.EQUIPMENT)
                    return;
            }
            
            countText.text = count.ToString();
        }
    }
}
