using UnityEngine;

namespace Swift_Blade
{
    public class DefaultPressurePlateListener : ListenerBase
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
            FireBaseAction();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            Gizmos.DrawRay(transform.position, transform.forward);
        }
    }
}
