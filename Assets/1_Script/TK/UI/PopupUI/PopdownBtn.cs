using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PopdownBtn : BaseButton
    {
        protected override void ClickEvent()
        {
            PopupManager.Instance.PopDown();
        }
    }
}
