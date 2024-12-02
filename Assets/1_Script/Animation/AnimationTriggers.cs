using System;
using UnityEngine;

namespace Swift_Blade
{
    public class AnimationTriggers : MonoBehaviour
    {
        public event Action OnAnimationEnd;
        public event Action OnAnimationEndable;

        private void OnAnimationEndTrigger()
        {
            OnAnimationEnd?.Invoke();
        }
        private void OnAnimationEndableTrigger()
        {
            OnAnimationEndable?.Invoke();
        }
    }
}
