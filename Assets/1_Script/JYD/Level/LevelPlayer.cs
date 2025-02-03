using System;
using UnityEngine;

namespace Swift_Blade.Level
{
    public class LevelPlayer : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private Vector3 lastPosition;

        private void Start()
        {
            
        }

        void LateUpdate()
        {
            Vector3 movement = (transform.position - lastPosition) / Time.deltaTime;
            lastPosition = transform.position;

            Vector3 localVelocity = transform.InverseTransformDirection(movement);

            animator.SetFloat("X" , localVelocity.x);
            animator.SetFloat("Z" , localVelocity.z);
            
        }
        
                        
        
    }
}