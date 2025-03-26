using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Audio
{
    [CreateAssetMenu(fileName = "AudioSOSet", menuName = "SO/AudioSOSet")]
    public class AudioSOSet : BaseAudioSO
    {
        [SerializeField] private AudioCollectionSO[] audioCollections;
        public override AudioSO GetAudio()
        {
            throw new NotSupportedException();
        }
    }
}
