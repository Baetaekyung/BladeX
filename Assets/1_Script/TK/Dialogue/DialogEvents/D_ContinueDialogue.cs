using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Continue_Dialog", menuName = "SO/Dialog/Events/Continue Dialog Event")]
    public class D_ContinueDialogue : DialogueEventSO
    {
        [FormerlySerializedAs("nextDialog")] public DialogueDataSO nextDialogue;

        //todo : 여기서 null 체크하기
        private void OnValidate()
        {
            
        }
        public override void DoEvent()
        {
            DialogueManager.Instance.DoDialog(nextDialogue);
        }
    }
}
