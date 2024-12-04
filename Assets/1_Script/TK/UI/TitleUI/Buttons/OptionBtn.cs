using UnityEngine;

namespace Swift_Blade.UI
{
    public class OptionBtn : BaseButton
    {
        protected override void ClickEvent()
        {
            PopupManager.Instance.PopUp(PopupType.Option);
        }
    }
}
