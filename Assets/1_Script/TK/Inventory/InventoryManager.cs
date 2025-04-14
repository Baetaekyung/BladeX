using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
        [Header("UI 부분")]
        [SerializeField] private QuickSlotUI         quickSlotUI;
        [SerializeField] private List<EquipmentSlot> equipSlots;
        [SerializeField] private TextMeshProUGUI     titleText;
        [SerializeField] private SlotChangeButton    inventoryButton;
        [SerializeField] private SlotChangeButton    skillButton;
        [SerializeField] private GameObject          inventoryUI;
        [SerializeField] private GameObject          skillUI;

        [Header("Item Information")]
        [SerializeField] private Image           itemIcon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemTypeInfo;


        //-------------------------------------------------------------

        public static bool IsNewGame = false;

        [SerializeField] private PlayerInventory playerInv;
        [SerializeField] private List<ItemSlot>  itemSlots  = new List<ItemSlot>();
        private Dictionary<ItemDataSO, int>      _itemDatas = new(); //How many item in there?
        private List<ItemDataSO>                 _itemTable = new(); //There is item?

        private int _currentItemIndex = 0;

        public ItemDataSO QuickSlotItem { get; set; }
        public static PlayerInventory  Inventory { get; set; }
        public static List<ItemDataSO> EquipmentDatas = new List<ItemDataSO>();
        
        protected override void Awake()
        {
            base.Awake();

            if (IsNewGame == false)
            {
                Inventory = playerInv.Clone();

                ChangeToInventory();
                EquipmentDatas.Clear();

                IsNewGame = true;
            }
            
            InitializeSlots();
        }

        public void InitializeSlots()
        {
            _currentItemIndex = 0;

            //인벤토리 초기화
            Inventory.itemSlots = new List<ItemSlot>();
            Inventory.currentInventoryCapacity = 0;

            // 빈 인벤토리 슬롯 채워주기
            for (int i = 0; i < itemSlots.Count; i++)
            {
                Inventory.itemSlots.Add(itemSlots[i]);
            }

            // 빈 장비 슬롯 채워주기
            for (int i = 0; i < EquipmentDatas.Count; i++)
            {
                var slot = GetMatchTypeEquipSlot(EquipmentDatas[i].equipmentData.slotType);
                slot.SetItemData(EquipmentDatas[i]);

                (EquipmentDatas[i].itemObject as Equipment).OnEquipment();
            }

            //인벤토리의 아이템 데이터를 슬롯에 넣어주기 (장비창 제외)
            for (int i = 0; i < Inventory.itemInventory.Count; i++)
            {
                ItemSlot matchSlot = GetMatchItemSlot(Inventory.itemInventory[i]);
                ItemSlot emptySlot = GetEmptySlot();

                ItemDataSO currentIndexItem = Inventory.itemInventory[i];

                //퀵슬롯 등록을 위한 item만 모아놓기
                if (currentIndexItem.itemType == ItemType.ITEM)
                {
                    if (_itemTable.Contains(currentIndexItem))
                    {
                        _itemDatas[currentIndexItem]++;
                    }
                    else
                    {
                        _itemTable.Add(currentIndexItem);
                        _itemDatas.Add(currentIndexItem, 1);
                        Inventory.currentInventoryCapacity += 1;
                    }

                    AssignItemToSlot(i, matchSlot, emptySlot);
                }
                else
                {
                    AssignItemToSlot(i, matchSlot, emptySlot);
                    Inventory.currentInventoryCapacity++;
                }
            }

            SetQuickSlotItem();
            UpdateAllSlots();

            static void AssignItemToSlot(int i, ItemSlot matchSlot, ItemSlot emptySlot)
            {
                if (matchSlot != null)
                {
                    matchSlot.SetItemData(Inventory.itemInventory[i]);
                    Inventory.itemInventory[i].ItemSlot = matchSlot;
                }
                else
                {
                    emptySlot.SetItemData(Inventory.itemInventory[i]);
                    Inventory.itemInventory[i].ItemSlot = emptySlot;
                }
            }
        }

        private void SetQuickSlotItem()
        {
            if (_itemTable.Count != 0)
            {
                QuickSlotItem = _itemTable[_currentItemIndex];

                UpdateQuickSlotUI(QuickSlotItem);
            }
            
            UpdateAllSlots();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                UseQuickSlotItem();

            if (Input.GetKeyDown(KeyCode.Tab))
                ChangeQuickSlotItem();
        }

        private void UseQuickSlotItem()
        {
            if (QuickSlotItem == null)
                return;

            if (QuickSlotItem.itemObject.CanUse() == false)
                return;

            QuickSlotItem.itemObject.ItemEffect(Player.Instance);
            _itemDatas[QuickSlotItem]--;

            //아이템 다 쓰면 넘어가기
            if (_itemDatas[QuickSlotItem] <= 0)
            {
                _itemDatas.Remove(QuickSlotItem);
                _itemTable.Remove(QuickSlotItem);
                Inventory.itemInventory.Remove(QuickSlotItem);

                ChangeQuickSlotItem();
                UpdateAllSlots();
            }
            UpdateQuickSlotUI(QuickSlotItem);
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
                if (!itemSlots[i].GetSlotItemData()
                    && itemSlots[i] is EquipmentSlot equipSlot)
                {
                    itemSlots[i].SetItemUI(equipSlot.GetInfoIcon);
                }
                //빈 슬롯이면 empty 이미지
                else if (!itemSlots[i].GetSlotItemData()
                    && itemSlots[i] is not EquipmentSlot)
                {
                    itemSlots[i].SetItemUI(null);
                }
                else //아이템이 존재하면 itemImage 넣어주기
                {
                    Sprite itemIcon = itemSlots[i].GetSlotItemData().itemImage;
                    itemSlots[i].SetItemUI(itemIcon);
                }
            }
        }

        //아이템을 클릭했을 때 커서에 표시되는 UI
        public void UpdateItemInformationUI(ItemDataSO itemData)
        {
            SetInfoUI(itemData);
        }

        public void ChangeToInventory()
        {
            inventoryUI.SetActive(true);
            skillUI.SetActive(false);

            titleText.text = "인벤토리";
        }

        public void ChangeToSkill()
        {
            skillUI.SetActive(true);
            inventoryUI.SetActive(false);
            
            titleText.text = "스킬 슬롯";
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
                //newItem.ItemSlot = matchSlot;
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
            
            var baseEquip = tempItemData.itemObject as Equipment;
            baseEquip?.OffEquipment();
            
            EquipmentDatas.Remove(tempItemData);
            Inventory.currentEquipment.Remove(tempItemData.equipmentData);
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
            if (!itemData)
            {
                quickSlotUI.SetIcon(null);
                return;
            }
            
            quickSlotUI.SetIcon(itemData.itemImage);
        }

        public int GetItemCount(ItemDataSO itemData)
        {
            if (_itemDatas.TryGetValue(itemData, out var count))
            {
                return count;
            }

            return -1;
        }
    }
}
