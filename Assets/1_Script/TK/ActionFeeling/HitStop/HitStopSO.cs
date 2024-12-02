using UnityEngine;

namespace Swift_Blade
{
    public enum HitStopType
    {
        IMMEDIATE,
        SMOOTH
    }

    public enum HitStopPriority
    {
        NONE = 0,
        PLAYER = 1,
        ENEMY = 2,
        LAST = 9 //9 is magic number
    }
    
    [CreateAssetMenu(fileName = "HitStopData", menuName = "SO/HitStopData")]
    public class HitStopSO : ScriptableObject
    {
        [Tooltip("지속시간은 리얼타임 기준이라 타임스케일에 영향을 받지 않음")]
        public float duration;
        [Tooltip("HitStop중일 때의 타임 스케일 느릴 수록 강조되어 보임")]
        public float timeScale;
        [Tooltip("이 타입에 따라서 부드럽게 타임스케일이 전환될 지 아니면 바로 전환 될 지 정해짐")]
        public HitStopType hitStopType;
        [Tooltip("우선순위 낮을 수록 우선적으로 판단함.")]
        public HitStopPriority hitStopPriority;
    }
}
