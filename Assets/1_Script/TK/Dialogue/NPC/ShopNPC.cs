using System;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class ShopNPC : NPC
    {
        [SerializeField] private ItemTableSO shopItems;
        [SerializeField] private Shop shop;
        
        private void Awake()
        {
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
            PopupManager.Instance.PopUp(PopupType.Shop);
            shop.SetItems(shopItems);
        }
    }
}
