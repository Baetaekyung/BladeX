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
            //���� �巡�� ���̸� �̹� ���õ� �������� �����Ѵٴ� ���̹Ƿ� return
            if (_inventoryManager.IsDragging) 
                return; 
            
            //�� ������ ��쿡 Ŭ���ص� �ǹ̰� ���� ������ return
            if (IsEmptySlot())
                return;
            
            if (eventData.button == PointerEventData.InputButton.Right) //��Ŭ������ ���
                TryEquipEquipment();
        }

        private void TryEquipEquipment()
        {
            //���� �κ��丮�� 1���� �ִٰ� �����ϰ� ���� ��
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
            textPopup.SetText("�̹� �������̴�.");
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
