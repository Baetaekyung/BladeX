using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class BossVFX : MonoBehaviour
    {
        
        [Header("Flash info")]
        [SerializeField] private Material _flashMat;
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
        private Material[] _originMats;


        private void Start()
        {
            _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _originMats = new Material[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _originMats[i] = _meshRenderers[i].material;
            }
        }
    
        public void FlashMat(ActionData actionData)
        {
            StartCoroutine(FlashRoutine());
        }
    
        private IEnumerator FlashRoutine()
        {
            foreach (var t in _meshRenderers)
            {
                t.material = _flashMat;
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originMats[i];
            }
        }
                

        /*
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private Transform createTrm;
        
        [SerializeField] private Transform target;
        
        
        
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
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(target.position , 5f);
        }
        */
           
        
    }
}
