using System;
using DG.Tweening;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ItemSlot : MonoBehaviour,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected Image accentFrame;
        [SerializeField] protected Sprite emptySprite;
        protected ItemDataSO _itemDataSO;
        protected InventoryManager inventoryManager => InventoryManager.Instance;

        protected bool _useItem = false;
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //현재 드래그 중이면 이미 선택된 아이템이 존재한다는 뜻이므로 return
            if (inventoryManager.IsDragging) 
                return; 
            //빈 슬롯인 경우에 클릭해도 의미가 없기 때문에 return
            if (IsEmptySlot())
                return;
            
            if (eventData.button == PointerEventData.InputButton.Right) //우클릭으로 사용
            {
                if (_itemDataSO.itemType == ItemType.EQUIPMENT)
                {
                    if (inventoryManager.Inventory
                        .currentEquipment.Contains(_itemDataSO.equipmentData))
                    {
                        PopupUI popup = PopupManager.Instance.GetPopupUI(PopupType.Text);
                        TextPopup textPopup = (TextPopup)popup;
                        textPopup.SetText("이미 장착중이다.");
                        PopupManager.Instance.DelayPopup(PopupType.Text, 2f, () =>
                        {
                            PopupManager.Instance.PopDown(PopupType.Text);
                        });

                        inventoryManager.UpdateAllSlots();
                        inventoryManager.DeselectItem();
                    
                        _useItem = true;
                        
                        return; //왜 리턴이 안돼는데
                    }
                    
                    inventoryManager.Inventory.currentEquipment.Add(_itemDataSO.equipmentData);
                    inventoryManager.GetEmptyEquipSlot().SetItemData(_itemDataSO);
                    inventoryManager.Inventory.itemInventory.Remove(_itemDataSO);
                    
                    BaseEquipment baseEquip = _itemDataSO.itemObject as BaseEquipment;
                    baseEquip?.OnEquipment();
            
                    _itemDataSO = null;
                    
                    inventoryManager.UpdateAllSlots();
                    inventoryManager.DeselectItem();
                    
                    _useItem = true;
                    return;
                }
                if (_itemDataSO.itemType == ItemType.ITEM)
                {
                    if (_itemDataSO.useQuickSlot == true)
                    {
                        inventoryManager.UpdateQuickSlotUI(_itemDataSO);
                        _itemDataSO = null;
                        itemImage.color = new Color(1, 1, 1, 0.4f);
                
                        inventoryManager.UpdateAllSlots();

                        _useItem = true;
                        return;
                    }
                    else
                    {
                        _itemDataSO.itemObject.ItemEffect(Player.Instance);
                        _itemDataSO = null;
                        itemImage.sprite = emptySprite;
                
                        inventoryManager.UpdateAllSlots();
                        _useItem = true;
                    }
                }
            }
            
            inventoryManager.UpdateInfoUI(_itemDataSO);
            
            inventoryManager.IsDragging = true; //드래그 중이라고 표시하기
            inventoryManager.SelectedItem = _itemDataSO; //현재 선택된 아이템은 이 슬롯의 아이템
            
            _itemDataSO = null; //이 슬롯 비워주기
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            inventoryManager.UpdateInfoUI(_itemDataSO);
            
            if (!accentFrame.gameObject.activeSelf)
                accentFrame.gameObject.SetActive(true);

            transform.DOKill();
            transform.DOScale(1.06f, 0.5f);
            
            if (!inventoryManager.IsDragging) return; //드래그 중이 아닐때는 return
            if (_itemDataSO != null) //이 슬롯에 아이템이 존재할 경우는 Change 불가 (나중에 가능하게 만들기)
            {
                inventoryManager.isSlotChanged = false;
                return;
            }
            
            inventoryManager.isSlotChanged = true; //슬롯에 변경이 일어남
            
            SetItemData(inventoryManager.SelectedItem); //이 슬롯의 Data를 선택된 ItemData로 변경
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            inventoryManager.UpdateInfoUI(null);
            if (accentFrame.gameObject.activeSelf)
                accentFrame.gameObject.SetActive(false);
            transform.DOScale(1f, 0.5f);
            
            if(!inventoryManager.IsDragging) return;
            if (inventoryManager.SelectedItem != _itemDataSO) return;
            if (inventoryManager.isSlotChanged == false) return;
            
            inventoryManager.isSlotChanged = false;
            SetItemData(null);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            inventoryManager.UpdateInfoUI(null);
            
            if (inventoryManager.SelectedItem == null && IsEmptySlot())
                return;
            
            if (_useItem)
            {
                _useItem = false;
                return;
            }
            
            if (inventoryManager.isSlotChanged == false)
                ResetItems();
            
            inventoryManager.UpdateAllSlots();
            inventoryManager.DeselectItem();
        }

        private void ResetItems() //아이템 제자리로
        {
            SetItemData(inventoryManager.SelectedItem);
                
            inventoryManager.isSlotChanged = false;
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
        }
        
        public bool IsEmptySlot() => _itemDataSO == null;

        public ItemDataSO GetSlotItemData()
        {
            if (_itemDataSO == null)
                return null;
            
            return _itemDataSO;
        }
        
        public void SetItemData(ItemDataSO newItemData)
        {
            if (!newItemData)
                _itemDataSO = null;
            
            _itemDataSO = newItemData;
        }
    }
}
