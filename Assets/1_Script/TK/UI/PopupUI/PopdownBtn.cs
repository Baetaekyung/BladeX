using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PopdownBtn : BaseButton
    {
        [SerializeField] private bool useCallback = false;
        [SerializeField] private PopupType callbackPopupType;

        protected override void ClickEvent()
        {
            PopupManager.Instance.PopDown();

            if(useCallback)
            {
                PopupManager.Instance.PopUp(callbackPopupType);
            }
        }
    }
}
