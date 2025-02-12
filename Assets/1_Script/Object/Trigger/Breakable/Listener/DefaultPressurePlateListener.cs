using UnityEngine;

namespace Swift_Blade
{
    public class DefaultPressurePlateListener : MonoBehaviour
    {
        [SerializeField] private PressurePlate pressurePlate;
        [SerializeField] private GameObject objectToSpawn;
        private void Awake()
        {
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
