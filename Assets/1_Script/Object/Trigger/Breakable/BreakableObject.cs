using DG.Tweening;
using System;
using UnityEngine;

namespace Swift_Blade
{
    public class BreakableObject : MonoBehaviour, IDamageble
    {
        public event Action OnDead;
        public event Action OnGameObjectDestroy;
        public event Action<float> OnHit;

        [SerializeField] private float health = 10;
        [SerializeField] private float delayDead = 1;

        private bool deadFlag;
        public void TakeDamage(ActionData actionData)
        {
            if (deadFlag) return;

            OnHit?.Invoke(health);
            health -= actionData.damageAmount;

            if (health <= 0)
            {
                OnDead?.Invoke();
                deadFlag = true;
                DeadDead();
            }
        }
        protected void DeadDead()
        {
            DOVirtual.DelayedCall(delayDead, () =>
            {
                Destroy(gameObject);
                OnGameObjectDestroy?.Invoke();
            }, false);
        }
        public void TakeHeal(float amount)
        {
            //what
            //throw new System.NotImplementedException();
        }

        public void Dead()
        {
            //throw new System.NotImplementedException();
        }
    }
}
