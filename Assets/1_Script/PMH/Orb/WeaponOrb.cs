using Swift_Blade.Pool;
using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{

    public class WeaponOrb : BaseOrb
    {
        [SerializeField] private WeaponOrbParticle weaponOrbParticle;
        [SerializeField] private WeaponSO weapon;

        [SerializeField] private PoolPrefabMonoBehaviourSO blastPrefab;

        protected override bool CanInteract => true;
        protected override void Awake()
        {
            base.Awake();
            MonoGenericPool<BlastParticle>.Initialize(blastPrefab);
            weaponOrbParticle.SetWeapon(weapon);
        }
        protected override Tween InteractTween()
        {
            return transform.DOPunchScale(Vector3.one, 0.2f, -1, 0.5f)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        protected override void Interact()
        {
            PlayerWeaponManager playerWeaponManager = Player.Instance.GetEntityComponent<PlayerWeaponManager>();
            WeaponSO previousPlayerWeapon = PlayerWeaponManager.CurrentWeapon;
            bool isAlreadyEquipted = previousPlayerWeapon == weapon;
            if (isAlreadyEquipted) return;

            playerWeaponManager.SetWeapon(weapon);

            Vector3 originalScale = new Vector3(startFadeScale, startFadeScale, startFadeScale);
            transform.localScale = originalScale;


            weapon = previousPlayerWeapon;
            weaponOrbParticle.SetWeapon(previousPlayerWeapon);
            MonoGenericPool<BlastParticle>.Pop().transform.position = transform.position;

            base.Interact();
        }
    }
}
