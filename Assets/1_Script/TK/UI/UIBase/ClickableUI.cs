using UnityEngine;
using UnityEngine.EventSystems;


namespace Swift_Blade
{
    [RequireComponent(typeof(AudioSource))]
    public class ClickableUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioClip toPlay;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void ClickSound()
        {
            if (_audioSource == null)
                return;

            _audioSource.Stop();
            _audioSource.clip = toPlay;
            _audioSource.Play();
        }

        public void OnPointerDown(PointerEventData eventData) => ClickSound();
    }
}
