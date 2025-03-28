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

            arrow.transform.position = firePos.transform.position;
            Vector3 targetDir = (target.position + new Vector3(0,1,0) - firePos.position).normalized;
            Vector3 rot = firePos.forward;
            arrow.transform.rotation = Quaternion.LookRotation(new Vector3(rot.x, targetDir.y, rot.z));
            
            arrow.Shot();
            
            return true;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        
    }
}
