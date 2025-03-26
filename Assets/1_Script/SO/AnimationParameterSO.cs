using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "AnimationParameterSO", menuName = "SO/AnimationParameterSO", order = int.MinValue)]
    public class AnimationParameterSO : ScriptableObject
    {
        [SerializeField] private string paramName;
        [SerializeField] private float period;
        public int GetAnimationHash { get; private set; }
        public float GetPeriod => period;

        private void Awake()
        {
            if (string.IsNullOrEmpty(paramName) == false)
            {
                GetAnimationHash = Animator.StringToHash(paramName);
            }
        }
        //private void OnEnable()
        //{
        //}
    }
}
