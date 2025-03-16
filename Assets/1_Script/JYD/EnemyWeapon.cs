using System;
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
            float explosionForce = 500f;
            float explosionRadius = 2f; 

            rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }
    }
}