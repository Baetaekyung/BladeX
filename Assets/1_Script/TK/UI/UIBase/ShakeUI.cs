using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class ShakeUI : HoverUI
    {
        protected override void HoverAnimation()
        {
            transform.DOShakeRotation(_hoverAnimationSpeed, Vector3.forward * animationScale);
        }

        protected override void HoverAnimationEnd()
        {
            _currentTween.Kill();
            transform.rotation = Quaternion.identity;
        }
    }
}
