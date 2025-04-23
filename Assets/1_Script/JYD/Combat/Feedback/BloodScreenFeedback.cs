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

    public VignetteInfo(Color color, float intensity, float smoothness)
    {
        this.color = color;
        this.intensity = intensity;
        this.smoothness = smoothness;
    }
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
            if(profile == null)
                Debug.LogError("Blood Screen Feedback Profile is null");
            
            profile.TryGet(out vignette);
            ApplyVignetteInfo(ref origin , new VignetteInfo(vignette.color.value , vignette.intensity.value , vignette.smoothness.value));
        }
        
        private void ApplyVignetteInfo(ref VignetteInfo info1,VignetteInfo info2)
        {
            info1.color = info2.color;
            info1.intensity = info2.intensity;
            info1.smoothness = info2.smoothness;
        }
        
        private void ApplyVignetteInfo(Vignette info1,VignetteInfo info2)
        {
            info1.color.value = info2.color;
            info1.intensity.value = info2.intensity;
            info1.smoothness.value = info2.smoothness;
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
            ApplyVignetteInfo(vignette,origin);
        }
    }
}