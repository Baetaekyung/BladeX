using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class ColorChangeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _targetImage;
        [SerializeField] private Color _targetColor;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private float _animationTime;

        private Tween _currentTween;
        
        private bool _isPointerEnter = false;
        
        private void Start()
        {
            _defaultColor = _targetImage.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isPointerEnter is true)
                return;

            if (_currentTween is not null)
            {
                _currentTween.Kill();
            }

            _isPointerEnter = true;
            
            _currentTween = _targetImage.DOColor(_targetColor, _animationTime);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isPointerEnter is false)
                return;

            if (_currentTween is not null)
            {
                _currentTween.Kill();
            }
            
            _isPointerEnter = false;

            _currentTween = _targetImage.DOColor(_defaultColor, _animationTime);
        }
    }
}
