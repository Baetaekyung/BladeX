using JetBrains.Annotations;
using UnityEngine;

namespace Swift_Blade
{
    public class KnightTagEffect : TagEffectBase
    {
        public override void EffectMinTag()
        {
            Debug.Log("2 set effect invoked");
        }

        public override void EffectMiddleTag()
        {
            Debug.Log("3 set effect invoked");
        }

        public override void EffectMaxTag()
        {
            Debug.Log("5 set effect invoked");
        }

        public override bool IsValidToEnable(int tagCount)
        {
            if (tagCount >= minTagCount)
                return true;

            return false;
        }
    }
}
