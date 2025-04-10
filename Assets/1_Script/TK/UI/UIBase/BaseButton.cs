using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public abstract class BaseButton : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private bool  _isAnimationUI      = false;
        [SerializeField] private float _animationSpeed     = 4f;
        [SerializeField] private float _clickedButtonScale = 1f;
        
        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
        }

        protected virtual void Start()
        {
            _button.onClick.AddListener(ClickEvent);
            _button.onClick.AddListener(ClickAnimation);
        }
                
        private void ClickAnimation()
        {
            if (_isAnimationUI is false) return;
            
            if(transform != null)
            {
                transform.DOScale(Vector3.one * _clickedButtonScale, 1 / _animationSpeed)
                    .SetEase(Ease.OutCirc).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }

        }

        protected abstract void ClickEvent();
        
        #region Delete delegates
        protected virtual void OnDisable() => _button.onClick.RemoveAllListeners();
        protected virtual void OnDestroy() => _button.onClick.RemoveAllListeners();
        #endregion
    }
}
