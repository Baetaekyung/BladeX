using Swift_Blade.Combat;
using Swift_Blade.Combat.Projectile;
using Swift_Blade.Feeling;
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

        [SerializeField] CameraShakeType camShakType;

        [SerializeField] private StoneType stoneType;
        [SerializeField] private bool isClosedToPlayer;

        [SerializeField] private GameObject parryObj;
        [SerializeField] private GameObject DestroyedEffectObj;

        protected override void Start()
        {
            //Invoke("MissCountdown", 2);
        }

        private void MissCountdown()
        {
            Debug.Log("놓친돌이 있습니다");
            CoinManager.Instance.DiscountCoin();
        }

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

                CameraShakeManager.Instance.DoShake(camShakType);

                transform.localScale *= 10;

                for (int i = 0; i < 4; i++)
                {
                    Vector3 spawnPos = transform.position;
                    switch (i)
                    {
                        case 0:
                            spawnPos += Vector3.zero;
                            break;
                        case 1:
                            spawnPos += new Vector3(.1f, 0, 0);
                            break;
                        case 2:
                            spawnPos += new Vector3(0, 0, .1f);
                            break;
                        case 3:
                            spawnPos += new Vector3(.1f, 0, .1f);
                            break;
                    }
                    
                    Instantiate(DestroyedEffectObj, spawnPos, Quaternion.identity);
                }
                for (int i = 0; i < 9; i++)
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
                IsPerryNow(other.GetComponent<PlayerParryController>().GetParry());
            }

            if(other.CompareTag("Ground"))
            {
                CameraShakeManager.Instance.DoShake(camShakType);
                MissCountdown();
                Destroy(this.gameObject);
            }
        }
    }
}
