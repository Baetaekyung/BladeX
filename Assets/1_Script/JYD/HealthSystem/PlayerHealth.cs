using System;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerHealth : MonoBehaviour,IDamageble
    {
        public float maxHealth;
        public float currentHealth;

        public event Action OnDeadEvent;
        
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

            if (currentHealth <= 0)
                Dead();
        }

        public void TakeHeal()
        {
            
        }

        public void Dead()
        {
            OnDeadEvent?.Invoke();
            Debug.Log("플레이어 죽었슴");
        }
    }
}
