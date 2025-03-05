using Swift_Blade.UI;
using UnityEngine;

namespace Swift_Blade
{
    public class PopdownBtn : BaseButton
    {
        [SerializeField] private PopupType openType;
        
        protected override void ClickEvent()
        {
            PopupManager.Instance.PopDown();

            if (openType != PopupType.None)
            {
                PopupManager.Instance.PopUp(openType);
            }
        }
    }
}
