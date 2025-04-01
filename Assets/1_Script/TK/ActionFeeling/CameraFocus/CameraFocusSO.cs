using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade.Feeling
{
    [CreateAssetMenu(fileName = "CameraFocusData", menuName = "SO/CameraFocusData")]
    public class CameraFocusSO : ScriptableObject
    {
        [Tooltip("얼마나 가까이 바라 볼 것인가? 10이면 생각보다 가깝다.")]
        public float focusAmount;
        
        [Tooltip("얼마나 오래 바라볼 것인가?")]
        public float focusTime;
        
        [Tooltip("앞쪽으로 포커스 할 것인가?")]
        public bool isFront = true;
        
        [Tooltip("얼마나 빠르게 바라볼 것인가?")]
        public float increaseSpeed;
        
        [Tooltip("얼마나 빠르게 돌아올 것인가?")]
        public float decreaseSpeed;
        
        [Tooltip("포커스 후 바로 원래 FOV로 돌아올 것인가?")]
        public bool isImmediatelyReturn = false;

        [HideInInspector] 
        public WaitForSeconds FocusWait => new WaitForSeconds(focusTime);
    }
}
