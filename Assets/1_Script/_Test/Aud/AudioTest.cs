using Swift_Blade.Audio;
using UnityEngine;

namespace Swift_Blade
{
    public class AudioTest : MonoBehaviour
    {
        [SerializeField] private AudioSO so;
        [SerializeField] private Vector3 instancePosition;
        [SerializeField] private AudioEmitter audioEmitterInstance;
        private void Start()
        {
            audioEmitterInstance = AudioManager.GetEmitter();
            audioEmitterInstance.Initialize(so);
        }
        void Update()
        {
            //audioEmitterInstance.transform.position = instancePosition;
            if (Input.GetKeyDown(KeyCode.J))
            {
                audioEmitterInstance.Initialize(so);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                audioEmitterInstance.Play(true);
            }
            if(Input.GetKeyDown(KeyCode.Space))
                AudioEmitter.Dbg(so);
            //bool a = audioEmitterInstance.AudioSource.isPlaying;
            //Debug.Log(a);
            //if (!a) Debug.Log("_stopped!");
        }
    }
}
