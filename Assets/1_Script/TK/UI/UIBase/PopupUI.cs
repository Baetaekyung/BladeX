using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade.UI
{
    [Serializable]
    public abstract class PopupUI : MonoBehaviour
    {
        protected Coroutine _delayRoutine;
        
        public abstract void Popup();
        public abstract void PopDown();

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
