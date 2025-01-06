using Unity.Cinemachine;
using UnityEngine;

namespace Swift_Blade.Feeling
{
    public enum CameraShakeType
    {
        UpDown,
        LeftRight,
        ParryShake,
        Weak,
        Middle,
        Strong
    }
    
    /// <summary>
    /// Low priority first
    /// </summary>
    public enum CameraShakePriority
    {
        NONE = 0,
        PLAYER = 1,
        ENEMY = 2,
        //else
        LAST = 9 //magic num
    }
    
    [CreateAssetMenu(fileName = "CameraShake_", menuName = "SO/CameraShakeData")]
    public class CameraShakeSO : ScriptableObject
    {
        [Tooltip("임펄스 모양")]
        public CinemachineImpulseSource cinemachineImpulseSource;
        [Tooltip("화면 흔들림 계수")]
        public float strength = 1f;
    }
}
