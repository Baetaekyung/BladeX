using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Continue_Dialog", menuName = "SO/Dialog/Events/Continue Dialog Event")]
    public class D_ContinueDialogue : DialogueEventSO
    {
        public DialogueDataSO nextDialogue;

        private void OnValidate()
        {
            Debug.Assert(nextDialogue != null, "다음 다이얼로그를 등록해 주세요");
        }
        
        public override void InvokeEvent()
        {
            DialogueManager.Instance.CancelDialogue();
            DialogueManager.Instance.DoDialogue(nextDialogue);
        }
    }
}
