using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Swift_Blade
{
    
    public class PressurePlate : MonoBehaviour
    {
        //todo : https://discussions.unity.com/t/fix-ontriggerexit-will-now-be-called-for-disabled-gameobjects-colliders/738523
        private const float Y_PRESSED   = -0.22f;
        private const float Y_UNPRESSED = -0.15f;

        public event Action OnPressed;
        public event Action OnUnpressed;
        public UnityEvent UE_Onpressed;
        public UnityEvent UE_Onunpressed;

        [SerializeField] private Transform floor;

        private const float delayUnpresse = 0.75f;
        private float unpresseTime;

        private bool hasContact;
        private bool exitFlag;      //flag for exitFire

        private Tween tween;
        //private void OnTriggerEnter(Collider other)
        //{
        //    
        //}
        private void OnTriggerStay(Collider other)
        {
            if (hasContact) return;

            hasContact = true;

            if (exitFlag) return;

            OnPressed?.Invoke();
            UE_Onpressed.Invoke();
            OnChange(true);
        }
        private void OnTriggerExit(Collider other)
        {
            hasContact = false;
            exitFlag = true;
            unpresseTime = Time.time + delayUnpresse;
        }
        private void Update()
        {
            if (!hasContact && exitFlag)
            {
                if (unpresseTime > Time.time)
                {
                    return;
                }

                OnChange(false);
                OnUnpressed?.Invoke();
                UE_Onunpressed.Invoke();
                hasContact = false;
                exitFlag = false;
            }
        }
        private void OnChange(bool isPressed)
        {
            if (tween != null)
            {
                tween.Kill();
            }

            float duration = isPressed ? 0.2f : 0.1f;
            float endValue = isPressed ? Y_PRESSED : Y_UNPRESSED;
            tween = floor.DOLocalMoveY(endValue, duration);
        }
    }
}