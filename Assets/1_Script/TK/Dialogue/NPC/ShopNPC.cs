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
            if (_isAlreadyRead) //�̹� ������ �޾���
            {
                //�̺�Ʈ ���� ���̾�α׸� ����
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
