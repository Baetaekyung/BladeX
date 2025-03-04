using Swift_Blade.Combat.Projectile;
using UnityEngine;

namespace Swift_Blade.Enemy.Throw
{
    public class ThrowAnimatorController : BaseEnemyAnimationController 
    {
        public Transform target;
        public Transform throwHolder;

        public Collider bodyCollider;
        
        private BaseThrow _throw;
        public void SetStone(BaseThrow stone)
        {
            if (stone == null)
                if (_throw != null)
                {
                    _throw.transform.SetParent(null);
                    _throw.SetPhysicsState(false);
                }
                        
            _throw = stone;
        }

        public void CatchStone()
        {
            _throw.SetPhysicsState(true);
            _throw.transform.SetParent(throwHolder);
            _throw.transform.localEulerAngles = Vector3.zero;
            _throw.transform.localPosition = Vector3.zero;
        }

        public void ThrowStone()
        {
            var direction = (target.position - transform.position).normalized;

            _throw.SetDirection(direction);
            _throw = null;
        }

        public void StartManualCollider()
        {
            bodyCollider.enabled = true;
        }
        
        public void StopManualCollider()
        {
            bodyCollider.enabled = false;
        }
        
        
    }
}
