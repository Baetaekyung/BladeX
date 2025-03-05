using UnityEngine;

namespace Swift_Blade
{
    public class Taedadada : MonoBehaviour, IInteractable
    {
        [SerializeField] private string d;
        void IInteractable.Interact()
        {
            print(d);
        }
    }
}
