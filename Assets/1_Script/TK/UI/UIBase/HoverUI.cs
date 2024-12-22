using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Swift_Blade
{
    public class HoverUI : MonoBehaviour
        ,IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _hoverTargetSize;
        [SerializeField, Tooltip("1 / 애니메이션 속도")] private float _hoverAnimationSpeed;
        private Tween _currentTween;
        private RectTransform _rectTrm;
        private bool _isHovering;

        private void Awake()
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
            
            _rectTrm.DOScale(Vector3.one * _hoverTargetSize, 1 / _hoverAnimationSpeed)
                .SetEase(Ease.InSine);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isHovering is false)
                return;
            
            if (_currentTween != null)
                _currentTween.Kill();
            
            _isHovering = false;
            
            _rectTrm.DOScale(Vector3.one, 1 / _hoverAnimationSpeed)
                .SetEase(Ease.OutSine);
        }

        public void SetHovering(bool isHovering)
        {
            _isHovering = isHovering;
        }
    }
}
