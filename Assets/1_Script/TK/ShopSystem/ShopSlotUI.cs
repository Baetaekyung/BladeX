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
            
            _buttonText.text = $"{_itemCost.ToString()}코인";
            remainCount.text = $"남은 갯수: {count.ToString()}";
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
                Debug.Log("소유중인 자원이 Item의 가격보다 적음");
                return;
            }
            
            if (inventory.currentInventoryCapacity == inventory.maxInventoryCapacity)
            {
                Debug.Log("Inventory 가득참");
                return;
            }
            
            BuyAnimation();
            
            _itemCount--;
            remainCount.text = $"남은 갯수: {_itemCount.ToString()}";
            
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
