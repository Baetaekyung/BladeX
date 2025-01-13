using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "DE_Dialog_End", menuName = "SO/Dialog/Events/Dialog End")]
    public class D_EndDialogue : DialogueEventSO
    {
        public override void DoEvent()
        {
            DialogueManager.Instance.StopDialog();
        }
    }
}
