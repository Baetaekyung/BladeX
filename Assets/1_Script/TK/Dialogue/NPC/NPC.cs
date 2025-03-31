using System;
using UnityEngine;
using UnityEngine.Events;

namespace Swift_Blade
{
    public abstract class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] protected DialogueDataSO dialogueData;
        
        [Header("Dialogue end Event")] 
        public UnityEvent OnDialogueEndEvent;

        public virtual void Interact()
        {
            TalkWithNPC();
        }

        protected abstract void TalkWithNPC(Action dialogueEndEvent = null);
    }
}
