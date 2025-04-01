using UnityEngine;

namespace Swift_Blade
{
    public class PlayerWeaponManager : MonoBehaviour, IEntityComponent
    {
        //todo : bool check if player initialized in this scene 
        // player is not initialized when new scene is loaded
        private bool isNotInitializedInThisScene = false;

        [SerializeField] private Transform leftHandleTransform;
        [SerializeField] private Transform rightdHandleTransform;
        [SerializeField] private Weapon defaultWeapon;

        private GameObject leftWeaponInstance;
        private GameObject rightWeaponInstance;

        private PlayerAnimator playerAnimator;
        private static Weapon currentWeapon;
        private void Awake()
        {
            Debug.Assert(defaultWeapon != null, "default weapon is null");
        }
        void IEntityComponent.EntityComponentAwake(Entity entity)
        {
            playerAnimator = entity.GetEntityComponent<PlayerAnimator>();
            isNotInitializedInThisScene = true;
            SetWeapon(currentWeapon != null ? currentWeapon : defaultWeapon);
        }
        public void SetWeapon(Weapon weapon)
        {
            if (!isNotInitializedInThisScene && currentWeapon == weapon)
            {
                Debug.LogWarning("weapon is already equipped");
                return;
            }

            //clear current holding weapon
            if (currentWeapon != null)
            {
                if (leftWeaponInstance != null)
                {
                    Destroy(leftWeaponInstance); // todo : need optimazation
                }
                if (rightWeaponInstance != null)
                {
                    Destroy(rightWeaponInstance); // todo : need optimazation
                }
            }

            if (weapon.LeftHandUsage != null)
            {
                GameObject handle = CreateWeaponHandle(weapon.LeftHandUsage, leftHandleTransform);
                leftWeaponInstance = handle;
            }

            if (weapon.RightHandUsage != null)
            {
                GameObject handle = CreateWeaponHandle(weapon.RightHandUsage, rightdHandleTransform);
                rightWeaponInstance = handle;
            }

            playerAnimator.GetAnimator.runtimeAnimatorController = weapon.WeaponAnimator;
            currentWeapon = weapon;

            return;

            static GameObject CreateWeaponHandle(GameObject prefab, Transform parent)
            {
                GameObject result = Instantiate(prefab, parent);
                result.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                return result;
            }
        }
    }
}
