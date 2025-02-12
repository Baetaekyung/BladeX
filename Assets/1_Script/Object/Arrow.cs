using System.Collections;
using UnityEngine;

namespace Swift_Blade.Object
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Rigidbody rigidBody;
        private void Awake()
        {
            Vector3 velocity = transform.forward;
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.linearVelocity = velocity * speed;
        }


    }
}
