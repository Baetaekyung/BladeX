using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade
{
    public class Cubeeee : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private int hash = Animator.StringToHash("New Animationcube");
        private void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                animator.Play(hash, -1, 0f);
            }
        }
    }
}
