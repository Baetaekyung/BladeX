using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public abstract class ParticlePoolAble : MonoBehaviour,IPoolable
    {
        private ParticleSystem _particle;

        [SerializeField] private float pushTime;
        private float pushTimer = 0;
        
        public void OnPopInitialize()
        {
            if (_particle == null)
                _particle = GetComponent<ParticleSystem>();

            pushTimer = 0;
            _particle.Simulate(0);
            _particle.Play();
        }

        private void Update()
        {
            pushTimer += Time.deltaTime;

            if (pushTimer >= pushTime)
                Push();
        }

        protected abstract void Push();
        
    }
}
