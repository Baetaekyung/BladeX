using DG.Tweening;
using Swift_Blade.Combat.Health;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ShopSlotUI : MonoBehaviour
    {
        private PlayerInventory playerInventory = InventoryManager.Inventory;

        [SerializeField] private CanvasGroup canvasGroup;
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

        public CanvasGroup GetCanvasGroup => canvasGroup;
        
        public void SetSlotItem(ItemDataSO newItem, int count, int cost)
        {
            _itemCost    = cost;
            _currentItem = newItem;
            _itemCount   = count;
            
            _buttonText.text         = $"{_itemCost.ToString()} 체력";
            remainCount.text         = $"남은 갯수: {count.ToString()}";
            itemIcon.sprite          = newItem.itemImage;
            itemNameText.text        = newItem.itemName;
            itemDescriptionText.text = newItem.description;
        }
        
        private void OnEnable()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.alpha = 0;
            buyButton.onClick.AddListener(TryBuy);
        }

        private void OnDestroy()
        {
            canvasGroup.alpha = 0;
            buyButton.onClick.RemoveListener(TryBuy);
        }

        public void TryBuy()
        {
            if (_itemCount <= 0)
            {
                LogFailedMessage("아이템 매진");

                return;
            }
            
            if (!_currentItem)
            {
                LogFailedMessage("아이템 없음");

                return;
            }

            PlayerHealth health = Player.Instance.GetPlayerHealth;
            
            if (health.GetCurrentHealth <= _itemCost)
            {
                health.DescreaseHealth(0);
                LogFailedMessage("체력이 부족합니다.");
                
                return;
            }
            
            if (InventoryManager.Instance.IsAllSlotsFull())
            {
                LogFailedMessage("인벤토리 슬롯 부족");

                return;
            }

            InventoryManager.Instance.AddItemToMatchSlot(_currentItem);
            health.DescreaseHealth(_itemCost);
            
            BuyAnimation();
            
            _itemCount--;
            remainCount.text = $"남은 갯수: {_itemCount.ToString()}";
            
            if(_itemCount <= 0)
                soldOutPanel.SetActive(true);
        }

        private void BuyAnimation()
        {
            buyButton.transform.DOKill();

            buyButton.transform.DOShakeScale(0.3f, new Vector3(0.3f, 0.3f, 0));
            buyButton.transform.DOShakeRotation(0.3f, new Vector3(0, 0, 2.5f));
        }

        private void LogFailedMessage(string message) => PopupManager.Instance.LogMessage(message);
    }
}
