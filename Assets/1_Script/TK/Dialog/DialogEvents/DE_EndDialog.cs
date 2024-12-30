using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "DE_Dialog_End", menuName = "SO/Dialog/Events/Dialog End")]
    public class DE_EndDialog : DialogEventSO
    {
        public override void DoEvent()
        {
            DialogManager.Instance.StopDialog();
        }
    }
}
