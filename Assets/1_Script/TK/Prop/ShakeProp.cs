using UnityEngine;

namespace Swift_Blade
{
    public class ShakeProp : MonoBehaviour
    {
        [SerializeField] private Vector3 shakeDirection;
        [SerializeField] private float   maxShakeAmount;
        [SerializeField] private float   minShakeAmount;
        [SerializeField] private float   shakeSpeed;

        private float currentShakeAmount;
        private int   shakeMultiplier;

        private void Start() => shakeMultiplier = 1;

        private void Update()
        {
            if(currentShakeAmount > maxShakeAmount)
            {
                shakeMultiplier = -1;
            }
            else if(currentShakeAmount < minShakeAmount)
            {
                shakeMultiplier = 1;
            }

            currentShakeAmount += Time.deltaTime * shakeSpeed * shakeMultiplier;

            transform.rotation = Quaternion.Euler(shakeDirection * currentShakeAmount);
        }
    }
}
