using UnityEngine;

namespace Swift_Blade
{
    public class PlayerAnimator : PlayerComponentBase
    {
        [SerializeField] private Animator animator;
        public Animator GetAnimator => animator;

    }
}
