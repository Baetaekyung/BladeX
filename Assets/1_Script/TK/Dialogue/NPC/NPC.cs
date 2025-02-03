using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] protected DialogueDataSO dialogueData;
        
        protected bool  _isAlreadyRead = false;

        [Header("Dialogue end Event")] 
        public UnityEvent OnDialogueEndEvent;
        
        public virtual void Interact()
        {
            if (_isAlreadyRead) //이미 보상을 받았음
            {
                //이벤트 없이 다이얼로그만 진행
                DialogueManager.Instance.DoDialog(dialogueData);
                return;
            }
            
            DialogueManager.Instance.DoDialog(dialogueData).OnComplete(HandleEndEventRegister);
        }

        protected void HandleEndEventRegister()
        {
            _isAlreadyRead = true;
            OnDialogueEndEvent?.Invoke();
        }
    }
}
