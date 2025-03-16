using DG.Tweening;
using System;
using UnityEngine;

namespace Swift_Blade
{
    public class BreakableObject : MonoBehaviour, IDamageble
    {
        public event Action<float> OnHit;
        public event Action OnDeadStart;
        public event Action OnGameObjectDestroy;

        public float delayDead = 1;
        public float health = 10;

        private bool deadFlag;

        private void DelayDead()
        {
            DOVirtual.DelayedCall(delayDead, 
                () =>
            {
                Destroy(gameObject);
                OnGameObjectDestroy?.Invoke();
            }, false);
        }
        void IDamageble.TakeDamage(ActionData actionData)
        {
            if (deadFlag) return;

            OnHit?.Invoke(health);
            health -= actionData.damageAmount;

            if (health <= 0)
            {
                deadFlag = true;
                gameObject.SetActive(false);
                OnDeadStart?.Invoke();
                DelayDead();
            }
        }
        void IDamageble.TakeHeal(float amount)
        {
            throw new NotImplementedException();
        }

        void IDamageble.Dead()
        {
            throw new NotImplementedException();
        }
    }
}
