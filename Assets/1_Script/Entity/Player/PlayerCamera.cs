using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerCamera : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float multiplier;
        [SerializeField] private Transform viewTransform;
        [SerializeField] private Transform resultTransform;
        [SerializeField] private Camera playerCamera;
        private CinemachinePositionComposer cinemachinePositionComposer;

        public float CameraTargetDistance { get; set; }
        public float CameraDistance { get; private set; }
        public Quaternion GetResultQuaternion => resultTransform.rotation;
        public Camera GetPlayerCamera => playerCamera;
        public void EntityComponentAwake(Entity entity)
        {
            cinemachinePositionComposer = GetComponentInChildren<CinemachinePositionComposer>();
            CameraDistance = cinemachinePositionComposer.CameraDistance;
            CameraTargetDistance = cinemachinePositionComposer.CameraDistance;
        }
        private void FixedUpdate()
        {
            UpdateCameraDistance();
        }
        private void UpdateCameraDistance()
        {
const float minValue = 3;
            float multiplierByDistance = Mathf.Abs(CameraDistance - CameraTargetDistance);
            multiplierByDistance = Mathf.Max(minValue, multiplierByDistance);
            //UI_DebugPlayer.Instance.GetList[0].text = multiplierByDistance.ToString() + " : " + CameraDistance;
            CameraDistance = Mathf.MoveTowards(CameraDistance, CameraTargetDistance, Time.deltaTime * multiplierByDistance * multiplier);
            cinemachinePositionComposer.CameraDistance = CameraDistance;
        }
        public void SetViewTransformRotation(Quaternion quaternion) => viewTransform.rotation = quaternion;
        public void SetViewTransformRotation(Vector3 vector3) => viewTransform.rotation = Quaternion.Euler(vector3);
        public void SetTransformRotation(Quaternion quaternion) => resultTransform.rotation = quaternion;
        public void SetTransformRotation(Vector3 vector3) => resultTransform.rotation = Quaternion.Euler(vector3);

    }
}
