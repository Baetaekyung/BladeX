using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade.Timeline
{
    public class TimelineCameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource   impulseSource;

        private void OnEnable()
        {
            InvokeRepeating(nameof(ClearImpulse), 0.2f, 0.2f);
            InvokeRepeating(nameof(GenerateImpulse), 0.2f, 0.2f);
        }

        private void OnDisable()
        {
            CancelInvoke();
            CinemachineImpulseManager.Instance.Clear();
        }

        private void GenerateImpulse()
        {
            impulseSource.GenerateImpulse();
        }

        private void ClearImpulse()
        {
            CinemachineImpulseManager.Instance.Clear();
        }
    }
}

