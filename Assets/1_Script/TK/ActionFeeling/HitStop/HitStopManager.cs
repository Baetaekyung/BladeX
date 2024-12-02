using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class HitStopManager : MonoBehaviour
    {
        private static HitStopManager _instance;
        
        public static HitStopManager Instance { get { return _instance; } }
        
        private Coroutine _hitStopCoroutine;
        public float CurrentTimeScale { get; private set; }
        private readonly float _defualtTimeScale = 1;
        private HitStopPriority _currentPriority = HitStopPriority.LAST;
        
        private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        
        private void Awake()
        {
            if (_instance is null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Time.timeScale = _defualtTimeScale;
        }

        public void HitStop(HitStopSO hitStopData)
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
        }

        public void StopHitStop()
        {
            if (_hitStopCoroutine is not null)
            {
                StopCoroutine(_hitStopCoroutine);
            }

            Time.timeScale = _defualtTimeScale;
        }
        
        private IEnumerator HitStopCoroutine(HitStopSO hitStopData)
        {
            if (hitStopData.hitStopType == HitStopType.SMOOTH) //Smooth change timeScale
            {
                float smoothValue = 0;
                for (int i = 0; i < 10; i++) //10 is smooth step
                {
                    smoothValue += 0.1f;
                    float tempTimeScale = Mathf.Lerp(CurrentTimeScale, hitStopData.timeScale, smoothValue);
                    Time.timeScale = tempTimeScale;
                    yield return _waitForEndOfFrame;
                }

                smoothValue = 0;
                yield return new WaitForSecondsRealtime(hitStopData.duration);
                
                for (int i = 0; i < 10; i++) //10 is smooth step
                {
                    smoothValue += 0.1f;
                    float tempTimeScale = Mathf.Lerp(CurrentTimeScale, _defualtTimeScale, smoothValue);
                    Time.timeScale = tempTimeScale;
                    yield return _waitForEndOfFrame;
                }
                
                Time.timeScale = _defualtTimeScale;
            }
            else if (hitStopData.hitStopType == HitStopType.IMMEDIATE)
            {
                Time.timeScale = hitStopData.timeScale;

                yield return new WaitForSecondsRealtime(hitStopData.duration);
                Time.timeScale = _defualtTimeScale;
            }
        }
    }
}
