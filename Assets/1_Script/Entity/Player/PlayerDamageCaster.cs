using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class PlayerDamageCaster : LayerCaster, IEntityComponent, IEntityComponentStart
    {
        [SerializeField][Range(0.5f, 10f)] private float _casterRadius = 1f;
        [SerializeField][Range(0f, 10f)] private float _casterInterpolation = 0.5f;
        [SerializeField][Range(0f, 10f)] private float _castingRange = 1f;

        [SerializeField] private Transform _visualTrm;
        [SerializeField] private PlayerMovement _playerMovement;

        [SerializeField] private StatSO damageStat;

        private Player _player;
        private PlayerStatCompo _statCompo;

        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
        }

        public void EntityComponentStart(Entity entity)
        {
            _statCompo = entity.GetEntityComponent<PlayerStatCompo>();
        }

        public override bool Cast()
        {
            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + _visualTrm.forward * _castingRange;

            Collider[] hitColliders = Physics.OverlapSphere(endPos, _casterRadius, targetLayer);

            bool isHit = false;
            HashSet<IDamageble> damagedEntities = new HashSet<IDamageble>();

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageble health))
                {
                    if (damagedEntities.Contains(health))
                        continue;

                    damagedEntities.Add(health);
                    isHit = true;

                    Vector3 hitPoint = hitCollider.ClosestPoint(startPos);
                    Vector3 hitNormal = (hitPoint - hitCollider.transform.position).normalized;

                    float damageAmount = _statCompo.GetStat(damageStat).Value;
                    ActionData actionData = new ActionData(hitPoint, hitNormal, damageAmount, false);

                    OnCastDamageEvent?.Invoke(actionData);


                    health.TakeDamage(actionData);
                }
            }

            if (isHit)
            {
                _player.GetSkillController.UseSkill(SkillType.Attack, hitColliders.Select(x => x.transform).ToArray());
            }

            return isHit;
        }

        private Vector3 GetStartPosition()
        {
            return transform.transform.position
                   + _visualTrm.forward * (-_casterInterpolation * 2);
        }

        //protected void OnDrawGizmosSelected()
        //{
        //    if (_visualTrm == null) return;
        //    if (_playerMovement == null) return;
        //    
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireSphere(GetStartPosition(), _casterRadius);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(GetStartPosition() + _visualTrm.forward * _castingRange, _casterRadius);
        //    Gizmos.color = Color.white;
        //}


    }
}
