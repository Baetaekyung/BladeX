using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        //TODO: ���߿� UI�� Manager��� �и��ϱ�.
        [FormerlySerializedAs("equipInfoUIs")]
        [Header("UI �κ�")]
        [SerializeField] private QuickSlotUI quickSlotUI;
        [SerializeField] private List<EquipmentSlot> equipSlots;

        [Header("Item Information")]
        [SerializeField] private Image           itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemTypeInfo;
        
        //-------------------------------------------------------------
        
        [HideInInspector] public bool isSlotChanged = false; 
        private bool _isDragging = false;

        [SerializeField] private PlayerInventory playerInventory;
        [SerializeField] private List<ItemSlot> itemSlots = new List<ItemSlot>();
        
        public bool IsDragging { get => _isDragging; set => _isDragging = value; }
        public ItemDataSO SelectedItem { get; set; }
        public ItemDataSO QuickSlotItem { get; set; }
        public PlayerInventory Inventory => playerInventory;
        
        protected override void Awake()
        {
            base.Awake();

            playerInventory = playerInventory.Clone();
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            playerInventory.itemSlots = new List<ItemSlot>();
            
            for (int i = 0; i < itemSlots.Count; i++)
                playerInventory.itemSlots.Add(itemSlots[i]);
            
            //�κ��丮�� ������ �����͸� ���Կ� �־��ֱ� (���â ����)
            for (int i = 0; i < playerInventory.itemInventory.Count; i++)
            {
                ItemSlot emptySlot = GetEmptySlot();
                emptySlot.SetItemData(playerInventory.itemInventory[i]);
            }

            UpdateAllSlots();
        }

        private void Start()
        {
            Player.Instance.GetEntityComponent<PlayerHealth>()
                .OnDeadEvent.AddListener(playerInventory.Initialize);
        }

        private void OnDisable()
        {
            Player.Instance.GetEntityComponent<PlayerHealth>()
                .OnDeadEvent.AddListener(playerInventory.Initialize);
        }

        private void Update()
        {
            //�ӽ� ������ Ű
            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                QuickSlotItem.itemObject.ItemEffect(Player.Instance as Player);
                QuickSlotItem = null;
                UpdateQuickSlotUI(null);
            }
        }

        public void UpdateAllSlots()
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].GetSlotItemData() == null) //�� �����̸� empty �̹���
                    itemSlots[i].SetItemImage(null);
                else //�������� �����ϸ� itemImage �־��ֱ�
                {
                    Sprite itemIcon = itemSlots[i].GetSlotItemData().itemImage;
                    itemSlots[i].SetItemImage(itemIcon);
                }
            }
        }

        //�������� Ŭ������ �� Ŀ���� ǥ�õǴ� UI
        public void UpdateInfoUI(ItemDataSO itemData)
        {
            SetInfoUI(itemData);
        }

        private void SetInfoUI(ItemDataSO itemData)
        {
            itemIcon.sprite      = itemData ? itemData.itemImage : null;
            itemIcon.color       = itemData ? Color.white : Color.clear;
            itemName.text        = itemData ? itemData.itemName : String.Empty;
            itemDescription.text = itemData ? itemData.description : String.Empty;
            itemTypeInfo.text    = itemData ? itemData.itemType.ToString() : String.Empty;
        }
        
        //Mouse Up�� ���� �� �߻�
        public void DeselectItem()
        {
            // _createdItemUI.gameObject.SetActive(false);

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

        public EquipmentSlot GetEmptyEquipSlot()
        {
            return equipSlots.FirstOrDefault(slot => slot.IsEmptySlot());
        }
        
        public bool AllSlotsFull()
        {
            if (itemSlots.FirstOrDefault(item => item.IsEmptySlot()) == default)
            {
                return true;
            }
            return false;
        }

        public void UpdateQuickSlotUI(ItemDataSO itemData)
        {
            if (itemData == null)
            {
                quickSlotUI.SetIcon(null);
                return;
            }
            
            if (QuickSlotItem != null)
            {
                ItemSlot newSlot = GetEmptySlot();
                newSlot.SetItemData(itemData);
            }
            
            quickSlotUI.SetIcon(itemData.itemImage);
            QuickSlotItem = itemData;
        }
    }
}
