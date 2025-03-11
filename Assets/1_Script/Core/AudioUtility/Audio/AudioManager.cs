using UnityEngine;
using Swift_Blade.Pool;


namespace Swift_Blade.Audio
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO poolPrefabMonoBehaviourSO;
        //private static AudioEmitter audioEmitter2D;
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
        public static AudioEmitter GetEmitter(AudioSO audioSO)
        {
            AudioEmitter audioEmitter = MonoGenericPool<AudioEmitter>.Pop();
            audioEmitter.Initialize(audioSO);
            return audioEmitter;
        }
        public static AudioEmitter PlayWithInit(AudioSO audioSO, bool destroyOnEnd = false)
        {
            Debug.Assert(audioSO != null, "audioSO is null");

            AudioEmitter audioEmitter = GetEmitter();
            audioEmitter.PlayWithInit(audioSO, destroyOnEnd);

            return audioEmitter;
        }
        public static AudioEmitter PlayOneShotWithInit(AudioSO audioSO)
        {
            throw new System.NotImplementedException("no");
            return default;
        }

    }
}
