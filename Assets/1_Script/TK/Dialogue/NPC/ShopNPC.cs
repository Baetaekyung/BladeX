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
            shopItems = shopItems.Clone();
            _isRewarded = false;
        }

        [ContextMenu("Interact")]
        public override void Interact()
        {
            TalkWithNPC(HandleOpenShop);
        }

        protected override void TalkWithNPC(Action dialogueEndEvent = null)
        {
            DialogueManager.Instance.DoDialogue(dialogueData).Subscribe(HandleDialogueEndEvent);
            
            void HandleDialogueEndEvent()
            {
                dialogueEndEvent?.Invoke();
                OnDialogueEndEvent?.Invoke();
            }
        }

        private void HandleOpenShop()
        {
            PopupManager.Instance.DelayPopup(PopupType.Shop, 0.5f, () => _shop.SetItems(shopItems));
        }
    }
}
