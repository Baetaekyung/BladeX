using UnityEngine;

namespace Swift_Blade
{
    public abstract class TagEffectBase : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] protected int minTagCount = 2;

        [Space]
        [TextArea] public string tagEffectDescription;

        protected Player _player;

        public virtual void Initialize(Player player)
        {
            _player = player;
        }
        protected abstract void TagDisableEffect();
        protected abstract void TagEnableEffect(int tagCount);
        public virtual void TagEffect(int tagCount, bool isIncreasing)
        {
            bool isValid = IsTagCountValid(tagCount);
            if (!isValid)
            {
                bool isTagCountOut = tagCount == minTagCount - 1;

                if (!isIncreasing && isTagCountOut)
                {
                    TagDisableEffect();
                }
                return;
            }
            TagEnableEffect(tagCount);
        }
        protected virtual bool IsTagCountValid(int tagCount)
        {
            bool result = tagCount >= minTagCount;
            return result;
        }
    }
}
