using UnityEngine;
using Swift_Blade.Pool;
using System.Collections.Generic;

namespace Swift_Blade.Audio
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class AudioManager : MonoSingleton<AudioManager>
    {
		//https://docs.unity3d.com/ScriptReference/ScriptableSingleton_1.html
		//https://docs.unity3d.com/ScriptReference/FilePathAttribute.html

        [SerializeField] private PoolPrefabMonoBehaviourSO poolPrefabMonoBehaviourSO;
        //private static AudioEmitter audioEmitter2D;
        internal static readonly Dictionary<int, uint> audioDictionary = new(16);
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
        public static AudioEmitter Play(AudioSO audioSO)
        {
            Debug.Assert(audioSO != null, "audioSO is null");

            AudioEmitter audioEmitter = GetEmitter();
            audioEmitter.PlayWithInit(audioSO);

            return audioEmitter;
        }
    }
}
