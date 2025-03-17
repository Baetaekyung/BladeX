using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade
{
    public class HealingStatue : MonoBehaviour, IInteractable
    {
        [SerializeField] private int healAmount;
        [SerializeField] private DialogueDataSO dialogueData;
        [SerializeField] private DialogueDataSO afterRewardDialogueData;

        private bool _isRewarded = false;

        [ContextMenu("Interact")]
        public void Interact()
        {
            if (_isRewarded)
            {
                DialogueManager.Instance.DoDialogue(afterRewardDialogueData);
            }
            else
            {
                DialogueManager.Instance.DoDialogue(dialogueData).OnAccept(Heal);
                _isRewarded = true;
            }
        }

        private void Heal()
        {
            var health = Player.Instance.GetEntityComponent<PlayerHealth>();
            
            if (health == null)
            {
                Debug.Log("Player Health is missing");
                return;
            }
            
            health.TakeHeal(healAmount);
        }

        private void OnDisable()
        {
            _isRewarded = false;
        }
    }
}
