using System;
using DG.Tweening;
using Swift_Blade.UI;


namespace Swift_Blade
{
    public class StatusPopup : PopupUI
    {
        public override void Popup()
        {
            cG.alpha = 1f;
            cG.transform.DOScaleX(1, fadeTime).SetEase(Ease.OutCirc);

            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            cG.transform.DOScaleX(0, fadeTime).SetEase(Ease.OutCirc);

            _raycaster.enabled = false;
        }
    }
}
