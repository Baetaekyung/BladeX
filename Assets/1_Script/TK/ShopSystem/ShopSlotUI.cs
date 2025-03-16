using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ShopSlotUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
        
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI remainCount;
        [SerializeField] private TextMeshProUGUI _buttonText;
        
        [SerializeField] private GameObject soldOutPanel;

        private ItemDataSO _currentItem;
        private int _itemCount;
        private int _itemCost;
        
        public void SetSlotItem(ItemDataSO newItem, int count, int cost)
        {
            _itemCost = cost;
            _currentItem = newItem;
            _itemCount = count;
            
            _buttonText.text = $"{_itemCost.ToString()}����";
            remainCount.text = $"���� ����: {count.ToString()}";
            itemIcon.sprite = newItem.itemImage;
            itemNameText.text = newItem.itemName;
            itemDescriptionText.text = newItem.description;
        }
        
        private void OnEnable()
        {
            buyButton.onClick.AddListener(Buy);
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(Buy);
        }

        public void Buy()
        {
            if (_itemCount <= 0)
                return;
            
            if (_currentItem == null)
                return;

            if (inventory.Coin < _itemCost)
            {
                Debug.Log("�������� �ڿ��� Item�� ���ݺ��� ����");
                return;
            }
            
            if (inventory.currentInventoryCapacity == inventory.maxInventoryCapacity)
            {
                Debug.Log("Inventory ������");
                return;
            }
            
            BuyAnimation();
            
            _itemCount--;
            remainCount.text = $"���� ����: {_itemCount.ToString()}";
            
            InventoryManager.Instance.AddItemToEmptySlot(_currentItem);
            inventory.currentInventoryCapacity++;
            
            if(_itemCount <= 0)
                soldOutPanel.SetActive(true);
        }

        private void BuyAnimation()
        {
            buyButton.transform.DOKill();

            buyButton.transform.DOShakeScale(0.3f, new Vector3(0.3f, 0.3f, 0));
            buyButton.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 2.5f));
        }
    }
}
