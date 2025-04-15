using System.Collections.Generic;
using Swift_Blade.Feeling;
using Swift_Blade.Audio;
using UnityEngine;
using System;
using Swift_Blade.Pool;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
    public class WeaponSO : ScriptableObject
    {
        [field: Header("Damage")]
        [field: SerializeField] public float AdditionalNormalDamage { get; private set; }
        [field: SerializeField] public float AdditionalHeavyDamage { get; private set; }
        [field: SerializeField] public float RollAttackDamage { get; private set; }

        [field: Header("Feeling")]
        [field: SerializeField] public CameraShakeType WeaponCameraShkaeType { get; private set; }
        [field: SerializeField] public CameraFocusSO WeaponCameraFocus { get; private set; }
        [field: SerializeField] public HitStopSO WeaponHitStop { get; private set; }

        [field: Header("Setting")]
        [field: SerializeField] public RuntimeAnimatorController WeaponAnimator { get; private set; }
        [field: SerializeField] public WeaponHandler LeftWeaponHandler { get; set; }
        [field: SerializeField] public WeaponHandler RightWeaponHandler { get; set; }
        [field:SerializeField] public Mesh PreviewMesh { get; private set; }

        [SerializeField] private SerializableDictionary<EAudioType, BaseAudioSO> audioDictionary;
        public IReadOnlyDictionary<EAudioType, BaseAudioSO> GetAudioDictionary => audioDictionary;
        /// <summary>
        /// color is limited to (red, blu, green)
        /// </summary>
        [field: SerializeField] public ColorType ColorType { get; private set; }
        [SerializeField] private float specialModifier;
        [SerializeField] private float rollModifier;
        [field: SerializeField, Range(1, 3)] public float CastRange { get; private set; }

        private const float BASE_SPECIAL_DELAY = 1f;
        private const float BASE_ROLL_DELAY = 1f;
        public float GetSpecialDelay => BASE_SPECIAL_DELAY + specialModifier;
        public float GetRollDelay => BASE_ROLL_DELAY + rollModifier;

        [SerializeField] private PoolPrefabMonoBehaviourSO specialParticle;
        private Transform playerTransform;
        
        
        private void OnValidate()
        {
            ColorType banType = ~(ColorType.RED | ColorType.BLUE | ColorType.GREEN);

            if ((ColorType & banType) != 0)
            {
                Debug.LogError($"{nameof(ColorType)} contains banned type");
                Debug.Log(ColorType);
                ColorType = ColorType.RED;
            }
            //can't detect yellow because enum is not a flag
            else if (ColorType == ColorType.YELLOW)
            {
                Debug.LogError("yellow yellow");
                ColorType = ColorType.RED;
            }

            int enumLength = Enum.GetValues(typeof(EAudioType)).Length;
            if (enumLength != audioDictionary.Keys.Count)
            {
                Debug.LogWarning(name + " doesn't have all EAudioType values in " + nameof(audioDictionary), this);
            }
        }
        public Action GetSpecialBehaviour(Player entity)
        {
            Action result = default;
            if (playerTransform == null)
            {
                playerTransform = entity.GetPlayerTransform;
            }
            
            switch (ColorType)
            {
                case ColorType.RED:
                    result = () =>
                    {
                        entity.GetStateMachine.ChangeState(PlayerStateEnum.Parry);
                    };
                    break;
                case ColorType.GREEN:
                    result = () =>
                    {
                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.HEALTH, nameof(StatType.HEALTH), 5, 3 , PlayHexagonParticle);
                    };
                    break;
                case ColorType.BLUE:
                    result = () =>
                    {
                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.ATTACKSPEED, nameof(StatType.ATTACKSPEED), 3, 1 , PlayBlastEffect);
                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.MOVESPEED, nameof(StatType.MOVESPEED), 3, 1);
                    };
                    break;
                default:
                    throw new NotImplementedException($"color type {ColorType} is not implemented");
            }
            return result;
        }

        private void PlayBlastEffect()
        {
            MonoGenericPool<HexagonParticle>.Initialize(specialParticle);
            
            HexagonParticle hexagonParticle = MonoGenericPool<HexagonParticle>.Pop();
            hexagonParticle.transform.SetParent(playerTransform);
            hexagonParticle.transform.position = playerTransform.position + new Vector3(0,1f,0);
        }
        
        private void StopBlastEffect()
        {
            
        }
        
        private void PlayHexagonParticle()
        {
            MonoGenericPool<HexagonParticle>.Initialize(specialParticle);
            
            HexagonParticle hexagonParticle = MonoGenericPool<HexagonParticle>.Pop();
            hexagonParticle.transform.SetParent(playerTransform);
            hexagonParticle.transform.position = playerTransform.position + new Vector3(0,1f,0);
        }

        
        
    }
}
