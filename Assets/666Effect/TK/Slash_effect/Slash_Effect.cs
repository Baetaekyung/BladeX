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

        [SerializeField] private Material slashMaterial;
        
        private void Awake()
        {
            _ps = GetComponent<ParticleSystem>();
            _mat = new Material(slashMaterial);
            ParticleSystemRenderer renderModule = _ps.GetComponent<ParticleSystemRenderer>();
            
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

            while (time < _ps.main.duration)
            {
                _mat.SetFloat(_durationHash, time / _ps.main.duration);
                
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
