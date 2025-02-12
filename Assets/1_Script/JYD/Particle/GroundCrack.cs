using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class GroundCrack : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<GroundCrack>.Push(this);
        }
    }
}
