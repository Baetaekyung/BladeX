using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Swift_Blade
{
    public class AnimationTriggers : MonoBehaviour
    {
        public event Action OnAnimationEndEvent;
        public event Action OnAnimationnEndableEvent;
        public event Action OnAnimationEndableListenEvent;
        public event Action<float> OnSpeedMultiplierDefaultEvent;
        public event Action<float> OnForceEvent;
        public event Action<Vector3> OnMovementSetEvent;
        public event Action OnAttackTriggerEvent;

        [Preserve]
        private void OnAnimationEndTrigger()
        {
            OnAnimationEndEvent?.Invoke();
            OnSpeedMultiplierDefaultEvent?.Invoke(1);
        }
        [Preserve]
        private void OnAnimationEndableTrigger()
        {
            OnAnimationnEndableEvent?.Invoke();
        }
        [Preserve]
        private void OnAnimationEndableListenTrigger()
        {
            OnAnimationEndableListenEvent?.Invoke();
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
        //[Preserve]
        //private void OnMovementSet(float set)
        //{
        //    OnMovementSetEvent?.Invoke(new Vector3(set, 0, set));
        //}
    }
}
