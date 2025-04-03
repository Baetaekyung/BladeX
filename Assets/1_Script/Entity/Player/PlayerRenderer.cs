using UnityEngine;

namespace Swift_Blade
{
    public class PlayerRenderer : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Transform playerVisualTransform;
        public PlayerAnimator GetPlayerAnimator => playerAnimator;
        public Transform GetPlayerVisualTrasnform => playerVisualTransform;
        public void LookAtPosition(Vector3 worldPos)
        {
            Vector3 targetVector = worldPos - playerVisualTransform.position;
            targetVector.y = 0;
            Quaternion result = Quaternion.LookRotation(targetVector, Vector3.up);
            SetVisualRotation(result);
        }
        public void LookAtDirection(Vector3 direction)
        {
            Vector3 targetVector = direction;
            targetVector.y = 0;
            Quaternion result = Quaternion.LookRotation(targetVector, Vector3.up);
            SetVisualRotation(result);
        }
        public void LookAtDirectionSmooth(Vector3 direction, float angleMultiplier)
        {
            Transform playerVisualTransform = GetPlayerVisualTrasnform;
            if (direction.sqrMagnitude > 0)
            {
                Quaternion result = Quaternion.LookRotation(direction, Vector3.up);
                float angle = Vector3.Angle(direction, playerVisualTransform.forward) * 0.8f;
                float maxDegreesDelta = Time.deltaTime * angle * angleMultiplier;
                result = Quaternion.RotateTowards(playerVisualTransform.rotation, result, maxDegreesDelta);
                SetVisualRotation(result);
            }
        }
        public void SetVisualRotation(Quaternion quaternion) => playerVisualTransform.rotation = quaternion;

        public void EntityComponentAwake(Entity entity)
        {

        }
    }
}
