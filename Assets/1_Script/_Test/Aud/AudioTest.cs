using Swift_Blade.Audio;
using UnityEngine;

namespace Swift_Blade
{
    public class AudioTest : MonoBehaviour
    {
        [SerializeField] private AudioSO so;
        [SerializeField] private Vector3 instancePosition;
        private AudioEmitter audioEmitterInstance;
        private void Start()
        {
            audioEmitterInstance = AudioManager.GetEmitter();
            audioEmitterInstance.Initialize(so);
        }
        void Update()
        {
            audioEmitterInstance.transform.position = instancePosition;
            if (Input.GetKeyDown(KeyCode.J))
            {
                AudioEmitter instance = audioEmitterInstance;
                instance.transform.position = instancePosition;
                instance.Play(true);
            }
            if (Input.GetKeyDown(KeyCode.K))
                audioEmitterInstance.StopAudio();
        }
    }
}
