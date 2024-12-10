using UnityEngine;

namespace Swift_Blade
{
    public class VFXPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private Transform createTrm;
        
        public void PlayParticle(int idx)
        {
            particleSystems[idx].Simulate(0);
            particleSystems[idx].Play();
        }
        
        public void CreateParticle(int idx)
        {
            ParticleSystem newObj = Instantiate(particleSystems[idx],createTrm.position, Quaternion.LookRotation(transform.forward));
            
            
            newObj.Simulate(0);
            newObj.Play();
        }
        
    }
}
