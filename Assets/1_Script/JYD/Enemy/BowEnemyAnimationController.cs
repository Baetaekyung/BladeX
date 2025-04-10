using Swift_Blade.Audio;
using UnityEngine;
using System;

namespace Swift_Blade.Enemy.Bow
{
    public class BowEnemyAnimationController : BaseEnemyAnimationController
    {
        [SerializeField] private Bowstring bowstring;
        
        private event Action<AudioSO> OnAudioPlayEvent;
        
        protected override void Start()
        {
            base.Start();
            StopDrawBowstring();
            
            OnAudioPlayEvent += OnAudioPlayTrigger;
        }

        private void OnDestroy()
        {
            OnAudioPlayEvent -= OnAudioPlayTrigger;
        }

        public void StartDrawBowstring()
        {
            bowstring.canDraw = true;
        }
        
        public void StopDrawBowstring()
        {
            bowstring.canDraw = false;
        }
        
        private void OnAudioPlay(AudioSO audio) => OnAudioPlayEvent?.Invoke(audio);
        private void OnAudioPlayTrigger(AudioSO audioSO)
        {
            AudioManager.PlayWithInit(audioSO, true);
        }
        
        private void OnAudioPlayCollection(AudioCollectionSO audioCollectionSo) => OnAudioPlay(audioCollectionSo.GetRandomAudio);
        
    }
}
