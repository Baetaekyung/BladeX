using UnityEngine;

namespace Swift_Blade
{
    public class ShakeCardObj : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log($"Interacted + {transform.name}");
        }
    }
}
