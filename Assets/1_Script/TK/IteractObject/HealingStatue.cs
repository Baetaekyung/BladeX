using System;
using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class HealingStatue : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueDataSO dialogueData;
        [SerializeField] private DialogueDataSO afterRewardDialogueData;
        [SerializeField] private ItemDataSO potion;

        private bool _isRewarded = false;

        [SerializeField] private GameObject outlieObject;
        GameObject IInteractable.GetMeshGameObject()
        {
            return outlieObject;
        }

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
            if (!InventoryManager.Instance.TryAddItemToEmptySlot(potion))
            {
                return;
            }

            _isRewarded = true;
        }
    }
}
