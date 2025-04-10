using UnityEngine;
using DG.Tweening;


namespace Swift_Blade
{
    public class WeaponOrb : BaseOrb
    {
        [SerializeField] private WeaponSO weapon;
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
            base.Interact();
        }

    }
}
