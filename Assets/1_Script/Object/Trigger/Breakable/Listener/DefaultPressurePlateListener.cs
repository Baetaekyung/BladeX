using UnityEngine;

namespace Swift_Blade
{
    public class DefaultPressurePlateListener : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSpawn;
        private void Awake()
        {
            PressurePlate pressurePlate = GetComponentInParent<PressurePlate>();
            pressurePlate.OnPressed += OnPressed;
        }
        private void OnPressed()
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            Gizmos.DrawRay(transform.position, transform.forward);
        }
    }
}
