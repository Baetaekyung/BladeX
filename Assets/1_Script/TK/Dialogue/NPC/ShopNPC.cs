using System;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class ShopNPC : NPC
    {
        private Shop _shop;

        [SerializeField] private ItemTableSO shopItems;
        
        private void Awake()
        {
            _shop = FindFirstObjectByType<Shop>(FindObjectsInactive.Include);
            _isRewarded = false;
        }

        [ContextMenu("Interact")]
        public override void Interact()
        {
            TalkWithNPC(HandleOpenShop);
        }

        private void HandleOpenShop()
        {
            _shop.SetItems(shopItems);
            Debug.Log("handle open shop");
            
            PopupManager.Instance.PopUp(PopupType.Shop);
        }
    }
}
