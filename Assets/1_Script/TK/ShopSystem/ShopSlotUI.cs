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
        
        [SerializeField] private GameObject soldOutPanel;

        private ItemDataSO _currentItem;
        
        public void SetSlotItem(ItemDataSO newItem)
        {
            _currentItem = newItem;
            
            itemIcon.sprite = newItem.itemImage;
            itemNameText.text = newItem.itemName;
            itemDescriptionText.text = newItem.description;
        }

        private void GiveItemToPlayer()
        {
            if (_currentItem == null)
                return;

            if (inventory.currentInventoryCapacity == inventory.maxInventoryCapacity)
                return;

            inventory.itemInventory.Add(_currentItem);
        }
        
        private void OnEnable()
        {
            buyButton.onClick.AddListener(SoldOut);
            buyButton.onClick.AddListener(GiveItemToPlayer);
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(SoldOut);
            buyButton.onClick.RemoveListener(GiveItemToPlayer);
        }

        public void SoldOut() => soldOutPanel.SetActive(true);
    }
}
