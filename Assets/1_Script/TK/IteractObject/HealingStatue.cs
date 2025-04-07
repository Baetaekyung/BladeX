using System;
using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class HealingStatue : MonoBehaviour, IInteractable
    {
        [SerializeField] private int            healAmount;
        [SerializeField] private DialogueDataSO dialogueData;
        [SerializeField] private DialogueDataSO afterRewardDialogueData;

        private bool _isRewarded = false;

        private void OnEnable()
        {
            _isRewarded = false;
        }

        public void Interact()
        {
            if (_isRewarded)
                DialogueManager.Instance.StartDialogue(afterRewardDialogueData);
            else
                DialogueManager.Instance.StartDialogue(dialogueData).Subscribe(Heal);
        }

        private void Heal()
        {
            var health = Player.Instance.GetEntityComponent<PlayerHealth>();

            Debug.Assert(health != null, "PlayerHealth Component is missing");
            
            health.TakeHeal(healAmount); 
            _isRewarded = true;
        }
    }
}
