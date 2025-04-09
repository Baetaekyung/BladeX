using System;
using UnityEngine;

namespace Swift_Blade
{
    public class DisplayObject : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 2f;
        [SerializeField] private float moveSpeed = 1f;
        private float originalY;

        private void Awake()
        {
            originalY = transform.position.y;
        }

        void Update()
        {
            float sinWave = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            Vector3 r = transform.position;
            r.y = sinWave + originalY;
            transform.position = r;
        }
    }
}