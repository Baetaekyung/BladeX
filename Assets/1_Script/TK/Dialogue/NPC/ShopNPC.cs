using System;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class ShopNPC : NPC
    {
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
            PopupManager.Instance.PopUp(PopupType.Shop);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Interact();
            }
        }
    }
}
