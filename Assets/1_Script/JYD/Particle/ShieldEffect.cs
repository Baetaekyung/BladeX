using System.Collections;
using System.Linq;
using UnityEngine;
using Swift_Blade.Pool;

namespace Swift_Blade
{
    public class ShieldEffect : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 300)] private float rotateSpeed;
        [SerializeField] private float delayTime = 0.35f;
        
        [SerializeField] [Range(0.1f, 5)] private float fadeInTime;
        [SerializeField] [Range(0.1f, 2)] private float fadeOutTime;

        [SerializeField] private PoolPrefabMonoBehaviourSO hexagonParticle;
        
        private Material[] shieldMats;
        
        private const string TINT_COLOR = "_TintColor";
        private const float MAX_ALPHA_VALUE = 0.4f;

        private int _currentShieldAmount = 0;
        
        private void Awake()
        {
            shieldMats = GetComponentsInChildren<MeshRenderer>().Select(x => x.material).ToArray();
        }
        
        private void Update()
        {
            if(_currentShieldAmount == 0) return;

            transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        }

        public void SetShield(int amount)
        {
            if(amount == _currentShieldAmount) return;

            float angle = 360f / amount;
            for(int i = 0; i < amount; ++i)
            {
                transform.GetChild(i).localEulerAngles = Vector3.up * (angle * i);
            }

            if(amount > _currentShieldAmount)
            {
                AddShield(amount);
            }
            else
            {
                BreakShield(amount);
            }

            _currentShieldAmount = amount;
        }

        private void AddShield(int amount)
        {
            MonoGenericPool<HexagonParticle>.Initialize(this.hexagonParticle);
            
            HexagonParticle hexagonParticle = MonoGenericPool<HexagonParticle>.Pop();
            hexagonParticle.transform.SetParent(transform);
            hexagonParticle.transform.localPosition = Vector3.zero;

            for(int i = 0; i < amount; ++i)
            {
                StartCoroutine(FadeCoroutine(shieldMats[i], MAX_ALPHA_VALUE, fadeInTime));
            }
        }
        
        private void BreakShield(int amount)
        {
            for(int i = _currentShieldAmount - 1; i >= amount; --i)
            {
                StartCoroutine(FadeCoroutine(shieldMats[i], 0f, fadeOutTime));
            }
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
            Color c = material.GetColor(TINT_COLOR);
            c.a = endValue;
            material.SetColor(TINT_COLOR, c);
        }
    }
}
