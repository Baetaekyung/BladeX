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
        Strong,
        PlayerAttack,
        PlayerDamage,
        PlayerParry,
        EnterDoor
    }
    
    /// <summary>
    /// 우선순위가 낮을 수록 우선적임
    /// </summary>
    public enum CameraShakePriority
    {
        NONE = 0,
        PLAYER = 1,
        ENEMY = 2,
        //else
        LAST = 9 //이 이상으로 만들 것 같지는 않음
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
