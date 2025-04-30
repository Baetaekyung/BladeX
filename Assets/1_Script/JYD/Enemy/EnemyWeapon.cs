using UnityEngine;
using System.Collections;

namespace Swift_Blade.Enemy
{
    public class EnemyWeapon : MonoBehaviour
    {
        private float rotateDuration = 0.7f;
        private float rotateSpeed = 240;
    
        private void Awake()
        {
            transform.parent = null;
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
        }
        
        private void Start()
        {
            StartCoroutine(RotateOverTime());
        }
        
        private IEnumerator RotateOverTime()
        {
            float elapsed = 0f;

            Vector3 randomAxis = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)).normalized;
            
            while (elapsed < rotateDuration)
            {
                float delta = Time.deltaTime;
                transform.Rotate(randomAxis * (rotateSpeed * delta), Space.Self);
                elapsed += delta;
                yield return null;
            }
        }
    }
}