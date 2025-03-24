using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Audio
{
    [CreateAssetMenu(fileName = "AudioSOSet", menuName = "Scriptable Objects/AudioSOSet")]
    public class AudioSOSet : BaseAudioSO
    {
        [SerializeField] private AudioSOCollection[] audioCollections;
        public override AudioSO GetAudio()
        {
            throw new NotSupportedException();
        }
    }
}
