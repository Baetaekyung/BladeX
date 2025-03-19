using UnityEngine;

namespace Swift_Blade.Enemy.Bow
{
    public class Bowstring : MonoBehaviour
    {
        [SerializeField] private Transform leftEnd;  
        [SerializeField] private Transform rightEnd; 
        [SerializeField] private Transform drawPoint;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] [Range(1,20)] private float bowStringDecreaseSpeed;


        public bool canDraw;
        private Vector3 lastDrawPosition;

        void Start()
        {
            lastDrawPosition = (rightEnd.position + leftEnd.position) / 2;
        }

        private void Update()
        {
            
            if (canDraw)
            {
                lineRenderer.SetPosition(0, leftEnd.position);
                lineRenderer.SetPosition(1, drawPoint.position);
                lineRenderer.SetPosition(2, rightEnd.position);
            }
            else
            {
                lineRenderer.SetPosition(0 , leftEnd.position);

                lineRenderer.SetPosition(1, Vector3.Lerp(lastDrawPosition, rightEnd.position, Time.deltaTime * bowStringDecreaseSpeed));
                lastDrawPosition = lineRenderer.GetPosition(1);
                
                lineRenderer.SetPosition(2, rightEnd.position);
            }
        }
    }


}
