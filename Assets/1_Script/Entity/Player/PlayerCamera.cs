using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerCamera : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float multiplier;
        [SerializeField] private Transform resultTransform;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Camera staticCamera;
        private CinemachinePositionComposer cinemachinePositionComposer;

        public float CameraTargetDistance { get; set; }
        public float CameraDistance { get; private set; }
        public Quaternion GetResultQuaternion => resultTransform.rotation;
        public Quaternion GetResultQuaternionOnlyY
        {
            get
            {
                Vector3 resultVector = resultTransform.eulerAngles;
                resultVector.x = 0;
                resultVector.z = 0;
                return Quaternion.Euler(resultVector);
            }
        }
        public Camera GetPlayerCamera => playerCamera;
        public Camera GetStaticCamera => staticCamera;
        public void EntityComponentAwake(Entity entity)
        {
            cinemachinePositionComposer = GetComponentInChildren<CinemachinePositionComposer>();
            CameraDistance = cinemachinePositionComposer.CameraDistance;
            CameraTargetDistance = cinemachinePositionComposer.CameraDistance;
        }
        private void FixedUpdate()
        {
            //UpdateCameraDistance();
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


    }
}
