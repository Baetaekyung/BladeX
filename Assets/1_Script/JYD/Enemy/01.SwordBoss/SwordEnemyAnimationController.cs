using Swift_Blade.Enemy;
using UnityEngine;

namespace Swift_Blade.Enemy.Sword
{
    public class SwordEnemyAnimationController : BaseEnemyAnimationController
    {
        public float maxAnimationSpeed;
        public float minAnimationSpeed;
        
        protected void Start()
        {
            float animationSpeed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
            Animator.SetFloat("Speed" ,animationSpeed);
        }

        public void Rebind()
        {
            Animator.Rebind();
        }
    }
}
