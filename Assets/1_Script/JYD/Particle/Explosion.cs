using System;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class Explosion : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<Explosion>.Push(this);
        }
        
    }
}
