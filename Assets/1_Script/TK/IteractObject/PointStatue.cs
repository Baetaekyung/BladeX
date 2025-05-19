using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PointStatue : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueDataSO dialogueData;

        [SerializeField] private GameObject outlineObject;
        GameObject IInteractable.GetMeshGameObject()
        {
            return outlineObject;
        }
        public void Interact()
        {
            DialogueManager.Instance.StartDialogue(dialogueData).Subscribe(OpenStatus);
        }

        private void OpenStatus()
        {
            PopupManager.Instance.PopUp(PopupType.Status);
        }
    }
}
