using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
        [SerializeField] private QuickSlotUI         quickSlotUI;
        [SerializeField] private List<EquipmentSlot> equipSlots;

        [Header("Item Information")]
        [SerializeField] private Image           itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemTypeInfo;
        
        //-------------------------------------------------------------
        
        [HideInInspector] 
        public bool  isSlotChanged = false; 
        private bool _isDragging = false;

        [SerializeField] private List<ItemSlot>  itemSlots = new List<ItemSlot>();
        private Dictionary<ItemDataSO, int> _itemDatas = new();
        private List<ItemDataSO> _itemTable = new();
        private int _currentItemIndex = 0;
        
        public bool IsDragging { get => _isDragging; set => _isDragging = value; }
        public ItemDataSO QuickSlotItem { get; set; }
        public static PlayerInventory Inventory { get; set; }
        public static readonly List<ItemDataSO> EquipmentDatas = new List<ItemDataSO>(5);
        public static bool IsAfterInit = false;

        private void Start()
        {
            if (IsAfterInit == false)
                return;
            
            InitializeSlots();
        }

        public void InitializeSlots()
        {
            _currentItemIndex = 0;

            Inventory.itemSlots = new List<ItemSlot>();
            
            for (int i = 0; i < itemSlots.Count; i++)
                Inventory.itemSlots.Add(itemSlots[i]);

            for (int i = 0; i < EquipmentDatas.Count; i++)
            {
                var slot = GetMatchTypeEquipSlot(EquipmentDatas[i].equipmentData.slotType);
                slot.SetItemData(EquipmentDatas[i]);
            }
            
            //�κ��丮�� ������ �����͸� ���Կ� �־��ֱ� (���â ����)
            for (int i = 0; i < Inventory.itemInventory.Count; i++)
            {
                ItemSlot matchSlot = GetMatchItemSlot(Inventory.itemInventory[i]);
                ItemSlot emptySlot = GetEmptySlot();

                ItemDataSO currentIndexItem = Inventory.itemInventory[i];
                
                //������ ����� ���� item�� ��Ƴ���
                if (currentIndexItem.itemType == ItemType.ITEM)
                {
                    if (_itemDatas.ContainsKey(currentIndexItem))
                    {
                        _itemDatas[currentIndexItem]++;
                        continue;
                    }
                    if (!_itemTable.Contains(currentIndexItem))
                        _itemTable.Add(currentIndexItem);
                    
                    _itemDatas.Add(currentIndexItem, 1);
                }

                if (matchSlot != null)
                {
                    matchSlot.SetItemData(Inventory.itemInventory[i]);
                    Inventory.itemInventory[i].ItemSlot = matchSlot;
                    continue;
                }
                
                emptySlot.SetItemData(Inventory.itemInventory[i]);
                Inventory.itemInventory[i].ItemSlot = emptySlot;
            }

            QuickSlotItem = _itemTable[_currentItemIndex];
            UpdateQuickSlotUI(QuickSlotItem);
            
            UpdateAllSlots();
        }

        // private void Start()
        // {
        //     Player.Instance.GetEntityComponent<PlayerHealth>()
        //         .OnDeadEvent.AddListener(Inventory.Initialize);
        // }
        //
        // private void OnDisable()
        // {
        //     Player.Instance.GetEntityComponent<PlayerHealth>()
        //         .OnDeadEvent.AddListener(Inventory.Initialize);
        // }

        private void Update()
        {
            //�ӽ� ������ Ű
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (QuickSlotItem == null)
                    return;
                
                QuickSlotItem.itemObject.ItemEffect(Player.Instance);
                
                //������ �� ���� �Ѿ��
                if (--_itemDatas[QuickSlotItem] <= 0)
                {
                    _itemDatas.Remove(QuickSlotItem);
                    _itemTable.Remove(QuickSlotItem);
                    Inventory.itemInventory.Remove(QuickSlotItem);
                    
                    ChangeQuickSlotItem();
                    UpdateAllSlots();
                }
                UpdateQuickSlotUI(QuickSlotItem);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                quickSlotUI.transform.DOKill();
                quickSlotUI.transform.DOShakeScale(0.2f, Vector3.one * 1.03f);
                ChangeQuickSlotItem();
            }
        }

        private void ChangeQuickSlotItem()
        {
            if (_itemTable.Count == 0)
            {
                QuickSlotItem = null;
                UpdateQuickSlotUI(QuickSlotItem);
                return;
            }
                
            if (_currentItemIndex >= _itemTable.Count - 1)
                _currentItemIndex = 0;
            else
                _currentItemIndex++;
                
            QuickSlotItem = _itemTable[_currentItemIndex];
            UpdateQuickSlotUI(QuickSlotItem);
        }

        public void UpdateAllSlots()
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                //�� �����̸� empty �̹���
                if (itemSlots[i].GetSlotItemData() == null
                    && itemSlots[i] is not EquipmentSlot)
                {
                    itemSlots[i].SetItemImage(null);
                }
                else if (itemSlots[i].GetSlotItemData() == null
                         && itemSlots[i] is EquipmentSlot equipSlot)
                {
                    itemSlots[i].SetItemImage(equipSlot.GetInfoIcon);
                }
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

        public void AddItemToMatchSlot(ItemDataSO newItem)
        {
            if (AllSlotsFull())
            {
                Debug.Log("All inventory slots are full");
                return;
            }
            
            Inventory.itemInventory.Add(newItem);

            var matchSlot = GetMatchItemSlot(newItem);

            if (matchSlot)
            {
                matchSlot.SetItemData(newItem);
                newItem.ItemSlot = matchSlot;
            }
            else
                AddItemToEmptySlot(newItem);
            
            UpdateAllSlots();
        }

        public void AddItemToEmptySlot(ItemDataSO newItem)
        {
            var emptySlot = GetEmptySlot();
            emptySlot.SetItemData(newItem);
            newItem.ItemSlot = emptySlot;
            
            UpdateAllSlots();
        }

        private ItemSlot GetEmptySlot()
        {
            return itemSlots.FirstOrDefault(item => item.IsEmptySlot());
        }

        private ItemSlot GetMatchItemSlot(ItemDataSO item)
        {
            return itemSlots.FirstOrDefault(slot => slot.GetSlotItemData() == item);
        }

        public EquipmentSlot GetMatchTypeEquipSlot(EquipmentSlotType type)
        {
            EquipmentSlot matchSlot = equipSlots.FirstOrDefault(slot => slot.GetSlotType == type);
            
            if (matchSlot == null)
            {
                Debug.LogError($"Doesn't exist match type, typename: {type.ToString()}");
                return default;
            }
            
            if (matchSlot.IsEmptySlot())
                return matchSlot;

            //Original item need to go to the inventory
            ItemDataSO tempItemData = matchSlot.GetSlotItemData();
            GetEmptySlot().SetItemData(tempItemData);

            return matchSlot;
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
            
            quickSlotUI.SetIcon(itemData.itemImage);
        }

        public int GetItemCount(ItemDataSO itemData)
        {
            if (_itemDatas.ContainsKey(itemData))
            {
                return _itemDatas[itemData];
            }

            return -1;
        }
    }
}
