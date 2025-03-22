using UnityEngine;

namespace Swift_Blade.Pool
{
    public class ParticlePoolAble<T> :  MonoBehaviour,IPoolable where T : MonoBehaviour,IPoolable
    {
        private ParticleSystem _particle;

        [SerializeField] private float pushTime;
        private float pushTimer = 0;
        
        public virtual void OnPop()
        {
            if (_particle == null)
                _particle = GetComponent<ParticleSystem>();
            
            pushTimer = 0;
            _particle.Simulate(0);
            _particle.Play();
        }
        
        protected virtual void Update()
        {
            pushTimer += Time.deltaTime;

            if (pushTimer >= pushTime)
                Push();
        }

        protected virtual void Push()
        {
            MonoGenericPool<T>.Push((this as T));
        }
    }
}
