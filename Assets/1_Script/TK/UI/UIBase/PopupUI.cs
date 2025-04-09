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
        [SerializeField] protected float       fadeTime;

        public PopupType popupType;
        
        protected GraphicRaycaster _raycaster;
        protected Coroutine        _delayRoutine;

        protected virtual void Awake()
        {
            _raycaster = GetComponent<GraphicRaycaster>();
        }
        
        private void OnDisable()
        {
            if(cG != null)
            {
                cG.DOKill();
            }
        }

        public virtual void Popup()
        {
            if(cG != null)
            {
                cG.DOFade(1, fadeTime)
                    .SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }


            _raycaster.enabled = true;
        }
        
        public virtual void PopDown()
        {
            _raycaster.enabled = false;

            if(cG != null)
            {
                cG.DOFade(0, fadeTime).SetEase(Ease.OutCirc)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
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
