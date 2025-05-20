using Swift_Blade.Audio;
using UnityEngine;

namespace Swift_Blade
{
    [DefaultExecutionOrder(10)]
    //[MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class BGM : MonoSingleton<BGM>
    {
        [SerializeField] private AudioEmitter emitter;
        public static bool HasInit { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);//what???/
            HasInit = true;
        }
        private void Start()
        {
            emitter.Play();
            emitter.GetAudioSource.loop = true;
        }
        public void Stop()
        {
            emitter.StopAudio();
        }
        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
