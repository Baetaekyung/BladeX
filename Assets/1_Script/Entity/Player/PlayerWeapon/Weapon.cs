using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
    public class Weapon : ScriptableObject
    {
        [field: SerializeField] public GameObject LeftHandUsage { get; private set; }
        [field: SerializeField] public GameObject RightHandUsage { get; private set; }

        [field: SerializeField] public RuntimeAnimatorController WeaponAnimator { get; private set; }

        /// <summary>
        /// color is limited to (red, blu, green)
        /// </summary>
        [SerializeField] private ColorType colorType;
        [SerializeField] private float specailModifier;
        [SerializeField] private float rollModifier;

        private const float BASE_SPECIAL_DELAY = 1f;
        private const float BASE_ROLL_DELAY = 1f;
        public float GetSpecialDelay => BASE_SPECIAL_DELAY + specailModifier;
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
        }
        public Action GetSpecialBehaviour(Player entity)
        {
            Action result = default;
            switch (colorType)
            {
                case ColorType.RED:
                    result = () => { Debug.Log("red"); };
                    break;
                case ColorType.GREEN:
                    result = () => { Debug.Log("green"); };
                    break;
                case ColorType.BLUE:
                    result = () => { Debug.Log("blue"); };
                    break;
                default:
                    throw new NotImplementedException("color type is not implemented");
            }
            return result;
        }
    }
}
