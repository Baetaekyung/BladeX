using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.Rendering;

namespace Swift_Blade.Combat.Feedbck
{
    public class BloodScreenFeedback : Feedback
    {
        public Volume volume;

        private Vignette vignette;
        
        [Range(0.1f, 1)] [SerializeField] private float increaseDuration = 0.5f; 
        [Range(0.1f, 2)] [SerializeField] private float decreaseDuration = 0.5f; 
        [Range(0.1f, 1)] [SerializeField] private float maxValue = 0.5f;

        private Coroutine feedbackCoroutine;
        
        private void Start()
        {
            volume.profile.TryGet(out vignette);
        }

        public override void PlayFeedback()
        {
            if(vignette == null)return;
            
            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
            }
            feedbackCoroutine = StartCoroutine(PlayVignetteEffect());
        }

        private IEnumerator PlayVignetteEffect()
        {
            float elapsedTime = 0f;

            while (elapsedTime < increaseDuration)
            {
                elapsedTime += Time.deltaTime;
                vignette.intensity.value = Mathf.Lerp(0f, maxValue, elapsedTime / increaseDuration);
                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < decreaseDuration)
            {
                elapsedTime += Time.deltaTime;
                vignette.intensity.value = Mathf.Lerp(maxValue, 0f, elapsedTime / decreaseDuration);
                yield return null;
            }

            ResetFeedback();
        }

        public override void ResetFeedback()
        {
            vignette.intensity.value = 0f; 
        }
    }
}