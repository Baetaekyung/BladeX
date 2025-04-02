using UnityEngine;

namespace Swift_Blade
{
    public class Letter : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueDataSO dialogueData;

        public void Interact()
        {
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }
}
