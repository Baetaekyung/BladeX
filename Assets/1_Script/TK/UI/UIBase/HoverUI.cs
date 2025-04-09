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
        [SerializeField] 
        protected float animationScale;
        [SerializeField, Tooltip("1 / 애니메이션 속도")]
        protected float _hoverAnimationSpeed;

        protected RectTransform _rectTrm;
        protected Tween         _currentTween;

        private bool    _isHovering;
        private Vector3 _originScale;

        protected virtual void Awake()
        {
            _rectTrm = GetComponent<RectTransform>();

            _originScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isHovering == true)
                return;
            
            if (_currentTween != null)
                _currentTween.Kill();
            
            _isHovering = true;
            
            HoverAnimation();
        }

        protected virtual void HoverAnimation()
        {
            _rectTrm.DOScale(_originScale * animationScale, 1 / _hoverAnimationSpeed)
                .SetEase(Ease.InSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isHovering == false)
                return;
            
            if (_currentTween != null)
                _currentTween.Kill();
            
            _isHovering = false;
            
            HoverAnimationEnd();
        }

        protected virtual void HoverAnimationEnd()
        {
            _rectTrm.DOScale(_originScale, 1 / _hoverAnimationSpeed)
                .SetEase(Ease.OutSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public virtual void SetHovering(bool isHovering)
        {
            _isHovering = isHovering;
        }
    }
}
