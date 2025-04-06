using Swift_Blade.Audio;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Swift_Blade
{
    public enum EAudioType
    {
        Swing,
        QSwing,
        Roll
    }
    public enum EAttackType
    {
        Normal,
        Heavy
    }

    public class AnimationTriggers : MonoBehaviour
    {
        public event Action<EAttackType> OnAttackTriggerEvent;

        public event Action OnAnimationEndEvent;
        public event Action OnAnimationEndableEvent;
        public event Action OnAnimationEndTriggeristenEvent;
        public event Action OnAnimationEndTriggerStopListenEvent;

        public event Action<float> OnSpeedMultiplierDefaultEvent;
        public event Action<float> OnForceEvent;
        public event Action<float> OnForceEvent2;

        public event Action OnRotateAllowSetEvent;
        public event Action OnRotateDisallowSetEvent;

        public event Action<AudioSO> OnAudioPlayEvent;
        public event Action<EAudioType> OnAudioPlayWithTypeEvent;

        [Preserve]
        private void OnAnimationEndTrigger()
        {
            OnAnimationEndEvent?.Invoke();
            OnSpeedMultiplierDefaultEvent?.Invoke(1);
            OnRotateAllowSetEvent?.Invoke();
        }
        [Preserve]
        private void OnAnimationEndableTrigger() => OnAnimationEndableEvent?.Invoke();
        [Preserve]
        private void OnAnimationEndableListenTrigger() => OnAnimationEndTriggeristenEvent?.Invoke();
        [Preserve]
        private void OnAnimationEndableStopListenTrigger() => OnAnimationEndTriggerStopListenEvent?.Invoke();
        [Preserve]
        private void OnForceEventTrigger(float force) => OnForceEvent?.Invoke(force);
        [Preserve]
        private void OnForceEventTrigger2(float force) => OnForceEvent2?.Invoke(force);

        [Preserve]
        private void OnSpeedMultiplierDefaultTrigger(float set) => OnSpeedMultiplierDefaultEvent?.Invoke(set);
        [Preserve]
        private void OnAttackTrigger(EAttackType attackType) => OnAttackTriggerEvent?.Invoke(attackType);
        [Preserve]
        private void OnRotateAllowTrigger() => OnRotateAllowSetEvent?.Invoke();
        [Preserve]
        private void OnRotateDisallowTrigger() => OnRotateDisallowSetEvent?.Invoke();
        [Preserve]
        private void OnAudioPlay(AudioSO audio) => OnAudioPlayEvent?.Invoke(audio);
        [Preserve]
        private void OnAudioPlayCollection(AudioCollectionSO audioCollectionSo) => OnAudioPlay(audioCollectionSo.GetRandomAudio);
        [Preserve]
        private void OnAudioPlayWithType(EAudioType type) => OnAudioPlayWithTypeEvent?.Invoke(type);
    }
}
