using UnityEngine;
using System.Collections.Generic;

namespace Swift_Blade
{
    public class ColorMixer : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ColorType, ColorRecorder> colorRecordDictionary = new();


        public ColorRecorder GetColorRecorder(ColorType colorType)
        {
            if(colorRecordDictionary.TryGetValue(colorType, out var colorRecorder))
                return colorRecorder;

            Debug.LogWarning("Color recorder is not exist in dictionary", transform);
            return default;
        }

        //Mix color to mixed color
        public void MixColor(ColorType colorType, int value)
        {
            var colorList = ColorUtils.GetCotainColors(colorType);

            bool isValid = CheckIsValidToMix(colorList);

            if(isValid)
            {
                DowngradeIngredientColors(colorList);

                Player.Instance.GetEntityComponent<PlayerStatCompo>().IncreaseColorValue(colorType, 1);
            }
        }

        //ingredients value -1
        private void DowngradeIngredientColors(List<ColorType> colorList)
        {
            foreach (var color in colorList)
            {
                GetColorRecorder(color).Downgrade();
            }
        }

        //check it is valid to mix, increased color
        private bool CheckIsValidToMix(List<ColorType> colorList)
        {
            foreach (var color in colorList)
            {
                if (!GetColorRecorder(color).CheckValidToDowngrade())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
