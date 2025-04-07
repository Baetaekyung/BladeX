using UnityEngine;
using Swift_Blade.Combat.Caster;
using Swift_Blade.FSM;
namespace Swift_Blade
{
    public class PlayerWeaponManager : MonoBehaviour, 
        IEntityComponent,
        IEntityComponentStart
    {
        //note : bool check if player is initialized in this scene 
        // player is not initialized when new scene is loaded
        private bool isNotInitializedInThisScene = false;

        [SerializeField] private Transform leftHandleTransform;
        [SerializeField] private Transform rightdHandleTransform;
        [SerializeField] private Weapon defaultWeapon;
        [SerializeField] private AudioTrigger audioTrigger;

        private GameObject leftWeaponInstance;
        private GameObject rightWeaponInstance;

        private FiniteStateMachine<PlayerStateEnum> playerFsm;
        private PlayerAnimator playerAnimator;
        private PlayerDamageCaster playerDamageCaster;
        public static Weapon CurrentWeapon { get; private set; }

        private void Awake()
        {
            Debug.Assert(defaultWeapon != null, "default weapon is null");
        }
        void IEntityComponent.EntityComponentAwake(Entity entity)
        {
            playerFsm = (entity as Player).GetStateMachine;
            isNotInitializedInThisScene = true;
        }
        void IEntityComponentStart.EntityComponentStart(Entity entity)
        {
            playerAnimator = entity.GetEntityComponent<PlayerAnimator>();
            playerDamageCaster = entity.GetEntityComponent<PlayerDamageCaster>();
            SetWeapon(CurrentWeapon != null ? CurrentWeapon : defaultWeapon);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                SetWeapon(defaultWeapon);
            }
        }
        public void SetWeapon(Weapon weapon)
        {
            if (!isNotInitializedInThisScene && CurrentWeapon == weapon)
            {
                Debug.LogWarning("weapon is already equipped");
                return;
            }

            //clear current holding weapon
            if (CurrentWeapon != null)
            {
                if (leftWeaponInstance != null)//note : automatically destroyed when new scene is loaded
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
            CurrentWeapon = weapon;

            audioTrigger.AudioType = weapon.GetAudioDictionary;

            playerDamageCaster.CastingRange = weapon.CastRange;

            playerAnimator.GetAnimator.Rebind();
            playerFsm.ChangeState(PlayerStateEnum.Move);

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
