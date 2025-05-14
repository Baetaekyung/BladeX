using System;
using Swift_Blade.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Combat.Feedback
{
    public class FlashMatChangeFeedback : Feedback
    {
        public Material changeMat;
        public EffectType effectType;
        
        private Material originalMat;
        private FlashFeedback flashFeedback;

        private BaseEnemy enemy;
        
        private void Start()
        {
            flashFeedback = GetComponent<FlashFeedback>();
            if (effectType != EffectType.None)
            {
                enemy = transform.root.GetComponent<BaseEnemy>();
                enemy.GetEffectController().OnEffectEvents.Add(effectType , HandleEffect);
            }
        }
        
        private void OnDestroy()
        {
            enemy.GetEffectController().OnEffectEvents.Remove(effectType);
        }

        private void HandleEffect(bool active)
        {
            if(active)
                PlayFeedback();
            else
            {
                ResetFeedback();
            }
        }
        
        public override void PlayFeedback()
        {
            originalMat = flashFeedback.GetFlashMat();
            flashFeedback.SetFlashMat(changeMat);
            
            flashFeedback.PlayFeedback();
        }

        public override void ResetFeedback()
        {
            flashFeedback.SetFlashMat(originalMat);
            flashFeedback.ResetFeedback();
        }
        
    }
}
