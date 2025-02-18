using UnityEngine;

namespace Swift_Blade.Combat.Caster
{
    public class PlayerDamageCaster : LayerCaster, IEntityComponent
    {
        [SerializeField][Range(0.5f, 10f)] private float _casterRadius = 1f;
        [SerializeField][Range(0f, 10f)] private float _casterInterpolation = 0.5f;
        [SerializeField][Range(0f, 10f)] private float _castingRange = 1f;
        
        [SerializeField] private Transform _visualTrm;
        [SerializeField] private PlayerMovement _playerMovement;
        
        private Player _player;
        
        public void EntityComponentAwake(Entity entity)
        {
            _player = entity as Player;
        }
        
        public override bool CastDamage()
        {
            Vector3 startPos = GetStartPosition();
                    
            bool isHit = Physics.SphereCast(
                startPos,
                _casterRadius,
                _visualTrm.forward, 
                out RaycastHit hit,
                _castingRange, targetLayer);
        
            if(isHit)
            {
                if(hit.collider.TryGetComponent(out IDamageble health))
                {
                    ActionData actionData = new ActionData(hit.point, hit.normal, 10, transform , true);
                                    
                    OnCastDamageEvent?.Invoke(actionData);
                    
                    health.TakeDamage(actionData);
                }
            }
            return isHit;
        }
        
        private Vector3 GetStartPosition()
        {
            return transform.transform.position
                   + _visualTrm.forward * (-_casterInterpolation * 2);
        }
        
        protected void OnDrawGizmosSelected()
        {
            if (_visualTrm == null) return;
            if (_playerMovement == null) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetStartPosition(), _casterRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetStartPosition() + _visualTrm.forward * _castingRange, _casterRadius);
            Gizmos.color = Color.white;
        }
    }
}
