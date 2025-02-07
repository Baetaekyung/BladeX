using UnityEngine;
using Swift_Blade.Pool;
using System.Collections.Generic;

namespace Swift_Blade.Audio
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO poolPrefabMonoBehaviourSO;
        //private static AudioEmitter audioEmitter2D;
        internal static readonly Dictionary<int, int> audioDictionary = new(16);
        protected override void Awake()
        {
            base.Awake();
            MonoGenericPool<AudioEmitter>.Initialize(poolPrefabMonoBehaviourSO);
            //audioEmitter2D = MonoGenericPool<AudioEmitter>.Pop();
            //audioEmitter2D.name = "audioEmitter2D";
        }
        public static AudioEmitter GetEmitter()
        {
            AudioEmitter audioEmitter = MonoGenericPool<AudioEmitter>.Pop();
            return audioEmitter;
        }
        public static AudioEmitter PlayWithInit(AudioSO audioSO)
        {
            Debug.Assert(audioSO != null, "audioSO is null");

            int hash = audioSO.clip.GetHashCode();
            audioDictionary[hash] = audioDictionary.TryGetValue(hash, out int count) 
                ? count + 1 
                : 1;

            AudioEmitter audioEmitter = GetEmitter();
            audioEmitter.PlayWithInit(audioSO);

            return audioEmitter;
        }
    }
}
