using Swift_Blade.Feeling;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;

namespace Swift_Blade
{
    public class PlayerCamera : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float multiplier;
        [SerializeField] private Transform resultTransform;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Camera staticCamera;
        
        [Header("Level Clear")]
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private CameraFocusSO focusData;
        [SerializeField] private CameraShakeType cameraShakeType;
        
        [Range(0.1f,50f)][SerializeField] private float levelClearCameraDistance;
        [Range(0.1f,3f)][SerializeField] private float increaseDuration;
        [Range(0.1f,3f)][SerializeField] private float delay;
        [Range(0.1f,3f)][SerializeField] private float cameraShakeDelay;
        [Range(0.1f,3f)][SerializeField] private float decreaseDuration;
        private float originalCameraDistance;
        
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
        
            sceneManager.LevelClearEvent += LevelClearCameraEffect;

            originalCameraDistance = CameraDistance;
        }

        private void OnDestroy()
        {
            sceneManager.LevelClearEvent -= LevelClearCameraEffect;
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

        private void LevelClearCameraEffect()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                DOVirtual.Float(CameraDistance, levelClearCameraDistance, increaseDuration,
                        x => cinemachinePositionComposer.CameraDistance = x)
                    .SetEase(Ease.OutSine)
            );
            
            sequence.JoinCallback(()=>DOVirtual.DelayedCall(cameraShakeDelay , ()=> CameraShakeManager.Instance.DoShake(cameraShakeType)));
            
            sequence.AppendInterval(delay);
                
            sequence.Append(
                DOVirtual.Float(levelClearCameraDistance, originalCameraDistance, decreaseDuration,
                        x => cinemachinePositionComposer.CameraDistance = x)
                    .SetEase(Ease.OutSine)
            );
        }
                
    }
}
