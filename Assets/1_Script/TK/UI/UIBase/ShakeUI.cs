using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class ShakeUI : HoverUI
    {
        protected override void HoverAnimation()
        {
            if(transform != null)
            {
                transform.DOShakeRotation(_hoverAnimationSpeed, Vector3.forward * animationScale)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }

        protected override void HoverAnimationEnd()
        {
            if(_currentTween != null)
            {
                _currentTween.Kill();
            }

            transform.rotation = Quaternion.identity;
        }
    }
}
