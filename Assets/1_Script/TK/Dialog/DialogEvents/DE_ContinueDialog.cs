using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Continue_Dialog", menuName = "SO/Dialog/Events/Continue Dialog Event")]
    public class DE_ContinueDialog : DialogEventSO
    {
        public DialogDataSO nextDialog;

        public override void DoEvent()
        {
            DialogManager.Instance.DoDialog(nextDialog);
        }
    }
}
