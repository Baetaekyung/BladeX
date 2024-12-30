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
        
        private void Start()
        {
            currentHealth = maxHealth;
            
        }
        
        private void OnDestroy()
        {
            
        }

        public void TakeDamage(ActionData actionData)
        {
            float damageAmount = actionData.damageAmount;
            currentHealth -= damageAmount;
            
            OnHitEvent?.Invoke();
            
            if (currentHealth <= 0)
                Dead();
        }
        

        public void TakeHeal(float amount)
        {
            
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            //Debug.Log("플레이어 죽었슴");
        }
        
    }
}
