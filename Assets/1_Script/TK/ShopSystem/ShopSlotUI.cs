using System;
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
        private TextMeshProUGUI _buttonText;
        
        [SerializeField] private GameObject soldOutPanel;

        private ItemDataSO _currentItem;
        private int _itemCount;
        private int _itemCost;
        
        public void SetSlotItem(ItemDataSO newItem, int count, int cost)
        {
            _itemCost = cost;
            _currentItem = newItem;
            _itemCount = count;
            
            _buttonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
            _buttonText.text = $"Buy - {_itemCost.ToString()}$";
            
            remainCount.text = $"remain: {count.ToString()}";
            itemIcon.sprite = newItem.itemImage;
            itemNameText.text = newItem.itemName;
            itemDescriptionText.text = newItem.description;
        }

        private void GiveItemToPlayer()
        {
            if (_currentItem == null)
                return;

            if (inventory.Currency < _itemCost)
            {
                Debug.Log("소유중인 자원이 Item의 가격보다 적음");
                return;
            }
            
            if (inventory.currentInventoryCapacity == inventory.maxInventoryCapacity)
            {
                Debug.Log("Inventory 가득참");
                return;
            }

            inventory.itemInventory.Add(_currentItem);
            inventory.currentInventoryCapacity++;
        }
        
        private void OnEnable()
        {
            buyButton.onClick.AddListener(Buy);
            buyButton.onClick.AddListener(GiveItemToPlayer);
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(Buy);
            buyButton.onClick.RemoveListener(GiveItemToPlayer);
        }

        public void Buy()
        {
            _itemCount--;
            remainCount.text = $"remain: {_itemCount.ToString()}";
            
            if(_itemCount <= 0)
                soldOutPanel.SetActive(true);
        }
    }
}
