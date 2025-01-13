using UnityEngine;

namespace Swift_Blade
{
    public class PlayerAnimator : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Animator animator;
        public Animator GetAnimator => animator;

        public void EntityComponentAwake(Entity entity)
        {

        }
        public void PlayAnimation(int hash, int layer = -1)
        {
            animator.Play(hash, layer);
        }

    }
}
