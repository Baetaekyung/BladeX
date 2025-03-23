using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Audio
{
    public abstract class BaseAudioSO : ScriptableObject
    {
        public abstract AudioSO GetAudio();
    }
}
