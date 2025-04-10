using Swift_Blade.Pool;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class WeaponOrb : BaseOrb
    {
        [SerializeField] private WeaponSO weapon;
        [SerializeField] private PoolPrefabMonoBehaviourSO blastPrefab;

        private void Start()
        {
            MonoGenericPool<BlastParticle>.Initialize(blastPrefab);
        }

        protected override TweenCallback CreateDefaultCallback()
        {
            return 
                () => 
            { 
                Player.Instance.GetEntityComponent<PlayerWeaponManager>().SetWeapon(weapon); 
                //Debug.Log("onend" + weapon.name); 

                MonoGenericPool<BlastParticle>.Pop().transform.position = transform.position + new Vector3(0, 0.5f , 0);
                
                Destroy(gameObject); 
            };
        }

    }
}
