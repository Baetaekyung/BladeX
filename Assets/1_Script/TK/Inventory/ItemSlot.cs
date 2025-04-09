using DG.Tweening;
using Swift_Blade.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ItemSlot : MonoBehaviour,
        IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region UI region

        [SerializeField] protected Image           itemImage;
        [SerializeField] protected Image           itemBackground;
        [SerializeField] protected Image           accentFrame;
        [SerializeField] protected Sprite          emptySprite;
        [SerializeField] protected TextMeshProUGUI countText;

        #endregion
        
        protected ItemDataSO _itemDataSO;
        
        protected InventoryManager InvenManager => InventoryManager.Instance;
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
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
                if(transform != null)
                    transform.DOKill();

                if (InventoryManager.Inventory.currentEquipment
                    .Contains(_itemDataSO.equipmentData))
                {
                    ShowDuplicatedEquipmentMessage(); //Not error but can't equip
                    return;
                }
                    
                InventoryManager.EquipmentDatas.Add(_itemDataSO);
                InventoryManager.Inventory.currentEquipment.Add(_itemDataSO.equipmentData);
                InventoryManager.Inventory.itemInventory.Remove(_itemDataSO);

                InvenManager
                    .GetMatchTypeEquipSlot(_itemDataSO.equipmentData.slotType)
                    .SetItemData(_itemDataSO);

                var baseEquip = _itemDataSO.itemObject as Equipment;
                baseEquip.OnEquipment();

                _itemDataSO = null;
                    
                InvenManager.UpdateAllSlots();
            }
        }

        private void ShowDuplicatedEquipmentMessage()
        {
            PopupUI popup = PopupManager.Instance.GetPopupUI(PopupType.Text);
            TextPopup textPopup = popup as TextPopup;

            Debug.Assert(textPopup != null, "Popup is not matched with Text Type");

            PopupManager.Instance.LogMessage("이미 장착 중인 장비이다.");
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            InvenManager.UpdateItemInformationUI(_itemDataSO);
            
            if (!accentFrame.gameObject.activeSelf)
                accentFrame.gameObject.SetActive(true);

            if(transform != null)
            {
                transform.DOKill();
                transform.DOScale(1.06f, 0.5f);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            InvenManager.UpdateItemInformationUI(null);
            
            if (accentFrame.gameObject.activeSelf)
                accentFrame.gameObject.SetActive(false);
            
            if(transform != null)
            {
                transform.DOKill();
                transform.DOScale(1f, 0.5f);
            }
        }

        public void SetItemUI(Sprite sprite)
        {
            SetBackgroundColor();

            if (sprite == null)
            {
                itemImage.sprite = emptySprite;
                itemImage.color = Color.clear;
            }
            else
            {
                SetItemImage(sprite);
            }
        }

        private void SetItemImage(Sprite sprite)
        {
            if (this is not EquipmentSlot)
            {
                itemImage.color = Color.white;
                itemImage.sprite = sprite;

                if (_itemDataSO.itemType == ItemType.EQUIPMENT)
                {
                    countText.text = string.Empty;
                    return;
                }

                int count = InvenManager.GetItemCount(_itemDataSO);

                if (count == -1)
                {
                    SetItemUI(null);
                    countText.text = string.Empty;
                }
            }
            else if (this is EquipmentSlot equipmentSlot)
            {
                if (sprite == equipmentSlot.GetInfoIcon)
                    itemImage.color = new Color(1, 1, 1, 0.2f);
                else
                    itemImage.color = Color.white;

                itemImage.sprite = sprite;
            }
        }

        private void SetBackgroundColor()
        {
            if(_itemDataSO == null)
            {
                itemBackground.color = Color.clear;
                return;
            }

            if (_itemDataSO.itemType == ItemType.EQUIPMENT)
            {
                EquipmentData equipData = _itemDataSO.equipmentData;

                (int a, int b, int c) = ColorUtils.GetRGBColor(equipData.colorType);
                Color newColor = new Color(a, b, c, 0.26f);
                itemBackground.color = newColor;
            }
        }

        public virtual void SetItemData(ItemDataSO newItemData)
        {
            _itemDataSO = newItemData;
            
            int count = InvenManager.GetItemCount(newItemData);

            if (count == -1)
            {
                if (newItemData.itemType == ItemType.EQUIPMENT)
                    return;
            }
            
            countText.text = count.ToString();
        }

        public ItemDataSO GetSlotItemData() => _itemDataSO ? _itemDataSO : null;

        public bool IsEmptySlot() => _itemDataSO == null;
    }
}
