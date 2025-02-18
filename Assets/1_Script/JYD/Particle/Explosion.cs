using System;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Pool.Explosion
{
    public class Explosion : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<Explosion>.Push(this);
        }
        
    }
}
