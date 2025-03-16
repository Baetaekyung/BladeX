using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    [Serializable]
    public abstract class PopupUI : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup cG;
        protected GraphicRaycaster _raycaster;
        [SerializeField] protected float _fadeTime;
        
        protected Coroutine _delayRoutine;
        public PopupType popupType;

        protected virtual void Awake()
        {
            _raycaster = GetComponent<GraphicRaycaster>();
        }
        
        protected virtual void OnDestroy()
        {
            cG.DOKill();
        }

        private void OnDisable()
        {
            cG.DOKill();
        }

        public virtual void Popup()
        {
            cG.DOFade(1, _fadeTime)
                .SetEase(Ease.OutCirc);
            _raycaster.enabled = true;
        }
        
        public virtual void PopDown()
        {
            _raycaster.enabled = false;
            cG.DOFade(0, _fadeTime).SetEase(Ease.OutCirc);
        }
        
        //팝업하고 딜레이 이후 팝업 닫기
        #region Delay Popup

        public virtual void DelayPopup(float delay)
        {
            if (_delayRoutine is not null)
            {
                StopCoroutine(_delayRoutine);
            } 
            _delayRoutine = StartCoroutine(DelayRoutine(delay));
        }

        public virtual void DelayPopup(float delay, Action callback)
        {
            if (_delayRoutine is not null)
            {
                StopCoroutine(_delayRoutine);
            }
            _delayRoutine = StartCoroutine(DelayRoutine(delay, callback));
        }

        private IEnumerator DelayRoutine(float delay)
        {
            Popup();
            yield return new WaitForSeconds(delay);
        }
        
        private IEnumerator DelayRoutine(float delay, Action callback)
        {
            Popup();
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }

        #endregion
    }
}
