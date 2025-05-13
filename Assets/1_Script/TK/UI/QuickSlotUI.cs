using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class QuickSlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI countText;

        private void Awake()
        {
            icon.color = Color.clear;
        }

        private void Start()
        {
            if (icon.sprite == null)
                icon.color = Color.clear;
            else
                icon.color = Color.white;

            InventoryManager.Instance.OnUseQuickSlotEvent += HandleChangeText;
        }

        private void HandleChangeText()
        {
            if(InventoryManager.Instance.QuickSlotItem == null)
            {
                countText.text = string.Empty;
                return;
            }

            int count = InventoryManager.Instance.GetItemCount(InventoryManager.Instance.QuickSlotItem);
            countText.text = count.ToString();
        }

        public void SetIcon(Sprite newSprite)
        {   
            icon.color  = newSprite  ? Color.white : Color.clear;
            icon.sprite = newSprite ? newSprite   : null;
        }
    }
}
