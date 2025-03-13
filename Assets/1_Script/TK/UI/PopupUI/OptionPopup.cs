using DG.Tweening;
using Swift_Blade.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class OptionPopup : PopupUI
    {
        public override void Popup()
        {
            cG.alpha = 1f;
            cG.transform.DOScaleY(1, _fadeTime).SetEase(Ease.OutQuart);
            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            cG.transform.DOScaleY(0, _fadeTime).SetEase(Ease.Linear);
            _raycaster.enabled = false;
        }
    }
}
