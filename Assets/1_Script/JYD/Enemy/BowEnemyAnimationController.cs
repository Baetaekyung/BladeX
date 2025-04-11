using Swift_Blade.Audio;
using UnityEngine;
using System;

namespace Swift_Blade.Enemy.Bow
{
    public class BowEnemyAnimationController : BaseEnemyAnimationController
    {
        [SerializeField] private Bowstring bowstring;
        
        
        protected override void Start()
        {
            base.Start();
            StopDrawBowstring();
            
        }
        
        public void StartDrawBowstring()
        {
            bowstring.canDraw = true;
        }
        
        public void StopDrawBowstring()
        {
            bowstring.canDraw = false;
        }

        private void OnAudioPlay(AudioSO audio)
        {
            AudioManager.PlayWithInit(audio, true);
        }
        
        private void OnAudioPlayCollection(AudioCollectionSO audioCollectionSo) => OnAudioPlay(audioCollectionSo.GetRandomAudio);
        
    }
}
