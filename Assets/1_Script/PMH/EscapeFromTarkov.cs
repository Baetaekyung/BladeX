using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade
{
    public class EscapeFromTarkov : MonoBehaviour
    {
        [SerializeField] private SceneManagerSO levelClearSO;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            {
                levelClearSO.LoadScene("Store");
            }
        }
    }
}
