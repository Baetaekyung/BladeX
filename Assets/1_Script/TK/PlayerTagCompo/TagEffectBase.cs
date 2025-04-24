using UnityEngine;

namespace Swift_Blade
{
    public abstract class TagEffectBase : MonoBehaviour
    {
        [SerializeField] protected int minTagCount = 2;
        [SerializeField] protected int middleTagCount = 3;
        [SerializeField] protected int maxTagCount = 4;

        [Space]
        [TextArea] public string tagEffectDescription;

        protected Player _player;

        public virtual void Initialize(Player player)
        {
            _player = player;
        }

        public abstract void EnableTagEffect(int tagCount);
        public abstract void DisableTagEffect(int tagCount);

        public abstract bool IsValidToEnable(int tagCount);
    }
}
