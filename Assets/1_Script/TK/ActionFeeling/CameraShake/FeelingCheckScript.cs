using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Swift_Blade.Feeling
{
    public class FeelingCheckScript : MonoBehaviour
    {
        [SerializeField] private CameraFocusSO _cameraFocusData;
        [SerializeField] private CinemachineCamera _targetCamera;
        [SerializeField] private StatComponent statComponent;
        [SerializeField] private StatSO _testStat;

        private void Start()
        {
            CameraFocusManager.Instance.SetTargetCamera(_targetCamera);
        }

        void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                CameraFocusManager.Instance.DoFocus(_cameraFocusData);
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                Debug.Log(statComponent.GetStat(_testStat).Value);
            }

            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                statComponent.AddModifier(_testStat, "test", 30);
            }
        }
    }
}
