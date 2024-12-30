using UnityEngine;

namespace Swift_Blade
{
    public class PlayerAnimator : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Animator animator;
        public Animator GetAnimator => animator;

    }
}
