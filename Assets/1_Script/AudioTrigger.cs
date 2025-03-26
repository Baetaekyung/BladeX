using Swift_Blade.Audio;
using UnityEngine;

namespace Swift_Blade
{
    public class AudioTrigger : MonoBehaviour
    {
        [SerializeField] private AnimationTriggers animTrigger;

        [SerializeField] private SerializableDictionary<EAudioType, BaseAudioSO> audioType;
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
            BaseAudioSO audioCollection = audioType[type];
            AudioManager.PlayWithInit(audioCollection.GetAudio(), true);
        }
    }
}
