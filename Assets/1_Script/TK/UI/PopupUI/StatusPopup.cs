using System;
using Swift_Blade.UI;


namespace Swift_Blade
{
    public class StatusPopup : PopupUI
    {
        private StatusUI statusUI;

        private void Start()
        {
            statusUI = GetComponent<StatusUI>();
        }

        public override void Popup()
        {
            base.Popup();
            statusUI.HandleInfoChange();
        }
    }
}
