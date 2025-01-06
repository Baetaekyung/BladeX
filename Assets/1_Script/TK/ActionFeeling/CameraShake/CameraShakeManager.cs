using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


namespace Swift_Blade.Feeling
{
    public class CameraShakeManager : MonoBehaviour
    {
        private static CameraShakeManager _instance;

        public static CameraShakeManager Instance => _instance;

        [Header("Camera Shaking")]
        [SerializeField] private SerializableDictionary<CameraShakeType, CameraShakeSO> impulseDictionary;
        private Coroutine _cameraShakeCoroutine;
        private CameraShakePriority _currentPriority = CameraShakePriority.LAST;

        private Action _onCompleteEvent = null;
        
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
        }

        public CameraShakeManager DoShake(CameraShakeType shakeType, CameraShakePriority priority = CameraShakePriority.NONE)
        {
            if (_cameraShakeCoroutine is not null)
            {
                //같은 우선순위면 흔들림 덮어 씌우기
                if ((int)priority <= (int)_currentPriority)
                {
                    StopCoroutine(_cameraShakeCoroutine);
                    
                    _cameraShakeCoroutine = StartCoroutine(GenerateImpulseRoutine(shakeType, priority));
                }
            }
            else
                _cameraShakeCoroutine = StartCoroutine(GenerateImpulseRoutine(shakeType, priority));

            return this;
        }
        
        /// <param name="shakeType">카메라 셰이크 타입</param>
        /// <param name="priority">기본은 NONE, 우선순위가 낮을 수록 먼저 실행된다.</param>
        private IEnumerator GenerateImpulseRoutine(CameraShakeType shakeType, CameraShakePriority priority = CameraShakePriority.NONE)
        {
            CinemachineImpulseManager.Instance.Clear(); //모든 흔들림을 초기화
            float force = impulseDictionary[shakeType].strength;
            impulseDictionary[shakeType].cinemachineImpulseSource.GenerateImpulse(force);

            float duration = impulseDictionary[shakeType].cinemachineImpulseSource
                .ImpulseDefinition.ImpulseDuration;
            _currentPriority = priority;
            
            yield return new WaitForSeconds(duration);
            
            InvokeCompleteEvent();

            _currentPriority = CameraShakePriority.LAST;
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
