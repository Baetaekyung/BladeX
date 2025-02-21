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
            _shop = FindFirstObjectByType<Shop>(FindObjectsInactive.Include);//todo : change this
        }

        [ContextMenu("Interact")]
        public override void Interact()
        {
            if (_isAlreadyRead) //이미 보상을 받았음
            {
                //이벤트 없이 다이얼로그만 진행
                DialogueManager.Instance.DoDialog(dialogueData).OnComplete(HandleOpenShop);
                return;
            }
            
            DialogueManager.Instance.DoDialog(dialogueData).OnComplete(() =>
            {
                HandleOpenShop();
                HandleEndEventRegister();
            });
        }

        private void HandleOpenShop()
        {
            _shop.SetItems(shopItems);
            
            PopupManager.Instance.PopUp(PopupType.Shop);
        }
    }
}
