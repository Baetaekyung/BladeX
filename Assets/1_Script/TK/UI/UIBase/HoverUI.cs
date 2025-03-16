using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class HoverUI : MonoBehaviour
        ,IPointerEnterHandler, IPointerExitHandler
    {
        [FormerlySerializedAs("_hoverTargetSize")] [SerializeField] protected float animationScale;
        [SerializeField, Tooltip("1 / 애니메이션 속도")] protected float _hoverAnimationSpeed;
        protected RectTransform _rectTrm;
        protected Tween _currentTween;
        private bool _isHovering;

        protected virtual void Awake()
        {
            _rectTrm = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isHovering is true)
                return;
            
            if (_currentTween != null)
                _currentTween.Kill();
            
            _isHovering = true;
            
            HoverAnimation();
        }

        protected virtual void HoverAnimation()
        {
            _rectTrm.DOScale(Vector3.one * animationScale, 1 / _hoverAnimationSpeed)
                .SetEase(Ease.InSine);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isHovering is false)
                return;
            
            if (_currentTween != null)
                _currentTween.Kill();
            
            _isHovering = false;
            
            HoverAnimationEnd();
        }

        protected virtual void HoverAnimationEnd()
        {
            _rectTrm.DOScale(Vector3.one, 1 / _hoverAnimationSpeed)
                .SetEase(Ease.OutSine);
        }

        public virtual void SetHovering(bool isHovering)
        {
            _isHovering = isHovering;
        }
    }
}
