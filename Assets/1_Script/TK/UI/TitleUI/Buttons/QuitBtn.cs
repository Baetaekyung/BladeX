using UnityEngine;

namespace Swift_Blade.UI
{
    public class QuitBtn : BaseButton
    {
        protected override void ClickEvent()
        {
            Application.Quit();
        }
    }
}
