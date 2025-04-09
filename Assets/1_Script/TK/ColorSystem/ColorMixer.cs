using UnityEngine;
using System.Collections.Generic;

namespace Swift_Blade
{
    public class ColorMixer : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ColorType, ColorRecorder> colorRecordDictionary = new();

        public void MixColor(ColorType colorType)
        {
            var colorList = ColorUtils.GetCotainColors(colorType);

            if(CheckIsValidToMix(colorList))
            {
                //Decrease ingredient colors ex) make yellow 1, red -1, green -1
                DecreaseIngredientColors(colorList);

                //Increase mixed color value 1
                Player.Instance.GetEntityComponent<PlayerStatCompo>().IncreaseColorValue(colorType, 1);
            }
            else
            {
                PopupManager.Instance.LogMessage("가지고 있는 색이 부족합니다.");
            }
        }

        //ingredients value -1
        private void DecreaseIngredientColors(IEnumerable<ColorType> colorList)
        {
            foreach(var color in colorList)
                GetColorTypeRecorder(color).DecreaseColor();
        }

        //check it is valid to mix, increased color
        private bool CheckIsValidToMix(IEnumerable<ColorType> colorList)
        {
            foreach (var color in colorList)
            {
                //every colors are valid. if not return false
                if (GetColorTypeRecorder(color).CheckValidToDecrease() == false)
                    return false;
            }

            return true;
        }

        public ColorRecorder GetColorTypeRecorder(ColorType colorType)
        {
            if (colorRecordDictionary.TryGetValue(colorType, out var colorRecorder))
                return colorRecorder;

            Debug.LogWarning("Color recorder is not exist in dictionary", transform);
            return null;
        }
    }
}
