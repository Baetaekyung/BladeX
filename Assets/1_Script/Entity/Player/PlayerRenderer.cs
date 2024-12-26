using UnityEngine;

namespace Swift_Blade
{
    public class PlayerRenderer : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Transform playerVisualTransform;
        public PlayerAnimator GetPlayerAnimator => playerAnimator;
        public Transform GetPlayerVisualTrasnform => playerVisualTransform;
        public void LookTarget(Vector3 worldPos)
        {
            Vector3 targetVector = worldPos - playerVisualTransform.position;
            targetVector.y = 0;//or player y value;
            Quaternion result = Quaternion.LookRotation(targetVector, Vector3.up);
            SetVisualRotation(result);
        }
        public void LookTargetSmooth(Vector3 inputDirection, float angleMultiplier)
        {
            Transform playerVisualTransform = GetPlayerVisualTrasnform;
            if (inputDirection.sqrMagnitude > 0)
            {
                Quaternion result = Quaternion.LookRotation(inputDirection, Vector3.up);
                float angle = Vector3.Angle(inputDirection, playerVisualTransform.forward);
                float maxDegreesDelta = Time.deltaTime * angle * angleMultiplier;
                result = Quaternion.RotateTowards(playerVisualTransform.rotation, result, maxDegreesDelta);
                SetVisualRotation(result);
            }

        }
        public void SetVisualRotation(Quaternion quaternion) => playerVisualTransform.rotation = quaternion;
    }
}
