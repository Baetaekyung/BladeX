using UnityEngine;

namespace Swift_Blade.Enemy
{
    public class EnemyWeapon : MonoBehaviour
    {
        private Rigidbody rb;

        private void Awake()
        {
            transform.parent = null;
            gameObject.AddComponent<BoxCollider>();
            
            rb = gameObject.AddComponent<Rigidbody>();
        }

        void Start()
        {
            Vector3 explosionPosition = transform.position;
            float explosionForce = 100;
            float explosionRadius = 4f;
            
            rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            rb.angularVelocity = new Vector3(UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-20f, 20f), UnityEngine.Random.Range(-5f, 5f));
        }
        
    }
}