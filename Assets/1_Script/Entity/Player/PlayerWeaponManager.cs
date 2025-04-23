using UnityEngine;
using Swift_Blade.Combat.Caster;
using Swift_Blade.FSM;
using Swift_Blade.Combat.Feedback;
namespace Swift_Blade
{
    public class PlayerWeaponManager : MonoBehaviour,
        IEntityComponent,
        IEntityComponentStart
    {
        //note : bool check if player is initialized in this scene 
        // player is not initialized when new scene is loaded

        private bool isInitializedInThisScene = false;

        [Header("Feedback")]
        [SerializeField] private HitStopFeedback hitStopFeedback;
        [SerializeField] private CameraFocusFeedback cameraFocusFeedback;
        [SerializeField] private CameraShakeFeedback cameraShakeFeedback;

        [Header("Setting")]
        [SerializeField] private WeaponSO defaultWeapon;
        [SerializeField] private AudioTrigger audioTrigger;

        [SerializeField] private Transform leftHandleTransform;
        [SerializeField] private Transform rightdHandleTransform;

        [SerializeField] private SerializableDictionary<ColorType, GameObject> colorTrails;

        private WeaponHandler leftWeaponInstance;
        private WeaponHandler rightWeaponInstance;
        private GameObject leftTrailHandle;
        private GameObject rightTrailHandle;

        public bool TrailActive
        {
            set
            {
                if (leftTrailHandle != null)
                {
                    leftTrailHandle.SetActive(value);
                }
                if (rightTrailHandle != null)
                {
                    rightTrailHandle.SetActive(value);
                }
            }
        }

        private FiniteStateMachine<PlayerStateEnum> playerFsm;
        private PlayerAnimator playerAnimator;
        private PlayerDamageCaster playerDamageCaster;
        public static WeaponSO CurrentWeapon { get; private set; }

        private void Awake()
        {
            Debug.Assert(defaultWeapon != null, "default weapon is null");
            isInitializedInThisScene = false;
        }
        void IEntityComponent.EntityComponentAwake(Entity entity)
        {
            playerFsm = (entity as Player).GetStateMachine;
        }
        void IEntityComponentStart.EntityComponentStart(Entity entity)
        {
            playerAnimator = entity.GetEntityComponent<PlayerAnimator>();
            playerDamageCaster = entity.GetEntityComponent<PlayerDamageCaster>();
            
            SetWeapon(CurrentWeapon != null ? CurrentWeapon : defaultWeapon);
        }
        public void SetDefaultWeapon()
        {
            //SetWeapon(defaultWeapon);
            CurrentWeapon = defaultWeapon;
        }
        public void SetWeapon(WeaponSO weapon)
        {
            Debug.Assert(weapon != null, "weapon is null");

            if (isInitializedInThisScene && CurrentWeapon == weapon)
            {
                Debug.LogWarning("weapon is already equipped");
                return;
            }

            //clear currently holding weapons
            if (CurrentWeapon != null)
            {
                if (leftWeaponInstance != null)//note : automatically destroyed when new scene is loaded
                {
                    Destroy(leftWeaponInstance.gameObject); // todo : need optimazation
                }
                if (rightWeaponInstance != null)
                {
                    Destroy(rightWeaponInstance.gameObject); // todo : need optimazation
                }
            }

            GameObject colorGameobject = colorTrails[weapon.ColorType];
            Debug.Assert(colorGameobject != null, "can't find color");

            WeaponHandler leftWeaponHandler = weapon.LeftWeaponHandler;
            if (leftWeaponHandler != null)
            {
                SetWeaponHandle(leftWeaponHandler, leftHandleTransform, colorGameobject, ref leftWeaponInstance, ref leftTrailHandle);
            }
            WeaponHandler rightWeaponHandler = weapon.RightWeaponHandler;
            if (rightWeaponHandler != null)
            {
                SetWeaponHandle(weapon.RightWeaponHandler, rightdHandleTransform, colorGameobject, ref rightWeaponInstance, ref rightTrailHandle);
            }

            CurrentWeapon = weapon;

            audioTrigger.AudioType = weapon.GetAudioDictionary;
            playerDamageCaster.CastingRange = weapon.CastRange;

            playerFsm.ChangeState(PlayerStateEnum.Move);

            //feedback settings
            hitStopFeedback.HitStopData = weapon.WeaponHitStop;
            cameraFocusFeedback.FocusData = weapon.WeaponCameraFocus;
            cameraShakeFeedback.ShakeType = weapon.WeaponCameraShkaeType;

            playerAnimator.GetAnimator.runtimeAnimatorController = weapon.WeaponAnimator;
            playerAnimator.GetAnimator.Rebind();

            isInitializedInThisScene = true;

            return;

            static void SetWeaponHandle(WeaponHandler weaponHandler, Transform weaponHandleTransform, GameObject colorGameobject, ref WeaponHandler weaponHandleInstance,
                ref GameObject trailInstance)
            {
                WeaponHandler weaponHandle = CreateWeaponHandle(weaponHandler, weaponHandleTransform);
                weaponHandleInstance = weaponHandle;

                Transform trailTransform = weaponHandleInstance.TrailTransform;
                if (trailTransform != null)
                {
                    trailInstance = Instantiate(colorGameobject, trailTransform);
                    trailInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    trailInstance.SetActive(false);
                }
            }

            static WeaponHandler CreateWeaponHandle(WeaponHandler prefab, Transform parent)
            {
                WeaponHandler result = Instantiate(prefab, parent);
                result.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                return result;
            }
        }


    }
}
