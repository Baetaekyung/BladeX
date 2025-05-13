using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum ColorType
    {
        RED,
        GREEN,
        BLUE,
        YELLOW, //RED + GREEN
        TURQUOISE, //BLUE + GREEN
        PURPLE, //RED + BLUE
        BLACK //RED + BLUE + GREEN
    }

    public static class ColorUtils
    {
        private static List<ColorType> remainColors = new List<ColorType>();

        public static ColorType GetColor(List<ColorType> colors)
        {
            int redColor = 0;
            int greenColor = 0;
            int blueColor = 0;

            foreach (var color in colors)
            {
                switch (color)
                {
                    case ColorType.RED:
                        redColor = 1;
                        continue;
                    case ColorType.GREEN:
                        greenColor = 1;
                        continue;
                    case ColorType.BLUE:
                        blueColor = 1;
                        continue;
                }
            }

            return GetRGBColorType((redColor, greenColor, blueColor));
        }

        public static List<ColorType> GetCotainColors(ColorType myType)
        {
            remainColors.Clear();

            switch (myType)
            {
                case ColorType.RED:
                    remainColors.Add(ColorType.RED);
                    break;
                case ColorType.GREEN:
                    remainColors.Add(ColorType.GREEN);
                    break;
                case ColorType.BLUE:
                    remainColors.Add(ColorType.BLUE);
                    break;
                case ColorType.YELLOW:
                    remainColors.Add(ColorType.RED);
                    remainColors.Add(ColorType.GREEN);
                    break;
                case ColorType.TURQUOISE:
                    remainColors.Add(ColorType.BLUE);
                    remainColors.Add(ColorType.GREEN);
                    break;
                case ColorType.PURPLE:
                    remainColors.Add(ColorType.RED);
                    remainColors.Add(ColorType.BLUE);
                    break;
                case ColorType.BLACK:
                    remainColors.Add(ColorType.RED);
                    remainColors.Add(ColorType.GREEN);
                    remainColors.Add(ColorType.BLUE);
                    break;
            }

            //if do not input type get empty list.
            return remainColors;
        }

        public static ColorType GetRGBColorType((int r, int g, int b) rgb)
        {
            ColorType type = rgb switch
            {
                (0, 0, 0) => ColorType.BLACK,
                (1, 0, 0) => ColorType.RED,
                (0, 1, 0) => ColorType.GREEN,
                (0, 0, 1) => ColorType.BLUE,
                (1, 1, 0) => ColorType.YELLOW,
                (1, 0, 1) => ColorType.PURPLE,
                (0, 1, 1) => ColorType.TURQUOISE,
                (1, 1, 1) => ColorType.BLACK,
                _ => throw new Exception("rgb each value must be 1 or 0")
            };

            return type;
        }

        public static (int r, int g, int b) GetRGBColorTuple(ColorType colorType)
        {
            (int, int, int) rgb = colorType switch
            {
                ColorType.RED => (1, 0, 0),
                ColorType.GREEN => (0, 1, 0),
                ColorType.BLUE => (0, 0, 1),
                ColorType.YELLOW => (1, 1, 0),
                ColorType.PURPLE => (1, 0, 1),
                ColorType.TURQUOISE => (0, 1, 1),
                ColorType.BLACK => (1, 1, 1),
                _ => throw new Exception("rgb each value must be 1 or 0")
            };

            return rgb;
        }

        public static Color GetCustomColor(ColorType colorType, float alpha = 0.9f)
        {
            (float r, float g, float b) = colorType switch
            {
                // if think it's not good, change values
                ColorType.RED => (0.86f, 0.3f, 0.3f),
                ColorType.GREEN => (0.54f, 0.86f, 0.31f),
                ColorType.BLUE => (0.25f, 0.46f, 1),
                ColorType.YELLOW => (0.95f, 0.95f, 0.41f),
                ColorType.PURPLE => (0.62f, 0.31f, 1f),
                ColorType.TURQUOISE => (0.4f, 0.99f, 0.99f),
                ColorType.BLACK => (1, 1, 1),
                _ => throw new Exception("rgb each value must be 1 or 0")
            };

            return new Color(r, g, b, alpha);
        }

        public static Color GetColorRGBUnity(ColorType colorType)
        {
            Color rgb = colorType switch
            {
                ColorType.RED => Color.red,
                ColorType.GREEN => Color.green,
                ColorType.BLUE => Color.blue,
                ColorType.YELLOW => Color.yellow,
                ColorType.PURPLE => new Color(1, 0, 1),
                ColorType.TURQUOISE => new Color(0, 1, 1),
                ColorType.BLACK => new Color(1, 1, 1),
                _ => throw new ArgumentOutOfRangeException("ColorType has no matching color" + colorType)
            };

            return rgb;
        }
        public static bool ContainsNonRGBColor(this ColorType ColorType)
        {
            ColorType banType = ~(ColorType.RED | ColorType.BLUE | ColorType.GREEN);

            bool result = (ColorType & banType) != 0 || ColorType == ColorType.YELLOW;
            return result;
        }

        public static string ColorText(string text, Color textColor)
        {
            string replaceText = text;
            string colorText = ColorUtility.ToHtmlStringRGB(textColor);

            replaceText = $"<color=#{colorText}>{replaceText}</color>";

            return replaceText;
        }
    }
}
