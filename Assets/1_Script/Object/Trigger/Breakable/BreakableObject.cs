using DG.Tweening;
using System;
using UnityEngine;

namespace Swift_Blade
{
    public class BreakableObject : MonoBehaviour, IDamageble
    {
        public event Action<float> OnHit;
        public event Action<BreakableObject> OnDeadStart;
        public event Action<BreakableObject> OnGameObjectDestroy;

        public float delayDead = 1;
        public float health = 10;

        private bool deadFlag;
        public void TakeDamage(ActionData actionData)
        {
            if (deadFlag) return;

            OnHit?.Invoke(health);
            health -= actionData.damageAmount;

            if (health <= 0)
            {
                OnDeadStart?.Invoke(this);
                deadFlag = true;
                DeadDead();
            }
        }
        protected void DeadDead()
        {
            DOVirtual.DelayedCall(delayDead, () =>
            {
                Destroy(gameObject);
                OnGameObjectDestroy?.Invoke(this);
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
