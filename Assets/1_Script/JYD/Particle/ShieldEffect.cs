using System.Collections;
using System.Linq;
using DG.Tweening;
using Swift_Blade.Combat.Health;
using UnityEngine;

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
            
            StartCoroutine(FadeCoroutine(mat, 0f, fadeOutTime));
        }

        public void StopShield()
        {
            health.OnShieldBreakEvent -= BreakShield;
            
            StartCoroutine(StopShieldCoroutine());
        }

        private IEnumerator PlayShieldCoroutine()
        {
            yield return new WaitForSeconds(delayTime);
                        
            foreach (var mat in shieldMats)
            {
                StartCoroutine(FadeCoroutine(mat, 0.4f, fadeInTime));
            }
        }

        private IEnumerator StopShieldCoroutine()
        {
            foreach (var mat in shieldMats)
            {
                yield return StartCoroutine(FadeCoroutine(mat, 0f, fadeOutTime));
            }

            yield return new WaitForSeconds(fadeOutTime);  // FadeOut이 끝날 때까지 기다림
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

            c.a = endValue; // 마지막 값을 정확히 설정
            material.SetColor(TINT_COLOR, c);
        }
    }
}
