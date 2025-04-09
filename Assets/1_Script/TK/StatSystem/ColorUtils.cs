using System;
using System.Collections.Generic;

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
            int redColor   = 0;
            int greenColor = 0;
            int blueColor  = 0;

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

        public static (int r, int g, int b) GetRGBColor(ColorType colorType)
        {
            (int, int, int) rgb = colorType switch
            {
                ColorType.RED       => (1, 0, 0),
                ColorType.GREEN     => (0, 1, 0),
                ColorType.BLUE      => (0, 0, 1),
                ColorType.YELLOW    => (1, 1, 0),
                ColorType.PURPLE    => (1, 0, 1),
                ColorType.TURQUOISE => (0, 1, 1),
                ColorType.BLACK     => (1, 1, 1),
                _ => throw new Exception("rgb each value must be 1 or 0")
            };

            return rgb;
        }
    }
}
