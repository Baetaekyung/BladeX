namespace Swift_Blade.Pool
{
    public class FollowFireParticle : FollowParticle<FollowFireParticle>
    {
        private const float FIRE_LIFE_TIME = 1.5f;
        public void SetDuration(float duration)
        {
            pushTime = duration;
            
            _particle.Stop();
            
            var module = _particle.main;
            module.duration = duration - FIRE_LIFE_TIME;
            
            _particle.Simulate(0);
            _particle.Play();
        }
                
    }
            
}
