

namespace Swift_Blade.Pool.ParryParticle
{
    public class ParryParticle : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<ParryParticle>.Push(this);
        }
    }
}
