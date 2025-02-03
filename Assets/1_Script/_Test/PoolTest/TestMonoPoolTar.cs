using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class TestMonoPoolTar : MonoBehaviour, IPoolable
    {
        [SerializeField] private int a = 0;
        private float timer = 0;
        public void OnPopInitialize()
        {
            a = 0;
            timer = 0;
        }
        private void Update()
        {
            if (timer > 5)
            {
                a = 10;
            }
            else
                timer += Time.deltaTime;
        }

    }
}
