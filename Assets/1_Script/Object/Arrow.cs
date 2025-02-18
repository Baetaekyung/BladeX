using System.Collections;
using UnityEngine;

namespace Swift_Blade.Object
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Rigidbody rigidBody;
        private bool deadFlag;
        private void Awake()
        {
            Vector3 velocity = transform.forward;
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.linearVelocity = velocity * speed;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (deadFlag) return;
            deadFlag = true;
            //do dead arrow stuff here
            Destroy(gameObject, 0.1f);

            if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(new ActionData() { damageAmount = 1, stun = false });
            }
        }

    }
}
