using UnityEngine;

namespace Swift_Blade
{
    public class VFXPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;
                
        public void PlayParticle(int idx)
        {
            particleSystems[idx].Simulate(0);
            particleSystems[idx].Play();
        }
    }
}
