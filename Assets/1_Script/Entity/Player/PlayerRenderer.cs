using UnityEngine;

namespace Swift_Blade
{
    public class PlayerRenderer : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Transform playerVisualTransform;
        public Animator GetPlayerAnimator => playerAnimator;
        public Transform GetPlayerVisualTrasnform => playerVisualTransform;

    }
}
