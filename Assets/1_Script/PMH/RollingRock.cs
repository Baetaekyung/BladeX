using UnityEngine;

namespace Swift_Blade
{
    public class RollingRock : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animCurve;
        [SerializeField] private Transform VisualTrm;

        private float timeing = 0;

        private void Update()
        {
            timeing += Time.deltaTime;

            float valueAnimCurv = animCurve.Evaluate(Mathf.Clamp((timeing / 5) % 1, 0, 1));
            transform.position += new Vector3(0, 0, -9) * (Time.deltaTime * valueAnimCurv * 3);

            VisualTrm.transform.Rotate(new Vector3(-60, 0, 0) * (Time.deltaTime * 5));
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerHealth ph;
                print(other.gameObject);
            if(other.TryGetComponent<PlayerHealth>(out ph))
            {
                ActionData ad = new ActionData();
                ad.damageAmount = 1;
                ph.TakeDamage(ad);
            }
        }

        public void DestroyTheRock()
        {

        }
    }
}
