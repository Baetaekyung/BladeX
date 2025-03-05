using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ItemSlot : MonoBehaviour,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] protected Image itemImage;
        [SerializeField] protected Sprite emptySprite;
        protected ItemDataSO _itemDataSO;
        protected InventoryManager inventoryManager => InventoryManager.Instance;

        protected bool _useItem = false;
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //���� �巡�� ���̸� �̹� ���õ� �������� �����Ѵٴ� ���̹Ƿ� return
            if (inventoryManager.IsDragging) return; 
            //�� ������ ��쿡 Ŭ���ص� �ǹ̰� ���� ������ return
            if (IsEmptySlot()) return;

            if (eventData.button == PointerEventData.InputButton.Right) //��Ŭ������ ���
            {
                if (_itemDataSO.itemType == ItemType.EQUIPMENT)
                {
                    _useItem = true;
                    return; //��ȸ�� �������� �ƴϱ� ������ return
                }
                
                _itemDataSO.itemObject.ItemEffect();
                inventoryManager.Inventory.itemInventory.Remove(_itemDataSO);

                _itemDataSO = null;
                itemImage.sprite = emptySprite;
                
                inventoryManager.UpdateAllSlots();

                _useItem = true;
                return;
            }
            
            inventoryManager.UpdateCursorUI(_itemDataSO); //Ŀ���� UI ����
            
            inventoryManager.IsDragging = true; //�巡�� ���̶�� ǥ���ϱ�
            inventoryManager.SelectedItem = _itemDataSO; //���� ���õ� �������� �� ������ ������
            
            _itemDataSO = null; //�� ���� ����ֱ�
            itemImage.sprite = emptySprite; //���� UI ����ֱ�
            
            inventoryManager.UpdateAllSlots(); //���� UI �����Ű��
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            inventoryManager.UpdateCursorUI(_itemDataSO); //Ŀ���� UI ����
            
            if (!inventoryManager.IsDragging) return; //�巡�� ���� �ƴҶ��� return
            if (_itemDataSO != null) //�� ���Կ� �������� ������ ���� Change �Ұ� (���߿� �����ϰ� �����)
            {
                inventoryManager.isSlotChanged = false;
                return;
            }
            
            inventoryManager.isSlotChanged = true; //���Կ� ������ �Ͼ
            
            SetItemData(inventoryManager.SelectedItem); //�� ������ Data�� ���õ� ItemData�� ����
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            inventoryManager.DisableCursor();
            
            if(!inventoryManager.IsDragging) return;
            if (inventoryManager.SelectedItem != _itemDataSO) return;
            if (inventoryManager.isSlotChanged == false) return;
            
            inventoryManager.isSlotChanged = false;
            SetItemData(null);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            inventoryManager.DisableCursor();
            
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

        private void ResetItems() //������ ���ڸ���
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
