using Swift_Blade.Pool;
using DG.Tweening;
using UnityEngine;
using DG.Tweening;


namespace Swift_Blade
{
    public class WeaponOrb : BaseOrb
    {
        [SerializeField] private WeaponSO weapon;
        [SerializeField] private PoolPrefabMonoBehaviourSO blastPrefab;
        
        protected override bool CanInteract => true;
        protected override Tween InteractTween()
        {
            return transform.DOPunchScale(Vector3.one, 0.2f, -1, 0.5f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        protected override void Interact()
        {
            Vector3 originalScale = new Vector3(startFadeScale, startFadeScale, startFadeScale);
            transform.localScale = originalScale;
            PlayerWeaponManager playerWeaponManager = Player.Instance.GetEntityComponent<PlayerWeaponManager>();
            WeaponSO tempWeapon = PlayerWeaponManager.CurrentWeapon;
            playerWeaponManager.SetWeapon(weapon);
            weapon = tempWeapon;

            MonoGenericPool<BlastParticle>.Pop().transform.position = transform.position;
            
            base.Interact();
        }

       
        private void Start()
        {
            MonoGenericPool<BlastParticle>.Initialize(blastPrefab);
        }

    }
}
