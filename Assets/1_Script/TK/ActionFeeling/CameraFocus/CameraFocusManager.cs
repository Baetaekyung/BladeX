using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade.Feeling
{
    [MonoSingletonUsage(MonoSingletonFlags.DontDestroyOnLoad)]
    public class CameraFocusManager : MonoSingleton<CameraFocusManager>
    {
        private const float DEFAULT_CAMERA_FOV = 46f; //기본 FOV
        private int currentLayered = 1;

        [Header("포커스 할 카메라")]
        [SerializeField]
        private CinemachineCamera _camera;
        private CinemachineCamera _targetCamera;
        
        [Header("코루틴 변수들")]
        private Coroutine _focusRoutine;

        private event Action _onCompleteEvent;

        private void Start()
        {
            SceneManager.sceneLoaded += FindCamera;

            //나중에 지워야함.테스트용.
            FindCamera(SceneManager.GetActiveScene(), LoadSceneMode.Additive);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            SceneManager.sceneLoaded -= FindCamera;
        }


        private void OnValidate()
        {
            Debug.Assert(_camera != null, "Camera 인스펙터에서 넣어주기");
        }


        private void FindCamera(Scene scene, LoadSceneMode mode)
        {
            _camera = GameObject.FindWithTag("PlayerCamera").GetComponent<CinemachineCamera>();
            if (_camera != null)
                _targetCamera = _camera;
        }

        //포커스할 카메라 바꿀거면 이거 실행해서 변경
        public void SetTargetCamera(CinemachineCamera targetCamera)
        {
            _targetCamera = targetCamera;
        }

        //포커스 실행
        public CameraFocusManager StartFocus(CameraFocusSO focusData)
        {
            if (_focusRoutine != null) //코루틴이 현재 실행중이면
            {
                StopCoroutine(_focusRoutine); //코루틴 중지하고
                currentLayered++;
            }

            _focusRoutine = StartCoroutine(FocusRoutine(focusData, currentLayered)); //포커스 진행

            return this;
        }

        private IEnumerator FocusRoutine(CameraFocusSO focusData, int currentLayer)
        {
            if (_targetCamera == null)
            {
                Debug.LogWarning("타겟 카메라가 존재하지 않음, SetTargetCamera를 호출하여 카메라 설정하기.");
                yield break;
            }

            float focusProgress = 0; //포커스 진행도

            //FOV (클 수록 멀리보고, 작을 수록 가까이 본다)
            float lensFOV = _targetCamera.Lens.FieldOfView;
            float currentFOV = lensFOV; //현재의 FOV의 시작은 lensFOV부터

            //클 수록 멀리 보기 때문에 플레이어 쪽으로 당기면 -, 아니면 +이다.
            var targetFOV = focusData.isFront
                ? lensFOV - (focusData.focusAmount / currentLayer)
                : lensFOV + (focusData.focusAmount / currentLayer);

            yield return FocusOn(focusData, focusProgress, currentFOV, targetFOV);

            yield return focusData.FocusWait; //포커스 지속시간 동안 지속

            if (focusData.isImmediatelyReturn) //즉시 리턴할 경우 바로 기본 FOV로 변경
            {
                _targetCamera.Lens.FieldOfView = DEFAULT_CAMERA_FOV;
            }
            else //아니면 포커스 속도에 따라서 변경
            {
                focusProgress = 0; //시간 초기화

                yield return FocusOff(focusData, focusProgress, currentFOV);
            }

            currentLayered = 1;
            InvokeCompleteEvent();
        }

        private IEnumerator FocusOff(CameraFocusSO focusData, float focusProgress, float currentFOV)
        {
            while (focusProgress < 1)
            {
                focusProgress += focusData.decreaseSpeed * Time.deltaTime;
                currentFOV = Mathf.Lerp(currentFOV, DEFAULT_CAMERA_FOV, focusProgress);

                _targetCamera.Lens.FieldOfView = currentFOV;
                yield return null;
            }
            _targetCamera.Lens.FieldOfView = DEFAULT_CAMERA_FOV;
        }

        private IEnumerator FocusOn(CameraFocusSO focusData, float focusProgress, float currentFOV, float targetFOV)
        {
            while (focusProgress < 1)
            {
                focusProgress += focusData.increaseSpeed * Time.deltaTime;
                currentFOV = Mathf.Lerp(currentFOV, targetFOV, focusProgress);

                _targetCamera.Lens.FieldOfView = currentFOV;
                yield return null;
            }
            _targetCamera.Lens.FieldOfView = targetFOV; //focusProgress가 1이 아닐 수도 있으니 마지막에
        }

        public void OnComplete(Action onComplete)
        {
            _onCompleteEvent = onComplete;
        }
        
        private void InvokeCompleteEvent()
        {
            _onCompleteEvent?.Invoke();
            _onCompleteEvent = null;
        }
    }
}