using System;
using DG.Tweening;
using Swift_Blade.Feeling;
using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade.Level
{
    public class Door : MonoBehaviour
    {
        [Range(0.1f , 10)] [SerializeField] private float delay;
        [Range(0.1f , 10)] [SerializeField] private float duration;
        [SerializeField] private CameraShakeType cameraShakeType;
        
        [SerializeField] private Transform door;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        private void OnEnable()
        {
            cinemachineCamera.Priority = 1;
            DOVirtual.DelayedCall(delay, UpDoor);
        }

        private void OnDisable()
        {
            door.transform.position -= new Vector3(0,2.6f , 0);
        }
            
        private void UpDoor()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(()=>CameraShakeManager.Instance.DoShake(cameraShakeType));
            sequence.Join(door.DOMoveY(transform.position.y + 0.25f, duration));
            sequence.AppendInterval(delay);
            sequence.AppendCallback(() => cinemachineCamera.Priority = -1);
                            
        }
    }
}
