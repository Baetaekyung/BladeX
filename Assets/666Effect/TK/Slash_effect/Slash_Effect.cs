using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class Slash_Effect : MonoBehaviour
    {
        private readonly int _durationHash = Shader.PropertyToID("_Duration");
        
        private ParticleSystem _ps;
        private Material _mat;
        private Coroutine _coroutine;
        
        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
            ParticleSystemRenderer renderModule = _ps.GetComponent<ParticleSystemRenderer>();
            _mat = new Material(renderModule.material);
            
            renderModule.material = _mat;
        }
        
        private void OnEnable()
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);
            
            _coroutine = StartCoroutine(nameof(DurationCo));
        }

        private IEnumerator DurationCo()
        {
            float time = 0;
            _mat.SetFloat(_durationHash, 0);

            while (time < _ps.main.startLifetimeMultiplier)
            {
                _mat.SetFloat(_durationHash, 1 - time / _ps.main.startLifetimeMultiplier);
                
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
