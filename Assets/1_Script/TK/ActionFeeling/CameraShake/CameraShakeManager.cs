using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade.Feeling
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class CameraShakeManager : MonoSingleton<CameraShakeManager>
    {
        [Header("Camera Shaking")]
        [SerializeField] private SerializableDictionary<CameraShakeType, CameraShakeSO> impulseDictionary;

        private Coroutine           _cameraShakeCoroutine;
        private CameraShakePriority _currentPriority = CameraShakePriority.LAST;
        
        private Action _onCompleteEvent = null;
        
        public CameraShakeManager DoShake(
            CameraShakeType shakeType, 
            CameraShakePriority priority = CameraShakePriority.NONE)
        {
            if (_cameraShakeCoroutine != null)
            {
                //같은 우선순위면 흔들림 덮어 씌우기
                if ((int)priority <= (int)_currentPriority)
                    StopCoroutine(_cameraShakeCoroutine);
            }
            
            _cameraShakeCoroutine = StartCoroutine(GenerateImpulseRoutine(shakeType, priority));

            return this;
        }
        
        private IEnumerator GenerateImpulseRoutine(
            CameraShakeType shakeType, 
            CameraShakePriority priority = CameraShakePriority.NONE)
        {
            CinemachineImpulseManager.Instance.Clear(); //모든 흔들림을 초기화

            float force    = impulseDictionary[shakeType].strength;
            float duration = impulseDictionary[shakeType].cinemachineImpulseSource
                .ImpulseDefinition.ImpulseDuration;

            _currentPriority = priority;
            impulseDictionary[shakeType].cinemachineImpulseSource.GenerateImpulse(force);
            
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
            _onCompleteEvent = onComplete;
        }
                
        
    }
}
