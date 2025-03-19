using UnityEngine;

namespace Swift_Blade.Audio
{
    [CreateAssetMenu(fileName = "AudioCollection", menuName = "Scriptable Objects/Audio/AudioCollection")]
    public class AudioSOCollection : BaseAudioSO
    {
        [field: SerializeField] public AudioSO[] AudioList { get; private set; }
        public AudioSO GetRandomAudio => AudioList[Random.Range(0, AudioList.Length)];

        public override AudioSO GetAudio()
        {
            return GetRandomAudio;
        }
    }
}
