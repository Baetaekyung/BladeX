using UnityEngine;

namespace Swift_Blade.UI
{
    public class HelpBtn : BaseButton
    {
        protected override void ClickEvent()
        {
            PopupManager.Instance.LogMessage("아직 구현되지 않은 기능");
        }
    }
}
