using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerCamera : PlayerComponentBase, IEntityComponentRequireInit
    {
        public float CameraDistance { get; private set; }
        public float CameraTargetDistance { get; set; }
        [SerializeField] private float multiplier;

        private CinemachinePositionComposer cinemachinePositionComposer;
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

    }
}
