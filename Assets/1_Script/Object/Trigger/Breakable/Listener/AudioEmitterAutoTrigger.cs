using Swift_Blade.Audio;
using UnityEngine;

namespace Swift_Blade
{
    public class AudioEmitterAutoTrigger : MonoBehaviour
    {
        [SerializeField] private AudioEmitter audioEmitter;
        [SerializeField] private ListenerBase listener;
        [SerializeField] private bool oneShot;
        private void Awake()
        {
            if (oneShot) listener.DefaultFireEvent += () => audioEmitter.PlayOneShot();
            else listener.DefaultFireEvent += () => audioEmitter.Play();
        }
    }
}
