using Swift_Blade.Combat;
using Swift_Blade.Combat.Projectile;
using UnityEngine;

namespace Swift_Blade
{
    public class MinigameThrowed : BaseThrow
    {
        public enum StoneType
        {
            Gem,
            Boom
        }

        [SerializeField] private StoneType stoneType;
        [SerializeField] private bool isClosedToPlayer;
        [SerializeField] private GameObject parryObj;
        public override void SetDirection(Vector3 force)
        {
            base.SetDirection(force);
            base.SetRigid(false, 0.5f);
            Debug.Log("따라오너라~");
        }

        public void IsPerryNow(bool canParry)
        {
            if(canParry)
            {
                Debug.Log("패리밍");
                transform.localScale *= 10;
                for(int i = 0; i < 9; i++)
                {
                    Instantiate(parryObj, transform.position, Quaternion.identity);
                }
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Debug.Log("페리범위내에들어오다");
                IsPerryNow(other.GetComponent<PlayerParryController>().CanParry());
            }
        }
    }
}
