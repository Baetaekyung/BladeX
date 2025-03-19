using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Linq;
using UnityEngine.Rendering;

[System.Serializable]
public struct VignetteInfo
{
    public Color color;
    public float intensity;
    public float smoothness;
}

namespace Swift_Blade.Combat.Feedback
{
    public class BloodScreenFeedback : Feedback
    {
        public VolumeProfile profile;
        public VignetteInfo origin;
        public VignetteInfo bloodScreen;
        
        private Vignette vignette;
        
        [Range(0.1f, 1)] [SerializeField] private float increaseDuration = 0.5f; 
        [Range(0.1f, 2)] [SerializeField] private float decreaseDuration = 0.5f; 
        
        private Coroutine feedbackCoroutine;
        
        private void Start()
        {
            profile.TryGet(out vignette);
            ApplyVignetteInfo(origin);
        }
        
        private void ApplyVignetteInfo(VignetteInfo info)
        {
            vignette.color.value = info.color;
            vignette.intensity.value = info.intensity;
            vignette.smoothness.value = info.smoothness;
        }

        [ContextMenu("Test")]
        public override void PlayFeedback()
        {
            if(vignette == null) return;
            
            if (feedbackCoroutine != null)
            {
                StopCoroutine(feedbackCoroutine);
            }
            feedbackCoroutine = StartCoroutine(PlayVignetteEffect());
        }

        private IEnumerator PlayVignetteEffect()
        {
            float elapsedTime = 0f;

            Color startColor = vignette.color.value;
            float startIntensity = vignette.intensity.value;
            float startSmoothness = vignette.smoothness.value;
            
            while (elapsedTime < increaseDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / increaseDuration;
                
                vignette.color.value = Color.Lerp(startColor, bloodScreen.color, t);
                vignette.intensity.value = Mathf.Lerp(startIntensity, bloodScreen.intensity, t);
                vignette.smoothness.value = Mathf.Lerp(startSmoothness, bloodScreen.smoothness, t);
                
                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < decreaseDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / decreaseDuration;
                
                vignette.color.value = Color.Lerp(bloodScreen.color, origin.color, t);
                vignette.intensity.value = Mathf.Lerp(bloodScreen.intensity, origin.intensity, t);
                vignette.smoothness.value = Mathf.Lerp(bloodScreen.smoothness, origin.smoothness, t);
                
                yield return null;
            }

            ResetFeedback();
        }
        private void OnDestroy()
        {
            ResetFeedback();
        }

        public override void ResetFeedback()
        {
            ApplyVignetteInfo(origin);
        }
    }
}