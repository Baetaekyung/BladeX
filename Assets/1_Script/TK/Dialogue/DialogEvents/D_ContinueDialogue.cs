using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Continue_Dialog", menuName = "SO/Dialog/Events/Continue Dialog Event")]
    public class D_ContinueDialogue : DialogueEventSO
    {
        [FormerlySerializedAs("nextDialog")] public DialogueDataSO nextDialogue;

        //todo : ���⼭ null üũ�ϱ�
        private void OnValidate()
        {
            
        }
        public override void DoEvent()
        {
            DialogueManager.Instance.DoDialog(nextDialogue);
        }
    }
}
