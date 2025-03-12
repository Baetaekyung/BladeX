using UnityEngine;

namespace Swift_Blade
{
    public class EscapeFromTarkov : MonoBehaviour
    {
        [SerializeField] private LevelClearEventSO levelClearSO;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            {
                levelClearSO.SceneChangeEvent?.Invoke("Store");
            }
        }
    }
}
