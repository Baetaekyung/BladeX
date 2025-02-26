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

        protected void TalkWithNPC(Action dialogueEndEvent = null)
        {
            if (_isRewarded) //�̹� ������ �޾���
            {
                //�̺�Ʈ ���� ���̾�α׸� ����
                DialogueManager.Instance.DoDialogue(dialogueData).OnComplete(dialogueEndEvent);
                return;
            }

            DialogueManager.Instance.DoDialogue(dialogueData)
                .OnComplete(() =>
                {
                    dialogueEndEvent?.Invoke();
                    HandleEndEventRegister();
                });
        }

        protected void HandleEndEventRegister()
        {
            _isRewarded = true;
            OnDialogueEndEvent?.Invoke();
        }
    }
}
