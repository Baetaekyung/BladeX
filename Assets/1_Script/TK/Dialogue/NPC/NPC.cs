using System;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] protected DialogueDataSO dialogueData;
        
        protected bool _isRewarded = false;

        [Header("Dialogue end Event")] 
        public UnityEvent OnDialogueEndEvent;

        public virtual void Interact()
        {
            TalkWithNPC();
        }
        void IInteractable.OnEndCallbackSubscribe(Action onEndCallback)
        {
            // DialogueManager.Instance.Subscribe(onEndCallback);
            //ad += onEndCallback;
        }
        void IInteractable.OnEndCallbackUnsubscribe(Action onEndCallback)
        {
            // DialogueManager.Instance.Desubscribe(onEndCallback);
            //ad -= onEndCallback;
        }
        protected void TalkWithNPC(Action dialogueEndEvent = null)
        {
            //�̹� ������ �޾���
            if (_isRewarded)
            {
                //�̺�Ʈ ���� ���̾�α׸� ����
                DialogueManager.Instance.DoDialogue(dialogueData);
                return;
            }

            DialogueManager.Instance.DoDialogue(dialogueData).Subscribe(HandleDialogueEndEvent);
            
            void HandleDialogueEndEvent()
            {
                _isRewarded = true;
                dialogueEndEvent?.Invoke();
                OnDialogueEndEvent?.Invoke();
            }
        }
    }
}
