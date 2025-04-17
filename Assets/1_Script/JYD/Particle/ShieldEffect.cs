using Swift_Blade.Combat.Health;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Swift_Blade
{
    public class ShieldEffect : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 300)] private float rotateSpeed;
        [SerializeField] private float delayTime = 0.35f;
        
        [SerializeField] [Range(0.1f, 5)] private float fadeInTime;
        [SerializeField] [Range(0.1f, 2)] private float fadeOutTime;
        
        private Material[] shieldMats;
        private Transform[] shieldTrms;
        
        private const string TINT_COLOR = "_TintColor";
        private const float MAX_ALPHA_VALUE = 0.4f;

        private bool IsfadeInCompleted = false; 
            
        private Transform followTrm;
        private PlayerHealth health;
        
        private void Awake()
        {
            shieldMats = GetComponentsInChildren<MeshRenderer>().Select(x => x.material).ToArray();
            shieldTrms = GetComponentsInChildren<Transform>().ToArray();
        }
        
        public void PlayShield(Transform followTarget)
        {
            followTrm = followTarget;
            health = followTarget.GetComponentInParent<PlayerHealth>();
            health.OnShieldBreakEvent+=BreakShield;
            StartCoroutine(PlayShieldCoroutine());
        }
        
        private void Update()
        {
            transform.position = followTrm.position;
            transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        }
        
        private void BreakShield(int shieldAmount)
        {
            Material mat = shieldMats[shieldAmount];
            //shieldTrms[shieldIndex].DOScale();
            
            if (mat == null)
            {
                StopShield();
                return;
            }

            if (IsfadeInCompleted == false)
            {
                IsfadeInCompleted = true;

                StopAllCoroutines();

                foreach (var item in shieldMats)
                {
                    CompleteFade(item,MAX_ALPHA_VALUE);
                }
            }
                                    
            StartCoroutine(FadeCoroutine(mat, 0f, fadeOutTime));
        }

        public void StopShield()
        {
            health.OnShieldBreakEvent -= BreakShield;
            
            StartCoroutine(StopShieldCoroutine());
        }

        private IEnumerator PlayShieldCoroutine()
        {
            for (float timer = 0; timer < delayTime; timer += Time.deltaTime)
            {
                yield return null;
            }
            
            foreach (var mat in shieldMats)
            {
                StartCoroutine(FadeCoroutine(mat, MAX_ALPHA_VALUE, fadeInTime));
            }
        }

        private IEnumerator StopShieldCoroutine()
        {
            foreach (var mat in shieldMats)
            {
                yield return StartCoroutine(FadeCoroutine(mat, 0f, fadeOutTime));
            }

            for (float timer = 0; timer < fadeOutTime; timer += Time.deltaTime)
            {
                yield return null;
            }
            
            Destroy(gameObject);
        }

        private IEnumerator FadeCoroutine(Material material, float endValue, float time)
        {
            Color c = material.GetColor(TINT_COLOR);
            float startValue = c.a;
            float elapsedTime = 0f;
            
            while (elapsedTime < time)
            {
                c.a = Mathf.Lerp(startValue, endValue, elapsedTime / time);
                material.SetColor(TINT_COLOR, c);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            CompleteFade(material , endValue);
        }

        private void CompleteFade(Material material, float endValue)
        {
            IsfadeInCompleted = true;
            
            Color c = material.GetColor(TINT_COLOR);
            c.a = endValue;
            material.SetColor(TINT_COLOR, c);
            
        }
        
    }
}
