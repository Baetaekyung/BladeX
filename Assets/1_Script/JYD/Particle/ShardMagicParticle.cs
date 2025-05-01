using Swift_Blade.Combat.Health;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public class ShardMagicParticle : ParticlePoolAble<ShardMagicParticle>
    {
        [SerializeField] private string color;
        private float damage = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BaseEnemyHealth health))
            {
                ActionData actionData = new ActionData
                {
                    stun = true,
                    damageAmount =  damage,
                };
                actionData.hurtType = 1;
                FloatingTextGenerator.Instance.GenerateText($"<color={color}>{damage}</color>",other.gameObject.transform.position + new Vector3(0,0.5f,0));
                                
                health.TakeDamage(actionData);
            }    
        }
        
        public void SetDamage(float damage)
        {
            this.damage = damage;
        }
    }
    
}
