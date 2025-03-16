using System;
using UnityEngine;

namespace Swift_Blade.Enemy
{
    public class Bowstring : MonoBehaviour
    {
        [SerializeField] private Transform leftEnd;  
        [SerializeField] private Transform rightEnd; 
        [SerializeField] private Transform drawPoint;
        [SerializeField] private LineRenderer lineRenderer;

        public bool canDraw;

        private void Update()
        {
            lineRenderer.enabled = canDraw;
            
            if (lineRenderer.enabled)
            {
                lineRenderer.SetPosition(0, leftEnd.position);
                lineRenderer.SetPosition(1, drawPoint.position);
                lineRenderer.SetPosition(2, rightEnd.position);
            }
            
            
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}
