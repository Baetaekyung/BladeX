using UnityEngine;

namespace Swift_Blade.Enemy.Bow
{
    public class Bow : EnemyWeapon
    {
        private Vector3 colliderSize;
        private Vector3 colliderOffset;
        protected override void Awake()
        {
            base.Awake();
            GetComponent<LineRenderer>().enabled = false;
            rigid.mass = 120f;
            
            colliderOffset = new Vector3(-0.1712249f ,-0.03094427f,-0.02161495f);
            colliderSize = new Vector3(0.3385282f, 1.664268f, 0.07702503f);
            
            boxCollider.size = colliderSize;
            boxCollider.center = colliderOffset;
            
        }
    }
}
