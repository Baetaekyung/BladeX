using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class BowEnemyCaster : MonoBehaviour,ICasterAble
    {
        public PoolPrefabMonoBehaviourSO arrow;
        public Transform firePos;

        private void Start()
        {
            MonoGenericPool<Arrow>.Initialize(arrow);
        }

        public bool Cast()
        {
            Arrow arrow = MonoGenericPool<Arrow>.Pop();
            arrow.transform.position = firePos.transform.position;
            arrow.transform.rotation = Quaternion.LookRotation(firePos.transform.forward);
            arrow.Shot();
            
            return true;
        }
    }
}
