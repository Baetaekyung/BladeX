using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade
{
    [Serializable]
    public enum ItemType
    {
        ITEM,
        EQUIPMENT
    }
    
    public class InventoryManager : MonoSingleton<InventoryManager>
    {
        [Header("UI 부분")]
        [SerializeField] private RectTransform canvasTrm;
        [SerializeField] private RectTransform cursorTrm;
        [SerializeField] private SelectItem_UI selectedItemImage;
        [SerializeField] private List<EquipInfoUI> equipInfoUIs = new(4);
        private SelectItem_UI _createdItemUI;
        
        [HideInInspector] public bool isSlotChanged = false; 
        private bool _isDragging = false;
        private bool _isExistUIObject = false;

        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private List<ItemSlot> itemSlots = new List<ItemSlot>();
        
        public bool IsDragging { get => _isDragging; set => _isDragging = value; }
        public ItemDataSO SelectedItem { get; set; }
        public PlayerInventory Inventory => playerInventory;
        
        protected override void Awake()
        {
            base.Awake();
            
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            playerInventory.itemSlots = new List<ItemSlot>();
            
            for (int i = 0; i < itemSlots.Count; i++)
                playerInventory.itemSlots.Add(itemSlots[i]);
            
            //인벤토리의 아이템 데이터를 슬롯에 넣어주기 (장비창 제외)
            for (int i = 0; i < playerInventory.itemInventory.Count; i++)
            {
                ItemSlot emptySlot = GetEmptySlot();
                emptySlot.SetItemData(playerInventory.itemInventory[i]);
            }

            UpdateAllSlots();
        }
        
        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTrm, //RectTransform Object에 부착되어 있어야 작동한다. 
                Input.mousePosition,
                null,
                out Vector2 mousePosInUISpace);
            
            cursorTrm.position = mousePosInUISpace + new Vector2(Screen.width / 2f, Screen.height / 2f);
        }
        
        public void UpdateAllSlots()
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].GetSlotItemData() == null) //빈 슬롯이면 empty 이미지
                    itemSlots[i].SetItemImage(null);
                else //아이템이 존재하면 itemImage 넣어주기
                {
                    Sprite itemIcon = itemSlots[i].GetSlotItemData().itemImage;
                    itemSlots[i].SetItemImage(itemIcon);
                }
            }
        }

        //아이템을 클릭했을 때 커서에 표시되는 UI
        public void UpdateCursorUI(ItemDataSO itemData) 
        {
            void SetCursorUI()
            {
                _createdItemUI.iconImage.sprite = itemData.itemImage;
                _createdItemUI.SetUI(
                    itemData.itemName,
                    itemData.itemType.ToString(),
                    itemData.description);
                _createdItemUI.gameObject.SetActive(true);
            }
            
            if (_isExistUIObject)
            {
                SetCursorUI();
                return;
            }
            _isExistUIObject = true;
            _createdItemUI = Instantiate(selectedItemImage, cursorTrm);
            
            SetCursorUI();
            _createdItemUI.transform.position = cursorTrm.position;
        }

        //Mouse Up을 했을 때 발생
        public void DeselectItem()
        {
            _createdItemUI.gameObject.SetActive(false);

            SelectedItem = null;
            _isDragging = false;
            isSlotChanged = false;
        }

        public void AddItemToEmptySlot(ItemDataSO newItem)
        {
            if (AllSlotsFull())
            {
                Debug.Log("All inventory slots are full");
                return;
            }
            
            playerInventory.itemInventory.Add(newItem);
            
            var emptySlot = GetEmptySlot();
            emptySlot.SetItemData(newItem);
            
            UpdateAllSlots();
        }

        private ItemSlot GetEmptySlot()
        {
            return itemSlots.FirstOrDefault(item => item.IsEmptySlot());
        }

        public bool AllSlotsFull()
        {
            if (itemSlots.FirstOrDefault(item => item.IsEmptySlot()) == default)
            {
                return true;
            }
            return false;
        }

        public void UpdateEquipInfoUI()
        {
            if (Inventory.currentEquipment.Count == 0)
                return;

            for (int j = 0; j < equipInfoUIs.Count; j++)
            {
                equipInfoUIs[j].SetIcon(null);
            }
            
            for (int i = 0; i < Inventory.currentEquipment.Count; i++)
            {
                equipInfoUIs[i].SetIcon(Inventory.currentEquipment[i].equipmentIcon);
            }
        }
    }
}
