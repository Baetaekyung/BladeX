using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour,IDamageble
    {
        public float maxHealth;
        public float currentHealth;

        public event Action OnDeadEvent;
        public event Action OnHitEvent;


        [Header("Flash info")]
        [SerializeField] private float flashDuration;
        [SerializeField] private Material _flashMat;
        [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;
        private Material[] _originMats;
        
        private void Start()
        {
            currentHealth = maxHealth;
            
            _meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _originMats = new Material[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _originMats[i] = _meshRenderers[i].material;
            }

            OnHitEvent += FlashMat;
        }
        
        private void OnDestroy()
        {
            OnHitEvent -= FlashMat;
        }

        public void TakeDamage(ActionData actionData)
        {
            float damageAmount = actionData.damageAmount;
            currentHealth -= damageAmount;
            
            OnHitEvent?.Invoke();
            
            if (currentHealth <= 0)
                Dead();
        }

        public void TakeHeal()
        {
            
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            //Debug.Log("플레이어 죽었슴");
        }
        
        private void FlashMat()
        {
            StartCoroutine(FlashRoutine());
        }
    
        private IEnumerator FlashRoutine()
        {
            foreach (var t in _meshRenderers)
            {
                t.material = _flashMat;
            }
        
            yield return new WaitForSeconds(flashDuration);

            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originMats[i];
            }
        }
    }
}
