using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

namespace Swift_Blade
{

    public class PressurePlate : MonoBehaviour
    {
        //todo : https://discussions.unity.com/t/fix-ontriggerexit-will-now-be-called-for-disabled-gameobjects-colliders/738523
        private const float yPressed = -0.22f;
        private const float yUnpressed = -0.15f;

        public event Action OnPressed;
        public event Action OnUnpressed;

        [SerializeField] private Transform floor;
        private bool pressed;
        private bool exitFlag;
        private Tween tween;
        private void OnTriggerEnter(Collider other)
        {
            if (pressed == true) return;
            pressed = true;
            OnPressed?.Invoke();
            OnChange(true);
            //print("OnENT");
        }
        private void OnTriggerStay(Collider other)
        {
            pressed = true;
        }
        private void OnTriggerExit(Collider other)
        {
            pressed = false;
            exitFlag = true;
        }
        private void Update()
        {
            if (exitFlag && !pressed)
            {
                OnChange(false);
                //print("onExit");
                OnUnpressed?.Invoke();
            }
            exitFlag = false;
        }
        private void OnChange(bool isPressed)
        {
            if (tween != null) tween.Kill();
            float duration = isPressed ? 0.2f : 0.1f;
            float endValue = isPressed ? yPressed : yUnpressed;
            tween = floor.DOLocalMoveY(endValue, duration);
        }
    }
}