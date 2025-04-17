using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class PlayerDamageCaster : LayerCaster, IEntityComponent, IEntityComponentStart
    {
        public float CastingRange { get => _castingRange; set => _castingRange = value; }

        [SerializeField] private Transform _visualTrm;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private StatSO damageStat;

        private Player _player;
        private PlayerStatCompo _statCompo;
        private float GetBaseDamage => _statCompo.GetStat(damageStat).Value;

        private readonly Collider[] hitColliders = new Collider[10];
        private HashSet<IHealth> damagedEntities;

        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
        }

        public void EntityComponentStart(Entity entity)
        {
            _statCompo = entity.GetEntityComponent<PlayerStatCompo>();
            damagedEntities = new HashSet<IHealth>();
        }

        public override bool Cast()
        {
            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + _visualTrm.forward * _castingRange;

            Physics.OverlapSphereNonAlloc(endPos, _casterRadius, hitColliders, whatIsTarget);

            bool isHit = false;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IHealth health))
                {
                    isHit = true;

                    Vector3 hitPoint = hitCollider.ClosestPoint(startPos);
                    Vector3 hitNormal = (hitPoint - hitCollider.transform.position).normalized;

                    float damageAmount = _statCompo.GetStat(damageStat).Value;

                    ActionData actionData = new ActionData(hitPoint, hitNormal, damageAmount, false);
                    ApplyDamage(health, actionData);
                }
            }

            if (isHit)
            {
                _player.GetSkillController.UseSkill(SkillType.Attack, hitColliders.Select(x => x.transform).ToArray());
            }

            return isHit;
        }
        public bool Cast(float additionalDamage = 0, float additionalCastingDistance = 0, bool stun = false)
        {
            damagedEntities.Clear();

            Vector3 startPos = GetStartPosition();
            Vector3 endPos = startPos + _visualTrm.forward * (_castingRange + additionalCastingDistance);

            Collider[] hitColliders = Physics.OverlapSphere(endPos, _casterRadius, whatIsTarget);

            bool isHit = false;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IHealth health))
                {
                    if (!damagedEntities.Add(health))
                        continue;

                    isHit = true;

                    Vector3 hitPoint = hitCollider.ClosestPoint(startPos);
                    Vector3 hitNormal = (hitPoint - hitCollider.transform.position).normalized;

                    float damageAmount = _statCompo.GetStat(damageStat).Value + additionalDamage;
                    float critialPercent = _statCompo.GetStat(StatType.CRITICAL_CHANCE).Value;
                    float critialDamageMultiplier = _statCompo.GetStat(StatType.CRITICAL_DAMAGE).Value;

                    bool  isCritial = Random.Range(0, 100f) < critialPercent;

                    if (isCritial)
                    {
                        damageAmount = (damageAmount * (critialDamageMultiplier / 100f));
                        FloatingTextGenerator.Instance.GenerateText(Mathf
                            .RoundToInt(damageAmount).ToString(), hitPoint, Color.yellow);
                    }
                    else
                    {
                        FloatingTextGenerator.Instance.GenerateText(Mathf
                            .RoundToInt(damageAmount).ToString(), hitPoint);
                    }

                    ActionData actionData = new ActionData(hitPoint, hitNormal, damageAmount, stun);

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
    }
}
