using UnityEngine;

namespace Swift_Blade
{
    public class PlayerRenderer : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Transform playerVisualTransform;
        public PlayerAnimator GetPlayerAnimator => playerAnimator;
        public Transform GetPlayerVisualTrasnform => playerVisualTransform;

    }
}
