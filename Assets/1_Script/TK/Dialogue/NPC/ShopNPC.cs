using System;
using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class ShopNPC : NPC
    {
        private readonly int talk1AnimationHash = Animator.StringToHash("Talk1");
        private readonly int talk2AnimationHash = Animator.StringToHash("Talk2");

        [SerializeField] private ItemTableSO shopItems;
        [SerializeField] private Shop        shop;
        [SerializeField] private int         itemCount = 6;

        private bool _isShopSetted = false;

        private Animator animator;
        
        private void Awake()
        {
            if(transform.childCount > 0)
                animator = transform.GetChild(0).GetComponent<Animator>();

            shopItems = shopItems.GetClonedItemTable();
            _isShopSetted = false;
        }

        public override void Interact()
        {
            TalkWithNPC(HandleOpenShop);

            if(animator != null)
                PlayRandomAnimation();
        }

        protected override void TalkWithNPC(Action dialogueEndEvent = null)
        {
            DialogueManager.Instance.StartDialogue(dialogueData).Subscribe(HandleDialogueEndEvent);
            
            void HandleDialogueEndEvent()
            {
                if (animator != null)
                    ClearAnimation();

                dialogueEndEvent?.Invoke();
                OnDialogueEndEvent?.Invoke();
            }
        }

        private void HandleOpenShop()
        {
            PopupManager.Instance.PopUp(PopupType.Shop);
            SetShop();
        }

        private void SetShop()
        {
            if (_isShopSetted == false)
            {
                shop.SetItems(shopItems, itemCount);
                _isShopSetted = true;
            }
        }

        private void PlayRandomAnimation()
        {
            ClearAnimation();
            int randAnimation = UnityEngine.Random.Range(0, 2);
            int hash = randAnimation == 0 ? talk1AnimationHash : talk2AnimationHash;

            animator.SetBool(hash, true);
        }

        private void ClearAnimation()
        {
            animator.SetBool(talk1AnimationHash, false);
            animator.SetBool(talk2AnimationHash, false);
        }
    }
}
