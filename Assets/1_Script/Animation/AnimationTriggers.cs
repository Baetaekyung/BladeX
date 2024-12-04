using System;
using UnityEngine;

namespace Swift_Blade
{
    public class AnimationTriggers : MonoBehaviour
    {
        public event Action OnAnimationEnd;
        public event Action OnAnimationEndable;
        public event Action OnAnimationEndableListen;
        public event Action<float> OnForceEvent;

        private void OnAnimationEndTrigger()
        {
            OnAnimationEnd?.Invoke();
        }
        private void OnAnimationEndableTrigger()
        {
            OnAnimationEndable?.Invoke();
        }
        private void OnAnimationEndableListenTrigger()
        {
            OnAnimationEndableListen?.Invoke();
        }
        private void OnForceEventTrigger(float force)
        {
            OnForceEvent?.Invoke(force);
        }
    }
}
