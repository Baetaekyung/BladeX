using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ItemSlot : MonoBehaviour,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private string slotID;
        
        [SerializeField] protected Image itemImage;
        [SerializeField] protected Sprite emptySprite;
        protected ItemDataSO _itemDataSO;
        protected InventoryManager inventoryManager => InventoryManager.Instance;
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //현재 드래그 중이면 이미 선택된 아이템이 존재한다는 뜻이므로 return
            if (inventoryManager.isDragging) return; 
            //빈 슬롯인 경우에 클릭해도 의미가 없기 때문에 return
            if (IsEmptySlot()) return;
            
            inventoryManager.isDragging = true; //드래그 중이라고 표시하기
            inventoryManager.SelectedItem = _itemDataSO; //현재 선택된 아이템은 이 슬롯의 아이템
            inventoryManager.currentSlotID = this.slotID; //현재 선택된 슬롯의 ID는 이 슬롯의 ID
            
            inventoryManager.CreateUIObject(_itemDataSO); //커서에 UI 생성
            _itemDataSO = null; //이 슬롯 비워주기
            itemImage.sprite = emptySprite; //슬롯 UI 비워주기
            
            inventoryManager.UpdateAllSlots(); //슬롯 UI 적용시키기
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!inventoryManager.isDragging) return; //드래그 중이 아닐때는 return
            if (_itemDataSO != null) //이 슬롯에 아이템이 존재할 경우는 Change 불가 (나중에 가능하게 만들기)
            {
                inventoryManager.isSlotChanged = false;
                return;
            }

            inventoryManager.isSlotChanged = true; //슬롯에 변경이 일어남
            inventoryManager.currentSlotID = slotID; //현재 슬롯 ID 변경하기
            
            SetItemData(inventoryManager.SelectedItem); //이 슬롯의 Data를 선택된 ItemData로 변경
            inventoryManager.SelectedItem.SetSlot(this); //아이템의 Slot을 이 슬롯으로 변경
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if(!inventoryManager.isDragging) return;
            if (inventoryManager.SelectedItem != _itemDataSO) return;

            inventoryManager.isSlotChanged = false;
            SetItemData(null);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (inventoryManager.SelectedItem == null && IsEmptySlot()) return;
            
            if (inventoryManager.isSlotChanged == false)
                ResetItems();
            
            inventoryManager.UpdateAllSlots();
            inventoryManager.DeselectItem();
        }

        private void ResetItems() //아이템 제자리로
        {
            SetItemData(inventoryManager.SelectedItem);
            inventoryManager.SelectedItem.SetSlot(this);
                
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
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            Guid guid = Guid.NewGuid();
            slotID = guid.ToString();
        }
#endif
    }
}
