using UnityEngine;

namespace Swift_Blade
{
    public class HealingStatue : MonoBehaviour, IInteractable
    {
        [SerializeField] private int healAmount;
        [SerializeField] private DialogueDataSO dialogueData;

        private bool _isRewarded = false;
        
        [ContextMenu("Interact")]
        public void Interact()
        {
            if (_isRewarded)
            {
                Debug.Log("이미 보상을 받았다..");
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
    }
}
