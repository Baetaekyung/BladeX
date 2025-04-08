using Swift_Blade.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private AnimationTriggers animTrigger;

        public IReadOnlyDictionary<EAudioType, BaseAudioSO> AudioType { get; set; }
        private void Awake()
        {
            animTrigger.OnAudioPlayWithTypeEvent += AudioPlayWithType;
        }
        private void OnDestroy()
        {
            animTrigger.OnAudioPlayWithTypeEvent -= AudioPlayWithType;
        }
        private void AudioPlayWithType(EAudioType type)
        {
            BaseAudioSO audioCollection = AudioType[type];
            if (audioCollection == null) return;
            AudioManager.PlayWithInit(audioCollection.GetAudio(), true);
        }
    }
}
