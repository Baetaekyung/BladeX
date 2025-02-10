using UnityEngine;

namespace Swift_Blade.UI
{
    public class HelpBtn : BaseButton
    {
        protected override void ClickEvent()
        {
            PopupManager.Instance.PopUp(PopupType.Text);
        }
    }
}
