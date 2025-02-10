using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class Explosion : MonoBehaviour,IPoolable
    {
        private ParticleSystem _particle;
        
        public void OnPopInitialize()
        {
            if (_particle == null)
                _particle = GetComponent<ParticleSystem>();
            
            _particle.Simulate(0);
            _particle.Play();
        }
    }
}
