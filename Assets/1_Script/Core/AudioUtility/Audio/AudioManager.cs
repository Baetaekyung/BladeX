using UnityEngine;
using Swift_Blade.Pool;

namespace Swift_Blade.Audio
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        //dont destory on load?
        //readonly static 2D audio emitter
        //return audioEmitter instead on Comopnent
        //preload setting (audioUtility
        //AudioEmitter OneShot, Stop, Pause?
		//https://docs.unity3d.com/ScriptReference/ScriptableSingleton_1.html
		//https://docs.unity3d.com/ScriptReference/FilePathAttribute.html

        [SerializeField] private PoolPrefabMonoBehaviourSO poolPrefabMonoBehaviourSO;
        private static AudioEmitter audioEmitter2D;
        //private static readonly Dictionary<AudioSO, uint> audioDictionary = new(16);
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
            audioEmitter.Play(audioSO);

            return audioEmitter;
        }
    }
}
