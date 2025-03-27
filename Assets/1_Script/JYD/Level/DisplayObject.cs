using UnityEngine;

namespace Swift_Blade
{
    public class DisplayObject : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 2f;
        [SerializeField] private float moveSpeed = 1f;
        private Vector3 startPos;
        
        void Start()
        {
            startPos = transform.position;
        }

        void Update()
        {
            float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }
}