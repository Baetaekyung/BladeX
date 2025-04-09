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
            if(cG != null)
            {
                cG.transform
                    .DOScaleX(1, fadeTime)
                    .SetEase(Ease.OutQuart)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

            _raycaster.enabled = true;
        }

        public override void PopDown()
        {
            if(cG != null)
            {
                cG.transform
                    .DOScaleX(0, fadeTime)
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
            _raycaster.enabled = false;
        }
    }
}
