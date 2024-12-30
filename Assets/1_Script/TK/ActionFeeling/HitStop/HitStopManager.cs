using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade.Feeling
{
    public class HitStopManager : MonoSingleton<HitStopManager>
    {        
        [Header("타임 스케일 관련 변수 및 우선순위")]
        public float CurrentTimeScale { get; private set; }
        private const float DEFAULT_TIMESCALE = 1; //기본 타임 스케일
        //일단 무조건 실행되도록 가장 중요하지 않은 우선순위
        private HitStopPriority _currentPriority = HitStopPriority.LAST; 
        
        [Header("코루틴 관련 변수들")]
        private Coroutine _hitStopCoroutine;
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame(); //코루틴 최적화

        private Action _onCompleteEvent = null;
        
        protected override void Awake()
        {
            Time.timeScale = DEFAULT_TIMESCALE;
        }

        public HitStopManager DoHitStop(HitStopSO hitStopData)
        {
            if (_hitStopCoroutine is not null)
            {
                if ((int)hitStopData.hitStopPriority <= (int)_currentPriority)
                {
                    StopCoroutine(_hitStopCoroutine);

                    _hitStopCoroutine = StartCoroutine(HitStopCoroutine(hitStopData));
                    _currentPriority = hitStopData.hitStopPriority;
                }
            }
            else
            {
                _hitStopCoroutine = StartCoroutine(HitStopCoroutine(hitStopData));
                _currentPriority = hitStopData.hitStopPriority;
            }

            return this;
        }
        
        //타임 스케일 원래대로 돌리는 함수
        public HitStopManager StopHitStop() 
        {
            if (_hitStopCoroutine is not null)
            {
                StopCoroutine(_hitStopCoroutine);
            }

            InvokeCompleteEvent();

            Time.timeScale = DEFAULT_TIMESCALE;

            return this;
        }

        private IEnumerator HitStopCoroutine(HitStopSO hitStopData)
        {
            if (hitStopData.hitStopType == HitStopType.SMOOTH) //타임스케일 부드럽게 변환
            {
                float smoothValue = 0;
                
                for (int i = 0; i < 10; i++) //10프레임동안 변환 (내마음대로정함)
                {
                    smoothValue += 0.1f;
                    float tempTimeScale = Mathf.Lerp(CurrentTimeScale, hitStopData.timeScale, smoothValue);
                    Time.timeScale = tempTimeScale;
                    yield return _waitForEndOfFrame;
                }

                smoothValue = 0;
                //타임 스케일과 무관하게 리얼타임으로
                yield return new WaitForSecondsRealtime(hitStopData.duration); 
                
                for (int i = 0; i < 10; i++)
                {
                    smoothValue += 0.1f;
                    float tempTimeScale = Mathf.Lerp(CurrentTimeScale, DEFAULT_TIMESCALE, smoothValue);
                    Time.timeScale = tempTimeScale;
                    yield return _waitForEndOfFrame;
                }
                
                Time.timeScale = DEFAULT_TIMESCALE;
            }
            else if (hitStopData.hitStopType == HitStopType.IMMEDIATE)
            {
                Time.timeScale = hitStopData.timeScale;

                yield return new WaitForSecondsRealtime(hitStopData.duration);
                Time.timeScale = DEFAULT_TIMESCALE;
            }

            InvokeCompleteEvent();
        }

        private void InvokeCompleteEvent()
        {
            _onCompleteEvent?.Invoke();
            _onCompleteEvent = null;
        }
        
        public void OnComplete(Action onComplete)
        {
            _onCompleteEvent = null;
            
            _onCompleteEvent = onComplete;
        }
    }
}
