using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Swift_Blade
{
    public class AnimationTriggers : MonoBehaviour
    {
        public event Action OnAttackTriggerEvent;

        public event Action OnAnimationEndEvent;
        public event Action OnAnimationEndableEvent;
        public event Action OnAnimationEndTriggeristenEvent;
        public event Action OnAnimationEndTriggerStopListenEvent;

        public event Action<float> OnSpeedMultiplierDefaultEvent;
        public event Action<float> OnForceEvent;

        public event Action OnRotateAllowSetEvent;
        public event Action OnRotateDisallowSetEvent;

        [Preserve]
        private void OnAnimationEndTrigger()
        {
            OnAnimationEndEvent?.Invoke();
            OnSpeedMultiplierDefaultEvent?.Invoke(1);
            OnRotateAllowSetEvent?.Invoke();
        }
        [Preserve]
        private void OnAnimationEndableTrigger()
        {
            OnAnimationEndableEvent?.Invoke();
        }
        [Preserve]
        private void OnAnimationEndableListenTrigger()
        {
            OnAnimationEndTriggeristenEvent?.Invoke();
        }
        [Preserve]
        private void OnAnimationEndableStopListenTrigger()
        {
            OnAnimationEndTriggerStopListenEvent?.Invoke();
        }
        [Preserve]
        private void OnForceEventTrigger(float force)
        {
            OnForceEvent?.Invoke(force);
        }
        [Preserve]
        private void OnSpeedMultiplierDefaultTrigger(float set)
        {
            OnSpeedMultiplierDefaultEvent?.Invoke(set);
        }
        [Preserve]
        private void OnAttackTrigger()
        {
            OnAttackTriggerEvent?.Invoke();
        }
        [Preserve]
        private void OnRotateAllowTrigger()
        {
            OnRotateAllowSetEvent?.Invoke();
        }
        [Preserve]
        private void OnRotateDisallowTrigger()
        {
            OnRotateDisallowSetEvent?.Invoke();
        }
        //[Preserve]
        //private void OnMovementSet(float set)
        //{
        //    OnMovementSetEvent?.Invoke(new Vector3(set, 0, set));
        //}
    }
}
