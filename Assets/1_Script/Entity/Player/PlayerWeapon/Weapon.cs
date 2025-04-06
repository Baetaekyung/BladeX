using Swift_Blade.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
    public class Weapon : ScriptableObject
    {
        [field: Header("Damage")]
        [field: SerializeField] public float AdditionalNormalDamage { get; private set; }
        [field: SerializeField] public float AdditionalHeavyDamage { get; private set; }

        [field: Header("Setting")]
        [field: SerializeField] public GameObject LeftHandUsage { get; private set; }
        [field: SerializeField] public GameObject RightHandUsage { get; private set; }
        [field: SerializeField] public RuntimeAnimatorController WeaponAnimator { get; private set; }

        [SerializeField] private SerializableDictionary<EAudioType, BaseAudioSO> audioDictionary;
        public IReadOnlyDictionary<EAudioType, BaseAudioSO> GetAudioDictionary => audioDictionary;
        /// <summary>
        /// color is limited to (red, blu, green)
        /// </summary>
        [SerializeField] private ColorType colorType;
        [SerializeField] private float specialModifier;
        [SerializeField] private float rollModifier;
        [field: SerializeField, Range(1, 2.5f)] public float CastRange { get; private set; }

        private const float BASE_SPECIAL_DELAY = 1f;
        private const float BASE_ROLL_DELAY = 1f;
        public float GetSpecialDelay => BASE_SPECIAL_DELAY + specialModifier;
        public float GetRollDelay => BASE_ROLL_DELAY + rollModifier;
        private void OnValidate()
        {
            ColorType banType = ~(ColorType.RED | ColorType.BLUE | ColorType.GREEN);

            if ((colorType & banType) != 0)
            {
                Debug.LogError($"{nameof(colorType)} contains banned type");
                Debug.Log(colorType);
                colorType = ColorType.RED;
            }
            //can't detect yellow because enum is not a flag
            else if (colorType == ColorType.YELLOW)
            {
                Debug.LogError("yellow yellow");
                colorType = ColorType.RED;
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
            switch (colorType)
            {
                case ColorType.RED:
                    result = () => { entity.GetStateMachine.ChangeState(PlayerStateEnum.Parry); };
                    break;
                case ColorType.GREEN:
                    result = () => { entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.HEALTH, nameof(StatType.HEALTH), 5, 3); };
                    break;
                case ColorType.BLUE:
                    result = () => 
                    {
                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.ATTACKSPEED, nameof(StatType.ATTACKSPEED), 3, 1);
                        entity.GetEntityComponent<PlayerStatCompo>().BuffToStat(StatType.MOVESPEED, nameof(StatType.MOVESPEED), 3, 1);
                    };
                    break;
                default:
                    throw new NotImplementedException($"color type {colorType} is not implemented");
            }
            return result;
        }
        private void OnDestroy()
        {
        }
    }
}
