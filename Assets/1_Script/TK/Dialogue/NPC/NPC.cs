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
            if (_isAlreadyRead) //�̹� ������ �޾���
            {
                //�̺�Ʈ ���� ���̾�α׸� ����
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
