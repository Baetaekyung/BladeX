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
        public event Action<float> OnSpeedMultiplierEvent;
        public event Action<float> OnForceEvent;
        public event Action<Vector3> OnMovementSetEvent;

        [Preserve]
        private void OnAnimationEndTrigger()
        {
            OnAnimationEndEvent?.Invoke();
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
        private void OnMovementMultiplierSet(float set)
        {
            OnSpeedMultiplierEvent?.Invoke(set);
        }
        [Preserve]
        private void OnSetMovementSet(float a)
        {
            OnMovementSetEvent?.Invoke(new Vector3(a, 0, a));
        }
    }
}
