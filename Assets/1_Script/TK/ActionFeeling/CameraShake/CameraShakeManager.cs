using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class CameraShakeManager : MonoBehaviour
    {
        private static CameraShakeManager _instance;

        public static CameraShakeManager Instance => _instance;

        [Header("Camera Shaking")]
        [SerializeField] private SerializableDictionary<CameraShakeType, CameraShakeSO> impulseDictionary;
        private Coroutine _cameraShakeCoroutine;
        private CameraShakePriority _currentPriority = CameraShakePriority.LAST;
        
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

        public void GenerateImpulse(CameraShakeType shakeType, CameraShakePriority priority = CameraShakePriority.NONE)
        {
            if (_cameraShakeCoroutine is not null)
            {
                //Same priority overwrite shaking
                if ((int)priority <= (int)_currentPriority)
                {
                    StopCoroutine(_cameraShakeCoroutine);
                    
                    _cameraShakeCoroutine = StartCoroutine(GenerateImpulseRoutine(shakeType, priority));
                }
            }
            else
                _cameraShakeCoroutine = StartCoroutine(GenerateImpulseRoutine(shakeType, priority));
        }
        
        /// <param name="shakeType">Camera Shake Type</param>
        /// <param name="priority">Default is none, none is first priority</param>
        private IEnumerator GenerateImpulseRoutine(CameraShakeType shakeType, CameraShakePriority priority = CameraShakePriority.NONE)
        {
            CinemachineImpulseManager.Instance.Clear(); //Clear all of shaking
            float force = impulseDictionary[shakeType].strength;
            impulseDictionary[shakeType].cinemachineImpulseSource.GenerateImpulse(force);

            float duration = impulseDictionary[shakeType].cinemachineImpulseSource
                .ImpulseDefinition.ImpulseDuration;
            _currentPriority = priority;
            
            //Initialize priority
            yield return new WaitForSeconds(duration);
            _currentPriority = CameraShakePriority.LAST;
        }
    }
}
