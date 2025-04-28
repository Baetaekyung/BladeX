using Swift_Blade.Combat.Caster;
using Swift_Blade.Pool;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Swift_Blade
{
    public class DragonTagEffect : TagEffectBase
    {
        [SerializeField] private GameObject effect;
        private Transform _playerTrm;
        private GameObject _currentEffect;

        [SerializeField] private float detectRange;

        [SerializeField, Range(0, 10f)] private float maxEffectTick;
        [SerializeField, Range(0, 10f)] private float middleEffectTick;
        [SerializeField, Range(0, 10f)] private float minEffectTick;

        [SerializeField] private LayerMask lm_Enemy;
        [SerializeField] private float maxEffectDamage;
        [SerializeField] private float middleEffectDamage;
        [SerializeField] private float minEffectDamage;

        private float _currentTick;
        private float _currentDamage;

        public override void Initialize(Player player)
        {
            base.Initialize(player);

            _playerTrm = player.transform.GetChild(0);
        }

        public override void DisableTagEffect(int tagCount)
        {
            CancelInvoke(nameof(HandleDamage));

            if(_currentEffect != null)
            {
                Destroy(_currentEffect);
                _currentEffect = null;
            }
        }

        public override void EnableTagEffect(int tagCount)
        {
            if (tagCount < minTagCount)
                return;

            if (tagCount >= maxTagCount)
                SetEffect(maxEffectTick, maxEffectDamage);
            else if (tagCount >= middleTagCount)
                SetEffect(middleEffectTick, middleEffectDamage);
            else if (tagCount >= minTagCount)
                SetEffect(minEffectTick, minEffectDamage);

            InvokeRepeating(nameof(HandleDamage), _currentTick, _currentTick);

            _currentEffect = Instantiate(effect, _playerTrm);
            _currentEffect.transform.localPosition = Vector3.up * 0.15f;
        }

        private void SetEffect(float tick, float damage)
        {
            _currentTick = tick;
            _currentDamage = damage;
        }

        private void HandleDamage()
        {
            RaycastHit[] hits = new RaycastHit[10];

            int cnt = Physics.SphereCastNonAlloc(
                _playerTrm.transform.position,
                detectRange,
                _playerTrm.transform.up,
                hits,
                Mathf.Infinity,
                lm_Enemy);

            if (cnt == 0)
                return;

            for(int i = 0; i < cnt; i++)
            {
                if (hits[i].collider.TryGetComponent(out IHealth health))
                {
                    Vector3 hitPoint = hits[i].collider.ClosestPoint(hits[i].point);

                    ActionData ack = new ActionData(hitPoint, hits[i].normal, _currentDamage, false);
                    health.TakeDamage(ack);

                    FloatingTextGenerator.Instance.GenerateText(Mathf
                            .RoundToInt(_currentDamage).ToString(), hitPoint);
                }
            }
        }

        public override bool IsValidToEnable(int tagCount)
        {
            if (tagCount < minTagCount)
                return false;

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
}
