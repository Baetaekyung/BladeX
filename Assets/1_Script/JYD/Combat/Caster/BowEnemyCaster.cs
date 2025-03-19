using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class BowEnemyCaster : MonoBehaviour,ICasterAble
    {
        public PoolPrefabMonoBehaviourSO arrow;
        public Transform firePos;
        
        private Transform target;
        
        private void Start()
        {
            MonoGenericPool<Arrow>.Initialize(arrow);
        }
        
        public bool Cast()
        {
            Arrow arrow = MonoGenericPool<Arrow>.Pop();
            
            if(firePos == null)return false;
            arrow.transform.position = firePos.transform.position;
            Vector3 targetDir = (target.position - firePos.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(targetDir);
            
            arrow.Shot();
            
            return true;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        
    }
}
