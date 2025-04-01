using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Continue_Dialog", menuName = "SO/Dialog/Events/Continue Dialog Event")]
    public class D_ContinueDialogue : DialogueEventSO
    {
        public DialogueDataSO nextDialogue;
        
        public override void InvokeEvent()
        {
            DialogueManager.Instance.CancelDialogue();
            DialogueManager.Instance.StartDialogue(nextDialogue);
        }
    }
}
