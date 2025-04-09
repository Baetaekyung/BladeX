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
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                SetWeapon(defaultWeapon);
            }
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

            //instansiate new weapon visuals
            if (weapon.LeftWeaponHandler != null)
            {
                WeaponHandler handle = CreateWeaponHandle(weapon.LeftWeaponHandler, leftHandleTransform);
                leftWeaponInstance = handle;
                leftTrailHandle = Instantiate(colorTrails[weapon.ColorType], leftWeaponInstance.TrailTransform);
                leftTrailHandle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                leftTrailHandle.SetActive(false);
            }

            if (weapon.RightWeaponHandler != null)
            {
                WeaponHandler handle = CreateWeaponHandle(weapon.RightWeaponHandler, rightdHandleTransform);
                rightWeaponInstance = handle;
                rightTrailHandle = Instantiate(colorTrails[weapon.ColorType], rightWeaponInstance.TrailTransform);
                rightTrailHandle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                rightTrailHandle.SetActive(false);
            }

            CurrentWeapon = weapon;

            playerAnimator.GetAnimator.runtimeAnimatorController = weapon.WeaponAnimator;
            playerAnimator.GetAnimator.Rebind();

            audioTrigger.AudioType = weapon.GetAudioDictionary;
            playerDamageCaster.CastingRange = weapon.CastRange;

            playerFsm.ChangeState(PlayerStateEnum.Move);

            hitStopFeedback.HitStopData = weapon.WeaponHitStop;
            cameraFocusFeedback.FocusData = weapon.WeaponCameraFocus;
            cameraShakeFeedback.ShakeType = weapon.WeaponCameraShkaeType;

            isInitializedInThisScene = true;

            return;

            static TResult CreateWeaponHandle<TResult>(TResult prefab, Transform parent)
                where TResult : MonoBehaviour
            {
                TResult result = Instantiate(prefab, parent);
                result.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                return result;
            }
        }


    }
}
