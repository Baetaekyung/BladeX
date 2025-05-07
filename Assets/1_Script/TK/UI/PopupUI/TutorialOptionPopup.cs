using DG.Tweening;
using Swift_Blade.UI;

namespace Swift_Blade
{
    public class TutorialOptionPopup : PopupUI
    {
        public override void Popup()
        {
            if(cG != null)
            {
                cG.alpha = 1f;
                cG.transform
                    .DOScaleX(1, fadeTime)
                    .SetEase(Ease.OutCirc)
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
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
            _raycaster.enabled = false;
        }
    }
}
