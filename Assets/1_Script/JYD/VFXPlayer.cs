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
        
        public void CreateParticles(int idx)
        {
            int rand = Random.Range(4, 7); 
            float angleRange = 120f; 
            float halfRange = angleRange / 2; 
            float angleStep = angleRange / (rand - 1); 
            float radius = 2f; 
            
            for (int i = 0; i < rand; i++)
            {
                float angle = -halfRange + (angleStep * i);
                
                Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
                Vector3 offset = rotation * transform.forward * radius;

                ParticleSystem newObj = Instantiate(
                    particleSystems[idx],
                    createTrm.position + offset,
                    Quaternion.LookRotation(offset.normalized) 
                );

                newObj.Simulate(0);
                newObj.Play();
            }
        }
    }
}
