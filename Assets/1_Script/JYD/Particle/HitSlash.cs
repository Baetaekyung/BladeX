using UnityEngine;

namespace Swift_Blade.Pool.HitSlash
{
    public class HitSlash : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<HitSlash>.Push(this);
        }
    }
}
