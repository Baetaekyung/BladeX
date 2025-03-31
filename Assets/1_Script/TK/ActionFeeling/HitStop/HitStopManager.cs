using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade.Feeling
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class HitStopManager : MonoSingleton<HitStopManager>
    {
        private const float DEFAULT_TIMESCALE = 1; //기본 타임 스케일
        
        [Header("타임 스케일 관련 변수 및 우선순위")]
        private HitStopPriority _currentPriority = HitStopPriority.LAST; //일단 무조건 실행되도록 가장 중요하지 않은 우선순위

        [Header("코루틴 관련 변수들")]
        private Coroutine _hitStopCoroutine;

        private Action _onCompleteEvent = null;

        public float CurrentTimeScale { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            Time.timeScale = DEFAULT_TIMESCALE;
        }

        public HitStopManager StartHitStop(HitStopSO hitStopData)
        {
            if (_hitStopCoroutine != null)
            {
                if ((int)hitStopData.hitStopPriority <= (int)_currentPriority)
                    StopCoroutine(_hitStopCoroutine);
            }
            
            _hitStopCoroutine = StartCoroutine(HitStopCoroutine(hitStopData));
            _currentPriority  = hitStopData.hitStopPriority;

            return this;
        }
        
        public void EndHitStop() 
        {
            if (_hitStopCoroutine != null)
                StopCoroutine(_hitStopCoroutine);

            InvokeCompleteEvent();

            Time.timeScale = DEFAULT_TIMESCALE;
        }

        private IEnumerator HitStopCoroutine(HitStopSO hitStopData)
        {
            if (hitStopData.hitStopType == HitStopType.SMOOTH) //타임스케일 부드럽게 변환
            {
                yield return StartCoroutine(ChangeTimeScale(hitStopData.smoothTime, hitStopData.timeScale));
                
                yield return new WaitForSecondsRealtime(hitStopData.duration);

                yield return StartCoroutine(ChangeTimeScale(hitStopData.smoothTime, DEFAULT_TIMESCALE));
                
            }
            else if (hitStopData.hitStopType == HitStopType.IMMEDIATE)
            {
                Time.timeScale = hitStopData.timeScale;

                yield return new WaitForSecondsRealtime(hitStopData.duration);

                Time.timeScale = DEFAULT_TIMESCALE;
            }

            InvokeCompleteEvent();
        }

        private IEnumerator ChangeTimeScale(float smoothTime, float targetScale)
        {
            float smoothValue = 0;
            
            while(smoothValue < smoothTime)
            {
                smoothValue += Time.unscaledDeltaTime;

                CurrentTimeScale = Mathf.Lerp(CurrentTimeScale, targetScale, smoothValue / smoothTime);

                Time.timeScale = CurrentTimeScale;

                yield return null;
            }

            Time.timeScale = targetScale;
        }
        
        private void InvokeCompleteEvent()
        {
            _onCompleteEvent?.Invoke();
            _onCompleteEvent = null;
        }
        
        public void OnComplete(Action onComplete)
        {
            _onCompleteEvent = onComplete;
        }
    }
}
