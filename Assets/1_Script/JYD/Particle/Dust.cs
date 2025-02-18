using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Pool.Dust
{
    public class Dust : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<Dust>.Push(this);
        }
    }
}
