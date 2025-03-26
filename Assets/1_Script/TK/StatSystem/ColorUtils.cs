using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

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

    //I feel sad cause it is dont need to!
    public static class ColorUtils
    {
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
            switch (myType)
            {
                case ColorType.RED:
                    return new List<ColorType>() { ColorType.RED };
                case ColorType.GREEN:
                    return new List<ColorType>() { ColorType.GREEN };
                case ColorType.BLUE:
                    return new List<ColorType>() { ColorType.BLUE };
                case ColorType.YELLOW:
                    return new List<ColorType>() { ColorType.RED, ColorType.GREEN };
                case ColorType.TURQUOISE:
                    return new List<ColorType>() { ColorType.BLUE, ColorType.GREEN };
                case ColorType.PURPLE:
                    return new List<ColorType>() { ColorType.RED, ColorType.BLUE };
                case ColorType.BLACK:
                    return new List<ColorType>() { ColorType.RED, ColorType.GREEN, ColorType.BLUE };
            }

            //if do not input type get empty list.
            return default;
        }

        public static ColorType GetRGBColorType((int r, int g, int b) rgb)
        {
            ColorType type = rgb switch
            {
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
    }
}
