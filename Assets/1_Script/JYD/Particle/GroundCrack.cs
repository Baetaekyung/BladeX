

namespace Swift_Blade.Pool.GroundCrack
{
    public class GroundCrack : ParticlePoolAble
    {
        protected override void Push()
        {
            MonoGenericPool<GroundCrack>.Push(this);
        }
    }
}
